using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DataBases
{
    public class HeroAttributes
    {
        [ForeignKey("HeroModelAttribute")]
        public int HeroAttributesId { set; get; }

        public int Atk { set; get; }

        public int Def { set; get; }

        public int Hp { set; get; }

        public virtual HeroModel HeroModelAttribute { set; get; }
    }
}
