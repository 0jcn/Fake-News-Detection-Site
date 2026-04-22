using Microsoft.EntityFrameworkCore;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI
{
    /// <summary>
    /// Db Context for the application, used to interact with the database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<SavedDetections> SavedDetections { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("users", schema: "fake_news_site");
            modelBuilder.Entity<SavedDetections>().ToTable("saved_detections", schema: "fake_news_site");
        }
    }
}
