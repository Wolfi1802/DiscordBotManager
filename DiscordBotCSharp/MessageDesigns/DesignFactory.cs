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
            string Title = "", string Description = "")
        {
            List<(string name, string value, bool inline)> list = new List<(string name, string value, bool inline)>();
            list.Add(field);

            return this.GetEmbed(Title, Description, list);
        }

        public DiscordEmbedBuilder GetEmbed(string Title = "", string Description = "",
            List<(string name, string value, bool inline)> fieldList = null)
        {
            var embed = new DiscordEmbedBuilder { Footer = new DiscordEmbedBuilder.EmbedFooter() };
            embed.WithAuthor(name: TextFragments.WOLFIS_NAME, url: TextFragments.WOLFI_IMAGE, iconUrl: TextFragments.WOLFI_IMAGE);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = TextFragments.WOLFI_BOT_PICUTRE };
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = TextFragments.WOLFI_BOT_PICUTRE;
            embed.Footer.Text = TextFragments.BOT_INFO;

            if (!string.IsNullOrEmpty(Title))
                embed.Title = TextFragments.BOT_INFO;

            if (!string.IsNullOrEmpty(Title))
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
    }
}
