using Library.Data;
using Library.Data.DataAccess;
using Library.Models;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private const int PageSize = 4;

        public BookController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public IActionResult Index(string sortBy = "Title", string searchString = "",
            string genre = "all", int page = 1)
        {
            ViewData["TitleSortParam"] = sortBy == "Title" ? "Title_desc" : "Title";
            ViewData["AuthorSortParam"] = sortBy == "Author" ? "Author_desc" : "Author";
            ViewData["PriceSortParam"] = sortBy == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;
            ViewData["SelectedGenre"] = genre;
            ViewBag.Genres = _repo.Genre.FindAll();

            Expression<Func<Book, object>> orderBy;
            string direction;

            if (sortBy.EndsWith("_desc"))
            {
                sortBy = sortBy[..^5];
                direction = "desc";
            }
            else direction = "asc";

            orderBy = p => EF.Property<object>(p, sortBy);

            Expression<Func<Book, bool>>? where = null;

            if (!string.IsNullOrEmpty(searchString) && genre != "all")
                where = b => (b.Title.Contains(searchString) || b.Author.Contains(searchString))
                             && b.Genre!.GenreName == genre;
            else if (!string.IsNullOrEmpty(searchString))
                where = b => b.Title.Contains(searchString) || b.Author.Contains(searchString);
            else if (genre != "all")
                where = b => b.Genre!.GenreName == genre;

            var countOptions = new QueryOptions<Book> { Where = where };
            int totalBooks = _repo.Book.CountWithOptions(countOptions);

            var books = _repo.Book.GetWithOptions(new QueryOptions<Book>
            {
                OrderBy = orderBy,
                OrderByDirection = direction,
                Where = where,
                PageNumber = page,
                PageSize = PageSize
            });

            return View(new BookListViewModel
            {
                Books = books,
                SelectedGenre = genre,
                PagingInfo = new PagingInfoViewModel
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalBooks
                }
            });
        }

        public IActionResult Details(int id)
        {
            Book? book = _repo.Book.GetAllBooksWithGenre()
                .FirstOrDefault(b => b.BookID == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [Authorize]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            PopulateDDL(new Book());
            return View("AddUpdate", new Book());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                _repo.Book.Create(book);
                _repo.Save();
                TempData["Message"] = $"'{book.Title}' added successfully.";
                return RedirectToAction("Index");
            }
            ViewBag.Action = "Add";
            PopulateDDL(book);
            return View("AddUpdate", book);
        }

        [Authorize]
        public IActionResult Update(int id)
        {
            Book? book = _repo.Book.GetById(id);
            if (book == null) return NotFound();
            ViewBag.Action = "Update";
            PopulateDDL(book);
            return View("AddUpdate", book);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Book book)
        {
            if (ModelState.IsValid)
            {
                _repo.Book.Update(book);
                _repo.Save();
                TempData["Message"] = $"'{book.Title}' updated successfully.";
                return RedirectToAction("Index");
            }
            ViewBag.Action = "Update";
            PopulateDDL(book);
            return View("AddUpdate", book);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Book? book = _repo.Book.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int bookID)
        {
            Book? book = _repo.Book.GetById(bookID);
            if (book != null)
            {
                _repo.Book.Delete(book);
                _repo.Save();
                TempData["Message"] = $"'{book.Title}' deleted.";
            }
            return RedirectToAction("Index");
        }

        private void PopulateDDL(Book book)
        {
            ViewBag.Genres = new SelectList(
                _repo.Genre.FindAll(), "GenreID", "GenreName", book.GenreID);
        }
    }
}
