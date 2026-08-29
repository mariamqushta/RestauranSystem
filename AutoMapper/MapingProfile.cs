using restaurantAPI.DTO;
using RestaurantReservationSystem.Models;
using AutoMapper;

namespace restaurantAPI.AutoMapper
{
    public class MapingProfile: Profile
    {
        public MapingProfile() {
            CreateMap<Restaurant, GetRestaurantDto>();
            CreateMap<RestaurantDto, Restaurant>();

            CreateMap<RestaurantTable, GetRestaurantTableDto>();
            CreateMap<RestaurantTableDto, RestaurantTable>();

        }
    }
}
