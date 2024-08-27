using ProjectMovie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMovie.Services
{
	public interface IMovieService
	{
		Task<IEnumerable<Movie>> GetAllMoviesAsync();
		Task<Movie> GetMovieByIdAsync(Guid id);
		Task AddMovieAsync(Movie movie);
		Task UpdateMovieAsync(Movie movie);
		Task Delete(Guid id);
	}
}
