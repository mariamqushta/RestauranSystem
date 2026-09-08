using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO.Category;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public CategoryController(UnitWork unit, IMapper mapper)
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
                var categories =
                    _unitWork.Categoryrepo.GetByCondition(
                        c => c.RestaurantId == restaurantId &&
                             c.Restaurant.OwnerId == userId
                    );

                var dto =
                    _mapper.Map<List<GetCategoryDto>>(categories);

                return Ok(dto);
            }
            else
            {
                var categories =
                    _unitWork.Categoryrepo.GetByCondition(
                        c => c.RestaurantId == restaurantId
                    );

                var dto =
                    _mapper.Map<List<GetCategoryDto>>(categories);

                return Ok(dto);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var category =
                _unitWork.Categoryrepo.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    category.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (User.IsInRole("RestaurantOwner"))
            {
                if (restaurant.OwnerId != userId)
                {
                    return Forbid();
                }
            }

            var dto =
                _mapper.Map<GetCategoryDto>(category);

            return Ok(dto);
        }


        // =====================================================
        // CREATE CATEGORY
        // POST: /Category/{restaurantId}
        // =====================================================

        [HttpPost]
        [Route("{restaurantId}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Create(
            int restaurantId,
            CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }


            // Find the restaurant
            var restaurant =
                _unitWork.Restaurantrepo.GetById(restaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }


            // Check that the logged-in owner owns
            // this restaurant.
            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }


            // Create category
            var category =
                _mapper.Map<Category>(dto);

            // Connect category to restaurant
            category.RestaurantId = restaurantId;


            _unitWork.Categoryrepo.add(category);
            _unitWork.Categoryrepo.Save();


            var result =
                _mapper.Map<GetCategoryDto>(category);

            return Ok(result);
        }


        // =====================================================
        // UPDATE CATEGORY
        // PUT: /Category/{id}
        // =====================================================

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Update(
            int id,
            CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }


            // Find category
            var existingCategory =
                _unitWork.Categoryrepo.GetById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }


            // Find the restaurant that owns this category
            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingCategory.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }


            // Check ownership
            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }


            // Update only the fields from DTO.
            // RestaurantId will not change.
            _mapper.Map(dto, existingCategory);


            _unitWork.Categoryrepo.Edit(existingCategory);
            _unitWork.Categoryrepo.Save();


            var result =
                _mapper.Map<GetCategoryDto>(
                    existingCategory);

            return Ok(result);
        }


        // =====================================================
        // DELETE CATEGORY
        // DELETE: /Category/{id}
        // =====================================================

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Delete(int id)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }


            // Find category
            var existingCategory =
                _unitWork.Categoryrepo.GetById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }


            // Find its restaurant
            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingCategory.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }


            // Check ownership
            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }


            _unitWork.Categoryrepo.Delete(id);
            _unitWork.Categoryrepo.Save();

            return NoContent();
        }
    }
}
