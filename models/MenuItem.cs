
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models;

public class MenuItem
{
    public int Id { get; set; }


    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;


    public decimal Price { get; set; }


    public string? ImageUrl { get; set; }



    // Restaurant

    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;



    // Category

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}