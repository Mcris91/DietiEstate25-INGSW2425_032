using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietiEstate.Infrastracture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributeTableBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Service",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_BookingId",
                table: "Service",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Booking_BookingId",
                table: "Service",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Booking_BookingId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_BookingId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Service");
        }
    }
}
