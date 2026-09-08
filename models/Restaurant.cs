
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models;

public class Restaurant
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public TimeOnly ClosingTime { get; set; }

    // Restaurant Owner

    public string? OwnerId { get; set; }

    public ApplicationUser? Owner { get; set; }


    // Relationships

    public ICollection<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();
    public ICollection<Category> Categories { get; set; }
    = new List<Category>();

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}