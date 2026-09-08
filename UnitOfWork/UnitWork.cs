using restaurantAPI.models;
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
        GenericRepository<InventoryItem> _inventoryRepo;
        GenericRepository<MenuItem> _menuItemrepo;
        GenericRepository<Category> _categoryrepo;
        GenericRepository<Reservation> _reservationrepo; 
        GenericRepository<Review> _reviewrepo;
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

        public GenericRepository<InventoryItem> Inventoryrepo
        {
            get
            {
                if (_inventoryRepo == null)
                {
                    _inventoryRepo = new GenericRepository<InventoryItem>(_context);
                }

                return _inventoryRepo;
            }
        }

        public GenericRepository<MenuItem> MenuItemrepo
        {
            get
            {
                if (_menuItemrepo == null)
                {
                    _menuItemrepo = new GenericRepository<MenuItem>(_context);
                }

                return _menuItemrepo;
            }
        }



        public GenericRepository<Category> Categoryrepo
        {
            get
            {
                if (_categoryrepo == null)
                {
                    _categoryrepo = new GenericRepository<Category>(_context);
                }

                return _categoryrepo;
            }
        }

        public GenericRepository<Reservation> Reservationrepo
        {
            get
            {
                if (_reservationrepo == null)
                {
                    _reservationrepo = new GenericRepository<Reservation>(_context);
                }

                return _reservationrepo;
            }
        }

        public GenericRepository<Review> Reviewrepo
        {
            get
            {
                if (_reviewrepo == null)
                {
                    _reviewrepo = new GenericRepository<Review>(_context);
                }

                return _reviewrepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
