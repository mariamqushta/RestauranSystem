using AutoMapper;
using restaurantAPI.DTO;
using restaurantAPI.DTO.Category;
using restaurantAPI.DTO.InventoryItem;
using restaurantAPI.DTO.logDto;
using restaurantAPI.DTO.MenuItem;
using restaurantAPI.DTO.Reservation;
using restaurantAPI.DTO.Review;
using restaurantAPI.models;
using RestaurantReservationSystem.Models;

namespace restaurantAPI.AutoMapper
{
    public class MapingProfile: Profile
    {
        public MapingProfile() {
            CreateMap<Restaurant, GetRestaurantDto>();
            CreateMap<RestaurantDto, Restaurant>();

            CreateMap<RestaurantTable, GetRestaurantTableDto>();
            CreateMap<RestaurantTableDto, RestaurantTable>();

            CreateMap<RegisterDto, ApplicationUser>()
           .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<MenuItemDto, MenuItem>();
            CreateMap<MenuItem, GetMenuItemDto>();

            CreateMap<CategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>();

            CreateMap<ReviewDto, Review>();
            CreateMap<Review, GetReviewDto>();

            CreateMap<ReservationDto, Reservation>();
            CreateMap<Reservation, GetReservationDto>();

            CreateMap<InventoryItemDto, InventoryItem>();
            CreateMap<InventoryItem, GetInventoryItemDto>();

        }
    }
}
