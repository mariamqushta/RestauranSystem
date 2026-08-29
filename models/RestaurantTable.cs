
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models;

public class RestaurantTable
{
    public int Id { get; set; }


    [Required]
    public int TableNumber { get; set; }


    [Required]
    public int Capacity { get; set; }


    // Foreign Key

    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;


    public ICollection<Reservation> Reservations { get; set; }
        = new List<Reservation>();
}