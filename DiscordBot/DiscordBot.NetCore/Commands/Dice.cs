using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.NetCore.Commands
{
    /// <summary>
    ///     サイコロ機能クラス
    /// </summary>
    public class Dice : ModuleBase
    {
        /// <summary>
        ///     サイコロを振ります
        /// </summary>
        /// <param name="face">振るダイスの面の数</param>
        /// <param name="throwCount">投げる回数</param>
        /// <returns></returns>
        [Command("dice"), Alias("dicethrow")]
        public async Task DiceThrow(byte face = 6, byte throwCount = 1)
        {
            // 入力値検証
            if (face < 1)
            {
                await ReplyAsync("せめて面を1つは下さい :pray:");
                return;
            }
            if (throwCount < 1)
            {
                await ReplyAsync("振るなら1回以上は振ろうぜ :anger:");
                return;
            }

            // サイコロを振ってます・・・
            var resultText = new StringBuilder();
            var firstLine = true;
            var summary = 0;
            for (int i = 0; i < throwCount; i++)
            {
                var result = new Random().Next(1, face + 1);
                resultText.Append(firstLine ? result.ToString().PadLeft(3) : $", {result.ToString().PadLeft(3)}");
                summary += result;

                firstLine = false;
                if ((i + 1) % 10 == 0)
                {
                    resultText.AppendLine();
                    firstLine = true;
                }
            }

            // 結果一覧の埋め込み要素を作成
            var embed = new EmbedBuilder();
            embed.WithTitle("結果一覧");
            embed.WithDescription(resultText.ToString());

            // 結果返却
            await ReplyAsync($"{face}面ダイスを{throwCount}回振ったよ！\r\n合計は{summary}、平均値は{((double)summary / throwCount).ToString("#,0.00")}でした。", embed: embed.Build());
        }
    }
}
