using restaurantAPI.Models.Context;
using restaurantAPI.Repository;
using RestaurantReservationSystem.Models;

namespace restaurantAPI.UnitOfWork
{
    public class UnitWork
    {
        private readonly RestaurantDbContext _context;
         GenericRepository<Restaurant> _restaurantRepo;
         GenericRepository<RestaurantTable> _tableRepo;

        public UnitWork(RestaurantDbContext context)
        {
            _context = context;

        }

        public GenericRepository<Restaurant> Restaurantrepo {
            get
            {
                if (_restaurantRepo == null) {
                    _restaurantRepo = new GenericRepository<Restaurant>(_context);
                       }
                return _restaurantRepo;
            } 
        }

        public GenericRepository<RestaurantTable> Tablerepo
        {
            get
            {
                if (_tableRepo == null)
                {
                    _tableRepo = new GenericRepository<RestaurantTable>(_context);
                }
                return _tableRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
