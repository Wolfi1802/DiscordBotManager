using DiscordBotCSharp.Games.TicTacTo;
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
        private const string DISCORDLIB = "https://docs.stillu.cc/api/Discord.EmbedFieldBuilder.html";
        private const string EVERYONE_TAG = "@everyone";
        private const string WOLFI_BOT_PICUTRE = "https://i.imgur.com/3h5CSAf.png";
        private const string EMBED_FOOTER = "Made by Wolfi";
        private const string WOLFI_IMAGE = "https://cdn.discordapp.com/avatars/323381699675422724/865457f5c8253fee583bbb916d827af1.png?size=1024";
        private const string BOT_CREATED_TIME = "03.12.2020 19:22:59";
        private const string BOT_UPDATE_TIME = "20.11.2021 23:58:55";
        private const string COMMANDS = ";MyUserInfo\n;Info\n;TTT\nOracle\nAddOracle";
        private const string PATCH_NOTES = "TicTacTo now aviable type->  ;ttt\nOrakel ausgeben ->Oracle\nOrakel hinzufügen-> AddOracle";

        private readonly TimeSpan MaxDaysDeleteMessage = TimeSpan.FromDays(13);

        /// <summary>
        /// Die Variable darf nur zum darstellen des aktuellen löschen Zustandes benutzt werden.
        /// </summary>
        private bool deleteUnderWorking = false;

        #region GlobalStuff

        [Command("Delete")]
        [Description("Secret command from wolfi :P")]
        public async Task DeleteChannelMessages(CommandContext ctx, int amount)
        {
            TimeSpan valueOfDays = new TimeSpan();
            List<DiscordMessage> toDeleteMessages;
            bool showError = false;
            try
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
            catch
            {
                await ctx.Channel.SendMessageAsync("Ein Fehler beim löschen ist aufgetreten.").ConfigureAwait(false);
                this.deleteUnderWorking = false;
            }

        }

        [Command("UserInfo")]
        [Description("Stellt die Nutzer Informationen zur Verfügung")]
        public async Task GiveUserId(CommandContext ctx)
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
        [Description("Stellt die Bot Informationen zur Verfügung")]
        private async Task ShowBotInformations(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: ctx.Member.DisplayName, url: WOLFI_IMAGE, iconUrl: WOLFI_IMAGE);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = $"Informationen zum Bot";
            embed.Description = "Dieser Bot ist eine eigen Entwicklung.\nBitte geben Sie den Command [;help] ein um eine Auflistung und[;help [Comand]] um eine Beschreibung der Commands zu erhalten";
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = WOLFI_BOT_PICUTRE };
            embed.AddField(name: "Auflistung der Commands", value: COMMANDS);
            embed.AddField(name: "Erstellt", value: BOT_CREATED_TIME, inline: true);
            embed.AddField(name: "Mitwirkende", value: "--Keine--", inline: true);
            embed.AddField(name: "Letzes Update", value: BOT_UPDATE_TIME, inline: false);
            embed.AddField(name: "Update Notes", value: PATCH_NOTES, inline: false);
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = WOLFI_BOT_PICUTRE;
            embed.Footer.Text = "WolfiBot Informationen";

            await ctx.Channel.SendMessageAsync(embed: embed);
        }


        #endregion

        #region Games
        [Command("TTT")]
        [Description("Eröffnet das Klassische Spiel TicTacTo")]
        private async Task StartTicTacTo(CommandContext ctx)
        {
            TicTacToEntry gameStart = new TicTacToEntry(ctx.Member.Id);
            gameStart.StartGame(ctx);

        }

        #endregion
    }
}