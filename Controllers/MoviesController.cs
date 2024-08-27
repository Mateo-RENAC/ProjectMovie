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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			await _movieService.DeleteMovieAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
