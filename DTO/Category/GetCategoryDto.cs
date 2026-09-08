namespace restaurantAPI.DTO.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int RestaurantId { get; set; }
    }
}
