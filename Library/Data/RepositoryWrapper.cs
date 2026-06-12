namespace Library.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        private IBookRepository? _book;
        private IGenreRepository? _genre;

        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }

        public IBookRepository Book
        {
            get
            {
                _book ??= new BookRepository(_context);
                return _book;
            }
        }

        public IGenreRepository Genre
        {
            get
            {
                _genre ??= new GenreRepository(_context);
                return _genre;
            }
        }
        public void Save() => _context.SaveChanges();
    }

}
