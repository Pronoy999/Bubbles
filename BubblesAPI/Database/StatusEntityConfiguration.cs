using BubblesAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubblesAPI.Database
{
    public class StatusEntityConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(b => b.StatusName).IsRequired();
        }
    }
}