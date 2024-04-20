using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutorialBot2.Data;

namespace TutorialBot2.commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("Help")]
        public async Task MyFirstCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Comandos{ctx.User.Username} :\n!matches PlayerID MatchNumber(0-19)");
        }


        [Command("PlayerID")]

        private async Task playerData(CommandContext ctx, int PlayerID)
        {
            dataQuery query = new dataQuery();
            var oObject = await query.playerQuery(PlayerID);
            var message = new DiscordEmbedBuilder
            {
                Title = ctx.User.Username,
                Description = oObject.profile.personaname + "\n RankTier: " + oObject.rank_tier + "\n Steam Profile: "+ oObject.profile.profileurl + "\n Last Login: " + oObject.profile.last_login,
                Color = DiscordColor.Cyan,
                ImageUrl = oObject.profile.avatarfull
            };
            
            
            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("matches")]

        private async Task lastMatches(CommandContext ctx, int playerID, int partida)
        {
            decimal kdr;
            string winner;
            string team;
            if (partida > 19)
            {
                var embedError = new DiscordEmbedBuilder
                {
                    Title = "ERROR:",
                    Description = "Solo pueden visualizarse las ultimas 20 partidas (0-19)",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed: embedError);
                return;
            }
            dataQuery pquery = new dataQuery();
            var pObject = await pquery.playerQuery(playerID);
            dataQuery query = new dataQuery();
            var oObject = await query.matchQuery(playerID);
            dataQuery hquery = new dataQuery();
            var hObject = await hquery.heroQuery();
            if (oObject[partida].deaths == 0) kdr = (decimal)oObject[partida].kills;
            else kdr = (decimal)oObject[partida].kills / (decimal)oObject[partida].deaths;
            if (oObject[partida].radiant_win == true) winner = "Radiant Victory";
            else winner = "Dire Victory";
            if (oObject[partida].player_slot < 5) team = "Radiant";
            else team = "Dire";

            var heroName = hObject.Where(b => b.id == oObject[partida].hero_id)
                                    .Select(b => new
                                    {
                                        b.localized_name
                                    }).First();

            var embed = new DiscordEmbedBuilder
            {

            };
            embed.AddField($"Match Stadistics {oObject[partida].match_id}", $"Player: {pObject.profile.personaname}")
                .WithAuthor(ctx.User.Username)
                .WithTitle($"**Match Outcome: `{winner}`**")
                .WithDescription($"**Hero:** {heroName.localized_name}" + $"\n**Team: **{team}" + $"\n**Kills:** {oObject[partida].kills.ToString()}" + $"\n**Assists:** {oObject[partida].assists.ToString()}" + $"\n**Deaths:** {oObject[partida].deaths.ToString()}" + $"\n**KDR:** {kdr.ToString("F2")}" +
                $"\n**XPM:** {oObject[partida].xp_per_min.ToString()}" + $"\n**GPM:** {oObject[partida].gold_per_min.ToString()}")
                .WithColor(DiscordColor.Blue)
                .WithTimestamp(DateTime.Now)
                .WithImageUrl($"{pObject.profile.avatarfull}");

            /*
             * Codigo Viejo
             * var embed = new DiscordEmbedBuilder
            {
                
                //Author = { Name = ctx.User.Username },
                /*Title = $"Estas son las estadisticas de la partida {oObject[partida].match_id} de {pObject.profile.personaname}",
                Description = $"**Hero:** {hObject[oObject[partida].hero_id - 1].localized_name}" + $"\n**Kills:** {oObject[partida].kills.ToString()}" + $"\n**Assists:** {oObject[partida].assists.ToString()}" + $"\n**Deaths:** {oObject[partida].deaths.ToString()}" + $"\n**KDR:** {kdr.ToString("F2")}" +
                $"\n**XPM:** {oObject[partida].xp_per_min.ToString()}" + $"\n**GPM:** {oObject[partida].gold_per_min.ToString()}" + $"\n**Match Outcome: `{winner}`**",

                ImageUrl = pObject.profile.avatarfull,
                Color = DiscordColor.Teal,
                Timestamp = DateTime.Now,
                Footer = { Text = $"Requested by ", IconUrl = "https://i.imgur.com/AfFp7pu.png" }
            };*/



            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("UserReg")]
        private async Task userRegistration(CommandContext ctx, string steamID, int dotaID)
        {
          
            DBuser dBuser = new DBuser();
            users user = new users(ctx.User.Username, steamID, dotaID);
            dataQuery query = new dataQuery();
            var oObject = await query.playerQuery(dotaID);
            if (dBuser.Add(user))
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = ctx.User.Username,
                    Description = $"Ya existe el usuario \nSteamID: {steamID}",
                    Color = DiscordColor.Red,
                    ImageUrl = oObject.profile.avatarfull
                };
                await ctx.Channel.SendMessageAsync(embed: message);
            }
            else
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = ctx.User.Username,
                    Description = $"Se ha agregado el usuario \nSteamID: {steamID}   " + $"   DotaID: {dotaID}",
                    Color = DiscordColor.Green,
                    ImageUrl = oObject.profile.avatarfull
                };
                await ctx.Channel.SendMessageAsync(embed: message);
            }
           
        }

       [Command("CheckReg")]
        private async Task checkRegistration(CommandContext ctx)
        {
            DBuser dBuser = new DBuser();
            var userdB = dBuser.Get(ctx.User.Username);

            foreach (var userData in userdB)
            {
                Console.WriteLine(userData.steam_id);
            }

            var message = new DiscordEmbedBuilder
            {
                Title = ctx.User.Username,
                Description = $"DiscordID: {userdB[0].discord_id}"+ $"SteamID: https://steamcommunity.com/id/{userdB[0].steam_id}"+ $"DotaID: {userdB[0].dota_id}",
                Color = DiscordColor.Cyan
            };

            
            await ctx.Channel.SendMessageAsync(embed: message);
        }

    }
}
