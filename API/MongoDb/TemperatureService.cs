﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using MessageGenerator.MessageBodies;
using MongoDB.Driver;

namespace API.MongoDb
{

    public class TemperatureService
    {
        private readonly IMongoCollection<TemperatureDto> _collection;
        private readonly IMapper _mapper;

        public TemperatureService(MongoDbTemperatureSettings mongoDbSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(mongoDbSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = database.GetCollection<TemperatureDto>(mongoDbSettings.CollectionName);
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
            await _collection.Indexes.CreateOneAsync(indexModel);

            await _collection.InsertOneAsync(dto);
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}