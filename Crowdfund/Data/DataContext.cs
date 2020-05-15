using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace ConsoleApp.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=localhost;Database=crowdfund_db;User Id=sa;Password=!@#adm1n;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .ToTable("User");

            modelBuilder
                .Entity<Project>()
                .ToTable("Project");

            modelBuilder
                .Entity<RewardPackage>()
                .ToTable("RewardPackage");

            modelBuilder
                .Entity<Post>()
                .ToTable("Post");

            modelBuilder
                .Entity<Media>()
                .ToTable("Media");

            modelBuilder
                .Entity<UserProjectReward>()
                .ToTable("UserProjectReward");
        }
    }
}