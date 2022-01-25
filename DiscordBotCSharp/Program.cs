using System;
using System.Diagnostics;

namespace DiscordBotCSharp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Bot bot = new Bot();
                bot.RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}