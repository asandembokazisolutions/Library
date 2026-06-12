using Library.Models;

namespace Library.Data
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
       public GenreRepository(AppDbContext context) : base(context) { }
    }
}