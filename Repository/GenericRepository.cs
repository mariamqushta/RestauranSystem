using restaurantAPI.Models.Context;

namespace restaurantAPI.Repository
{
    public class GenericRepository<T>where T:class
    {
        private readonly RestaurantDbContext _context;

        public GenericRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public List<T> GetAll() {
            return _context.Set< T >().ToList();
        }

        public T GetById(int id) { 
            return _context.Set< T >().Find(id);
        }

        public void add(T entity)
        {
             _context.Set<T>().Add(entity);
        }

        public void Edit(T entity)
        {
            _context.Entry(entity).State=Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(int id)
        {
            T entity = _context.Set<T>().Find(id);
            if (entity != null) { 
            _context.Set<T>().Remove(entity); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
