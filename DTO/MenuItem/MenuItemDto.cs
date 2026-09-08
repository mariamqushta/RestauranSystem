namespace restaurantAPI.DTO.MenuItem
{
    public class MenuItemDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
    }
}
