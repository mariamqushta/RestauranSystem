namespace restaurantAPI.DTO.Reservation
{
    public class ReservationDto
    {
        public DateTime ReservationDate { get; set; }

        public int NumberOfGuests { get; set; }

        public int RestaurantTableId { get; set; }
    }
}
