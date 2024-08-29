using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMovie.Migrations
{
    /// <inheritdoc />
    public partial class AddingPathPictureinModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathPicture",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathPicture",
                table: "Movie");
        }
    }
}
