using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EntityTypeConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => new { x.Name }).IsUnique();
        builder.HasIndex(x => new { x.Email }).IsUnique();

        builder.Property(x => x.Name).IsRequired();
    }
}
