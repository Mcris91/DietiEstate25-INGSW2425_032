using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.Infrastracture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributeOfBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AgentUserId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "BookingAccepted",
                table: "Booking",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientUserId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Booking",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMeeting",
                table: "Booking",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ListingId",
                table: "Booking",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AgentUserId",
                table: "Booking",
                column: "AgentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ClientUserId",
                table: "Booking",
                column: "ClientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ListingId",
                table: "Booking",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Listing_ListingId",
                table: "Booking",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_User_AgentUserId",
                table: "Booking",
                column: "AgentUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_User_ClientUserId",
                table: "Booking",
                column: "ClientUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Listing_ListingId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_User_AgentUserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_User_ClientUserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_AgentUserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ClientUserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ListingId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AgentUserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingAccepted",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ClientUserId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DateMeeting",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Booking");
        }
    }
}
