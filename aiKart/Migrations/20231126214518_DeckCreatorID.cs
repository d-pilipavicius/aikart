using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aiKart.Migrations
{
    /// <inheritdoc />
    public partial class DeckCreatorID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Decks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Decks");
        }
    }
}
