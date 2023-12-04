using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aiKart.Migrations
{
    /// <inheritdoc />
    public partial class CardAnkiAlgorithmParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EFactor",
                table: "Cards",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IntervalInDays",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRepetition",
                table: "Cards",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EFactor",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IntervalInDays",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "LastRepetition",
                table: "Cards");
        }
    }
}
