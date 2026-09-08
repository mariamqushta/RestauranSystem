using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO.InventoryItem;
using restaurantAPI.models;
using restaurantAPI.UnitOfWork;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public InventoryController(UnitWork unit, IMapper mapper)
        {
            _unitWork = unit;
            _mapper = mapper;
        }


        // GET: /Inventory
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Get()
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var inventoryItems =
                _unitWork.Inventoryrepo.GetByCondition(
                    i => i.Restaurant.OwnerId == userId
                );

            var dto =
                _mapper.Map<List<GetInventoryItemDto>>(inventoryItems);

            return Ok(dto);
        }


        // GET: /Inventory/{id}
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

            var inventoryItems =
                _unitWork.Inventoryrepo.GetByCondition(
                    i => i.ID == id &&
                    i.Restaurant.OwnerId == userId
                );

            var inventoryItem = inventoryItems.FirstOrDefault();

            if (inventoryItem == null)
            {
                return NotFound();
            }

            var dto =
                _mapper.Map<GetInventoryItemDto>(inventoryItem);

            return Ok(dto);
        }


        // POST: /Inventory/{restaurantId}
        [HttpPost]
        [Route("{restaurantId}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Create(
            int restaurantId,
            InventoryItemDto dto)
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

            var newInventoryItem =
                _mapper.Map<InventoryItem>(dto);

            newInventoryItem.RestaurantId = restaurantId;

            _unitWork.Inventoryrepo.add(newInventoryItem);
            _unitWork.Inventoryrepo.Save();

            var result =
                _mapper.Map<GetInventoryItemDto>(
                    newInventoryItem);

            return Ok(result);
        }


        // PUT: /Inventory/{id}
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Update(
            int id,
            InventoryItemDto dto)
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

            var existingInventoryItem =
                _unitWork.Inventoryrepo.GetById(id);

            if (existingInventoryItem == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingInventoryItem.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _mapper.Map(dto, existingInventoryItem);

            _unitWork.Inventoryrepo.Edit(existingInventoryItem);
            _unitWork.Inventoryrepo.Save();

            var result =
                _mapper.Map<GetInventoryItemDto>(
                    existingInventoryItem);

            return Ok(result);
        }


        // DELETE: /Inventory/{id}
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

            var existingInventoryItem =
                _unitWork.Inventoryrepo.GetById(id);

            if (existingInventoryItem == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingInventoryItem.RestaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _unitWork.Inventoryrepo.Delete(id);
            _unitWork.Inventoryrepo.Save();

            return NoContent();
        }
    }
}