using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    /// <summary>
    ///     PingPongを実行するクラス
    /// </summary>
    public class PingPong : ModuleBase
    {
        /// <summary>
        ///     pingの発言があった場合、pongを返します
        /// </summary>
        /// <returns></returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
