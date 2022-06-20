using Domain.Entities.ProviderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CountryConfiguration: IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x=>x.Id); 
            
            builder.Property(x => x.Name).HasColumnType("varchar(200)");
            builder.Property(x => x.Iso).HasColumnType("varchar(20)");  
            builder.Property(x => x.Iso3).HasColumnType("varchar(20)");
            builder.Property(x => x.Region).HasColumnType("varchar(100)");
            
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Iso).IsRequired();
            builder.Property(x => x.Iso3).IsRequired();
            
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}