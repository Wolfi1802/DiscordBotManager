using DiscordBotCSharp.ShiningBeyondAnalytic.DataBases;
using System.Collections.Generic;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts
{
    public class HeroDatabaseContextWrapper
    {
        protected readonly HeroDatabaseContext heroDatabaseContext;

        public HeroDatabaseContextWrapper()
        {
            this.heroDatabaseContext = new HeroDatabaseContext();
        }

        #region HeroModel

        public void Insert(HeroModel hero)
        {
            this.heroDatabaseContext.HeroModel.Add(hero);
            this.heroDatabaseContext.SaveChanges();
        }

        public List<HeroModel> SelectAllHeroes()
        {
            return new List<HeroModel>(this.heroDatabaseContext.HeroModel);
        }
        public HeroModel GetModelBy(int id)
        {
            foreach (var item in this.heroDatabaseContext.HeroModel)
            {
                if (item.HeroModelId == id)
                    return item;
            }

            return null;
        }

        public void Remove(HeroModel hero)
        {
            this.heroDatabaseContext.HeroModel.Remove(hero);
            this.heroDatabaseContext.SaveChanges();
        }

        #endregion

        //#region HeroAttributes

        //public void Insert(HeroAttributes attributes)
        //{
        //    this.heroDatabaseContext.HeroAttributes.Add(attributes);
        //    this.heroDatabaseContext.SaveChanges();
        //}

        //public List<HeroAttributes> SelectAllAttributes()
        //{
        //    return new List<HeroAttributes>(this.heroDatabaseContext.HeroAttributes);
        //}

        //public void Remove(HeroAttributes attributes)
        //{
        //    this.heroDatabaseContext.HeroAttributes.Remove(attributes);
        //    this.heroDatabaseContext.SaveChanges();
        //}

        //#endregion
    }
}
