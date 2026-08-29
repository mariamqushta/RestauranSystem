using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.Repository;
using RestaurantReservationSystem.Models;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantTableController : ControllerBase
    {
        private readonly GenericRepository<RestaurantTable> _repo;
        private readonly IMapper _mapper;

        public RestaurantTableController(
            GenericRepository<RestaurantTable> repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var tables = _repo.GetAll();

            var dto = _mapper.Map<List<GetRestaurantTableDto>>(tables);

            return Ok(dto);
        }



        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var table = _repo.GetById(id);

            if (table == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<GetRestaurantTableDto>(table);

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(RestaurantTableDto tableDto)
        {
            var table = _mapper.Map<RestaurantTable>(tableDto);

            _repo.add(table);
            _repo.Save();

            var dto = _mapper.Map<GetRestaurantTableDto>(table);

            return Ok(dto);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, RestaurantTableDto tableDto)
        {
            var existingTable = _repo.GetById(id);

            if (existingTable == null)
            {
                return NotFound();
            }

            _mapper.Map(tableDto, existingTable);

            _repo.Edit(existingTable);
            _repo.Save();

            var dto = _mapper.Map<GetRestaurantTableDto>(existingTable);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var table = _repo.GetById(id);

            if (table == null)
            {
                return NotFound();
            }

            _repo.Delete(id);
            _repo.Save();

            return NoContent();
        }
    }
}
