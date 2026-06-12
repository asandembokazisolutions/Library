using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            if (!context.Genres.Any())
            {
                context.Genres.AddRange(
                    new Genre { GenreName = "Fiction" },
                    new Genre { GenreName = "Non-Fiction" },
                    new Genre { GenreName = "Science" },
                    new Genre { GenreName = "History" }
                );
                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", YearPublished = 1925, Price = 12.99m, GenreID = 1 },
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", YearPublished = 1960, Price = 14.99m, GenreID = 1 },
                    new Book { Title = "1984", Author = "George Orwell", YearPublished = 1949, Price = 11.99m, GenreID = 1 },
                    new Book { Title = "Brave New World", Author = "Aldous Huxley", YearPublished = 1932, Price = 13.50m, GenreID = 1 },
                    new Book { Title = "Sapiens", Author = "Yuval Noah Harari", YearPublished = 2011, Price = 18.99m, GenreID = 2 },
                    new Book { Title = "Educated", Author = "Tara Westover", YearPublished = 2018, Price = 16.99m, GenreID = 2 },
                    new Book { Title = "A Brief History of Time", Author = "Stephen Hawking", YearPublished = 1988, Price = 15.00m, GenreID = 3 },
                    new Book { Title = "The Selfish Gene", Author = "Richard Dawkins", YearPublished = 1976, Price = 13.99m, GenreID = 3 },
                    new Book { Title = "Guns, Germs, and Steel", Author = "Jared Diamond", YearPublished = 1997, Price = 17.50m, GenreID = 4 },
                    new Book { Title = "The History of the Ancient World", Author = "Susan Wise Bauer", YearPublished = 2007, Price = 20.00m, GenreID = 4 }
                );
                context.SaveChanges();
            }
        }
    }
}
