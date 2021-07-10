using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class CMDs : BaseCommandModule
    {
        private const string discordLibary = "https://docs.stillu.cc/api/Discord.EmbedFieldBuilder.html";
        private const long WOLFI_ID = 323381699675422724;
        private const ulong BOT_ID = 782960871890354186;
        private const string EVERYONE_TAG = "@everyone";
        private const string ADMIN_ROLE_NAME = "admin";
        private const string WOLFI_BOT_PICUTRE = "https://i.imgur.com/3h5CSAf.png";
        private const string EMBED_FOOTER = "Made by Wolfi";
        private const string WOLFI_IMAGE = "https://cdn.discordapp.com/avatars/782960871890354186/39dca5073422e3ace00c9f81c07ccf07.png?size=1024";//"https://cdn.discordapp.com/avatars/323381699675422724/1a36390053d0e2a0e50a000f8c4afb27.png?size=1024";
        private const string BOT_CREATED_TIME = "03.12.2020 19:22:59";
        private const string BOT_UPDATE_TIME = "15.03.2021 23:28:55";
        private const string COMMANDS = ";Login\n;CC\n;MyUserInfo\n;SendCode\n;Info\n;Oracle\n;Help\n;Guide\n;TierList\n;WP\n;cook";
        private const string PATCH_NOTES = "->;cook hinzugefügt der dir ein Gericht Zaubert. \n-> Versch. Bugfixes an (CC,WP,Oracle,SendCode)";//-> MusikBot Command hinzugefügt ->;play [your song]<-\n

        private List<ulong> activeSearchingIds = new List<ulong>();
        private Equipment equipment = Equipment.GetInstance;
        private readonly TimeSpan MaxDaysDeleteMessage = TimeSpan.FromDays(13);
        /// <summary>
        /// Die Variable darf nur zum darstellen des aktuellen löschen Zustandes benutzt werden.
        /// </summary>
        private bool deleteUnderWorking = false;

        private bool CanUseFilter(ulong idToFilter)
        {
            foreach (var id in activeSearchingIds)
            {
                if (id == idToFilter)
                    return false;
            }

            return true;
        }

        private async Task SendEmbed(CommandContext ctx, string embedTitle = "Titel der Einbettung ", string nameField = "Kategorie Titel", string valueField = "Inhalt")
        {

            var embed = new DiscordEmbedBuilder
            {
                Title = embedTitle,
                Color = DiscordColor.CornflowerBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
            };
            embed.Footer.Text = EMBED_FOOTER;
            embed.Footer.IconUrl = WOLFI_IMAGE;
            embed.AddField(nameField, valueField);//mehrere addfields sind möglich!

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        #region TROLLING
        [Command("troll")]
        private async Task SendTrollingGlobSendTrollingalMessage(CommandContext ctx, string code)
        {
            foreach (var item in ctx.Member.Roles)
            {
                if (item.Name == ADMIN_ROLE_NAME)
                {
                    var messages = await ctx.Channel.GetMessagesAsync(1);

                    if (messages.Count > 0)
                    {
                        await ctx.Channel.DeleteMessagesAsync(messages).ConfigureAwait(false);

                        for (int i = 0; i < 30; i++)
                        {
                            await ctx.Channel.SendMessageAsync($"{code} ich troll dich xD!");
                        }

                        await ctx.Channel.SendMessageAsync($"{code} ich bin fertig mit trollen geh kekse backen ");
                    }

                }
            }
        }

        #endregion

        #region stuff
        [Command("Login")]
        [Description("Secret command from wolfi :P")]
        public async Task LoginCommand(CommandContext ctx)
        {
            string toSendMessage;

            if (ctx.Member.Id == WOLFI_ID)
            {
                toSendMessage = $"Login versuch des Bots [{DateTime.Now.ToString()}]";
            }
            else
            {
                toSendMessage = "Du bist nicht der Programmierer.\nNur der Programmierer kann diesen Befehl verwenden";
            }
            await ctx.Channel.SendMessageAsync(toSendMessage).ConfigureAwait(false);
        }

        [Command("CC")]
        [Description("Secret command from wolfi :P")]
        public async Task DeleteCommand(CommandContext ctx, int amount)
        {
            TimeSpan valueOfDays = new TimeSpan();
            List<DiscordMessage> toDeleteMessages;
            bool showError = false;
            try
            {

                if (ctx.Member.Id == WOLFI_ID)
                {
                    if (!this.deleteUnderWorking)
                    {
                        if (amount > 0)
                        {
                            var messages = await ctx.Channel.GetMessagesAsync(amount);

                            if (amount <= messages.Count)
                            {
                                this.deleteUnderWorking = true;
                                toDeleteMessages = new List<DiscordMessage>();

                                foreach (var item in messages)
                                {
                                    valueOfDays = DateTime.Now - item.CreationTimestamp.DateTime;

                                    if (valueOfDays.Days <= MaxDaysDeleteMessage.Days)
                                    {
                                        toDeleteMessages.Add(item);
                                        Debug.WriteLine(item.CreationTimestamp.DateTime);
                                    }
                                    else
                                        showError = true;
                                }

                                await ctx.Channel.DeleteMessagesAsync(toDeleteMessages);
                                await ctx.Channel.SendMessageAsync($"Es wurden erfolgreich {toDeleteMessages.Count} gelöscht.").ConfigureAwait(false);

                                if (showError)
                                {
                                    await ctx.Channel.SendMessageAsync($"Es können keine Nachrichten gelöscht werden, die älter als 13 Tage sind.").ConfigureAwait(false);
                                }
                            }
                            else
                            {
                                await ctx.Channel.SendMessageAsync($"es können maximal [{messages.Count + 2}] Nachrichten gelöscht werden").ConfigureAwait(false);
                            }
                        }
                        this.deleteUnderWorking = false;
                    }
                    else
                        Debug.WriteLine("DELETE UNDER WORKING");
                }
                else
                    await ctx.Channel.SendMessageAsync("Du bist nicht der Programmierer.\nNur der Programmierer kann diesen Befehl verwenden").ConfigureAwait(false);
            }
            catch
            {
                await ctx.Channel.SendMessageAsync("Ein Fehler beim löschen ist aufgetreten.").ConfigureAwait(false);
                this.deleteUnderWorking = false;
            }

        }

        [Command("MyUserInfo")]
        [Description("Gibt ein parr Informationen zum eigenen Account")]
        public async Task GiveIdCommand(CommandContext ctx)
        {
            string toSendMessage = $"-> {ctx.Member.DisplayName} deine Id lautet {ctx.Member.Id.ToString()}\n";
            toSendMessage += $"-> Die ID deines avatars lautet {ctx.Member.AvatarUrl}\n";
            toSendMessage += $"-> Du hast den Server am {ctx.Member.JoinedAt} betreten\n";


            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.Color = DiscordColor.CornflowerBlue;
            embed.WithAuthor(name: ctx.Member.DisplayName, url: ctx.Member.AvatarUrl, iconUrl: ctx.Member.AvatarUrl);
            embed.AddField(name: "Deine Informationen", value: toSendMessage, inline: false);
            embed.Timestamp = DateTimeOffset.Now;
            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("Info")]
        [Description("Gibt die Bot Information")]
        private async Task ShowBotInformations(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: ctx.Member.DisplayName, url: WOLFI_IMAGE, iconUrl: WOLFI_IMAGE);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = $"Informationen zum Bot";
            embed.Description = "Dieser Bot ist eine eigen Entwicklung.\nBitte geben Sie den Command [;help] ein um eine Auflistung und[;help [Comand]] um eine Beschreibung der Commands zu erhalten";
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = WOLFI_BOT_PICUTRE };
            embed.AddField(name: "Auflistung der Commands", value: COMMANDS);
            embed.AddField(name: "\u200B", value: "\u200B");
            embed.AddField(name: "Erstellt", value: BOT_CREATED_TIME, inline: true);
            embed.AddField(name: "Mitwirkende", value: "--Keine--", inline: true);
            embed.AddField(name: "Letzes Update", value: BOT_UPDATE_TIME, inline: false);
            embed.AddField(name: "Update Notes", value: PATCH_NOTES, inline: false);
            embed.ImageUrl = WOLFI_BOT_PICUTRE;
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = WOLFI_IMAGE;
            embed.Footer.Text = "WolfiBot Informationen";

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("cook")]
        [Description("Gibt dir ein Rezept für ein Mittagessen.")]
        private async Task ShowMeal(CommandContext ctx)
        {
            KochBuch kochBuch = new KochBuch();
            var listedResult = kochBuch.GetRezepts(1);

            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: ctx.Member.DisplayName, url: WOLFI_IMAGE, iconUrl: WOLFI_IMAGE);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = $"Wolfis Bistro";
            embed.Description = "Hier wird deine Bestellung angezeigt";
            embed.AddField(name: $"Name: \"{listedResult[0].Name}\"", value: $"Zutaten: \"{listedResult[0].Content}\"");

            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = ctx.Member.AvatarUrl;
            embed.Footer.Text = $"{ctx.Member.DisplayName} deine Bestellung wurde fertig gestellt";

            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        #endregion

        #region games
        [Command("Oracle")]
        [Description("Befrage das Orakel nach typischen Ja und Nein Fragen und du wirst eine erleuchtende Antwort erhalten.#")]
        private async Task SendOracleMessage(CommandContext ctx, params string[] question)
        {
            EigthBall eigthBall = new EigthBall();

            await eigthBall.OnSendEigthBall(ctx, question, EMBED_FOOTER, WOLFI_IMAGE);

        }

        [Command("WP")]
        [Description("Starte Wahrheit oder Pflicht Spiel")]
        public async Task StartWPGame(CommandContext ctx)
        {
            WahrheitOderPflicht wahrheitOderPflicht = new WahrheitOderPflicht();
            await wahrheitOderPflicht.OnWPGameStart(ctx);
        }

        #endregion

        #region shining beyond
        [Command("SendCode")]
        [Description("[sendCode] [aviableRedeemDate] [code] -> Sendet eine Nachricht mit einem @everyone das ein neuer Code exestiert")]// und erstellt einen passenden privaten Sprachchannel")]
        private async Task SendGlobalMessage(CommandContext ctx, string aviableRedeemDate, string code)
        {
            var shiningBeyond = new ShiningBeyond();
            await shiningBeyond.OnSendCode(ctx, aviableRedeemDate, code,  EVERYONE_TAG, ADMIN_ROLE_NAME);
        }

        [Command("TierList")]
        [Description("Stellt die auf Basis von Spieler erhaltende Tier Listen als eine art Hilfe da.")]
        private async Task ShowTierList(CommandContext ctx)
        {
            var shiningBeyond = new ShiningBeyond();
            await shiningBeyond.OnSendTierList(ctx, WOLFI_IMAGE);
        }

        [Command("Guide")]
        [Description("Sendet ein Guide für Shining Beyond starter.")]
        private async Task ShowShiningBeyondGuide(CommandContext ctx)
        {
            var shiningBeyond = new ShiningBeyond();
            await shiningBeyond.OnSendBotInfomrations(ctx, WOLFI_IMAGE);
        }

        #endregion

    }
}