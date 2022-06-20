using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CardServiceDbContext>
    {
        public CardServiceDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional:true)
                .Build();
            var builder = new DbContextOptionsBuilder<CardServiceDbContext>();
            var connectionString = configuration.GetConnectionString("CardServiceConnection");
            builder.UseSqlServer(connectionString);
            return new CardServiceDbContext(builder.Options);
        }
    }
}