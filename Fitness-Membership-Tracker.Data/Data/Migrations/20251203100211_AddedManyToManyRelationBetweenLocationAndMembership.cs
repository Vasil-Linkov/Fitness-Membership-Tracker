using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedManyToManyRelationBetweenLocationAndMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MebershipTiers");

            migrationBuilder.DropColumn(
                name: "IsLocationRestricted",
                table: "MebershipTiers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationMembership",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMembership", x => new { x.LocationId, x.MembershipId });
                    table.ForeignKey(
                        name: "FK_LocationMembership_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationMembership_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PaymentId",
                table: "AspNetUsers",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMembership_MembershipId",
                table: "LocationMembership",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Payments_PaymentId",
                table: "AspNetUsers",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Payments_PaymentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LocationMembership");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PaymentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "MebershipTiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocationRestricted",
                table: "MebershipTiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
