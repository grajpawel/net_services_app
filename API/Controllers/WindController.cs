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
    public class WindController : ControllerBase
    {
        private readonly WindService _service;
        private readonly IMapper _mapper;

        public WindController(WindService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/wind
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WindDto>>> GetAllSensorData([FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            return await _service.GetAsync(parameters);
        }

        // GET: api/wind/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Wind>>> GetSensorData(int id, [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            var windDtos = await _service.GetSensorDataAsync(id, parameters);
            var wind = _mapper.Map<List<Wind>>(windDtos);

            return windDtos == null ? NotFound() : wind;
        }
        
        [HttpGet("csv")]
        public async Task<IActionResult> GetCsvFile([FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.csv";
            const string mimeType = "text/csv";
            
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetAsync(parameters);

            var data = FileHelper.WindToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }

        [HttpGet("json")]
        public async Task<IActionResult> GetJsonFile([FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.json";
            const string mimeType = "text/json";
            
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetAsync(parameters);

            var data = FileHelper.ListToJson(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        //GET: api/wind/3/csv
        [HttpGet("{id:int}/csv")]
        public async Task<IActionResult> GetCsvFile(int id, [FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.csv";
            const string mimeType = "text/csv";
            
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            var dtos = await _service.GetSensorDataAsync(id, parameters);

            var data = FileHelper.WindToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        // GET: api/wind/2/json
        [HttpGet("{id:int}/json")]
        public async Task<IActionResult> GetJsonFile(int id, [FromQuery] QueryParameters parameters)
        {
            const string fileName = "data.json";
            const string mimeType = "text/json";
            
            if (!parameters.ValidDateRange || !parameters.ValidDirectionRange || !parameters.ValidSpeedRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var dtos = await _service.GetSensorDataAsync(id, parameters);

            var data = FileHelper.ListToJson(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
    }
}