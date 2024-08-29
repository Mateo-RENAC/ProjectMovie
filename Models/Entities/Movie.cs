using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ProjectMovie.Models.Entities
{
	public class Movie
	{
		[Key]
		public Guid Id { get; set; }

		public required string Title { get; set; }

		public string? Description { get; set; }

		public required string Author { get; set; }

		public required string PathPicture { get; set; }



		public Movie()
		{
		}

		public Movie(Guid id, string title, string description, string author, string pathpicture)
		{
			Id = id;
			Title = title;
			Description = description;
			Author = author;
			PathPicture = pathpicture;
		}

	}
}