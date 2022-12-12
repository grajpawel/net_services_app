using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
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
        
        //GET: /api/temperature/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetCsvFile([FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.csv";
            const string mimeType = "text/csv";
            
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetAsync(parameters);

            var data = FileHelper.TemperatureToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }

        //GET: /api/temperature/json
        [HttpGet("json")]
        public async Task<IActionResult> GetJsonFile([FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.json";
            const string mimeType = "text/json";
            
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetAsync(parameters);

            var data = FileHelper.ListToJson(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        //GET: api/temperature/3/csv
        [HttpGet("{id:int}/csv")]
        public async Task<IActionResult> GetCsvFile(int id, [FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.csv";
            const string mimeType = "text/csv";
            
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetSensorDataAsync(id, parameters);

            var data = FileHelper.TemperatureToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        // GET: api/temperature/2/json
        [HttpGet("{id:int}/json")]
        public async Task<IActionResult> GetJsonFile(int id, [FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.json";
            const string mimeType = "text/json";
            
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetSensorDataAsync(id, parameters);

            var data = FileHelper.ListToJson(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
    }
}