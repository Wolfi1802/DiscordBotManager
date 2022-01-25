using DiscordBotCSharp.MessageDesigns;
using DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCSharp.ShiningBeyondAnalytic
{
    public sealed class ShiningBeyondManager
    {
        private static readonly Lazy<ShiningBeyondManager> lazy = new Lazy<ShiningBeyondManager>(() => new ShiningBeyondManager());

        public static ShiningBeyondManager Instance { get { return lazy.Value; } }


        protected List<HeroModel> listOfHeros;

        protected readonly HeroDatabaseContextWrapper heroDatabaseContextWrapper;

        private ShiningBeyondManager()
        {
            this.heroDatabaseContextWrapper = new HeroDatabaseContextWrapper();
            this.listOfHeros = new List<HeroModel>(this.heroDatabaseContextWrapper.SelectAll());
        }

        #region DBVerwaltung

        public bool AddtoDataBase(HeroModel model)
        {
            try
            {
                this.listOfHeros.Add(model);
                this.heroDatabaseContextWrapper.Insert(model);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool RemoveFromDataBase(HeroModel model)
        {
            try
            {
                this.listOfHeros.Remove(model);
                this.heroDatabaseContextWrapper.Remove(model);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public HeroModel GetModelBy(int id)
        {
            return this.heroDatabaseContextWrapper.GetModelBy(id);
        }

        #endregion

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

        private HeroModel GetHeroDataBy(string name, int lvl = default)
        {
            if (name == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(GetHeroDataBy)}");

            var model = this.GetHeroModel(name, lvl);

            return model;
        }

        private HeroModel GetHeroModel(string name, int lvl)
        {
            //HeroModel model = null;
            //richtig rotze...neu machen 
            //if (nameof(HeroEnum.Altima).ToLower().Equals(name))
            //{
            //    model = new HeroModel();
            //    model.Name = name.ToUpperInvariant();
            //    model.Lvl = lvl == default ? 1 : lvl;
            //    model.Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958";
            //    model.UltSkill = "description of effect coming soon";
            //    model.SecondarySkill = "description of effect coming soon";
            //    model.WeaponSkill = "description of effect coming soon";

            //    var attributes = new HeroAttributes();
            //    attributes.Atk = 5000;
            //    attributes.Def = 3000;
            //    attributes.Hp = 50000;

            //    model.Attributes = attributes;

            //    this.listOfHeros.Add(model);
            //    this.heroDatabaseContextWrapper.Insert(model);
            //}

            foreach (var item in this.listOfHeros)
            {
                if (item.Name.ToLower().Equals(name))
                    return item;
            }

            return null;
        }


        #region Get DiscordEmbedBuilder Methoden
        public HeroModel GetTESTDATA(int id = -1)
        {
            var model = new HeroModel();
            model.Name = "Altima";
            if (id != -1)
                model.HeroModelId = id;
            model.Lvl = 200;
            model.Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958";
            model.UltSkill = "description of effect coming soon";
            model.SecondarySkill = "description of effect coming soon";
            model.WeaponSkill = "description of effect coming soon";

            var attributes = new HeroAttributes();
            attributes.Atk = 5000;
            attributes.Def = 3000;
            attributes.Hp = 50000;

            model.Attributes = attributes;
            return model;
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

        private DiscordEmbedBuilder GetDataNotFoundEmbed(string name)
        {
            if (name == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(GetDataNotFoundEmbed)}");

            return new DesignFactory().GetEmbed(Title: "Data not found", Description: $"Couldn't found hero name [{name}].", setAuthor: false);
        }

        #endregion
    }
}
