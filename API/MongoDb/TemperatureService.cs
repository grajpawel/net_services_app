using System.Collections.Generic;
using System.Threading.Tasks;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb{

    public class TemperatureService
    {
        private readonly IMongoCollection<Temperature> _collection;

        public TemperatureService(MongoDbSettings mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<Temperature>(mongoDbSettings.CollectionName);
        }

        public async Task<List<Temperature>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<Temperature>.Empty).Result.ToListAsync();
        }

        public async Task InsertOneAsync(Temperature temperature)
        {
            await _collection.InsertOneAsync(temperature);
        }

        public async Task DeleteAsync(string id)
        {
        }
    }
}