using Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Cvv).HasColumnType("varchar(5)").IsRequired(false);
            builder.Property(x => x.CardNumber).HasColumnType("varchar(30)").IsRequired(false);
            builder.Property(x => x.CardIdentifier).HasColumnType("Varchar(50)").IsRequired(false);
            builder.Property(x => x.CardHolderName).HasColumnType("Varchar(200)").IsRequired(false);
            builder.Property(x => x.CardStatus).HasColumnType("Varchar(30)");
            builder.Property(x => x.CardCarrierType).HasColumnType("Varchar(30)");
            builder.Property(x => x.MaskedPan).HasColumnType("Varchar(30)");
            builder.Property(x => x.ExpiryDate).HasColumnType("Varchar(30)");
            builder.Property(x => x.LeatherBackCardDesign).HasColumnType("varchar(100)");

            builder.Property(x => x.CardStatusReason).HasColumnType("Varchar(500)");
            builder.Property(x => x.CardQrCodeContent).HasColumnType("Varchar(max)");
            builder.Property(x => x.ProviderResponse).HasColumnType("Varchar(max)");
            
            builder.Property(x => x.CardCarrierType).HasColumnType("Varchar(30)");
            builder.Property(x => x.CardStatus).HasColumnType("Varchar(30)");

            builder.Property(x => x.CardIdentifier).IsRequired();

            builder.HasIndex(x => x.CardIdentifier).IsUnique();
            
            builder.HasOne(x => x.CardDetails).WithMany().HasForeignKey(x => x.CardDetailId);

        }
    }
}