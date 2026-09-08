namespace restaurantAPI.DTO.InventoryItem
{
    public class GetInventoryItemDto
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public decimal MinimumQuantity { get; set; }

        public int RestaurantId { get; set; }
    }
}
