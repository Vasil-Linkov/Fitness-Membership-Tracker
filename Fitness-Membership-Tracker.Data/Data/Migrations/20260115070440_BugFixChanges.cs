using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class BugFixChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationMembership_Locations_LocationId",
                table: "LocationMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationMembership_Memberships_MembershipId",
                table: "LocationMembership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationMembership",
                table: "LocationMembership");

            migrationBuilder.RenameTable(
                name: "LocationMembership",
                newName: "LocationMemberships");

            migrationBuilder.RenameIndex(
                name: "IX_LocationMembership_MembershipId",
                table: "LocationMemberships",
                newName: "IX_LocationMemberships_MembershipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationMemberships",
                table: "LocationMemberships",
                columns: new[] { "LocationId", "MembershipId" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Iva.Kolev@gmail.com", "Iva", new DateTime(2020, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "6171465922", 1659m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Zahariev@gmail.com", "Elena", new DateTime(2020, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "0901955996", 1666m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Vasilev@gmail.com", "Nikola", new DateTime(2020, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "7356104259", 1535m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Bozhkov@gmail.com", "Dimitar", new DateTime(2020, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "6527414224", 1599m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Kovachev@gmail.com", "Maria", new DateTime(2020, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "1975626388", 1503m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Angelov@gmail.com", "Nikola", new DateTime(2020, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "2127902660", 1579m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Iva.Dimitrov@gmail.com", "Iva", new DateTime(2020, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "9269577225", 1688m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Hristov@gmail.com", "Dimitar", new DateTime(2020, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "8687063840", 1557m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Kolev@gmail.com", "Boris", new DateTime(2020, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "5067327446", 1509m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Iva.Kolev@gmail.com", "Iva", new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "2696475192", 1482m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Todorov@gmail.com", "Ivan", new DateTime(2020, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "9375720764", 1468m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Hristov@gmail.com", "Viktoria", new DateTime(2020, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "5816053642", 1642m });

            migrationBuilder.AddForeignKey(
                name: "FK_LocationMemberships_Locations_LocationId",
                table: "LocationMemberships",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationMemberships_Memberships_MembershipId",
                table: "LocationMemberships",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationMemberships_Locations_LocationId",
                table: "LocationMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationMemberships_Memberships_MembershipId",
                table: "LocationMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationMemberships",
                table: "LocationMemberships");

            migrationBuilder.RenameTable(
                name: "LocationMemberships",
                newName: "LocationMembership");

            migrationBuilder.RenameIndex(
                name: "IX_LocationMemberships_MembershipId",
                table: "LocationMembership",
                newName: "IX_LocationMembership_MembershipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationMembership",
                table: "LocationMembership",
                columns: new[] { "LocationId", "MembershipId" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Mihaylov@gmail.com", "Dimitar", new DateTime(2020, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihaylov", "6895036713", 1534m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Kristina.Bozhkov@gmail.com", "Kristina", new DateTime(2020, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "5961832521", 1668m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Kolev@gmail.com", "Georgi", new DateTime(2020, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "8364222141", 1530m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Desislava.Angelov@gmail.com", "Desislava", new DateTime(2020, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "4543896634", 1627m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Kovachev@gmail.com", "Mihail", new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "7801150338", 1692m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Todorov@gmail.com", "Dimitar", new DateTime(2020, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "2128659047", 1545m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Kovachev@gmail.com", "Mihail", new DateTime(2020, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "5634491252", 1416m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Petrov@gmail.com", "Georgi", new DateTime(2020, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "7755709734", 1683m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Petar.Vladimirov@gmail.com", "Petar", new DateTime(2020, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimirov", "6859300378", 1501m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Todorov@gmail.com", "Nikola", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "8740622847", 1502m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Nikolov@gmail.com", "Nikola", new DateTime(2020, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "9479855058", 1642m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Kovachev@gmail.com", "Katerina", new DateTime(2020, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "4198712334", 1532m });

            migrationBuilder.AddForeignKey(
                name: "FK_LocationMembership_Locations_LocationId",
                table: "LocationMembership",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationMembership_Memberships_MembershipId",
                table: "LocationMembership",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
