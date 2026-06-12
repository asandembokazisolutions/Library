using Library.Models;

namespace Library.Data
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        IEnumerable<Book> GetAllBooksWithGenre();
        IEnumerable<Book> GetBooksByGenre(string genreName);
    }
}