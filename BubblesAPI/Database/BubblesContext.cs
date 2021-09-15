using BubblesAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BubblesAPI.Database
{
    public class BubblesContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        public BubblesContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
            new StatusEntityConfiguration().Configure(modelBuilder.Entity<Status>());
            new CredentialsEntityConfiguration().Configure(modelBuilder.Entity<Credentials>());
        }
    }
}