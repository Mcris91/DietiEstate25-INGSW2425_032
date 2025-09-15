using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class FixManyToManyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Listing_ListingId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Listing_ListingId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Listing_ListingId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ListingId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Service_ListingId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Image_ListingId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Image");

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

            migrationBuilder.CreateTable(
                name: "ListingService",
                columns: table => new
                {
                    ListingServicesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingService", x => new { x.ListingServicesId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_ListingService_Listing_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingService_Service_ListingServicesId",
                        column: x => x.ListingServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingTag",
                columns: table => new
                {
                    ListingTagsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTag", x => new { x.ListingTagsId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_ListingTag_Listing_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingTag_Tag_ListingTagsId",
                        column: x => x.ListingTagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageListing_ListingsId",
                table: "ImageListing",
                column: "ListingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingService_ListingsId",
                table: "ListingService",
                column: "ListingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingTag_ListingsId",
                table: "ListingTag",
                column: "ListingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageListing");

            migrationBuilder.DropTable(
                name: "ListingService");

            migrationBuilder.DropTable(
                name: "ListingTag");

            migrationBuilder.AddColumn<Guid>(
                name: "ListingId",
                table: "Tag",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ListingId",
                table: "Service",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ListingId",
                table: "Image",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ListingId",
                table: "Tag",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ListingId",
                table: "Service",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ListingId",
                table: "Image",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Listing_ListingId",
                table: "Image",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Listing_ListingId",
                table: "Service",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Listing_ListingId",
                table: "Tag",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id");
        }
    }
}
