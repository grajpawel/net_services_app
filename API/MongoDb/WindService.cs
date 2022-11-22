using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb
{

    public class WindService
    {
        private readonly IMongoCollection<WindDto> _collection;
        private readonly IMapper _mapper;

        public WindService(MongoDbWindSettings mongoDbSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<WindDto>(mongoDbSettings.CollectionName);
        }

        public async Task<List<WindDto>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<WindDto>.Empty).Result.ToListAsync();
        }

        public async Task<List<WindDto>> GetSensorDataAsync(int sensorId)
        {
            return await _collection.FindAsync(Builders<WindDto>.Filter.Eq(wind => wind.SensorId, sensorId )).Result.ToListAsync();
        }
        public async Task InsertOneAsync(Wind wind)
        {
            var dto = _mapper.Map<WindDto>(wind);

            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<WindDto>.IndexKeys.Ascending(field => field.SensorId).Descending(field => field.Time);
            var indexModel = new CreateIndexModel<WindDto>(indexKeys, indexOptions);

            var ttlIndexKeysDefinition = Builders<WindDto>.IndexKeys.Ascending(field => field.Time);
            var ttlIndexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(24, 0, 0) };
            var ttlIndexModel = new CreateIndexModel<WindDto>(ttlIndexKeysDefinition, ttlIndexOptions);

            var indexList = new List<CreateIndexModel<WindDto>>
            {
                indexModel,
                ttlIndexModel
            };

            await _collection.Indexes.CreateManyAsync(indexList);
            await _collection.InsertOneAsync(dto);
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}