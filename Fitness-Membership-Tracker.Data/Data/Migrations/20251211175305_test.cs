using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Locations_LocationId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_LocationId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Memberships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Memberships",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_LocationId",
                table: "Memberships",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Locations_LocationId",
                table: "Memberships",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
