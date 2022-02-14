using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DataBases
{
    public class HeroModel
    {
        [Key]
        public int HeroModelId { set; get; }

        public string Name { set; get; }

        public int Lvl { set; get; }

        public string Url { set; get; }

        public int StarGrade { set; get; }

        //[ForeignKey("HeroAttributes")]
        //public virtual HeroAttributes HeroAttributes { set; get; }

        //[ForeignKey("Skills")]
        //public Skills Skills { set; get; }
    }
}
