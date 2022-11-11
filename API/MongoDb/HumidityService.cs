using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb
{

    public class HumidityService
    {
        private readonly IMongoCollection<HumidityDto> _collection;
        private readonly IMapper _mapper;

        public HumidityService(MongoDbHumiditySettings mongoDbSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<HumidityDto>(mongoDbSettings.CollectionName);
        }

        public async Task<List<HumidityDto>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<HumidityDto>.Empty).Result.ToListAsync();
        }

        public async Task<List<HumidityDto>> GetSensorDataAsync(int sensorId)
        {
            return await _collection.FindAsync(Builders<HumidityDto>.Filter.Eq(humidity => humidity.SensorId, sensorId )).Result.ToListAsync();
        }
        public async Task InsertOneAsync(Humidity humidity)
        {
            var dto = _mapper.Map<HumidityDto>(humidity);

            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<HumidityDto>.IndexKeys.Ascending(field => field.SensorId).Descending(field => field.Time);
            var indexModel = new CreateIndexModel<HumidityDto>(indexKeys, indexOptions);
            await _collection.Indexes.CreateOneAsync(indexModel);

            await _collection.InsertOneAsync(dto);
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}