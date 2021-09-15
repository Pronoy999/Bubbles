using BubblesAPI.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubblesAPI.Database.Models
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.UserId);
            builder.Property(b => b.UserId).IsRequired();
            builder.Property(b => b.FirstName).IsRequired();
            builder.Property(b => b.LastName).IsRequired();
            builder.Property(b => b.Email).IsRequired();
            builder.Property(b => b.OrganisationName);
            builder.Property(b => b.IsActive).HasDefaultValue(true);
            builder.Property(b => b.CreatedAt).HasDefaultValue(System.DateTime.Now);
        }
    }
}