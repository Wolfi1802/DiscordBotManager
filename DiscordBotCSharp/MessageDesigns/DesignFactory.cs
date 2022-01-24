using DiscordBotCSharp.ShiningBeyondAnalytic;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
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
            List<(string name, string value, bool inline)> list = new List<(string name, string value, bool inline)>();
            list.Add(field);

            return this.GetProfileEmbed(ctx, Title, Description, list);
        }

        public DiscordEmbedBuilder GetProfileEmbed(CommandContext ctx, string Title = "", string Description = "",
            List<(string name, string value, bool inline)> fieldList = null)
        {
            var embed = this.GetEmbed(Title, Description, fieldList);
            embed.WithAuthor(name: ctx.Member.DisplayName, url: ctx.Member.AvatarUrl, iconUrl: ctx.Member.AvatarUrl);

            return embed;
        }

        public DiscordEmbedBuilder GetShindingBeyondHeroEmbed(HeroModel model)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: model.Name, url: model.Url);
            embed.AddField(TextFragments.SB_LVL, model.Lvl.ToString());
            embed.AddField(TextFragments.SB_STATES, this.GetHeroModelStates(model));
            embed.AddField(TextFragments.SB_SKILL_ULT, model.UltSkill);
            embed.AddField(TextFragments.SB_SKILL_SECONDARY, model.SecondarySkill);
            embed.AddField(TextFragments.SB_SKILL_WEAPON, model.WeaponSkill);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958" };
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = TextFragments.WOLFI_BOT_PICUTRE;
            embed.Footer.Text = TextFragments.BOT_INFO;


            return embed;
        }

        private string GetHeroModelStates(HeroModel model)
        {
            return $"Atk: {model.Attributes.Atk}\nDef: {model.Attributes.Def}\nHp: {model.Attributes.Hp}\n";//TODO formatierung!
        }
    }
}
