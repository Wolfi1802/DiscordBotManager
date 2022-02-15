using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DataBases
{
    public class Skills
    {
        [ForeignKey("HeroModelSkills")]
        public int SkillsId { get; set; }

        public string UltSkillTitleName { set; get; }

        public string SecondarySkillsTitleName { set; get; }

        public string WeaponSkillsTitleName { set; get; }

        [ForeignKey("SkillsSkillModelId")]
        public virtual ICollection<SkillModel> SkillsSkillModel { set; get; }
        public virtual HeroModel HeroModelSkills { set; get; }

    }
}
