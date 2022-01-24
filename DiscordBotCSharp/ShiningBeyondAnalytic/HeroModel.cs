using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic
{
    public class HeroModel
    {
        public string Name { set; get; }

        public int Lvl { set; get; }

        public string Url { set; get; }

        public string UltSkill { set; get; }

        public string SecondarySkill { set; get; }

        public string WeaponSkill { set; get; }

        public HeroAttributes Attributes { set; get; }
    }
}
