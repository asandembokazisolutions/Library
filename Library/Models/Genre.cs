using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Genre
    {
        public int GenreID { get; set; }
        [Required(ErrorMessage = "Please enter a genre name.")]
        [Display(Name = "Genre")]
        public string GenreName { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = new();

    }
}
