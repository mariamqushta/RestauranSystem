
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models;

public class Review
{
    public int Id { get; set; }


    [Range(1, 5)]
    public int Rating { get; set; }


    [MaxLength(500)]
    public string Comment { get; set; } = string.Empty;



    // User

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;



    // Restaurant

    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
}