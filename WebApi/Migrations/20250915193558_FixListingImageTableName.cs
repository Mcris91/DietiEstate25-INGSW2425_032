using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class FixListingImageTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageListing");

            migrationBuilder.CreateTable(
                name: "ListingImage",
                columns: table => new
                {
                    ListingImagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImage", x => new { x.ListingImagesId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_ListingImage_Image_ListingImagesId",
                        column: x => x.ListingImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingImage_Listing_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingImage_ListingsId",
                table: "ListingImage",
                column: "ListingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingImage");

            migrationBuilder.CreateTable(
                name: "ImageListing",
                columns: table => new
                {
                    ListingImagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageListing", x => new { x.ListingImagesId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_ImageListing_Image_ListingImagesId",
                        column: x => x.ListingImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageListing_Listing_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageListing_ListingsId",
                table: "ImageListing",
                column: "ListingsId");
        }
    }
}
