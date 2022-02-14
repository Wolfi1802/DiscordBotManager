﻿using DiscordBotCSharp.MessageDesigns;
using DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts;
using DiscordBotCSharp.ShiningBeyondAnalytic.DataBases;
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
            this.listOfHeros = new List<HeroModel>(this.heroDatabaseContextWrapper.SelectAllHeroes());
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

        public async void TryShowHeroData(CommandContext ctx, string[] msg)
        {

            if (ctx == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(TryShowHeroData)}");
            if (msg == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(TryShowHeroData)}");

            try
            {
                DiscordEmbedBuilder embed = null;

                if (this.ExistsHero(msg))
                    embed = this.GetHeroDataEmbed(msg);
                else
                    embed = this.GetDataNotFoundEmbed(msg[0]);

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }

        private bool ExistsHero(string[] msg)
        {
            if (msg == null)
                throw new NullReferenceException($"{nameof(ShiningBeyondManager)}, {nameof(ExistsHero)}");

            foreach (HeroModel hero in listOfHeros)
            {
                if (hero.Name.Equals(msg[0]))
                    return true;
            }

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
            foreach (var item in this.listOfHeros)//TODO[TS] lvl suche implementieren
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
            //model.HeroAttributes = new HeroAttributes();
            //model.Skills = new Skills();

            model.Name = "Altima";

            //model.Skills.UltSkillTitleName = "Celestial_Polarity";
            //model.Skills.SecondarySkillsTitleName = "Crystal_Skill";
            //model.Skills.WeaponSkillsTitleName = "Altima_Weapon_Skill";

            //model.Skills.UltSkills.Add("Leap_Atk", "Leaps and deals X% damage of Hero's DEF to enemies within the targeted area\n");
            //model.Skills.UltSkills.Add("Vortex", "Pull enemies towards the targeted location\n");
            //model.Skills.UltSkills.Add("Heal Shield", "X% Change to apply. Each hit received will heal for x%\n");

            //model.Skills.SecondarySkills.Add("Shield:Magic", "Negates X% of skill damagereceived\n");
            //model.Skills.SecondarySkills.Add("HP Recovery Up", "Increase healing received by X%\n");

            //model.Skills.WeaponSkills.Add("Thorns", "Relect X% damage Taken\n");

            model.Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958";
            model.Lvl = 1;
            model.StarGrade = 3;

            //model.HeroAttributes.Hp = 1383;
            //model.HeroAttributes.Atk = 138;
            //model.HeroAttributes.Def = 209;

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
