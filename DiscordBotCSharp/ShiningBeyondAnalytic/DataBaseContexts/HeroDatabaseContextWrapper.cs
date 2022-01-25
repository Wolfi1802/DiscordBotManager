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

        public List<HeroModel> SelectAll()
        {
            return new List<HeroModel>(this.heroDatabaseContext.Model);
        }

        public void Insert(HeroModel hero)
        {
            this.heroDatabaseContext.Model.Add(hero);
            this.heroDatabaseContext.SaveChanges();
        }

        public HeroModel GetModelBy(int id)
        {
            foreach (var item in this.heroDatabaseContext.Model)
            {
                if (item.HeroModelId == id)
                    return item;
            }

            return null;
        }


        public void Remove(HeroModel hero)
        {
            this.heroDatabaseContext.Model.Remove(hero);
            this.heroDatabaseContext.SaveChanges();
        }
    }
}
