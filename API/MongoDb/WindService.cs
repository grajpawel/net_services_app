using System.Collections.Generic;
using System.Threading.Tasks;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb{

    public class WindService
    {
        private readonly IMongoCollection<Wind> _collection;

        public WindService(MongoDbSettings mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<Wind>(mongoDbSettings.CollectionName);
        }

        public async Task<List<Wind>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<Wind>.Empty).Result.ToListAsync();
        }

        public async Task InsertOneAsync(Wind wind)
        {
            await _collection.InsertOneAsync(wind);
        }

        public async Task DeleteAsync(string id)
        {
        }
    }
}