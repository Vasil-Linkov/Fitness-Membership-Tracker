using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRedundantPropertyOfMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Memberships");

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
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Kovachev@gmail.com", "Mihail", new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "7801150338", 1692m });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Dimitrov@gmail.com", "Boris", new DateTime(2020, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "0150498755", 1579m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Vasilev@gmail.com", "Dimitar", new DateTime(2020, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "1214631667", 1572m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Angelov@gmail.com", "Mihail", new DateTime(2020, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "9268465789", 1571m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Zahariev@gmail.com", "Elena", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "0792019548", 1517m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Todorov@gmail.com", "Maria", new DateTime(2020, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "3074066239", 1403m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Ivanov@gmail.com", "Todor", new DateTime(2020, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", "1586247994", 1535m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Petrov@gmail.com", "Nikola", new DateTime(2020, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "1270570055", 1606m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Angelov@gmail.com", "Katerina", new DateTime(2020, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "3288615843", 1695m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Kolev@gmail.com", "Maria", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", "9430787880", 1451m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Nikolaev@gmail.com", "Todor", new DateTime(2020, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolaev", "8030859187", 1497m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Dimitrov@gmail.com", "Todor", new DateTime(2020, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "7558721670", 1559m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Georgiev@gmail.com", "Elena", new DateTime(2020, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "7540878841", 1623m });
        }
    }
}
