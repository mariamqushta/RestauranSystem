using RestaurantReservationSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace restaurantAPI.models
{
    public class InventoryItem
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public decimal Quantity { get; set; }

        [Required]
        [StringLength(30)]
        public string Unit { get; set; }

        public decimal MinimumQuantity { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}
