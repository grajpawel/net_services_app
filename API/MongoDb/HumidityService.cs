using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Parameters;
using AutoMapper;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb
{

    public class HumidityService
    {
        private readonly IMongoCollection<HumidityDto> _collection;
        private readonly IMapper _mapper;
        private readonly ISortHelper<HumidityDto> _sortHelper;

        public HumidityService(MongoDbHumiditySettings mongoDbSettings, IMapper mapper, ISortHelper<HumidityDto> sortHelper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<HumidityDto>(mongoDbSettings.CollectionName);
            _sortHelper = sortHelper;
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

            var ttlIndexKeysDefinition = Builders<HumidityDto>.IndexKeys.Ascending(field => field.Time);
            var ttlIndexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(24, 0, 0) };
            var ttlIndexModel = new CreateIndexModel<HumidityDto>(ttlIndexKeysDefinition, ttlIndexOptions);

            var indexList = new List<CreateIndexModel<HumidityDto>>
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
        
        public async Task<List<HumidityDto>> GetAsync(QueryParameters parameters)
        {
            var all = await GetAsync();
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }

        public async Task<List<HumidityDto>> GetSensorDataAsync(int sensorId, QueryParameters parameters)
        {
            var all = await GetSensorDataAsync(sensorId);
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }
    }
}
