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
        public async Task<ActionResult<IEnumerable<PressureDto>>> GetAllSensorData([FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            return await _service.GetAsync(parameters);
        }

        // GET: api/pressure/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Pressure>>> GetSensorData(int id, [FromQuery] QueryParameters parameters)
        {
            if (!parameters.ValidDateRange || !parameters.ValidValueRange)
            {
                return BadRequest("Invalid filtering parameters");
            }
            
            var pressureDtos = await _service.GetSensorDataAsync(id, parameters);
            var pressure = _mapper.Map<List<Pressure>>(pressureDtos);

            return pressureDtos == null ? NotFound() : pressure;
        }
    }
}