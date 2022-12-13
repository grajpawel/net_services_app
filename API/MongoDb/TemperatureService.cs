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

    public class TemperatureService
    {
        private readonly IMongoCollection<TemperatureDto> _collection;
        private readonly IMapper _mapper;
        private readonly ISortHelper<TemperatureDto> _sortHelper;

        public TemperatureService(MongoDbTemperatureSettings mongoDbSettings, IMapper mapper, ISortHelper<TemperatureDto> sortHelper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<TemperatureDto>(mongoDbSettings.CollectionName);
            _sortHelper = sortHelper;
        }

        public async Task<List<TemperatureDto>> GetAsync()
        {
            return await _collection.FindAsync(FilterDefinition<TemperatureDto>.Empty).Result.ToListAsync();
        }

        public async Task<List<TemperatureDto>> GetSensorDataAsync(int sensorId)
        {
            return await _collection.FindAsync(Builders<TemperatureDto>.Filter.Eq(temperature => temperature.SensorId, sensorId)).Result.ToListAsync();
        }
        public async Task InsertOneAsync(Temperature temperature)
        {
            var dto = _mapper.Map<TemperatureDto>(temperature);

            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<TemperatureDto>.IndexKeys.Ascending(field => field.SensorId).Descending(field => field.Time);
            var indexModel = new CreateIndexModel<TemperatureDto>(indexKeys, indexOptions);

            var ttlIndexKeysDefinition = Builders<TemperatureDto>.IndexKeys.Ascending(field => field.Time);
            var ttlIndexOptions = new CreateIndexOptions { ExpireAfter = new TimeSpan(24, 0, 0) };
            var ttlIndexModel = new CreateIndexModel<TemperatureDto>(ttlIndexKeysDefinition, ttlIndexOptions);

            var indexList = new List<CreateIndexModel<TemperatureDto>>
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
        
        public async Task<List<TemperatureDto>> GetAsync(QueryParameters parameters)
        {
            var all = await GetAsync();
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }

        public async Task<List<TemperatureDto>> GetSensorDataAsync(int sensorId, QueryParameters parameters)
        {
            var all = await GetSensorDataAsync(sensorId);
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }
    }
}