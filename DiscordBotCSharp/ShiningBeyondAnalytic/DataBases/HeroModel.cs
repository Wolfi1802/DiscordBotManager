using DiscordBotCSharp.ShiningBeyondAnalytic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DataBases
{
    public class HeroModel
    {
        public int HeroModelId { set; get; }

        public string Name { set; get; }

        public int Lvl { set; get; }

        public string Url { set; get; }

        public int StarGrade { set; get; }

        public ClassTypeEnum ClassRole { set; get; }

        public virtual HeroAttributes Attributes { set; get; }
        public virtual Skills Skills { set; get; }
    }
}
