using Microsoft.AspNetCore.Mvc;
using ProjectMovie.Models.Views;
using ProjectMovie.Services;
using System;
using System.Threading.Tasks;

namespace ProjectMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _movieService.GetAllMoviesAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                await _movieService.AddMovieAsync(movieViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(movieViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();

            var movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Author = movie.Author,
            };

            return View(movieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieViewModel movieViewModel)
        {
            if (id != movieViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _movieService.UpdateMovieAsync(movieViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(movieViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _movieService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
