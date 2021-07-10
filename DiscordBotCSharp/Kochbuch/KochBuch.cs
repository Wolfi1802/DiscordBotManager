using System;
using System.Collections.Generic;

namespace DiscordBotCSharp
{
    public class KochBuch
    {

        private const int MAX_RANDOM_VALUE = 5;
        private Random rand;

        public KochBuch()
        {
            this.rand = new Random();
        }

        /// <summary>
        /// Erstellt und gibt eine Liste von Rezepten zurück die per random Methode ausgewählt werden.
        /// </summary>
        /// <param name="yourCount">Anzahl der Rezepte die erstellt werden sollen</param>
        /// <returns></returns>
        public List<Rezept> GetRezepts(int yourCount)
        {
            List<Rezept> listedContent = new List<Rezept>();

            for (int i = 0; i < yourCount; i++)
            {
                listedContent.Add(this.GetCreatetRezept());
            }

            if (listedContent.Count <= 0)
            {
                throw new Exception();
            }
            return listedContent;
        }

        private Rezept GetCreatetRezept()
        {
            Rezept rezept = new Rezept();
            //TODO methode schreiben die per random methode ein zufälliges rezept zurück gibt
            switch (this.GetRandomGeneratedNumber())
            {
                case 0:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 1:
                    {
                        rezept.Name = "EierPfannkuchen";
                        rezept.Content = "2 Eier, 500ml milch, 1 Löffel Zuccker,250g Mehl";
                        break;
                    }
                case 2:
                    {
                        rezept.Name = "Fischstäbchen ala Wolfi";
                        rezept.Content = "Fischstäbchen, Kartoffelpüre, Rostzwiebeln, Spiegelei, Spinat";
                        break;
                    }
                case 3:
                    {
                        rezept.Name = "GemüseSuppe";
                        rezept.Content = "Suppengemüse, Wasser, weißes Hähnhen, Hühnerbrühe";
                        break;
                    }
                case 4:
                    {
                        rezept.Name = "Pilze ala Wolfi";
                        rezept.Content = "Buttergemüse, Pilze, Reiß";
                        break;
                    }
                case 5://TODO weitere Rezepte einfügen
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 6:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 7:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 8:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 9:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 10:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                case 11:
                    {
                        rezept.Name = "Spaghetti Bolognese";
                        rezept.Content = "2x Fix Bolognese, Spaghetti, Hackfleisch, Tomatenmark";
                        break;
                    }
                default: throw new ArgumentNullException();
            }


            return rezept;
        }

        private int GetRandomGeneratedNumber()
        {
            return rand.Next(0, MAX_RANDOM_VALUE);
        }
    }
}
