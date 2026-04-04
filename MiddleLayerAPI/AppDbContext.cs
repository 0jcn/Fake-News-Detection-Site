using Microsoft.EntityFrameworkCore;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<SavedDetections> SavedDetections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<SavedDetections>().ToTable("saved_detections");
        }
    }
}
