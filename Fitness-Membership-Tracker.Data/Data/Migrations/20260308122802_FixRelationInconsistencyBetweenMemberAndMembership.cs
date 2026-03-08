using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationInconsistencyBetweenMemberAndMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_AspNetUsers_MemberId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Memberships");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Nikolov@gmail.com", "Ivan", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "7876373561", 1531m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Mihaylov@gmail.com", "Viktoria", new DateTime(2020, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihaylov", "3595356532", 1413m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Angelov@gmail.com", "Todor", new DateTime(2020, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "3606747674", 1410m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Kovachev@gmail.com", "Katerina", new DateTime(2020, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "0593043706", 1635m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Daskalov@gmail.com", "Katerina", new DateTime(2020, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "4562632700", 1669m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Zahariev@gmail.com", "Mihail", new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "1498978746", 1645m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Hristov@gmail.com", "Ivan", new DateTime(2020, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "5157030913", 1681m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Georgiev@gmail.com", "Hristo", new DateTime(2020, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "6848562989", 1538m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Vasilev@gmail.com", "Ivan", new DateTime(2020, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "2575297365", 1654m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Georgiev@gmail.com", "Katerina", new DateTime(2020, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "6452872148", 1475m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Petrov@gmail.com", "Nikola", new DateTime(2020, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "3242014495", 1421m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Svetlana.Angelov@gmail.com", "Svetlana", new DateTime(2020, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "4675127378", 1611m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Memberships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Kovachev@gmail.com", "Dimitar", new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "9470480998", 1594m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Kovachev@gmail.com", "Boris", new DateTime(2020, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "0345476011", 1494m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Zahariev@gmail.com", "Ivan", new DateTime(2020, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "1627322926", 1616m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Zahariev@gmail.com", "Hristo", new DateTime(2020, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "1148835719", 1513m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Radoslav.Bozhkov@gmail.com", "Radoslav", new DateTime(2020, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "9233779113", 1578m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Svetlana.Hristov@gmail.com", "Svetlana", new DateTime(2020, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "4399341116", 1634m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Kovachev@gmail.com", "Mihail", new DateTime(2020, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "0294378533", 1485m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Zahariev@gmail.com", "Maria", new DateTime(2020, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "1625380876", 1534m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Vladimirov@gmail.com", "Maria", new DateTime(2020, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimirov", "4376645777", 1682m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Kolev@gmail.com", "Alexander", new DateTime(2020, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "5263430484", 1416m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Vasilev@gmail.com", "Boris", new DateTime(2020, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "5434231100", 1677m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Petar.Kolev@gmail.com", "Petar", new DateTime(2020, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "3989778318", 1595m });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_AspNetUsers_MemberId",
                table: "Memberships",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
