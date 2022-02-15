using DiscordBotCSharp.ShiningBeyondAnalytic.DataBases;
using System.Data.Entity;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts
{
    public class HeroDatabaseContext : DbContext
    {
        public DbSet<HeroModel> HeroModel { get; set; }

        public DbSet<HeroAttributes> HeroAttributes { get; set; }

        public DbSet<Skills> Skills { get; set; }

        public DbSet<SkillModel> SkillModel { get; set; }

    }
}
