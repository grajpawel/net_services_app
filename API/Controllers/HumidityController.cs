using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.MongoDb;
using AutoMapper;
using MessageGenerator.MessageBodies;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumidityController : ControllerBase
    {
        private readonly HumidityService _service;
        private readonly IMapper _mapper;

        public HumidityController(HumidityService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/humidity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HumidityDto>>> GetAllSensorData()
        {
            return await _service.GetAsync();
        }

        // GET: api/humidity/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Humidity>>> GetSensorData(int id)
        {
            var humidityDtos = await _service.GetSensorDataAsync(id);
            var humidity = _mapper.Map<List<Humidity>>(humidityDtos);

            return humidityDtos == null ? NotFound() : humidity;
        }
    }
}