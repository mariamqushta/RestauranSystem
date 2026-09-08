using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO;
using restaurantAPI.Models.Context;
using restaurantAPI.Repository;
using RestaurantReservationSystem.Models;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        //private readonly RestaurantDbContext _context;
        private readonly GenericRepository<Restaurant> _Repo;
        private readonly IMapper _mapper;
        public RestaurantController(GenericRepository<Restaurant> Repo, IMapper mapper)
        {
            //_context = context;
            _Repo = Repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = "myscheme")]
        public IActionResult Get()
        {
            //var restaurants = _context.Restaurants
            //    .Select(r => new GetRestaurantDto
            //    {
            //        Id = r.Id,
            //        Name = r.Name,
            //        Address = r.Address,
            //        PhoneNumber = r.PhoneNumber
            //    })
            //    .ToList();
            var restaurants = _mapper.Map<List<GetRestaurantDto>>(_Repo.GetAll());

            return Ok(restaurants);

        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IActionResult Getbyid(int id)
        {
            //var restaurants = _context.Restaurants.Where(r => r.Id==id)
            //    .Select(r => new GetRestaurantDto
            //    {
            //        Id = r.Id,
            //        Name = r.Name,
            //        Address = r.Address,
            //        PhoneNumber = r.PhoneNumber
            //    }).FirstOrDefault();
            var restaurants = _Repo.GetById(id);
            if (restaurants == null) { return NotFound(); }
            var dto = _mapper.Map<GetRestaurantDto>(restaurants);


            return Ok(dto);

        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Create(RestaurantDto restaurant )

        {
            //var newRestaurant = new Restaurant
            //{
            //    Name = restaurant.Name,
            //    Address = restaurant.Address,
            //    PhoneNumber = restaurant.PhoneNumber
            //};
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var newRestaurant = _mapper.Map<Restaurant>(restaurant);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            newRestaurant.OwnerId = userId;
            _Repo.add(newRestaurant);
            _Repo.Save();

            return Ok(newRestaurant);


        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult update(int id ,RestaurantDto restaurant)

        {
            //var newRestaurant = _context.Restaurants.Find(id);
            //if (newRestaurant == null) { return NotFound(); }
            ////newRestaurant.Name=restaurant.Name;
            ////newRestaurant.Address=restaurant.Address;
            ////newRestaurant.PhoneNumber=restaurant.PhoneNumber;
            //_mapper.Map(restaurant, newRestaurant);
            var existingRestaurant = _Repo.GetById(id);
            if (existingRestaurant == null)
            {
                return NotFound();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (existingRestaurant.OwnerId != userId)
            {
                return Forbid();
            }
            _mapper.Map(restaurant, existingRestaurant);
            _Repo.Edit(existingRestaurant);
            _Repo.Save();

            return Ok(existingRestaurant);


        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult Delete(int id)

        {
            var existingRestaurant = _Repo.GetById(id);

            if (existingRestaurant == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (existingRestaurant.OwnerId != userId)
            {
                return Forbid();
            }

            _Repo.Delete(id);
            _Repo.Save();

            return NoContent();


        }


    }
}