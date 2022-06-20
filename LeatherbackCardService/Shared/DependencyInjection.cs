using Microsoft.Extensions.DependencyInjection;
using Shared.Encryption;
using Shared.Utility;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient(typeof(AESEncryption));
            services.AddTransient(typeof(RSAEncryption));
            return services;
        }
    }
}