using CoronaVirus_Bot_Sample;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace testbot
{
    public class BaseCommands : ModuleBase<SocketCommandContext>
    {
        //Help msg can be performanced using properties of the Bot.Commands property
        [Command("help")]
        public async Task Help()
        => await ReplyAsync(embed: new EmbedBuilder()
            .WithColor(Color.Red)
            .WithAuthor(Program.Bot.Client.CurrentUser)
            .WithDescription(".invite\n.virus\n.virus full").Build());
        [Command("invite")]
        public async Task Invite()
        => await ReplyAsync(embed: new EmbedBuilder()
            .WithColor(Color.Red)
            .WithAuthor(Program.Bot.Client.CurrentUser)
            .WithDescription("[Invite me to your server!!](https://discordapp.com/api/oauth2/authorize?client_id=675119026241798154&permissions=2147483127&scope=bot)").Build());
        [Command("virus")]
        public async Task Healp()
        => await ReplyAsync(embed:nCoV_2019_Core.CoronaVirusRegistry.embed_mini);
        [Command("virus full")]
        public async Task Helasp()
        => await ReplyAsync(embed: nCoV_2019_Core.CoronaVirusRegistry.embed_mini);
    }
}
