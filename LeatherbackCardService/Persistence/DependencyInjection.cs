using System;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repository;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("CardServiceConnection");
            services.AddDbContextPool<CardServiceDbContext>(options =>
	            {
		            options.UseSqlServer(connectionString,
			            b => b.MigrationsAssembly(typeof(CardServiceDbContext).Assembly.FullName));
		            options.LogTo(Console.WriteLine);
	            }
				);
            
            services.AddScoped<ICardServiceDbContext>(provider => provider.GetService<CardServiceDbContext>());
            services.AddTransient<ICardsRepository, CardRepository>();
	      
            return services;
		}
    }
}