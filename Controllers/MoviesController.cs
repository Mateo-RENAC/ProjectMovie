using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovie.Models.Entities;
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
        public async Task<IActionResult> Add([Bind("Id,Title,Description,Author")] Movie movie, IFormFile Picture)
        {
            if (ModelState.IsValid)
            {
                if (Picture != null && Picture.Length > 0)
                {
                    // Vérification du type MIME de l'image
                    var permittedExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif", ".bmp", ".tiff", ".webp" };
                    var extension = Path.GetExtension(Picture.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Picture", "Invalid file type. Only JPEG, PNG, GIF, BMP, TIFF, and WebP are allowed.");
                        return View(movie);
                    }

                    // Assure que le dossier où les images seront stockées existe
                    var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Génération d'un nom de fichier unique pour éviter les conflits
                    var fileName = Path.GetFileNameWithoutExtension(Picture.FileName);
                    var fileExtension = Path.GetExtension(Picture.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(imagesPath, uniqueFileName);

                    // Sauvegarde du fichier
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Picture.CopyToAsync(stream);
                    }

                    // Mise à jour du chemin de l'image dans le modèle
                    movie.PathPicture = "/images/" + uniqueFileName;
                }

                // Ajout du film à la base de données
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

		[HttpPost,ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			await _movieService.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
