namespace Library.Data
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }
        IGenreRepository Genre { get; }
        void Save();

    }
}
