using Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardProviderCurrencyConfiguration : IEntityTypeConfiguration<CardProviderCurrency>
    {
        public void Configure(EntityTypeBuilder<CardProviderCurrency> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.CardProviderId).IsRequired();
            builder.Property(x => x.CustomerType).IsRequired();
            builder.Property(x => x.CardType).IsRequired();
            builder.Property(x => x.CardDesign).IsRequired();
            builder.Property(x => x.CardProgramme).IsRequired();
            
            builder.Property(x => x.CardType).HasColumnType("Varchar(30)");
            builder.Property(x => x.CustomerType).HasColumnType("Varchar(30)");

            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);;
            builder.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);;
            builder.HasOne(x => x.CardProvider).WithMany().HasForeignKey(x => x.CardProviderId).OnDelete(DeleteBehavior.Restrict);;
        }
    }
}