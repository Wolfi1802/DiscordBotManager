using System.Data.Entity;

namespace DiscordBotCSharp.ShiningBeyondAnalytic.DatabaseContexts
{
    public class HeroDatabaseContext : DbContext
    {
        public DbSet<HeroModel> Model { get; set; }
    }
}
