using System.ComponentModel.DataAnnotations;

namespace restaurantAPI.DTO
{
    public class RestaurantDto{
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
