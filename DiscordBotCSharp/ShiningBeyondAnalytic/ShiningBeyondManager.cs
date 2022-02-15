using DiscordBotCSharp.MessageDesigns;
using DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts;
using DiscordBotCSharp.ShiningBeyondAnalytic.DataBases;
using DiscordBotCSharp.ShiningBeyondAnalytic.Enums;
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
                if (hero.Name.ToLower().Equals(msg[0].ToLower()))
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
            HeroModel model = null;
            foreach (var item in this.listOfHeros)
            {
                if (item.Name.ToLower().Equals(name))
                {
                    model = item;
                    break;
                }
            }

            if (lvl != default && model != null)
            {
                //TODO brechne states anhand des lvls 2-200!!!
            }

                return model;
        }


        #region Get DiscordEmbedBuilder Methoden
        public HeroModel GetTESTDATA(int id = -1)
        {
            var model = new HeroModel();
            var attributes = new HeroAttributes();
            var skills = new Skills();

            model.Name = "Altima";
            model.ClassRole = ClassTypeEnum.Warrior;
            model.Url = "https://static.wikia.nocookie.net/shining-beyond/images/e/e6/AltimaT1.png/revision/latest?cb=20210304040958";
            model.Lvl = 1;
            model.StarGrade = 3;

            skills.SkillsSkillModel = new List<SkillModel>();
            skills.UltSkillTitleName = "Celestial_Polarity";
            skills.SecondarySkillsTitleName = "Crystal_Skill";
            skills.WeaponSkillsTitleName = "Altima_Weapon_Skill";

            var Leap_Atk = new SkillModel();
            Leap_Atk.Name = "Leap_Atk";
            Leap_Atk.Description = "Leaps and deals X% damage of Hero's DEF to enemies within the targeted area";
            Leap_Atk.TypeOfSkill = SkillEnum.Ultimative;
            skills.SkillsSkillModel.Add(Leap_Atk);

            var Vortex = new SkillModel();
            Vortex.Name = "Vortex";
            Vortex.Description = "Pull enemies towards the targeted location";
            Vortex.TypeOfSkill = SkillEnum.Ultimative;
            skills.SkillsSkillModel.Add(Vortex);

            var Heal_Shield = new SkillModel();
            Heal_Shield.Name = "Heal_Shield";
            Heal_Shield.Description = "X% Change to apply. Each hit received will heal for x%";
            Heal_Shield.TypeOfSkill = SkillEnum.Ultimative;
            skills.SkillsSkillModel.Add(Heal_Shield);

            var Shield_Magic = new SkillModel();
            Shield_Magic.Name = "Shield_Magic";
            Shield_Magic.Description = "Negates X% of skill damagereceived";
            Shield_Magic.TypeOfSkill = SkillEnum.Secondary;
            skills.SkillsSkillModel.Add(Shield_Magic);

            var HP_Recovery_Up = new SkillModel();
            HP_Recovery_Up.Name = "HP_Recovery_Up";
            HP_Recovery_Up.Description = "Increase healing received by X%";
            HP_Recovery_Up.TypeOfSkill = SkillEnum.Secondary;
            skills.SkillsSkillModel.Add(HP_Recovery_Up);

            var Thorns = new SkillModel();
            Thorns.Name = "Thorns";
            Thorns.Description = "Relect X% damage Taken";
            Thorns.TypeOfSkill = SkillEnum.Weapon;
            skills.SkillsSkillModel.Add(Thorns);

            attributes.Hp = 1383;
            attributes.Atk = 138;
            attributes.Def = 209;

            model.Skills = skills;
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
