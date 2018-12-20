namespace DiscordBot.NetCore
{
    class EntryPoint
    {
        /// <summary>
        ///     エントリーポイント
        /// </summary>
        /// <remarks>
        ///     <see cref="MainLogic"/>インスタンスを生成し、<see cref="MainLogic.MainAsync"/>を呼び出すだけ
        /// </remarks>
        static void Main() => new MainLogic().MainAsync().GetAwaiter().GetResult();
    }
}
