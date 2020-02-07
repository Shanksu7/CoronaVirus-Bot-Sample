using nCoV_2019_Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using testbot;

namespace CoronaVirus_Bot_Sample
{
    class Program
    {
        public static DiscordBot Bot;
        static Timer _ncov;
        static async Task Main(string[] args)
        {
            _ncov = new Timer(e => CoronaVirusRegistry.Initialize(), null, TimeSpan.Zero, TimeSpan.FromHours(2));
            Bot = new DiscordBot(".", Environment.GetEnvironmentVariable("bot_token"));
            await Bot.Run();
            await Task.Delay(-1);
        }
    }
}
