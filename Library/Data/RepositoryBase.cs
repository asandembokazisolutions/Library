using Library.Data.DataAccess;
using System.Linq.Expressions;

namespace Library.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public void Create(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public T? GetById(int id) => _context.Set<T>().Find(id);

        public IEnumerable<T> FindAll() => _context.Set<T>().ToList();

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
            => _context.Set<T>().Where(expression).ToList();

        public IEnumerable<T> GetWithOptions(QueryOptions<T> options)
        {
            IQueryable<T> query = _context.Set<T>();

            if (options.HasWhere)
                query = query.Where(options.Where!);
            if (options.HasOrderBy)
            {
                query = options.OrderByDirection == "asc"
                    ? query.OrderBy(options.OrderBy!)
                    : query.OrderByDescending(options.OrderBy!);
            }

            if (options.HasPaging)
                query = query.Skip((options.PageNumber - 1) * options.PageSize)
                             .Take(options.PageSize);

            return query.ToList();
        }

        public int CountWithOptions(QueryOptions<T> options)
        {
            IQueryable<T> query = _context.Set<T>();
            if (options.HasWhere)
                query = query.Where(options.Where!);
            return query.Count();
        }
    }
}

