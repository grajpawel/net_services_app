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
    public class PressureController : ControllerBase
    {
        private readonly PressureService _service;
        private readonly IMapper _mapper;

        public PressureController(PressureService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/pressure
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PressureDto>>> GetAllSensorData(
            [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            return await _service.GetAsync(parameters);
        }

        // GET: api/pressure/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Pressure>>> GetSensorData(int id,
            [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }

            var pressureDtos = await _service.GetSensorDataAsync(id, parameters);
            var pressure = _mapper.Map<List<Pressure>>(pressureDtos);

            return pressureDtos == null ? NotFound() : pressure;
        }

        //GET: api/pressure/csv
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

            var data = FileHelper.PressureToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }

        //GET: api/pressure/json
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
        
        //GET: api/pressure/3/csv
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

            var data = FileHelper.PressureToCsv(dtos);

            return File(Encoding.UTF8.GetBytes(data), mimeType, fileName);
        }
        
        // GET: api/pressure/2/json
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