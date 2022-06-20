using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class TransactionLimitTypeConfiguration : IEntityTypeConfiguration<CardLimitType>
    {
        public void Configure(EntityTypeBuilder<CardLimitType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasColumnType("Varchar(200)");
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            
        }
    }
}