using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Cards;
using Domain.Entities.Customers;
using Domain.Entities.ProviderAggregate;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface ICardServiceDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<CardDetail> CardDetails { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<CardProviderCurrency> CardProviderCurrencies { get; set; }
        DbSet<CardProvider> CardProviders { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Currency> Currencies { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<CardRequest> CardRequests { get; set; }
        DbSet<CardPin> CardPins { get; set; }
        public DbSet<ProviderEndUser> ProviderEndUsers { get; set; }
        public DbSet<CardLimit> CardLimits { get; set; }
        public DbSet<CardLimitType> CardLimitTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}