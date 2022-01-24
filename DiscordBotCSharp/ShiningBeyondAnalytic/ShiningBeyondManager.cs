using DiscordBotCSharp.MessageDesigns;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic
{
    public class ShiningBeyondManager//TODO RENAME
    {

        public async void ShowHeroData(CommandContext ctx, string[] msg)
        {
            if (ctx == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(ShowHeroData)}");
            if (msg == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(ShowHeroData)}");

            DiscordEmbedBuilder embed = null;

            if (this.ExistsHero(msg))
                embed = this.GetHeroDataEmbed(msg);
            else
                embed = this.GetDataNotFoundEmbed(msg[0]);

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }
        private bool ExistsHero(string[] msg)
        {
            if (msg == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(ExistsHero)}");
            if (msg[0].ToLower().Equals("altima"))
                return true;
            return false;
        }

        private DiscordEmbedBuilder GetHeroDataEmbed(string[] msg)
        {
            if (msg == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(GetHeroDataEmbed)}");

            string name = default;
            int lvl = default;

            if (msg[0] != null)
                name = msg[0];
            if (msg.Length == 2 && msg[1] != null && int.TryParse(msg[1], out int parsedlvl))
                lvl = parsedlvl;

            HeroModel model = this.GetHeroDataBy(name, lvl);


            return new DesignFactory().GetShindingBeyondHeroEmbed(model);
        }

        private HeroModel GetHeroDataBy(string name, int lvl = default)
        {
            if (name == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(GetHeroDataBy)}");

            var model = this.GetHeroModel(name, lvl);

            return model;
        }

        private HeroModel GetHeroModel(string name, int lvl)
        {
            var model = new HeroModel();
            //richtig rotze...neu machen 
            if (nameof(HeroEnum.Altima).ToLower().Equals(name))
            {
                model.Name = name.ToUpperInvariant();
                model.Lvl = lvl == default ? 0 : lvl;
                model.Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958";
                model.UltSkill = "description of effect coming soon";
                model.SecondarySkill = "description of effect coming soon";
                model.WeaponSkill = "description of effect coming soon";

                var attributes = new HeroAttributes();
                attributes.Atk = 5000;
                attributes.Def = 3000;
                attributes.Hp = 50000;

                model.Attributes = attributes;
            }
            else
                model = null;

            return model;
        }

        private DiscordEmbedBuilder GetDataNotFoundEmbed(string name)
        {
            if (name == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(GetDataNotFoundEmbed)}");

            return new DesignFactory().GetEmbed(Title: "Data not found", Description: $"Couldn't found hero name [{name}].", setAuthor: false);
        }
    }
}
