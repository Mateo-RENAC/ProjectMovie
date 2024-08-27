using Microsoft.EntityFrameworkCore;
using ProjectMovie.Data;
using ProjectMovie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMovie.Services
{
	public class MovieService : IMovieService
	{
		private readonly ApplicationDbContext _context;

		public MovieService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
		{
			return await _context.Movie.ToListAsync();
		}

		public async Task<Movie> GetMovieByIdAsync(Guid id)
		{
			return await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task AddMovieAsync(Movie movie)
		{
			movie.Id = Guid.NewGuid();
			_context.Add(movie);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateMovieAsync(Movie movie)
		{
			_context.Update(movie);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteMovieAsync(Guid id)
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
