using Microsoft.AspNetCore.Mvc;
using ProjectMovie.Models.Entities;
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
		public async Task<IActionResult> Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([Bind("Id,Title,Description,Author")] Movie movie)
		{
			if (ModelState.IsValid)
			{
				await _movieService.AddMovieAsync(movie);
				return RedirectToAction(nameof(Index));
			}
			return View(movie);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null) return NotFound();

			// Récupère le film par son ID en utilisant le service
			var movie = await _movieService.GetMovieByIdAsync(id.Value);
			if (movie == null) return NotFound();

			// Retourne la vue avec les détails du film
			return View(movie);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,Author")] Movie movie)
		{
			if (id != movie.Id) return NotFound();

			if (ModelState.IsValid)
			{
				await _movieService.UpdateMovieAsync(movie);
				return RedirectToAction(nameof(Index));
			}
			return View(movie);
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			await _movieService.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
