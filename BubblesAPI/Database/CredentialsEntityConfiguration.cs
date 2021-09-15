using BubblesAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubblesAPI.Database
{
    public class CredentialsEntityConfiguration : IEntityTypeConfiguration<Credentials>
    {
        public void Configure(EntityTypeBuilder<Credentials> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd().IsRequired();
            
            builder.HasOne(c => c.User)
                .WithOne(u => u.Credentials)
                .HasForeignKey<Credentials>(c => c.UserId);

            builder.Property(b => b.Password)
                .IsRequired();

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);

            builder.Property(b => b.Created).HasDefaultValue(System.DateTime.Now);
        }
    }
}