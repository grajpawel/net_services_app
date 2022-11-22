using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb
{

    public class PressureService
    {
        private readonly IMongoCollection<PressureDto> _collection;
        private readonly IMapper _mapper;

        public PressureService(MongoDbPressureSettings mongoDbSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<PressureDto>(mongoDbSettings.CollectionName);
        }

        public async Task<List<PressureDto>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<PressureDto>.Empty).Result.ToListAsync();
        }

        public async Task<List<PressureDto>> GetSensorDataAsync(int sensorId)
        {
            return await _collection.FindAsync(Builders<PressureDto>.Filter.Eq(pressure => pressure.SensorId, sensorId )).Result.ToListAsync();
        }
        public async Task InsertOneAsync(Pressure pressure)
        {
            var dto = _mapper.Map<PressureDto>(pressure);

            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<PressureDto>.IndexKeys.Ascending(field => field.SensorId).Descending(field => field.Time);
            var indexModel = new CreateIndexModel<PressureDto>(indexKeys, indexOptions);

            var ttlIndexKeysDefinition = Builders<PressureDto>.IndexKeys.Ascending(field => field.Time);
            var ttlIndexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(24, 0, 0) };
            var ttlIndexModel = new CreateIndexModel<PressureDto>(ttlIndexKeysDefinition, ttlIndexOptions);

            var indexList = new List<CreateIndexModel<PressureDto>>
            {
                indexModel,
                ttlIndexModel
            };

            await _collection.Indexes.CreateManyAsync(indexList);
            await _collection.InsertOneAsync(dto);
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}