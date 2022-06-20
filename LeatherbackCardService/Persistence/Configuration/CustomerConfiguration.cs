using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.FirstName).IsRequired().HasColumnType("varchar(300)");
            builder.Property(x => x.MiddleName).HasColumnType("varchar(300)");
            builder.Property(x => x.LastName).IsRequired().HasColumnType("varchar(300)");
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar(50)");
            builder.Property(x => x.Sex).IsRequired().HasColumnType("varchar(20)");
            builder.Property(x => x.Address).HasColumnType("varchar(500)").IsRequired();
            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.ProductId);
            builder.Property(x => x.City).HasColumnType("varchar(30)").IsRequired();
            builder.Property(x => x.PostalCode).HasColumnType("varchar(15)").IsRequired();
            
            builder.Property(x => x.CustomerType).HasColumnType("Varchar(30)");

            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId)
                .IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}