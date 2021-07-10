using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCSharp
{
    public class EigthBall
    {
        private const int MAX_RANDOM_VALUE = 13;
        private Random rand;
        

        public EigthBall()
        {
            this.rand = new Random();
        }

        public async Task OnSendEigthBall(CommandContext ctx, string[] question, string EMBED_FOOTER, string WOLFI_IMAGE)
        {
            string input = string.Empty;
            string oracelAnswer = this.GetMessage();

            foreach (var item in question)
            {
                input += item + " ";
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = "Oracle",
                Color = DiscordColor.CornflowerBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
            };
            embed.Footer.Text = EMBED_FOOTER;
            embed.Footer.IconUrl = WOLFI_IMAGE;
            embed.AddField("Deine Frage lautet", input);
            embed.AddField("Hier nun deine Antwort", oracelAnswer);

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        public string GetMessage()
        { 
            string randomGeneratedMessage = string.Empty;
            randomGeneratedMessage = this.GetMessageByRandomNumber();


            return randomGeneratedMessage;
        }

        private string GetMessageByRandomNumber()
        {
            string message = string.Empty;
            

            switch (this.GetRandomGeneratedNumber())
            {
                case 1:
                    message = "Ich habe ich die Tiefen meiner KristallKugel reine Dunkelheit entdeckt, was ein schlechtes Ohmen ist.\nDeswegen eher weniger.";
                    break;
                case 2:
                    message = "Die Macht der Seife hat mir die Antwort verraten.\nGlaube an die Macht der Seife und du findest deinen Weg.";
                    break;
                case 3:
                    message = "Wie man so schön sagt  \"no risk no fun\"";
                    break;
                case 4:
                    message = "Ich habe nichts dazu zu sagen.";
                    break;
                case 5:
                    message = "Gott steh mir bei und helfe der Armen Seele eine Antwort zu finden.";
                    break;
                case 6:
                    message = "Diese art der Frage hat doch sicher einen sexuellen Hintergrund.\nMeine Antwort lautet daher JA!";
                    break;
                case 7:
                    message = "Ich schätze weniger";
                    break;
                case 8:
                    message = "Ich schätze schon";
                    break;
                case 9:
                    message = "Ich werde eine lange Reise bestreiten auf der Suche nach der legendären Antwort.\nIch werde dich benachrichtigen wenn ich diese gefunden habe.";
                    break;
                case 10:
                    message = "Blicke ins innere deiner schwarzen Seele und du wirst eine Antwort finden.";
                    break;
                case 11:
                    message = "Ich würde nicht darauf wetten";
                    break;
                case 12:
                    message = "Ich würde darauf wetten!";
                    break;
                default:
                    message = "Das Orakel wird sich zu einem späteren Zeitpunkt bei dir melden.";
                    break;
            }

            return message;
        }

        private int GetRandomGeneratedNumber()
        {
            return rand.Next(0, MAX_RANDOM_VALUE);
        }
    }
}
