using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.NetCore
{
    /// <summary>
    /// DiscordBot メイン処理
    /// </summary>
    public class MainLogic
    {
        /// <summary>
        /// Botクライアント
        /// </summary>
        public static DiscordSocketClient Client;
        /// <summary>
        /// Discordコマンドをやり取りするService層
        /// </summary>
        public static CommandService Commands;
        /// <summary>
        /// ServiceProvider
        /// </summary>
        public static IServiceProvider Provider;

        /// <summary>
        /// 起動時処理
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync()
        {
            // ServiceProviderインスタンス生成
            Provider = new ServiceCollection().BuildServiceProvider();

            // 自身のアセンブリにコマンドの処理を構築する為、自身をCommandServiceに追加
            Commands = new CommandService();
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly());

            // Botアカウントに機能を追加
            Client = new DiscordSocketClient();
            Client.MessageReceived += CommandRecieved;
            Client.Log += msg => { Console.WriteLine(msg.ToString()); return Task.CompletedTask; };
            // BotアカウントLogin
            var token = "";
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            // タスクを常駐
            await Task.Delay(-1);
        }

        /// <summary>
        /// メッセージの受信処理
        /// </summary>
        /// <param name="messageParam">受信メッセージ</param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);

            // コメントがユーザーかBotかの判定
            if (message?.Author.IsBot ?? true)
            {
                return;
            }

            // Botコマンドかどうか判定（判定条件は接頭辞"!"付き発言 or Botアカウントへのメンション）
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos)))
            {
                return;
            }

            // 実行
            var context = new CommandContext(Client, message);
            var result = await Commands.ExecuteAsync(context, argPos, Provider);

            //実行できなかった場合
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
