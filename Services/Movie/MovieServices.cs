using ProjectMovie.Data;
using ProjectMovie.Models.Entities;
using ProjectMovie.Models.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectMovie.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public MovieService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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
            var uniqueFileName = await _fileService.SaveFileAsync(movieViewModel.Picture, "images");

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = movieViewModel.Title,
                Author = movieViewModel.Author,
                Description = movieViewModel.Description,
                PathPicture = uniqueFileName
            };

            _context.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MovieExistsAsync(Guid id)
        {
            return await _context.Movie.AnyAsync(e => e.Id == id);
        }

        public async Task UpdateMovieAsync(MovieViewModel movieViewModel)
        {
            var movie = await _context.Movie.FindAsync(movieViewModel.Id);
            if (movie == null) return;

            movie.Title = movieViewModel.Title;
            movie.Author = movieViewModel.Author;
            movie.Description = movieViewModel.Description;

            if (movieViewModel.Picture != null && movieViewModel.Picture.Length > 0)
            {
                var uniqueFileName = await _fileService.SaveFileAsync(movieViewModel.Picture, "images");
                movie.PathPicture = uniqueFileName;
            }

            _context.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                if (!string.IsNullOrEmpty(movie.PathPicture))
                {
                    await _fileService.DeleteFileAsync(movie.PathPicture);
                }

                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
