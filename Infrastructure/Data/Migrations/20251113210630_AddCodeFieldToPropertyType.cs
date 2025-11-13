using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.Infrastracture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeFieldToPropertyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PropertyType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyType_Code",
                table: "PropertyType",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyType_Code",
                table: "PropertyType");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PropertyType");
        }
    }
}
