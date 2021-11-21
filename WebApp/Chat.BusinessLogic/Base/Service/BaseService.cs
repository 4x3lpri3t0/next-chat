using System.Threading.Tasks;
using Chat.BusinessLogic.Base.Service.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace Chat.BusinessLogic.Base.Service
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IDatabase _database;
        protected readonly IConnectionMultiplexer _redis;

        public BaseService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        protected async Task PublishMessage<T>(string type, T data)
        {
            // Quick way to handle json type serialization.
            string jsonData = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            // AXEL TODO: Remove this (debugging purposes)
            //var jsonData = JsonConvert.DeserializeObject<JsonDocument>(dataString);

            PubSubMessage pubSubMessage = new PubSubMessage()
            {
                Type = type,
                Data = jsonData
            };

            await PublishMessage(pubSubMessage);
        }

        private async Task PublishMessage(PubSubMessage pubSubMessage)
        {
            await _database.PublishAsync("MESSAGES", JsonConvert.SerializeObject(pubSubMessage));
        }
    }

    public class PubSubMessage
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public string ServerId { get; set; } = "123";
    }
}