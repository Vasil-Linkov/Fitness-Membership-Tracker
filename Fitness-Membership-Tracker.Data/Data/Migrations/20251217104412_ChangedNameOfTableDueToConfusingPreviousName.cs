using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameOfTableDueToConfusingPreviousName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MebershipTiers_TierId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MebershipTiers",
                table: "MebershipTiers");

            migrationBuilder.RenameTable(
                name: "MebershipTiers",
                newName: "MembershipTiers");

            migrationBuilder.RenameColumn(
                name: "TierId",
                table: "Memberships",
                newName: "MembershipTierId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_TierId",
                table: "Memberships",
                newName: "IX_Memberships_MembershipTierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipTiers",
                table: "MembershipTiers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipTiers_MembershipTierId",
                table: "Memberships",
                column: "MembershipTierId",
                principalTable: "MembershipTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipTiers_MembershipTierId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipTiers",
                table: "MembershipTiers");

            migrationBuilder.RenameTable(
                name: "MembershipTiers",
                newName: "MebershipTiers");

            migrationBuilder.RenameColumn(
                name: "MembershipTierId",
                table: "Memberships",
                newName: "TierId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_MembershipTierId",
                table: "Memberships",
                newName: "IX_Memberships_TierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MebershipTiers",
                table: "MebershipTiers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MebershipTiers_TierId",
                table: "Memberships",
                column: "TierId",
                principalTable: "MebershipTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
