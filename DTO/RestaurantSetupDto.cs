namespace restaurantAPI.DTO
{
    public class RestaurantSetupDto
    {
        public RestaurantDto Restaurant { get; set; }

        public List<RestaurantTableDto> Tables { get; set; }
    }
}
