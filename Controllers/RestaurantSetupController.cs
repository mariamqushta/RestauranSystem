using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.Repository;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantSetupController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public RestaurantSetupController(UnitWork unit,IMapper mapper)
        {
            _unitWork = unit;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create(RestaurantSetupDto dto)
        {
            var restaurant =
                _mapper.Map<Restaurant>(dto.Restaurant);

            _unitWork.Restaurantrepo.add(restaurant);

            // Save Restaurant first so it gets an Id
            _unitWork.Save();

            foreach (var tableDto in dto.Tables)
            {
                var table =
                    _mapper.Map<RestaurantTable>(tableDto);

                table.RestaurantId = restaurant.Id;

                _unitWork.Tablerepo.add(table);
            }

            // Save all tables
            _unitWork.Save();

            var result = _mapper.Map<GetRestaurantDto>(restaurant);

            return Ok(result);
        }
    }
}