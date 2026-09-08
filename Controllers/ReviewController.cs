using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO.Review;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public ReviewController(UnitWork unit, IMapper mapper)
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

            var restaurant =
                _unitWork.Restaurantrepo.GetById(restaurantId);

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

            var reviews =
                _unitWork.Reviewrepo.GetByCondition(
                    r => r.RestaurantId == restaurantId
                );

            var dto =
                _mapper.Map<List<GetReviewDto>>(reviews);

            return Ok(dto);
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

            var review =
                _unitWork.Reviewrepo.GetById(id);

            if (review == null)
            {
                return NotFound();
            }

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    review.RestaurantId);

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
                _mapper.Map<GetReviewDto>(review);

            return Ok(dto);
        }


        [HttpPost]
        [Route("{restaurantId}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Create(int restaurantId, ReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

       
            var restaurant = _unitWork.Restaurantrepo.GetById(restaurantId);

            if (restaurant == null)
                return NotFound("Restaurant not found.");

       
            var review = _mapper.Map<Review>(dto);

            review.UserId = userId;
            review.RestaurantId = restaurantId;

            _unitWork.Reviewrepo.add(review);
            _unitWork.Reviewrepo.Save();

            var result = _mapper.Map<GetReviewDto>(review);

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Update(int id, ReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var existingReview = _unitWork.Reviewrepo.GetById(id);

            if (existingReview == null)
                return NotFound();

           
            if (existingReview.UserId != userId)
                return Forbid();

            _mapper.Map(dto, existingReview);

            _unitWork.Reviewrepo.Edit(existingReview);
            _unitWork.Reviewrepo.Save();

            var result = _mapper.Map<GetReviewDto>(existingReview);

            return Ok(result);
        }

  
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var existingReview = _unitWork.Reviewrepo.GetById(id);

            if (existingReview == null)
                return NotFound();

            
            if (existingReview.UserId != userId)
                return Forbid();

            _unitWork.Reviewrepo.Delete(id);
            _unitWork.Reviewrepo.Save();

            return NoContent();
        }
    }
}