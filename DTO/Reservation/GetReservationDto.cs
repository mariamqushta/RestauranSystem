using RestaurantReservationSystem.Models.Enums;

namespace restaurantAPI.DTO.Reservation
{
    public class GetReservationDto
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfGuests { get; set; }

        public ReservationStatus Status { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int RestaurantId { get; set; }

        public int RestaurantTableId { get; set; }
    }
}