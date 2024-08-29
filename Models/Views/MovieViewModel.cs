using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ProjectMovie.Models.Views
{
	public class MovieViewModel
	{
		[Key]
		public Guid Id { get; set; }

		public required string Title { get; set; }

		public string? Description { get; set; }

		public required string Author { get; set; }

		public IFormFile Picture { get; set; }

	}
}