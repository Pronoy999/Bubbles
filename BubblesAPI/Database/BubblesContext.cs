using BubblesAPI.Database.Models;
using BubblesAPI.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace BubblesAPI.Database
{
    public class BubblesContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }

        public BubblesContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
            new StatusEntityConfiguration().Configure(modelBuilder.Entity<Status>());
        }
    }
}