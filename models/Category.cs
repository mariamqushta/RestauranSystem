using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystem.Models;

public class Category
{
    public int Id { get; set; }


    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;

    public ICollection<MenuItem> MenuItems { get; set; }
        = new List<MenuItem>();
}