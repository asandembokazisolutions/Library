using Library.Data.DataAccess;

namespace Library.Data
{
    public interface IRepositoryBase<T>
    {
        T? GetById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression);
        IEnumerable<T> GetWithOptions(QueryOptions<T> options);
        int CountWithOptions(QueryOptions<T> options);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
