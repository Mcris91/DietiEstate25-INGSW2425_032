using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.Infrastracture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCityColToListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Listing",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Listing");
        }
    }
}
