using Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CardPinConfiguration : IEntityTypeConfiguration<CardPin>
    {
        
        public void Configure(EntityTypeBuilder<CardPin> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Pin).HasColumnType("varchar(max)");
            builder.Property(x => x.CardId).IsRequired(true);
            
            builder.HasIndex(x => x.CardId).IsUnique();
        }
    }
}