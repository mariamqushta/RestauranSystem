using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO.MenuItem;
using restaurantAPI.models;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;
using System.Security.Claims;
namespace restaurantAPI.Controllers
{
    public class MenuItemController:ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public MenuItemController(UnitWork unit, IMapper mapper)
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
                var menuItems =
                _unitWork.MenuItemrepo.GetByCondition(
                    i=>i.Restaurant.OwnerId==userId&& i.RestaurantId==restaurantId);
                var dto =
                _mapper.Map<List<GetMenuItemDto>>(menuItems);
                return Ok(dto);
            }
            else
            {
                var menuItems =
                _unitWork.MenuItemrepo.GetByCondition(
                    i =>  i.RestaurantId == restaurantId);
                var dto =
                _mapper.Map<List<GetMenuItemDto>>(menuItems);
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

            var menuItem =
                _unitWork.MenuItemrepo.GetById(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    menuItem.RestaurantId);

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
                _mapper.Map<GetMenuItemDto>(menuItem);

            return Ok(dto);
        }


      
        [HttpPost]
        [Route("{restaurantId}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Create(
            int restaurantId,
            MenuItemDto dto)
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

            var category =
                _unitWork.Categoryrepo.GetById(dto.CategoryId);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            if (category.RestaurantId != restaurantId)
            {
                return BadRequest(
                    "Category does not belong to this restaurant.");
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(restaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            var newMenuItem =
                _mapper.Map<MenuItem>(dto);

            newMenuItem.RestaurantId = restaurantId;

            _unitWork.MenuItemrepo.add(newMenuItem);
            _unitWork.MenuItemrepo.Save();

            var result =
                _mapper.Map<GetMenuItemDto>(
                    newMenuItem);

            return Ok(result);
        }


       
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Update(
            int id,
            MenuItemDto dto)
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

            var category =
            _unitWork.Categoryrepo.GetById(dto.CategoryId);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var existingMenuItem =
                _unitWork.MenuItemrepo.GetById(id);

            if (existingMenuItem == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingMenuItem.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }


            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            if (category.RestaurantId != existingMenuItem.RestaurantId)
            {
                return BadRequest("Category does not belong to this restaurant.");
            }
            _mapper.Map(dto, existingMenuItem);

            _unitWork.MenuItemrepo.Edit(existingMenuItem);
            _unitWork.MenuItemrepo.Save();

            var result =
                _mapper.Map<GetMenuItemDto>(
                    existingMenuItem);

            return Ok(result);
        }


     
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

            var existingMenuItem =
                _unitWork.MenuItemrepo.GetById(id);

            if (existingMenuItem == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingMenuItem.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _unitWork.MenuItemrepo.Delete(id);
            _unitWork.MenuItemrepo.Save();

            return NoContent();
        }
    }
}
