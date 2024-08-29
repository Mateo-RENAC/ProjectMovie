using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ProjectMovie.Data;
using ProjectMovie.Models.Entities;
using ProjectMovie.Models.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMovie.Services
{
	public class MovieService : IMovieService
	{
		private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

		public MovieService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
            _webHostEnvironment = webHostEnvironment;
		}

		public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
		{
			return await _context.Movie.ToListAsync();
		}

		public async Task<Movie> GetMovieByIdAsync(Guid id)
		{
			return await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
		}

        public async Task AddMovieAsync(MovieViewModel movieViewModel)
        {
            // Check if an image file has been uploaded
            if (movieViewModel.Picture != null && movieViewModel.Picture.Length > 0)
            {
                // Define the directory path to save the image using IWebHostEnvironment
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a unique file name to avoid overwriting existing files
                var uniqueFileName = Guid.NewGuid().ToString() + "" + Path.GetFileName(movieViewModel.Picture.FileName);

                // Combine the folder path with the unique file name
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await movieViewModel.Picture.CopyToAsync(fileStream);
                }

                // Create a new Movie object to save the data Guido the database
                var movie = new Movie
                {
                    Title = movieViewModel.Title,
                    Author = movieViewModel.Author,
                    Description = movieViewModel.Description,
                    PathPicture = uniqueFileName // Save the ImageUrl in the Movie object
                };

                // Add the new movie to the context
                _context.Add(movie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MovieExistsAsync(Guid id)
        {
            return await _context.Movie.AnyAsync(e => e.Id == id);
        }

        public async Task UpdateMovieAsync(MovieViewModel movie)
		{
			_context.Update(movie);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(Guid id)
		{
			var movie = await _context.Movie.FindAsync(id);
			if (movie != null)
			{
				_context.Movie.Remove(movie);
				await _context.SaveChangesAsync();
			}
		}
	}
}
