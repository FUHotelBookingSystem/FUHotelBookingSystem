using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixHotelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "hotels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "hotels");
        }
    }
}
