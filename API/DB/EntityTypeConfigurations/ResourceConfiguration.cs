using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.EntityTypeConfigurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasIndex(x => x.Type).IsUnique();
        builder.Property(x => x.Type)
            .HasConversion(en => en.ToString(),
                str => Enum.Parse<ResourceType>(str));
    }
}
