using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DiscordBotCSharp.Helper
{
    public static class Helper
    {
        public static bool CheckGameList(ulong idToFind, List<ulong> userWhoApply)
        {
            foreach (var userId in userWhoApply)
            {
                if (idToFind == userId)
                {
                    Debug.WriteLine("GEFUNDEN");
                    return true;
                }
            }
            Debug.WriteLine("NICHT GEFUNDEN");
            return false;
        }

    }
}
