using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IRepositoryWrapper _repo;

        public GenreController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
            => View(_repo.Genre.FindAll().ToList());

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddUpdate", new Genre());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _repo.Genre.Create(genre);
                _repo.Save();
                TempData["Message"] = $"Genre '{genre.GenreName}' added.";
                return RedirectToAction("Index");
            }
            ViewBag.Action = "Add";
            return View("AddUpdate", genre);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Genre? genre = _repo.Genre.GetById(id);
            if (genre == null) return NotFound();
            ViewBag.Action = "Update";
            return View("AddUpdate", genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _repo.Genre.Update(genre);
                _repo.Save();
                TempData["Message"] = $"Genre '{genre.GenreName}' updated.";
                return RedirectToAction("Index");
            }
            ViewBag.Action = "Update";
            return View("AddUpdate", genre);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Genre? genre = _repo.Genre.GetById(id);
            if (genre == null) return NotFound();
            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int genreID)
        {
            Genre? genre = _repo.Genre.GetById(genreID);
            if (genre != null)
            {
                _repo.Genre.Delete(genre);
                _repo.Save();
                TempData["Message"] = $"Genre '{genre.GenreName}' deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}
