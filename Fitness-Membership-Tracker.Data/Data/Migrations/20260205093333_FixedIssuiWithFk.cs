using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedIssuiWithFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Nikola.Bozhkov@gmail.com", "Nikola", new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "4360381311", 1601m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Todorov@gmail.com", "Viktoria", new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "6044283531", 1552m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Petrov@gmail.com", "Elena", new DateTime(2020, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "7127392032", 1674m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Simeonov@gmail.com", "Stefan", new DateTime(2020, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simeonov", "8401644663", 1609m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Kristina.Daskalov@gmail.com", "Kristina", new DateTime(2020, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "4222543255", 1403m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Hristov@gmail.com", "Dimitar", new DateTime(2020, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "0485708395", 1516m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Simeonov@gmail.com", "Stefan", new DateTime(2020, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simeonov", "5363134945", 1605m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Kovachev@gmail.com", "Boris", new DateTime(2020, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "0080219227", 1614m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Ivanov@gmail.com", "Maria", new DateTime(2020, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", "8616765278", 1698m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Desislava.Ivanov@gmail.com", "Desislava", new DateTime(2020, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", "8710483506", 1659m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Bozhkov@gmail.com", "Ivan", new DateTime(2020, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "2740782645", 1573m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Bozhkov@gmail.com", "Elena", new DateTime(2020, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "1667686317", 1496m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Radoslav.Todorov@gmail.com", "Radoslav", new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "1720661589", 1446m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Stoyanov@gmail.com", "Katerina", new DateTime(2020, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stoyanov", "8639122754", 1574m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Georgiev@gmail.com", "Stefan", new DateTime(2020, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "1677524532", 1683m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Vladimirov@gmail.com", "Mihail", new DateTime(2020, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimirov", "8599558542", 1440m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Nikolov@gmail.com", "Georgi", new DateTime(2020, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "2205396394", 1576m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Nikolaev@gmail.com", "Boris", new DateTime(2020, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolaev", "1326881716", 1466m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Vasilev@gmail.com", "Dimitar", new DateTime(2020, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "3347471225", 1579m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Todorov@gmail.com", "Georgi", new DateTime(2020, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "1114786810", 1469m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Georgiev@gmail.com", "Mihail", new DateTime(2020, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "1142804141", 1543m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Dimitrov@gmail.com", "Boris", new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "3896516741", 1482m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Nikolov@gmail.com", "Boris", new DateTime(2020, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "2691803012", 1489m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Svetlana.Georgiev@gmail.com", "Svetlana", new DateTime(2020, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", "9399248629", 1632m });
        }
    }
}
