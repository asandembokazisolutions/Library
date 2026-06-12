using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an author.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a year.")]
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a genre.")]
        public int GenreID { get; set; }

        // Navigation property
        public Genre? Genre { get; set; }
    }

}
