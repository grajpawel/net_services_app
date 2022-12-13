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
        public async Task<ActionResult<IEnumerable<HumidityDto>>> GetAllSensorData([FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            return await _service.GetAsync(parameters);
        }

        // GET: api/humidity/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Humidity>>> GetSensorData(int id, [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            var humidityDtos = await _service.GetSensorDataAsync(id, parameters);
            var humidity = _mapper.Map<List<Humidity>>(humidityDtos);

            return humidityDtos == null ? NotFound() : humidity;
        }

        // GET: api/humidity/csv
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

            var data = FileHelper.HumidityToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }

        // GET: api/humidity/json
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
        
        //GET: api/humidity/3/csv
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

            var data = FileHelper.HumidityToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        // GET: api/humidity/2/json
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