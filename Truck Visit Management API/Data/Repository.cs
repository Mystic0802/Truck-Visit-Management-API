namespace Truck_Visit_Management_API.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        void Add(T item);
        T GetById(Func<T, bool> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _items = new List<T>();

        public IQueryable<T> GetAll()
        {
            return _items.AsQueryable();
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T GetById(Func<T, bool> predicate)
        {
            return _items.FirstOrDefault(predicate, null);
        }
    }
}
