using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aiKart.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserDecksStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "UserDecks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PartiallyCorrectAnswers",
                table: "UserDecks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesSolved",
                table: "UserDecks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "UserDecks");

            migrationBuilder.DropColumn(
                name: "PartiallyCorrectAnswers",
                table: "UserDecks");

            migrationBuilder.DropColumn(
                name: "TimesSolved",
                table: "UserDecks");
        }
    }
}
