using DiscordBotCSharp.Games.TicTacTo;
using DiscordBotCSharp.MessageDesigns;
using DiscordBotCSharp.ShiningBeyondAnalytic;
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

        #region Shining Beyong 
        [Command("SbInfo")]
        [Description("Get all Infos About Sb commands")]
        public async Task ShowShiningBeyongInfos(CommandContext ctx)
        {
            var embed = new DesignFactory().GetEmbed((TextFragments.SB_SHOW, TextFragments.SB_SHOW_DESC, false), TextFragments.SB_TEASER_TITLE,
                TextFragments.SB_TEASER_DESCRIPTION);

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("SbHero")]
        [Description("Get specific hero Datas")]
        public async Task ShowShiningBeyondHeroInfo(CommandContext ctx, params string[] msg)
        {
            try
            {
                ShiningBeyondManager.Instance.ShowHeroData(ctx, msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

        }

        [Command("SbAdd")]
        [Description("Add Hero to DataBase")]
        public async Task AddHeroToDataBase(CommandContext ctx, params string[] msg)
        {
            var embed2 = new DesignFactory().GetEmbed("Not Ready", "Current Command isnt ready!");
            await ctx.Channel.SendMessageAsync(embed: embed2).ConfigureAwait(false);
            return;

            try
            {//TODO add not rdy bcs missin parse!!!
                DiscordEmbedBuilder embed = null;
                DiscordEmbedBuilder heroEmbed = null;

                if (ShiningBeyondManager.Instance.AddtoDataBase(this.GetParsedModel(msg)))
                {
                    embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_S, TextFragments.SB_DB_ADD_S, setAuthor: false);
                    //heroEmbed = new DesignFactory().GetShindingBeyondHeroEmbed(ShiningBeyondManager.Instance.GetTESTDATA());
                }
                else
                    embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_E, TextFragments.SB_DB_ADD_E, setAuthor: false);

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                //await ctx.Channel.SendMessageAsync(embed: heroEmbed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        [Command("SbRemove")]
        [Description("Remove Hero From Database")]
        public async Task RemoveHeroToDataBase(CommandContext ctx, string id)
        {
            try
            {
                DiscordEmbedBuilder embed = null;
                DiscordEmbedBuilder heroEmbed = null;
                HeroModel foundedModel = ShiningBeyondManager.Instance.GetModelBy(int.Parse(id));

                if (foundedModel != null)
                {
                    if (ShiningBeyondManager.Instance.RemoveFromDataBase(foundedModel))
                    {
                        embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_S, TextFragments.SB_DB_REMOVE_S, setAuthor: false);
                        heroEmbed = new DesignFactory().GetShindingBeyondHeroEmbed(foundedModel);
                    }
                    else
                        embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_E, TextFragments.SB_DB_REMOVE_E, setAuthor: false);

                    await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                    await ctx.Channel.SendMessageAsync(embed: heroEmbed).ConfigureAwait(false);
                }
                else
                {
                    var errorEmbed = new DesignFactory().GetEmbed("Not Found", $"Current Id [{id}] not found");
                    await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        [Command("Sbaddtest")]
        [Description("add test data to db")]
        public async Task AddTestDataToDb(CommandContext ctx)
        {
            try
            {
                DiscordEmbedBuilder embed = null;
                DiscordEmbedBuilder heroEmbed = null;

                if (ShiningBeyondManager.Instance.AddtoDataBase(ShiningBeyondManager.Instance.GetTESTDATA()))
                {
                    embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_S, TextFragments.SB_DB_ADD_S, setAuthor: false);
                    heroEmbed = new DesignFactory().GetShindingBeyondHeroEmbed(ShiningBeyondManager.Instance.GetTESTDATA());
                }
                else
                    embed = new DesignFactory().GetEmbed(TextFragments.SB_DB_SHOW_E, TextFragments.SB_DB_ADD_E, setAuthor: false);

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: heroEmbed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private HeroModel GetParsedModel(string[] msg)
        {
            return new HeroModel();
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