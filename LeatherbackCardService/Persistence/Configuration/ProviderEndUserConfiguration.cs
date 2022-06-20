using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ProviderEndUserConfiguration : IEntityTypeConfiguration<ProviderEndUser>
    {
        public void Configure(EntityTypeBuilder<ProviderEndUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EndUserId).HasColumnType("varchar(50)");
            
            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.CardProviderId).IsRequired();
            builder.Property(x => x.CardRequestId).IsRequired();
            
            builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.CardProvider).WithMany().HasForeignKey(x => x.CardProviderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.CardRequest).WithMany().HasForeignKey(x => x.CardRequestId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}