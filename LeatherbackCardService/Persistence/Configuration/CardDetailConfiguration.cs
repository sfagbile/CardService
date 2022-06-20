using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardDetailConfiguration: IEntityTypeConfiguration<CardDetail>
    {
        public void Configure(EntityTypeBuilder<CardDetail> builder)
        {
            builder.HasKey(x=>x.Id); 
            
            builder.Property(x => x.ProviderLedgerId).HasColumnType("varchar(50)");
            builder.Property(x => x.CardDesign).HasColumnType("varchar(100)").IsRequired();
            builder.Property(x => x.CardProgramme).HasColumnType("varchar(50)").IsRequired();
            
            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.CardRequestId).IsRequired();
            builder.Property(x => x.CardType).IsRequired();
            builder.Property(x => x.ProviderEndUserId).IsRequired();
            builder.Property(x => x.AccountId);

            builder.Property(x => x.CardType).HasColumnType("Varchar(30)");
            builder.Property(x => x.Status).HasColumnType("Varchar(30)");

            builder.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.CardRequest).WithMany().HasForeignKey(x => x.CardRequestId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ProviderEndUser).WithMany().HasForeignKey(x => x.ProviderEndUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}