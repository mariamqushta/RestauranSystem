
using RestaurantReservationSystem.Models.Enums;


namespace RestaurantReservationSystem.Models;

public class Reservation
{
    public int Id { get; set; }


    public DateTime ReservationDate { get; set; }


    public int NumberOfGuests { get; set; }


    public ReservationStatus Status { get; set; }


    // User

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;


    // Restaurant

    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;


    // Table

    public int RestaurantTableId { get; set; }

    public RestaurantTable RestaurantTable { get; set; } = null!;
}