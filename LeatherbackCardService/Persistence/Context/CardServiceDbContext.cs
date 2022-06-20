using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Domain.Entities.Transactions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Extensions;

namespace Persistence.Context
{
    public class CardServiceDbContext : DbContext, ICardServiceDbContext
    {
        public CardServiceDbContext(DbContextOptions<CardServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CardDetail> CardDetails { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardProviderCurrency> CardProviderCurrencies { get; set; }
        public DbSet<CardProvider> CardProviders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CardRequest> CardRequests { get; set; }
        public DbSet<CardPin> CardPins { get; set; }
        public DbSet<ProviderEndUser> ProviderEndUsers { get; set; }
        public DbSet<CardLimit> CardLimits { get; set; }
        public DbSet<CardLimitType> CardLimitTypes { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CardServiceDbContext).Assembly);
            modelBuilder.ApplyGlobalFilters<bool>("IsDeleted", false);

            var customerTypeConverter = new EnumToStringConverter<CustomerType>();
            var cardStatusConverter = new EnumToStringConverter<CardStatus>();
            var cardCarrierTypeConverter = new EnumToStringConverter<CardCarrierType>();
            var sexConverter = new EnumToStringConverter<Sex>();
            var cardRequestStatus = new EnumToStringConverter<CardRequestStatus>();
            var cardType = new EnumToStringConverter<CardType>();
            var cardDesignType = new EnumToStringConverter<CardDesignType>();

            modelBuilder
                .Entity<Customer>()
                .Property(e => e.CustomerType)
                .HasConversion(customerTypeConverter);

            modelBuilder
                .Entity<CardProviderCurrency>()
                .Property(e => e.CustomerType)
                .HasConversion(customerTypeConverter);

            modelBuilder
                .Entity<Card>()
                .Property(e => e.CardStatus)
                .HasConversion(cardStatusConverter);

            modelBuilder
                .Entity<Card>()
                .Property(e => e.CardCarrierType)
                .HasConversion(cardCarrierTypeConverter);

            /*modelBuilder
                .Entity<CardRequest>()
                .Property(e => e.Sex)
                .HasConversion(sexConverter); */

            modelBuilder
                .Entity<CardRequest>()
                .Property(e => e.CustomerType)
                .HasConversion(customerTypeConverter);

            modelBuilder
                .Entity<CardRequest>()
                .Property(e => e.Status)
                .HasConversion(cardRequestStatus);

            modelBuilder
                .Entity<CardDetail>()
                .Property(e => e.Status)
                .HasConversion(cardRequestStatus);

            modelBuilder
                .Entity<CardDetail>()
                .Property(e => e.CardType)
                .HasConversion(cardType);
            
            modelBuilder
                .Entity<ProviderEndUser>()
                .Property(e => e.Status)
                .HasConversion(cardRequestStatus);
            
            modelBuilder
                .Entity<CardRequest>()
                .Property(e => e.Design)
                .HasConversion(cardDesignType);
        }
    }
}