using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.MongoDb;
using API.Parameters;
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
        public async Task<ActionResult<IEnumerable<TemperatureDto>>> GetAllSensorData([FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            return await _service.GetAsync(parameters);
        }

        // GET: api/temperature/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Temperature>>> GetSensorData(int id, [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            var temperatureDtos = await _service.GetSensorDataAsync(id, parameters);
            var temperature = _mapper.Map<List<Temperature>>(temperatureDtos);

            return temperatureDtos == null ? NotFound() : temperature;
        }
    }
}