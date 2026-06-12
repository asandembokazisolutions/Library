using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Book> GetAllBooksWithGenre()
            => _context.Books.Include(b => b.Genre).ToList();

        public IEnumerable<Book> GetBooksByGenre(string genreName)
        {
            Genre? genre = _context.Genres
                .FirstOrDefault(g => g.GenreName.ToLower() == genreName.ToLower());
            if (genre == null) return new List<Book>();
            return _context.Books
                .Include(b => b.Genre)
                .Where(b => b.GenreID == genre.GenreID)
                .ToList();
        }
    }
}