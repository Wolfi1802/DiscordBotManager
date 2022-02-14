using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCSharp.Helper
{
    public class Parsehelper
    {
        /// <summary>
        /// Parsed den String oder gibt default zurück
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return default;
        }

        /// <summary>
        /// PArsedn den String oder gibt null zurück
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? ParseNullableInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return null;
        }
    }
}
