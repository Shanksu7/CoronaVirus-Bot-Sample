using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace testbot
{
    public class DiscordBot
    {
        public string _prefix;

        public DiscordSocketClient Client { get; set; }
        public CommandService Commands { get; set; }
        private IServiceProvider Services { get; set; }
        private string bot_Token;

        public bool Initialized = false;
        public DiscordBot(string prefix, string token)
        {
            _prefix = prefix;
            bot_Token = token;
            Client = new DiscordSocketClient();
            Commands = new CommandService();
            Services = new ServiceCollection().AddSingleton(Client).AddSingleton(Commands).BuildServiceProvider();
            Client.Log += Log;
            Client.Ready += OnReady;
        }
        public async Task Run()
        {
            try
            {
                await RegisterCommandsAsync();
                await Client.LoginAsync(TokenType.Bot, bot_Token);
                await Client.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task OnReady()
        {
            await Client.SetGameAsync("[.help] Viralizing", type: ActivityType.Playing);
            Console.WriteLine(Client.CurrentUser.Username + "#" + Client.CurrentUser.Discriminator + " Is ready!");
        }



        private async Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            await Task.CompletedTask;
        }
        public async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot)
                return;

            int argPos = 0;
            if (message.HasStringPrefix(_prefix, ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(Client, message);
                await Commands.ExecuteAsync(context, argPos, Services);
                return;
            }
        }

    }
}
