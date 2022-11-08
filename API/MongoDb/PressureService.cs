using System.Collections.Generic;
using System.Threading.Tasks;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb{

    public class PressureService
    {
        private readonly IMongoCollection<Pressure> _collection;

        public PressureService(MongoDbSettings mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<Pressure>(mongoDbSettings.CollectionName);
        }

        public async Task<List<Pressure>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<Pressure>.Empty).Result.ToListAsync();
        }

        public async Task InsertOneAsync(Pressure pressure)
        {
            await _collection.InsertOneAsync(pressure);
        }

        public async Task DeleteAsync(string id)
        {
        }
    }
}