using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCSharp.MessageDesigns
{
    public class DesignFactory
    {
        private const string DISCORDLIB = "https://docs.stillu.cc/api/Discord.EmbedFieldBuilder.html";
        private const string EVERYONE_TAG = "@everyone";
        private const string WOLFI_BOT_PICUTRE = "https://i.imgur.com/3h5CSAf.png";
        private const string EMBED_FOOTER = "Made by Wolfi";
        private const string WOLFI_IMAGE = "https://cdn.discordapp.com/avatars/323381699675422724/be60edbf146f24c975d708e05d2030b1.png?size=1024";
        private const string WOLFIS_NAME = "Wolfi";
        private const string BOT_CREATED_TIME = "03.12.2020 19:22:59";
        private const string BOT_UPDATE_TIME = "24.01.2022 12:09:55";
        private const string COMMANDS = ";MyUserInfo\n;Info\n;TTT\n";
        private const string PATCH_NOTES = "Bot Language changed to english";
        private const string PROJECT_LINK = "https://github.com/Wolfi1802/DiscordBotManager";


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
            embed.WithAuthor(name: WOLFIS_NAME, url: WOLFI_IMAGE, iconUrl: WOLFI_IMAGE);
            embed.Color = DiscordColor.CornflowerBlue;
            embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = WOLFI_BOT_PICUTRE };
            embed.Timestamp = DateTimeOffset.Now;
            embed.Footer.IconUrl = WOLFI_BOT_PICUTRE;
            embed.Footer.Text = "WolfiBot Informations";

            if (!string.IsNullOrEmpty(Title))
                embed.Title = $"Informationen to Bot";

            if (!string.IsNullOrEmpty(Title))
                embed.Description = "This Bot is a free open Source Project,type ;help for more informations about commands";

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
