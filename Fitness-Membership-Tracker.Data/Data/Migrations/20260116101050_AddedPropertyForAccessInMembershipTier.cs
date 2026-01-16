using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyForAccessInMembershipTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Accessibility",
                table: "MembershipTiers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Daskalov@gmail.com", "Todor", new DateTime(2020, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "1477717284", 1621m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Bozhkov@gmail.com", "Ivan", new DateTime(2020, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "2400299588", 1443m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Petrov@gmail.com", "Hristo", new DateTime(2020, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "8443646192", 1645m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Daskalov@gmail.com", new DateTime(2020, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "3792603224", 1657m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Dimitrov@gmail.com", "Georgi", new DateTime(2020, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "2420741226", 1580m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Stoyanov@gmail.com", "Viktoria", new DateTime(2020, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stoyanov", "2451370384", 1671m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Ivanov@gmail.com", "Dimitar", new DateTime(2020, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", "6268091517", 1446m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Radoslav.Radoslavov@gmail.com", "Radoslav", new DateTime(2020, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "3931121388", 1414m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Simeonov@gmail.com", "Georgi", new DateTime(2020, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simeonov", "8626391915", 1696m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Bozhkov@gmail.com", "Stefan", new DateTime(2020, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "9895498508", 1406m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Dimitrov@gmail.com", "Viktoria", new DateTime(2020, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "8790665105", 1540m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Vladimirov@gmail.com", "Dimitar", new DateTime(2020, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimirov", "2910291020", 1581m });

            migrationBuilder.UpdateData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Accessibility",
                value: "address");

            migrationBuilder.UpdateData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Accessibility",
                value: "city");

            migrationBuilder.UpdateData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Accessibility",
                value: "country");

            migrationBuilder.UpdateData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Accessibility",
                value: "any");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessibility",
                table: "MembershipTiers");

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
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Bozhkov@gmail.com", new DateTime(2020, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "6527414224", 1599m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Kovachev@gmail.com", "Maria", new DateTime(2020, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "1975626388", 1503m });

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
        }
    }
}
