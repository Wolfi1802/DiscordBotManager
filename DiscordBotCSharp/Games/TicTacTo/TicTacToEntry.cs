using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordBotCSharp.Games.TicTacTo
{
    public class TicTacToEntry
    {

        public readonly string TTT_VERSION = $"TicTacTo Version 1.0";//technologie erlaubt keine Konstanten
         public readonly string GAME_REGISTRY = $"You asign for the game TicTacTo.\nWrite your Char in row and column formatter.\nYou see on bottom the formatter.\nIf its your turn you have to write something like 3,1.\nYou start with \"X\".\nHAVE FUN, game starts soon...";//1

        public readonly string GAME_START = $"Game start, you begin";//2
        public readonly string ERROR_PLACE_FIELD = $"This Field is set, you cant set it by yourself";
        public readonly string WINNER_TEXT = $"You won!";
        public readonly string LOSING_KI_TEXT = $"You lose vs Ki!";
        public readonly string DRAW_KI_TEXT = $"Nobody won, DRAW!";

        private readonly string X = "X";
        private readonly string O = "O";
        private readonly string FIELD_ICON = "☆";
        private readonly int MS = 100;
        private List<List<string>> gameField = null;
        private bool gameIsRunning = false;
        private ulong id;
        private DiscordMessage msg;

        public TicTacToEntry(ulong id)
        {
            this.gameField = this.CreateField();
            this.id = id;
        }

        public void StartGame(CommandContext ctx)
        {
            this.gameIsRunning = true;
            System.Diagnostics.Debug.WriteLine(ctx.Member.Mention);//aktuelle id

            this.ShowMessage(ctx, GAME_REGISTRY);
            Thread.Sleep(1000);
            this.ShowMessage(ctx, $"{GAME_START}\n\n{this.GetFieldAsString()}");
            Thread.Sleep(this.MS);
            this.Interact(ctx);
        }

        async private void Interact(CommandContext ctx)
        {
            ctx.Client.MessageCreated += OnMesssageCreated;

            //TODO[TS]ttt 4  multiplayer

            do
            {
                if (this.msg != null && !string.IsNullOrEmpty(this.msg.Content) && this.id == this.msg.Author.Id)
                {
                    try
                    {
                        if (msg.Content.Length >= 3)//TODO[TS] ttt auslagern 
                        {

                            bool fieldSetSuccessfull = this.SetFieldBy(this.msg.Content);//player set Field

                            if (!this.CanSomeoneWin()&& this.CheckWinner()!=WinnerType.Player)
                            {
                                this.gameIsRunning = false;
                                this.ShowMessage(ctx, $"{DRAW_KI_TEXT}");
                            }
                            else
                            {
                                if (fieldSetSuccessfull)
                                {
                                    this.SetFieldByKi(this.X);//Ki set Field,need player Icon to find

                                    if (this.gameIsRunning)
                                        this.ShowMessage(ctx, this.GetFieldAsString());
                                    this.msg = null;

                                    var winnerResult = this.CheckWinner();

                                    if (winnerResult.Equals(WinnerType.Player))
                                    {
                                        Thread.Sleep(this.MS);
                                        this.gameIsRunning = false;
                                        this.ShowMessage(ctx, $"{WINNER_TEXT}");
                                    }
                                    else if (winnerResult.Equals(WinnerType.KI))
                                    {
                                        this.ShowMessage(ctx, $"{LOSING_KI_TEXT}");
                                    }

                                }
                                else
                                {
                                    this.ShowMessage(ctx, $"{ERROR_PLACE_FIELD}\n{this.GetFieldAsString()}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        this.msg = null;
                    }
                }

                Thread.Sleep(2000);
            } while (this.gameIsRunning);

            ctx.Client.MessageCreated -= OnMesssageCreated;
        }

        private bool CanSomeoneWin()
        {
            int fieldCounter = 0;
            foreach (var item in this.gameField)
            {
                foreach (var pos in item)
                {
                    if (pos.Equals(FIELD_ICON))
                        fieldCounter++;
                }
            }

            if (fieldCounter <= 0)
                return false;
            return true;
        }

        private WinnerType CheckWinner()
        {
            if (this.CheckLines(this.X))
                return WinnerType.Player;
            else if (this.CheckLines(this.O))
                return WinnerType.KI;
            else
                return WinnerType.None;
        }

        private bool CheckLines(string ToFind)
        {
            for (int i = 0; i < this.gameField.Count; i++)
            {
                if (this.gameField[0][i].Equals(ToFind) && this.gameField[1][i].Equals(ToFind) && this.gameField[2][i].Equals(ToFind))
                    return true;
            }//alle horizontalen lines done

            for (int j = 0; j < this.gameField.Count; j++)
            {
                if (this.gameField[j][0].Equals(ToFind) && this.gameField[j][1].Equals(ToFind) && this.gameField[j][2].Equals(ToFind))
                    return true;
            }//alle vertikalen lines done


            return false;
        }

        private bool SetFieldBy(string fieldToSet)
        {
            var places = fieldToSet.Split(',');
            int y = int.Parse(places[0]) - 1;
            int x = int.Parse(places[1]) - 1;

            if (this.gameField[y][x].Equals(this.X) || this.gameField[y][x].Equals(this.O))
                return false;

            this.gameField[y][x] = this.X;

            return true;
        }

        private void SetFieldByKi(string ToFind)
        {
            string kiValue = null;
            //bool setByKiDone = false;

            if (ToFind.Equals(this.O))
                kiValue = this.X;
            else
                kiValue = this.O;

            List<Tuple<int, int>> toSetPos = new List<Tuple<int, int>>();

            for (int i = 0; i < this.gameField.Count; i++)
            {
                if (this.gameField[i][0].Equals(ToFind) && this.gameField[i][1].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 2));
                if (this.gameField[i][1].Equals(ToFind) && this.gameField[i][2].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 0));
                if (this.gameField[i][0].Equals(ToFind) && this.gameField[i][2].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 1));
            }//alle horizontalen lines done

            for (int j = 0; j < this.gameField.Count; j++)
            {
                if (this.gameField[0][j].Equals(ToFind) && this.gameField[1][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(2, j));
                if (this.gameField[1][j].Equals(ToFind) && this.gameField[2][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(0, j));
                if (this.gameField[0][j].Equals(ToFind) && this.gameField[2][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(1, j));
            }//alle vertikalen lines done

            if (this.SetWinningKiTurnIfAviable(kiValue))
            {
                return;
            }

            foreach (var pos in toSetPos)
            {
                if (!this.gameField[pos.Item1][pos.Item2].Equals(kiValue))//pos ist frei und kann gesetzt werden
                {
                    this.gameField[pos.Item1][pos.Item2] = kiValue;
                    return;
                }
            }

            this.SetRandomKiPoint(kiValue);

        }

        private bool SetWinningKiTurnIfAviable(string ToFind)
        {
            List<Tuple<int, int>> toSetPos = new List<Tuple<int, int>>();

            for (int i = 0; i < this.gameField.Count; i++)
            {
                if (this.gameField[i][0].Equals(ToFind) && this.gameField[i][1].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 2));
                if (this.gameField[i][1].Equals(ToFind) && this.gameField[i][2].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 0));
                if (this.gameField[i][0].Equals(ToFind) && this.gameField[i][2].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(i, 1));
            }//alle horizontalen lines done

            for (int j = 0; j < this.gameField.Count; j++)
            {
                if (this.gameField[0][j].Equals(ToFind) && this.gameField[1][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(2, j));
                if (this.gameField[1][j].Equals(ToFind) && this.gameField[2][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(0, j));
                if (this.gameField[0][j].Equals(ToFind) && this.gameField[2][j].Equals(ToFind))
                    toSetPos.Add(new Tuple<int, int>(1, j));
            }//alle vertikalen lines done

            foreach (var pos in toSetPos)
            {
                if (this.gameField[pos.Item1][pos.Item2].Equals(FIELD_ICON))//pos ist frei und kann gesetzt werden
                {
                    this.gameField[pos.Item1][pos.Item2] = ToFind;
                    return true;
                }
            }

            return false;
        }

        private void SetRandomKiPoint(string kiValueToSet)
        {
            Random rand = new Random();
            int x = 0;
            int y = 0;

            do
            {
                x = rand.Next(0, 3);
                y = rand.Next(0, 3);

            } while (!this.gameField[x][y].Equals(FIELD_ICON));
            System.Diagnostics.Debug.WriteLine("\n\n" + this.gameField[x][y] + $"|  {x},{y} random");
            this.gameField[x][y] = kiValueToSet;
        }

        private System.Threading.Tasks.Task OnMesssageCreated(DSharpPlus.DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            this.msg = e.Message;

            return null;
        }

        async public void ShowMessage(CommandContext ctx, string message)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Title = TTT_VERSION;
            embed.Description = $"{ctx.Member.Mention}\n{message}";
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        private string GetFieldAsString()
        {
            return $"{this.gameField[0][0]}{this.gameField[0][1]}{this.gameField[0][2]}\n" +
                $"{this.gameField[1][0]}{this.gameField[1][1]}{this.gameField[1][2]}\n" +
                $"{this.gameField[2][0]}{this.gameField[2][1]}{this.gameField[2][2]}\n\n"+
                $"1,1   |   1,2 |   1,3\n" +
                $"2,1   |   2,2 |   2,3\n" +
                $"3,1   |   3,2 |   3,3\n";
        }

        private List<List<string>> CreateField()
        {
            List<List<string>> field = new List<List<string>>();
            field.Add(this.CreateXField());
            field.Add(this.CreateXField());
            field.Add(this.CreateXField());

            return field;
        }

        private List<string> CreateXField()
        {
            List<string> xLine = new List<string>();
            xLine.Add(FIELD_ICON);
            xLine.Add(FIELD_ICON);
            xLine.Add(FIELD_ICON);

            return xLine;
        }

    }
}
