using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Transactions;
using Domain.Interfaces;

namespace Persistence.DbContextSeeds
{
    public class DbContextSeed
    {
        public static async Task SeedInitialAsync(ICardServiceDbContext context)
        {
            await SeedTransactionLimitType(context);
        }
        
        private static async Task SeedTransactionLimitType(ICardServiceDbContext context)
        {
            if (context.CardLimitTypes.Any())
                return;
            
            await context.CardLimitTypes.AddAsync( CardLimitType.CreateInstance("Daily", "Daily Transaction Limit").Value);
            await context.CardLimitTypes.AddAsync( CardLimitType.CreateInstance("Weekly", "Weekly Transaction Limit").Value);
            await context.CardLimitTypes.AddAsync( CardLimitType.CreateInstance("Monthly", "Monthly Transaction Limit").Value);
            
            await context.SaveChangesAsync();
        }

    }
}