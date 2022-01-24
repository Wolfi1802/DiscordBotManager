using DiscordBotCSharp.Games.TicTacTo;
using DiscordBotCSharp.MessageDesigns;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class CMDs : BaseCommandModule
    {

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

            if (323381699675422724 != ctx.Message.Author.Id)
                return;

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
                                    toDeleteMessages.Add(item);
                                else
                                    showError = true;
                            }

                            await ctx.Channel.DeleteMessagesAsync(toDeleteMessages);
                            await ctx.Channel.SendMessageAsync($"{TextFragments.SUCCESSFUL} {toDeleteMessages.Count} {TextFragments.DELETED}").ConfigureAwait(false);

                            if (showError)
                            {
                                await ctx.Channel.SendMessageAsync(TextFragments.RIP_DELETE).ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            await ctx.Channel.SendMessageAsync($"{TextFragments.AVIABLE_DELETE_MESS} {messages.Count + 2} {TextFragments.MESSAGE}").ConfigureAwait(false);
                        }
                    }
                    this.deleteUnderWorking = false;
                }
                else
                    Debug.WriteLine("DELETE UNDER WORKING");
            }
            catch
            {
                await ctx.Channel.SendMessageAsync(TextFragments.DELETE_ERROR).ConfigureAwait(false);
                this.deleteUnderWorking = false;
            }

        }

        [Command("UserInfo")]
        [Description("Show your personal Informations")]
        public async Task GiveUserInformations(CommandContext ctx)
        {
            string toSendMessage = $"{ctx.Member.Mention}{TextFragments.YOUR_ID}{ctx.Member.Id}\n";
            toSendMessage += $"{TextFragments.SERVER_JOIN}{ctx.Member.JoinedAt.UtcDateTime}\n";

            var embed = new DesignFactory().GetProfileEmbed(ctx, (TextFragments.PERSONAL_INFOS, toSendMessage, false));

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("Info")]
        [Description("Show all informations about this Bos")]
        private async Task ShowBotInformations(CommandContext ctx)
        {
            List<(string name, string value, bool inline)> field = new List<(string name, string value, bool inline)>();
            field.Add((TextFragments.LIST_COMMANDS, TextFragments.COMMANDS, false));
            field.Add((TextFragments.CREATED, TextFragments.BOT_CREATED_TIME, true));
            field.Add((TextFragments.HELPER, TextFragments.NOBODY, true));
            field.Add((TextFragments.LAST_UPDATE, TextFragments.BOT_UPDATE_TIME, false));
            field.Add((TextFragments.UPDATE_NOTES, TextFragments.PATCH_NOTES, false));

            var embed = new DesignFactory().GetEmbed(TextFragments.BOT_INFO,
                TextFragments.BOT_DESC, field);

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("GetGit")]
        [Description("Get project link")]
        public async Task GetGitCode(CommandContext ctx)
        {
            var embed = new DesignFactory().GetEmbed((TextFragments.PROJECT_NAME, TextFragments.PROJECT_LINK, false), TextFragments.BOT_INFO,
                TextFragments.BOT_DESC);

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }


        #endregion

        #region Games
        [Command("TTT")]
        [Description("Start TicTacTo (only German Language atm)")]
        private async Task StartTicTacTo(CommandContext ctx)
        {
            TicTacToEntry gameStart = new TicTacToEntry(ctx.Member.Id);
            gameStart.StartGame(ctx);

        }

        #endregion
    }
}