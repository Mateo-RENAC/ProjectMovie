namespace ProjectMovie.Models.Entities
{
	public class Movie
	{
		public Guid Id { get; set; }

		public required string Title { get; set; }

		public string? Description { get; set; }

		public required string Author { get; set; }


	}


}
