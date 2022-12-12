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

    public class PressureService
    {
        private readonly IMongoCollection<PressureDto> _collection;
        private readonly IMapper _mapper;
        private readonly ISortHelper<PressureDto> _sortHelper;

        public PressureService(MongoDbPressureSettings mongoDbSettings, IMapper mapper, ISortHelper<PressureDto> sortHelper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<PressureDto>(mongoDbSettings.CollectionName);
            _sortHelper = sortHelper;
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

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PressureDto>> GetAsync(QueryParameters parameters)
        {
            var all = await GetAsync();
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }

        public async Task<List<PressureDto>> GetSensorDataAsync(int sensorId, QueryParameters parameters)
        {
            var all = await GetSensorDataAsync(sensorId);
            all = all.FindAll(x =>
                x.Time >= parameters.ReadAfter && x.Time <= parameters.ReadBefore && x.Value >= parameters.MinValue &&
                x.Value <= parameters.MaxValue);
            return _sortHelper.ApplySort(all.AsQueryable(), parameters.OrderBy).ToList();
        }
    }
}