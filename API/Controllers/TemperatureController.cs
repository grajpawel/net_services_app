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
    public class TemperatureController : ControllerBase
    {
        private readonly TemperatureService _service;
        private readonly IMapper _mapper;

        public TemperatureController(TemperatureService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/temperature
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemperatureDto>>> GetAllSensorData()
        {
            return await _service.GetAsync();
        }

        // GET: api/temperature/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Temperature>>> GetSensorData(int id)
        {
            var temperatureDtos = await _service.GetSensorDataAsync(id);
            var temperature = _mapper.Map<List<Temperature>>(temperatureDtos);

            return temperatureDtos == null ? NotFound() : temperature;
        }
    }
}