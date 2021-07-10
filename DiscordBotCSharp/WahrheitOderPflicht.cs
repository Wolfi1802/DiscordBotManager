using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class WahrheitOderPflicht
    {
        private const string LEAVE_COMMAND = "leave";
        private const string ABORT_COMMAND = "cancel";
        private const string ADD_COMMAND = "apply";
        private const string START_GAME_COMMAND = "start";

        List<string> userWhoPlay;
        List<ulong> userWhoPlayID;
        public WahrheitOderPflicht(List<string> userWhoPlayName, List<ulong> userWhoPlayID)
        {
            this.userWhoPlay = userWhoPlayName;
            this.userWhoPlayID = userWhoPlayID;
        }

        public WahrheitOderPflicht()
        {
        }

        private ulong wpGameStartId;
        private bool wpGameHasStarted = false;

        public async Task OnWPGameStart(CommandContext ctx)
        {

            {
                ulong? startInteractivityUserId = null;
                List<ulong> userWhoApply = new List<ulong>();
                List<string> userWhoApplyName = new List<string>();

                if (startInteractivityUserId == null)
                {
                    startInteractivityUserId = ctx.Member.Id;
                    userWhoApply.Add(ctx.Member.Id);
                    userWhoApplyName.Add(ctx.Member.DisplayName);
                }

                if (!this.wpGameHasStarted)
                {
                    this.wpGameHasStarted = true;
                    var interactivity = ctx.Client.GetInteractivity();
                    await ctx.Channel.SendMessageAsync($"[{ctx.Member.DisplayName}] du hast ein Spiel gestartet, nun können Spieler teilnehmen in dem Sie [APPLY] eingeben.\nWenn alle Spieler rdy sind, muss der Ersteller des Spiels [START] eingeben.\nUm das Spiel zu verlassen tippe bitte [CANCEL]");

                    try
                    {
                        do
                        {
                            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
                            if (message.Result != null)
                            {
                                string userName = $"<@!{message.Result.Author.Id}>";

                                if (!message.Result.Author.IsBot)
                                {
                                    if (message.Result.Content.ToLower() == ABORT_COMMAND)
                                    {
                                        if (DiscordBotCSharp.Helper.Helper.CheckGameList(message.Result.Author.Id, userWhoApply))
                                        {
                                            userWhoApply.Remove(message.Result.Author.Id);
                                            userWhoApplyName.Remove(message.Result.Author.Username);
                                            await ctx.Channel.SendMessageAsync($"{userName} du hast das Spiel verlassen");
                                        }
                                        else
                                            Debug.WriteLine("\n\nuser hat schon verlassen\n\n");
                                    }
                                    else if (message.Result.Content.ToLower() == ADD_COMMAND)
                                    {
                                        if (!DiscordBotCSharp.Helper.Helper.CheckGameList(message.Result.Author.Id, userWhoApply))
                                        {
                                            userWhoApply.Add(message.Result.Author.Id);
                                            userWhoApplyName.Add(message.Result.Author.Username);
                                            await ctx.Channel.SendMessageAsync($"{userName} du hast dich für das Spiel angemeldet");
                                        }
                                        else
                                        {
                                            //await ctx.Channel.SendMessageAsync($"{userName} du hast dich schon für das Spiel angemeldet");//muss raus weil sonst gibts rate limit error
                                            Debug.WriteLine($"\n\n[{userName}] hat schon applyed\n\n");
                                        }
                                    }
                                    else if (message.Result.Content.ToLower() == START_GAME_COMMAND && message.Result.Author.Id == startInteractivityUserId)
                                    {
                                        await ctx.Channel.SendMessageAsync($"{userName} ES GEHT LOS !!!");
                                        WahrheitOderPflicht WOP = new WahrheitOderPflicht(userWhoApplyName, userWhoApply);

                                        WOP.StartGame(ctx);

                                        this.wpGameHasStarted = false;
                                    }
                                    //}
                                    //else
                                    //    Debug.WriteLine($"{nameof(CMDs)},{nameof(ResponseMultipleUsers)}Nachricht kommt von einem Bot\n");
                                }
                                else
                                    Debug.WriteLine($"{nameof(CMDs)},{nameof(OnWPGameStart)}Ein anderer Nutzer als der, der den Task gestartet hat will diesen nutzen!\n");
                            }
                            else
                                Debug.WriteLine($"{nameof(CMDs)},{nameof(OnWPGameStart)}[message.result] ist null\n");
                        }
                        while (this.wpGameHasStarted);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{nameof(CMDs)}{nameof(OnWPGameStart)} interaktionszeit ist um->nullref" + ex);
                        await ctx.Channel.SendMessageAsync($"EXCEPTION-> {ex}");
                    }
                }
                else
                {
                    Debug.WriteLine($"[{ctx.Member.DisplayName}] kann kein Game starten, da schon eines am laufen ist.");
                }
            }
        }


        public async void StartGame(CommandContext ctx)
        {

            this.ShowGameStartMessage(ctx, this.userWhoPlay);//TODO die anzeige der spieler in einem embed anzeigen
            //nimm die liste als reihenfolge

            do
            {
                var interactivity = ctx.Client.GetInteractivity();
                var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

                if (message.Result != null)
                {
                    if (CheckLeaveCommand(message.Result.Channel.LastMessageId.ToString()))
                    {
                        //delete user from userlist from this game!
                       
                    }
                    else
                    {
                        //await ctx.Channel.SendMessageAsync($"leave akzeptiert von [{ctx.Message.Author.Username}]");
                        Debug.WriteLine("PERMA SPAM!");
                        ulong idFromUser = this.userWhoPlayID[0];
                        await ctx.Channel.SendMessageAsync($"Spieler <@!{idFromUser}> ist dran. Bitte wähle Wahrheit oder Pflicht.");
                    }
                }

            }
            while (userWhoPlay.Count != 0);

            await ctx.Channel.SendMessageAsync($"Spiel beendet!");
        }

        /// <summary>
        /// Zeigt welche spieler sich angemeldet haben.
        /// </summary>
        /// <param name="ctx"></param>
        private async void ShowGameStartMessage(CommandContext ctx, List<string> userWhoPlay)
        {
            string user = string.Empty;
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.Title = $"Teilnehmer Liste";
            embed.Description = $"Es folgen alle Teilnehmer des Spiels";

            foreach (string names in userWhoPlay)
            {
                user += $"{names}\n";
            }
            try
            {
                embed.AddField(name: "Teilnehmer", value: user);
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{nameof(WahrheitOderPflicht)},{nameof(ShowGameStartMessage)},{ex}");
            }
        }

        /// <summary>
        /// Wenn Spieler mit dem Spiel aufhören wollen können sie mit "leave" das Spiel verlassen.Dies wird wird hier überprüft ob der content dem Verlassen entspricht.
        /// </summary>
        private bool CheckLeaveCommand(string message)
        {//TODO fertig machen sonst gibts stress
            Debug.WriteLine(message);
            var messageToCheck = string.Empty;

            if (messageToCheck == LEAVE_COMMAND)
            {
                return true;
            }

            return false;
        }

    }
}
