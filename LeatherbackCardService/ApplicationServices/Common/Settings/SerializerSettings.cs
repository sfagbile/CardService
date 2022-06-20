using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApplicationServices.Common.Settings
{
    public static class SerializerSettings
    {
        public static JsonSerializerSettings  GetSnakeCaseSerialisationSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } 
            };
            return settings;
        }
        
        public static JsonSerializerSettings  GetReferenceLoopHandlingSetting()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return settings;
        }
    }
}