using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class TransactionLimitConfiguration : IEntityTypeConfiguration<CardLimit>
    {
        public void Configure(EntityTypeBuilder<CardLimit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CardId).IsRequired();
            builder.Property(x => x.CardLimitTypeId).IsRequired();
            builder.Property(x => x.MinAmount).HasPrecision(18, 2);
            builder.Property(x => x.MaxAmount).HasPrecision(18, 2);
            
            builder.HasOne(x => x.CardLimitTypes).WithMany().HasForeignKey(x => x.CardLimitTypeId);
            builder.HasOne(x => x.Card).WithMany().HasForeignKey(x => x.CardId);
        }
    }
}