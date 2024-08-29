using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovie.Models.Entities;
using ProjectMovie.Models.Views;
using ProjectMovie.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectMovie.Controllers
{
	public class MoviesController : Controller
	{
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(IMovieService movieService, IWebHostEnvironment webHostEnvironment)
        {
            _movieService = movieService;
            _webHostEnvironment = webHostEnvironment;
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
		public async Task<IActionResult> Add()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _movieService.AddMovieAsync(movieViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _movieService.MovieExistsAsync(movieViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If the ModelState is invalid, return the view with the current model to display errors
            return View(movieViewModel);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();

            // Convertir Movie en MovieViewModel
            var movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Author = movie.Author
            };

            return View(movieViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,Author")] MovieViewModel movieViewModel)
        {
            if (id != movieViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieService.UpdateMovieAsync(movieViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _movieService.MovieExistsAsync(movieViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(movieViewModel);
        }

        [HttpGet]
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await _movieService.GetMovieByIdAsync(id.Value);
			if (movie == null)
			{
				return NotFound();
			}

			return View(movie); // Charge la vue de confirmation de suppression avec les détails du film
		}

		[HttpPost,ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			await _movieService.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
