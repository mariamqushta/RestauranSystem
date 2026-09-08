namespace restaurantAPI.DTO.Review
{
       public class GetReviewDto
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public int RestaurantId { get; set; }
    }
}
