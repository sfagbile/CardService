using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.JsonSerializerConfig
{
    public static class JsonSerializationConfiguration
    {
        public static JsonSerializerSettings  GetSnakeCaseSerialisationSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } 
            };
            return settings;
        }
    }
}