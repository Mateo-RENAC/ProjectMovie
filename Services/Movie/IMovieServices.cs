using ProjectMovie.Models.Entities;
using ProjectMovie.Models.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMovie.Services
{
	public interface IMovieService
	{
		Task<IEnumerable<Movie>> GetAllMoviesAsync();
		Task<Movie> GetMovieByIdAsync(Guid id);
        Task AddMovieAsync(MovieViewModel movieViewModel);
        Task<bool> MovieExistsAsync(Guid id);
        Task UpdateMovieAsync(MovieViewModel movie);
		Task Delete(Guid id);
	}
}
