namespace restaurantAPI.DTO
{
    public class RestaurantTableDto
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public int RestaurantId { get; set; }
    }
}
