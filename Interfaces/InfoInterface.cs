using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Globalization;

namespace MastersBot.Core.Interfaces
{
    public class InfoInterface : ModuleBase<SocketCommandContext>
    {
        TextInfo properText = new CultureInfo("en-US", false).TextInfo;
        Commands.infoCommands com = new Commands.infoCommands();

        [Command("hello"), Summary("Hello World!")]
        public async Task hello()
        {
            await Context.Channel.SendMessageAsync("Hi there.");
        }

        [Command("trainerInfo"), Summary("Gives information on a trainer")]
        public async Task trainer([Remainder]string input = "")
        {
            if(input.Equals(""))
            {
                await Context.Channel.SendMessageAsync("Please include a trainer name to look up.");
            }
            else
            {
                List<string> info = com.getTrainerInfo(input);
                EmbedBuilder result = makeEmbed(input, info);
                await Context.Channel.SendMessageAsync(null, false, result.Build());
            }
        }

        [Command("pokemonInfo"), Summary("Gives information on a pokemon")]
        public async Task pokemon([Remainder]string input = "")
        {
            if (input.Equals(""))
            {
                await Context.Channel.SendMessageAsync("Please include a trainer name to look up.");
            }
            else
            {
                List<string> info = com.getPokemonInfo(input);
                EmbedBuilder result = makeEmbed(input, null, info);
                await Context.Channel.SendMessageAsync(null, false, result.Build());
            }
        }

        [Command("syncpairInfo"), Summary("Gives information on a Sync Pair")]
        public async Task pair([Remainder]string input = "")
        {
            if (input.Equals(""))
            {
                await Context.Channel.SendMessageAsync("Please include a trainer name to look up.");
            }
            else
            {
                List<string> info = com.getPairInfo(input);
                EmbedBuilder result = makeEmbed(input, null, null, info);
                await Context.Channel.SendMessageAsync(null, false, result.Build());
            }
        }

        public EmbedBuilder makeEmbed(string name, List<string> trainerInfo = null, List<string> pokemonInfo = null, List<string> syncPairInfo = null)
        {
            EmbedBuilder result = new EmbedBuilder();
            result.WithAuthor("Poryphone", "https://cdn.discordapp.com/app-icons/607413084989947915/797b02e561699675af1a5321d1b49128.png?size=256");
            if(trainerInfo != null)
            {
                name = name.Replace("(", "").Replace(")", "").Replace(" ", "-");
                result.WithThumbnailUrl($"https://www.serebii.net/pokemonmasters/syncpairs/icons/{properText.ToLower(name)}.png");
                result.AddField("Name: ", trainerInfo[0]);
                result.AddField("Role: ", trainerInfo[1]);
                result.AddField("Rarity: ", trainerInfo[2]);
            }
            if(pokemonInfo != null)
            {
                name = name.Replace("(", "").Replace(")", "").Replace(" ", "-").ToLower();
                result.WithThumbnailUrl($"https://img.pokemondb.net/sprites/omega-ruby-alpha-sapphire/dex/normal/{name}.png");
                result.AddField("Name: ", pokemonInfo[0]);
                result.AddField("Type: ", pokemonInfo[1], true);
                result.AddField("Weakness: ", pokemonInfo[2], true);
                result.AddField("HP: ", pokemonInfo[3], true);
                result.AddField("SPD: ", pokemonInfo[8], true);
                result.AddField("ATK: ", pokemonInfo[4], true);
                result.AddField("DEF: ", pokemonInfo[5], true);
                result.AddField("S ATK: ", pokemonInfo[6], true);
                result.AddField("S DEF: ", pokemonInfo[7], true);
                result.AddField("Bulk Rank: ", pokemonInfo[9], true);
                result.AddField("Stat Rank: ", pokemonInfo[10], true);
            }
            if(syncPairInfo != null)
            {
                name = properText.ToTitleCase(syncPairInfo[1]) + " and " + properText.ToTitleCase(syncPairInfo[3]);
                result.WithImageUrl($"https://imagesatticus.files.wordpress.com/2019/08/{syncPairInfo[0]}.png");
                result.AddField("Name: ", name);
                result.AddField("Rarity: ", syncPairInfo[2], true);
                result.AddField("Role: ", syncPairInfo[4], true);
                result.AddField("Move 1: ", syncPairInfo[5], true);
                result.AddField("Move 2: ", syncPairInfo[6], true);
                result.AddField("Move 3: ", syncPairInfo[7], true);
                result.AddField("Move 4: ", syncPairInfo[8], true);
                result.AddField("Sync Move: ", syncPairInfo[9], true);
                result.AddField("Passive 1: ", syncPairInfo[10], true);
                if(syncPairInfo.Count >= 12)
                {
                    result.AddField("Passive 2: ", syncPairInfo[11], true);
                }
                if(syncPairInfo.Count >= 13)
                {
                    result.AddField("Passive 3: ", syncPairInfo[12], true);
                }
            }
            return result;
        }
    }
}
