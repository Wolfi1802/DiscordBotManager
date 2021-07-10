using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class ShiningBeyond
    {

        public async Task OnSendBotInfomrations(CommandContext ctx, string wolfiImage)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: ctx.Member.DisplayName, url: wolfiImage, iconUrl: wolfiImage);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = $"Beginner Guide 13.12.2020";
            embed.AddField(name: "Was sollte man mit den Diamanten machen?", value: "Man sollte sich täglich 1 schlüssel selector für 300 Gems kaufen.\nWenn du den Equipment Dungeon  4 schaffst, kaufe dir zu dem Schlüssel Selector nochmal 1 - 2 x diesen Schlüssel.\nWaum? ->im Spiel ist das Equipment wichtiger als der Held.Vom Equipment gehen die meisten states aus, weswegen du die Materialien dringend brauchst.");
            embed.AddField(name: "Wie sollte man sein Team aufbauen?", value: "1 Tank, 1 Heiler, 2 Damage Dealer (vorzugsweise einen assasin) Wichtig ist dabei zu beachten, dass du nur 4Helden vorerst lvln solltest. Zudem kann ein Held ab 7* zwei Job Pfade besitzen!");
            embed.AddField(name: "Wofür sollte man Valiante ausgeben?", value: "Man sollte sich 2 x [6* Runen Selector] (für 70 je stk.) sowie 4  [6* class Jobs] (für je 30 stk.) kaufen.");
            embed.AddField(name: "Wie sollte man das Equipment aufbauen?", value: "In das Equipment solltest am Anfang nicht viel investieren.\nDu solltest erst mit 3* Equipment starten, dieses auf tier 3 ziehen und die states passend aufwerten. Wenn dir die Ressourcen wert sind wechselst du zu 4* Equipment.\nWenn nicht, wartest du bist du 5* Equipment durch die den täglichen Dungeon bekommst.\n-->Dropes des Equipment Dungeons<--\nDungeon 3: [3* bis 4* Drop]\nDungeon 4: [4* Drop]\nDungeon 5: [4* Drop]\nDungeon 6: [4* bis 5* Drop]\nDungeon 7: [5* Drop]\nDungeon 8: [5* Drop]\nDungeon 9 : [5* bis 6* Drop]");
            embed.AddField(name: "Welche states sind für welche klasse wichtig?", value: "Tank: Def > Hp > Skill dmg Reduction > Dodge > Block Chance\nAssasin: Dmg > Crit Rate > dodge > hp/def > skill resist\nHealer:Atk > Def > Hp > Skill dmg Reduction > Dodge > Block Chance\nSchütze: Dmg > Crit Rate > dodge > hp/def > skill resist");
            embed.AddField(name: "Wichtige Information", value: "Den täglichen Dungeon wodurch du Equipment bekommst, solltest du nicht per \"Express\" sondern manuel laufen. Hintergrund ist du bekommst ein drittel weniger Staub wenn du es nicht manuell läufst.\n\nDasAttribut Block Chance hat einen eingebauten cooldown. Selbst mit 400 % Block Chance wird nicht jeder eingehende Treffer geblockt.");

            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.Text = "Beginner Guide for Shining Beyond";

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        public async Task OnSendTierList(CommandContext ctx, string wolfiImage)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: ctx.Member.DisplayName, url: wolfiImage, iconUrl: wolfiImage);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = $"Tier List from Shining Beyond 13.12.2020";
            embed.AddField(name: "S Tier", value: "Jenny(Burst Dmg)\nLucille(Healer\\support)\nAthena(Tank)\nEmiko(DamageDealer\\Suport)\nArtemis(Burst Dmg)");
            embed.AddField(name: "A Tier", value: "Freya(Tank)\nFeya(Tank\\Healer)\nEmilia(Tank)\nTheia(Dmg Dealer\\Healer)\nKane(Burst Dmg)\nShizu(Burst Dmg)\nFran(Tank\\HIGH Survival)");
            embed.AddField(name: "B Tier", value: "Beretta(Suport)\nAltima(Dmg\\Tank)\nTess(Damage Dealer)\nRaegar(Damage Dealer)\nEmilia(Support)");
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.Text = "TierList from Shining Beyond";

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        public async Task OnSendCode(CommandContext ctx, string aviableRedeemDate, string code, string everyoneTag, string adminRoleName)
        {
            foreach (var item in ctx.Member.Roles)
            {
                if (item.Name == adminRoleName)
                {
                    var messages = await ctx.Channel.GetMessagesAsync(1);

                    if (messages.Count > 0)
                    {
                        await ctx.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);
                    }

                    await ctx.Channel.SendMessageAsync($"{everyoneTag}  New Code released. You should claim it, before it expires;D\n\nExpected Date until {aviableRedeemDate}\nYour code-> {code}");
                }
            }
        }
    }
}
