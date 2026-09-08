using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantTableController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public RestaurantTableController(
            UnitWork unit,
            IMapper mapper)
        {
            _unitWork = unit;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Restaurant/{restaurantId}")]
        [Authorize]
        public IActionResult Get(int restaurantId)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            if (User.IsInRole("RestaurantOwner"))
            {
                var tables =
                    _unitWork.Tablerepo.GetByCondition(
                        t => t.RestaurantId == restaurantId &&
                             t.Restaurant.OwnerId == userId
                    );

                var dto =
                    _mapper.Map<List<GetRestaurantTableDto>>(tables);

                return Ok(dto);
            }
            else
            {
                var tables =
                    _unitWork.Tablerepo.GetByCondition(
                        t => t.RestaurantId == restaurantId &&
                             t.IsAvailable == true
                    );

                var dto =
                    _mapper.Map<List<GetRestaurantTableDto>>(tables);

                return Ok(dto);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult GetById(int id)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var table =
                _unitWork.Tablerepo.GetById(id);

            if (table == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    table.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            var dto =
                _mapper.Map<GetRestaurantTableDto>(table);

            return Ok(dto);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Create(RestaurantTableDto tableDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var restaurant = _unitWork.Restaurantrepo
                .GetById(tableDto.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            var table = _mapper.Map<RestaurantTable>(tableDto);

            _unitWork.Tablerepo.add(table);
            _unitWork.Save();

            var dto = _mapper.Map<GetRestaurantTableDto>(table);

            return Ok(dto);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Update(int id, RestaurantTableDto tableDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var existingTable = _unitWork.Tablerepo.GetById(id);

            if (existingTable == null)
            {
                return NotFound();
            }

            var restaurant = _unitWork.Restaurantrepo
                .GetById(existingTable.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _mapper.Map(tableDto, existingTable);

            _unitWork.Tablerepo.Edit(existingTable);
            _unitWork.Save();

            var dto = _mapper.Map<GetRestaurantTableDto>(existingTable);

            return Ok(dto);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var table = _unitWork.Tablerepo.GetById(id);

            if (table == null)
            {
                return NotFound();
            }

            var restaurant = _unitWork.Restaurantrepo
                .GetById(table.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _unitWork.Tablerepo.Delete(id);
            _unitWork.Save();

            return NoContent();
        }
    }
}