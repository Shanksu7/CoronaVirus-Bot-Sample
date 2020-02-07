using Discord;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace nCoV_2019_Core
{
    public class CoronaVirusRegistry
    {
        public static List<CoronaVirusRegistry> Entries = new List<CoronaVirusRegistry>();
        public static Embed embed, embed_mini;

        public static void Initialize()
        {
            try
            {
                Entries = new List<CoronaVirusRegistry>();
                var title = "**PAIS**: [CONFIRMADOS] [MUERTES] [RECUPERACIONES]\n\n";
                var desc = "";
                var html = @"https://docs.google.com/spreadsheets/d/1wQVypefm946ch4XDp37uZ-wartW4V7ILdg-qYiDXUHM/htmlview?usp=sharing&sle=true#";
                var web = new HtmlWeb();
                var htmlDoc = web.Load(html);
                var node = htmlDoc.DocumentNode.SelectNodes("//tr[@style='height:19px;']");
                var headersSkipped = false;
                foreach (var n in node)
                {
                    var country = n.ChildNodes[2].InnerText;
                    var date = n.ChildNodes[3].InnerText;
                    var confirm = n.ChildNodes[4].InnerText;
                    var death = n.ChildNodes[5].InnerText;
                    var recov = n.ChildNodes[6].InnerText;
                    var result = $"{country} - [{confirm}] [{death}] [{recov}]";
                    if (country == "Country/Region")
                    {
                        if (!headersSkipped)
                            headersSkipped = true;
                        else
                            break;
                    }
                    else
                        Entries.Add(new CoronaVirusRegistry(country, date, confirm, death, recov));
                }
                var query = Entries.GroupBy(p => p.Country, p => p, (key, val) => new
                {
                    Country = key,
                    Confirmed = val.Sum(x => int.Parse(x.Confirm)),
                    Deaths = val.Sum(x => int.Parse(x.Death)),
                    Recovered = val.Sum(x => int.Parse(x.Recov)),
                });
                foreach (var x in query)
                    desc += $"**{x.Country}**: [{x.Confirmed}] [{x.Deaths}] [{x.Recovered}]\n";
                embed = new EmbedBuilder()
                    .WithTitle($"nCov 2019 (Corona virus - registro actual)")
                    .WithColor(Color.Red)
                    .WithDescription(title + desc)
                    .AddField("Total", $"**Confirmados:** {query.Sum(x => x.Confirmed)}\n" +
                    $"**Muertes:** {query.Sum(x => x.Deaths)}\n" +
                    $"**Recuperados:** {query.Sum(x => x.Recovered)}")
                    .Build();
                embed_mini = new EmbedBuilder()
                   .WithTitle($"nCov 2019 (Corona virus - registro actual)")
                   .WithColor(Color.Red)
                   .AddField("Total", $"**Confirmados:** {query.Sum(x => x.Confirmed)}\n" +
                   $"**Muertes:** {query.Sum(x => x.Deaths)}\n" +
                   $"**Recuperados:** {query.Sum(x => x.Recovered)}")
                   .Build();
            }
            catch
            {
                embed = new EmbedBuilder()
                    .WithTitle($"nCov 2019 (Corona virus - registro actual)")
                    .WithDescription("Parece que hay problemas al recibir la informacion, intentalo mas tarde!")
                    .Build();
                System.Console.WriteLine("ncov failed");
            }
            //Console.WriteLine(msg);
        }
        private string Country { get; set; }
        private string Date { get; set; }
        private string Confirm { get; set; }
        private string Death { get; set; }
        private string Recov { get; set; }

        public CoronaVirusRegistry(string country, string date, string confirm, string death, string recov)
        {
            this.Country = country;
            this.Date = date;
            this.Confirm = confirm;
            this.Death = death;
            this.Recov = recov;
        }

    }
}
