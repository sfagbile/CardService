using Domain.Entities.ProviderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(x=>x.Id); 
            
            builder.Property(x => x.Name).HasColumnType("varchar(50)");
            builder.Property(x => x.Code).HasColumnType("varchar(10)");
            builder.Property(x => x.Symbol).HasColumnType("varchar(100)");
            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Code).IsRequired();
            
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}