using Domain.Entities.ProviderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardProviderConfiguration : IEntityTypeConfiguration<CardProvider>
    {
        public void Configure(EntityTypeBuilder<CardProvider> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Name).HasColumnType("varchar(100)");
            builder.Property(x => x.Description).HasColumnType("varchar(500)");
            builder.Property(x => x.Email).HasColumnType("varchar(30)");
            builder.Property(x => x.Address).HasColumnType("varchar(100)");
            builder.Property(x => x.Branch).HasColumnType("varchar(100)");
            builder.Property(x => x.Postcode).HasColumnType("varchar(30)");
            builder.Property(x => x.City).HasColumnType("varchar(50)");
            builder.Property(x => x.State).HasColumnType("varchar(50)");
            builder.Property(x => x.HasWebhook).HasDefaultValue(false);

            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);

            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}