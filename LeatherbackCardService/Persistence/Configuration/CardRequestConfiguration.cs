using Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardRequestConfiguration : IEntityTypeConfiguration<CardRequest>
    {
        public void Configure(EntityTypeBuilder<CardRequest> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            
            builder.Property(x => x.CardType).IsRequired();
            builder.Property(x => x.CustomerType).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.AccountId).IsRequired();
            builder.Property(x => x.TransactionLimit).HasPrecision(18, 2);
            
            //
            
            builder.Property(x => x.Address).HasColumnType("varchar(200)").IsRequired();
            builder.Property(x => x.City).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.PostalCode).HasColumnType("varchar(15)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar(50)");
            builder.Property(x => x.CountryIso).HasColumnType("varchar(5)");
            builder.Property(x => x.FirstName).HasColumnType("varchar(50)");
            builder.Property(x => x.LastName).HasColumnType("varchar(50)");
            builder.Property(x => x.MiddleName).HasColumnType("varchar(50)");
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar(50)");
            builder.Property(x => x.CreateCustomerResponse).HasColumnType("varchar(max)");
            builder.Property(x => x.CreateCardDetailsResponse).HasColumnType("varchar(max)");
            builder.Property(x => x.CreateProviderEndUserResponse).HasColumnType("varchar(max)");
            builder.Property(x => x.CreateCardResponse).HasColumnType("varchar(max)");
            builder.Property(x => x.CardRejectionReason).HasColumnType("varchar(500)");

            builder.Property(x => x.CardType).HasColumnType("Varchar(30)");
            builder.Property(x => x.CustomerType).HasColumnType("Varchar(30)");
            builder.Property(x => x.Status).HasColumnType("Varchar(30)");

            builder.Property(x => x.IsCreateCardInitiated).HasDefaultValue(false);
            builder.Property(x => x.IsCreateCustomerInitiated).HasDefaultValue(false);
            builder.Property(x => x.IsCreateProviderEndUserInitiated).HasDefaultValue(false);
            builder.Property(x => x.IsCreateCardDetailsInitiated).HasDefaultValue(false);
        }
    }
}