using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aiKart.Migrations
{
    /// <inheritdoc />
    public partial class AlgorithmUsageFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseAnki",
                table: "Decks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseAnki",
                table: "Decks");
        }
    }
}
