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
        public async Task<ActionResult<IEnumerable<WindDto>>> GetAllSensorData()
        {
            return await _service.GetAsync();
        }

        // GET: api/wind/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Wind>>> GetSensorData(int id)
        {
            var windDtos = await _service.GetSensorDataAsync(id);
            var wind = _mapper.Map<List<Wind>>(windDtos);

            return windDtos == null ? NotFound() : wind;
        }
    }
}