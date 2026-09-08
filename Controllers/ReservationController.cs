using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.DTO.Reservation;
using restaurantAPI.UnitOfWork;
using RestaurantReservationSystem.Models;
using RestaurantReservationSystem.Models.Enums;
using System.Security.Claims;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly UnitWork _unitWork;
        private readonly IMapper _mapper;

        public ReservationController(UnitWork unit, IMapper mapper)
        {
            _unitWork = unit;
            _mapper = mapper;
        }


        // =========================================================
        // GET: /Reservation
        // Customer  -> their own reservations
        // Owner     -> reservations of their restaurants
        // =========================================================

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            if (User.IsInRole("RestaurantOwner"))
            {
                var reservations =
                    _unitWork.Reservationrepo.GetByCondition(
                        r => r.Restaurant.OwnerId == userId
                    );

                var dto =
                    _mapper.Map<List<GetReservationDto>>(reservations);

                return Ok(dto);
            }
            else
            {
                var reservations =
                    _unitWork.Reservationrepo.GetByCondition(
                        r => r.UserId == userId
                    );

                var dto =
                    _mapper.Map<List<GetReservationDto>>(reservations);

                return Ok(dto);
            }
        }


        // =========================================================
        // GET: /Reservation/5
        // Customer -> own reservation
        // Owner    -> reservation belonging to their restaurant
        // =========================================================

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            var reservation =
                _unitWork.Reservationrepo.GetById(id);

            if (reservation == null)
                return NotFound("Reservation not found.");


            // Restaurant Owner
            if (User.IsInRole("RestaurantOwner"))
            {
                var restaurant =
                    _unitWork.Restaurantrepo.GetById(
                        reservation.RestaurantId
                    );

                if (restaurant == null)
                    return NotFound("Restaurant not found.");


                if (restaurant.OwnerId != userId)
                    return Forbid();
            }

            // Customer
            else
            {
                if (reservation.UserId != userId)
                    return Forbid();
            }


            var dto =
                _mapper.Map<GetReservationDto>(reservation);

            return Ok(dto);
        }


        // =========================================================
        // POST: /Reservation/3
        // Customer creates reservation for restaurant 3
        // =========================================================

        [HttpPost]
        [Route("{restaurantId}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Create(
            int restaurantId,
            ReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            // -----------------------------------------------------
            // 1. Check restaurant
            // -----------------------------------------------------

            var restaurant =
                _unitWork.Restaurantrepo.GetById(restaurantId);

            if (restaurant == null)
                return NotFound("Restaurant not found.");


            // -----------------------------------------------------
            // 2. Check table
            // -----------------------------------------------------

            var table =
                _unitWork.Tablerepo.GetById(
                    dto.RestaurantTableId
                );

            if (table == null)
                return NotFound("Table not found.");


            // -----------------------------------------------------
            // 3. Make sure table belongs to this restaurant
            // -----------------------------------------------------

            if (table.RestaurantId != restaurantId)
                return BadRequest(
                    "Table does not belong to this restaurant."
                );


            // -----------------------------------------------------
            // 4. Check number of guests
            // -----------------------------------------------------

            if (dto.NumberOfGuests > table.Capacity)
                return BadRequest(
                    "Number of guests exceeds table capacity."
                );


            // -----------------------------------------------------
            // 5. Check table availability
            // -----------------------------------------------------

            if (!table.IsAvailable)
                return BadRequest(
                    "This table is not available."
                );


            // -----------------------------------------------------
            // 6. Check reservation conflict
            // -----------------------------------------------------

            var existingReservation =
                _unitWork.Reservationrepo
                .GetAll()
                .Any(r =>
                    r.RestaurantTableId == dto.RestaurantTableId &&
                    r.ReservationDate == dto.ReservationDate &&
                    r.Status != ReservationStatus.Cancelled
                );


            if (existingReservation)
                return BadRequest(
                    "This table is already reserved at this time."
                );


            // -----------------------------------------------------
            // 7. Create reservation
            // -----------------------------------------------------

            var reservation =
                _mapper.Map<Reservation>(dto);


            // These values MUST come from the server
            reservation.UserId = userId;
            reservation.RestaurantId = restaurantId;
            reservation.Status = ReservationStatus.Pending;


            _unitWork.Reservationrepo.add(reservation);
            _unitWork.Reservationrepo.Save();


            var result =
                _mapper.Map<GetReservationDto>(reservation);

            return Ok(result);
        }


        // =========================================================
        // PUT: /Reservation/5
        // Customer updates their own reservation
        // =========================================================

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Update(
            int id,
            ReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            // -----------------------------------------------------
            // 1. Find existing reservation
            // -----------------------------------------------------

            var existingReservation =
                _unitWork.Reservationrepo.GetById(id);

            if (existingReservation == null)
                return NotFound("Reservation not found.");


            // -----------------------------------------------------
            // 2. Make sure customer owns reservation
            // -----------------------------------------------------

            if (existingReservation.UserId != userId)
                return Forbid();


            // -----------------------------------------------------
            // 3. Only Pending reservations can be updated
            // -----------------------------------------------------

            if (existingReservation.Status != ReservationStatus.Pending)
                return BadRequest(
                    "Only pending reservations can be updated."
                );


            // -----------------------------------------------------
            // 4. Check restaurant
            // -----------------------------------------------------

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    existingReservation.RestaurantId
                );

            if (restaurant == null)
                return NotFound("Restaurant not found.");


            // -----------------------------------------------------
            // 5. Check new table
            // -----------------------------------------------------

            var table =
                _unitWork.Tablerepo.GetById(
                    dto.RestaurantTableId
                );

            if (table == null)
                return NotFound("Table not found.");


            // -----------------------------------------------------
            // 6. Make sure table belongs to same restaurant
            // -----------------------------------------------------

            if (table.RestaurantId != existingReservation.RestaurantId)
                return BadRequest(
                    "Table does not belong to this restaurant."
                );


            // -----------------------------------------------------
            // 7. Check capacity
            // -----------------------------------------------------

            if (dto.NumberOfGuests > table.Capacity)
                return BadRequest(
                    "Number of guests exceeds table capacity."
                );


            // -----------------------------------------------------
            // 8. Check table availability
            // -----------------------------------------------------

            if (!table.IsAvailable)
                return BadRequest(
                    "This table is not available."
                );


            // -----------------------------------------------------
            // 9. Check reservation conflict
            // -----------------------------------------------------

            var conflictingReservation =
                _unitWork.Reservationrepo
                .GetAll()
                .Any(r =>
                    r.Id != id &&
                    r.RestaurantTableId == dto.RestaurantTableId &&
                    r.ReservationDate == dto.ReservationDate &&
                    r.Status != ReservationStatus.Cancelled
                );


            if (conflictingReservation)
                return BadRequest(
                    "This table is already reserved at this time."
                );


            // -----------------------------------------------------
            // 10. Update only fields customer is allowed to change
            // -----------------------------------------------------

            existingReservation.ReservationDate =
                dto.ReservationDate;

            existingReservation.NumberOfGuests =
                dto.NumberOfGuests;

            existingReservation.RestaurantTableId =
                dto.RestaurantTableId;


            // IMPORTANT:
            // We do NOT change:
            // UserId
            // RestaurantId
            // Status


            _unitWork.Reservationrepo.Edit(existingReservation);
            _unitWork.Reservationrepo.Save();


            var result =
                _mapper.Map<GetReservationDto>(
                    existingReservation
                );

            return Ok(result);
        }


        // =========================================================
        // PUT: /Reservation/5/status
        // Restaurant Owner changes reservation status
        // =========================================================

        [HttpPut]
        [Route("{id}/status")]
        [Authorize(Roles = "RestaurantOwner")]
        public IActionResult UpdateStatus(
            int id,
            ReservationStatus status)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            // -----------------------------------------------------
            // 1. Find reservation
            // -----------------------------------------------------

            var reservation =
                _unitWork.Reservationrepo.GetById(id);

            if (reservation == null)
                return NotFound("Reservation not found.");


            // -----------------------------------------------------
            // 2. Find reservation's restaurant
            // -----------------------------------------------------

            var restaurant =
                _unitWork.Restaurantrepo.GetById(
                    reservation.RestaurantId
                );

            if (restaurant == null)
                return NotFound("Restaurant not found.");


            // -----------------------------------------------------
            // 3. Make sure restaurant belongs to current owner
            // -----------------------------------------------------

            if (restaurant.OwnerId != userId)
                return Forbid();


            // -----------------------------------------------------
            // 4. Validate status transition
            // -----------------------------------------------------

            if (reservation.Status == ReservationStatus.Cancelled)
            {
                return BadRequest(
                    "A cancelled reservation cannot be changed."
                );
            }


            if (reservation.Status == ReservationStatus.Completed)
            {
                return BadRequest(
                    "A completed reservation cannot be changed."
                );
            }


            // -----------------------------------------------------
            // 5. Update status
            // -----------------------------------------------------

            reservation.Status = status;


            _unitWork.Reservationrepo.Edit(reservation);
            _unitWork.Reservationrepo.Save();


            var result =
                _mapper.Map<GetReservationDto>(
                    reservation
                );

            return Ok(result);
        }


        // =========================================================
        // DELETE: /Reservation/5
        // Customer cancels their reservation
        // =========================================================

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult Delete(int id)
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();


            // -----------------------------------------------------
            // 1. Find reservation
            // -----------------------------------------------------

            var reservation =
                _unitWork.Reservationrepo.GetById(id);

            if (reservation == null)
                return NotFound("Reservation not found.");


            // -----------------------------------------------------
            // 2. Make sure customer owns reservation
            // -----------------------------------------------------

            if (reservation.UserId != userId)
                return Forbid();


            // -----------------------------------------------------
            // 3. Cannot cancel completed reservation
            // -----------------------------------------------------

            if (reservation.Status == ReservationStatus.Completed)
                return BadRequest(
                    "A completed reservation cannot be cancelled."
                );


            // -----------------------------------------------------
            // 4. Cannot cancel an already cancelled reservation
            // -----------------------------------------------------

            if (reservation.Status == ReservationStatus.Cancelled)
                return BadRequest(
                    "Reservation is already cancelled."
                );


            // -----------------------------------------------------
            // 5. Soft delete = change status to Cancelled
            // -----------------------------------------------------

            reservation.Status =
                ReservationStatus.Cancelled;


            _unitWork.Reservationrepo.Edit(reservation);
            _unitWork.Reservationrepo.Save();


            return NoContent();
        }
    }
}

