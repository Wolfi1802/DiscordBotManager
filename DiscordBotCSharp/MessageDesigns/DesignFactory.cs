using DiscordBotCSharp.ShiningBeyondAnalytic;
using DiscordBotCSharp.ShiningBeyondAnalytic.DataBases;
using DiscordBotCSharp.ShiningBeyondAnalytic.Enums;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBotCSharp.MessageDesigns
{
    public class DesignFactory
    {
        public DiscordEmbedBuilder GetEmbed((string name, string value, bool inline) field,
            string Title = "", string Description = "", string name = "", string url = "", bool setAuthor = true)
        {
            List<(string name, string value, bool inline)> list = new List<(string name, string value, bool inline)>();
            list.Add(field);

            return this.GetEmbed(Title, Description, list, name, url, setAuthor);
        }

        public DiscordEmbedBuilder GetEmbed(string Title = "", string Description = "",
            List<(string name, string value, bool inline)> fieldList = null, string name = "", string url = "", bool setAuthor = true)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = TextFragments.WOLFI_BOT_PICUTRE };
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = TextFragments.WOLFI_BOT_PICUTRE;
            embed.Footer.Text = TextFragments.BOT_INFO;

            if (setAuthor)
            {
                if (string.IsNullOrEmpty(name))
                    embed.WithAuthor(name: TextFragments.WOLFIS_NAME, url: url, iconUrl: url);
                else if (string.IsNullOrEmpty(url))
                    embed.WithAuthor(name: name, url: TextFragments.WOLFI_IMAGE, iconUrl: TextFragments.WOLFI_IMAGE);
                else if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(name))
                    embed.WithAuthor(name: TextFragments.WOLFIS_NAME, url: TextFragments.WOLFI_IMAGE, iconUrl: TextFragments.WOLFI_IMAGE);
                else
                    embed.WithAuthor(name: name, url: url, iconUrl: url);
            }

            if (!string.IsNullOrEmpty(Title))
                embed.Title = Title;
            else
                embed.Title = TextFragments.BOT_INFO;

            if (!string.IsNullOrEmpty(Description))
                embed.Description = Description;
            else
                embed.Description = TextFragments.BOT_DESC;

            if (fieldList != null && fieldList.Count != 0)
            {
                foreach (var item in fieldList)
                {
                    embed.AddField(name: item.name, value: item.value, inline: item.inline);
                }
            }

            return embed;
        }

        public DiscordEmbedBuilder GetProfileEmbed(CommandContext ctx, (string name, string value, bool inline) field,
            string Title = "", string Description = "")
        {
            if (ctx == null)
                throw new Exception($"{nameof(DesignFactory)},{nameof(GetProfileEmbed)},{nameof(ctx)} ist null");

            List<(string name, string value, bool inline)> list = new List<(string name, string value, bool inline)>();
            list.Add(field);

            return this.GetProfileEmbed(ctx, Title, Description, list);
        }

        public DiscordEmbedBuilder GetProfileEmbed(CommandContext ctx, string Title = "", string Description = "",
            List<(string name, string value, bool inline)> fieldList = null)
        {
            if (ctx == null)
                throw new Exception($"{nameof(DesignFactory)},{nameof(GetProfileEmbed)},{nameof(ctx)} ist null");

            var embed = this.GetEmbed(Title, Description, fieldList);
            embed.WithAuthor(name: ctx.Member.DisplayName, url: ctx.Member.AvatarUrl, iconUrl: ctx.Member.AvatarUrl);

            return embed;
        }

        public DiscordEmbedBuilder GetShindingBeyondHeroEmbed(HeroModel model)
        {
            try
            {
                if (model == null)
                    throw new Exception($"{nameof(DesignFactory)},{nameof(GetShindingBeyondHeroEmbed)},{nameof(model)} ist null");

                var ultSkills = this.GetFormatedSkills(model.Skills, SkillEnum.Ultimative);
                var secondarySkills = this.GetFormatedSkills(model.Skills, SkillEnum.Secondary);
                var weaponSkills = this.GetFormatedSkills(model.Skills, SkillEnum.Weapon);

                var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
                embed.WithAuthor(name: $"{model.Name} ({model.ClassRole})", url: model.Url);
                embed.AddField(TextFragments.SB_LVL, model.Lvl.ToString());
                embed.AddField(TextFragments.SB_STATES, this.GetHeroModelStates(model));

                if (!string.IsNullOrWhiteSpace(ultSkills))
                    embed.AddField(TextFragments.SB_SKILL_ULT + model.Skills.UltSkillTitleName, ultSkills);
                else
                    embed.AddField(TextFragments.SB_SKILL_ULT + model.Skills.UltSkillTitleName, "not implemented");

                if (!string.IsNullOrWhiteSpace(secondarySkills))
                    embed.AddField(TextFragments.SB_SKILL_SECONDARY + model.Skills.SecondarySkillsTitleName, secondarySkills);
                else
                    embed.AddField(TextFragments.SB_SKILL_SECONDARY + model.Skills.SecondarySkillsTitleName, "not implemented");

                if (!string.IsNullOrWhiteSpace(weaponSkills))
                    embed.AddField(TextFragments.SB_SKILL_WEAPON + model.Skills.WeaponSkillsTitleName, weaponSkills);
                else
                    embed.AddField(TextFragments.SB_SKILL_WEAPON + model.Skills.WeaponSkillsTitleName, "not implemented");

                embed.Color = DiscordColor.CornflowerBlue;
                embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = model.Url };
                embed.Timestamp = DateTimeOffset.Now;
                embed.Footer.IconUrl = TextFragments.WOLFI_BOT_PICUTRE;
                embed.Footer.Text = TextFragments.BOT_INFO;

                return embed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetFormatedSkills(Skills skills, SkillEnum skillEnum)
        {
            string skillsToShowFormated = string.Empty;

            var skillsToShow = skills.SkillsSkillModel.Where(item => item.TypeOfSkill == skillEnum);

            foreach (var item in skillsToShow)
            {
                skillsToShowFormated += $"*{item.Name}*\n```{item.Description}```\n";
            }

            return skillsToShowFormated;
        }

        public DiscordEmbedBuilder GetHowToAddEmbed()
        {
            try
            {
                var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
                embed.WithAuthor(name: "Explanation how to add to database");
                embed.AddField("Formatter", ";SbAdd\n Name\n UltimativeSkillName\n SecondarySkillName\n WeaponSkillName\n HeroPictureUrl\n LVL\n StarGrade\n HP\n ATK\n DEF\n");
                embed.AddField("Example", ";SbAdd Altima Celestial_Polarity Crystal_Skill Altima_Weapon_Skill  https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958 1 3 1383 138 209");
                embed.Color = DiscordColor.Red;
                embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = TextFragments.WOLFI_BOT_PICUTRE };
                embed.Timestamp = DateTimeOffset.Now;
                embed.Footer.IconUrl = TextFragments.WOLFI_BOT_PICUTRE;
                embed.Footer.Text = TextFragments.BOT_INFO;

                return embed;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetHeroModelStates(HeroModel model)
        {
            if (model == null)
                throw new Exception($"{nameof(DesignFactory)},{nameof(GetHeroModelStates)},{nameof(model)} ist null");

            return $"Atk: {model.Attributes.Atk}\nDef: {model.Attributes.Def}\nHp: {model.Attributes.Hp}\n";
        }
    }
}
