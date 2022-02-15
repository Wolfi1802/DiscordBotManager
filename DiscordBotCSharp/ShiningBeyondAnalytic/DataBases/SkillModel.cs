using DiscordBotCSharp.ShiningBeyondAnalytic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DataBases
{
    public class SkillModel
    {
        public int SkillModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillEnum TypeOfSkill { get; set; }

        public int SkillsSkillModelId { get; set; }
        public Skills SkillsSkillModel { get; set; }
    }
}
