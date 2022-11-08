using System.Collections.Generic;
using System.Threading.Tasks;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb{

    public class HumidityService
    {
        private readonly IMongoCollection<Humidity> _collection;

        public HumidityService(MongoDbSettings mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<Humidity>(mongoDbSettings.CollectionName);
        }

        public async Task<List<Humidity>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<Humidity>.Empty).Result.ToListAsync();
        }

        public async Task InsertOneAsync(Humidity humidity)
        {
            await _collection.InsertOneAsync(humidity);
        }

        public async Task DeleteAsync(string id)
        {
        }
    }
}