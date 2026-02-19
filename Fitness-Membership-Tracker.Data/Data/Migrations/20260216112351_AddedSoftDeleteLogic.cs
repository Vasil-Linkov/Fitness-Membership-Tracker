using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSoftDeleteLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Memberships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Memberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Locations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Alexander.Radoslavov@gmail.com", "Alexander", new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Radoslavov", "1850224504", 1541m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Svetlana.Radoslavov@gmail.com", "Svetlana", new DateTime(2020, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Radoslavov", "6763625111", 1630m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Petar.Todorov@gmail.com", "Petar", new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Todorov", "5448845351", 1698m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Dimitar.Bozhkov@gmail.com", "Dimitar", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Bozhkov", "8280392452", 1624m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Mihail.Nikolov@gmail.com", "Mihail", new DateTime(2020, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Nikolov", "5044757937", 1673m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Ivan.Daskalov@gmail.com", "Ivan", new DateTime(2020, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Daskalov", "7007214308", 1400m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Iva.Angelov@gmail.com", "Iva", new DateTime(2020, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Angelov", "7131109770", 1432m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Dimitar.Daskalov@gmail.com", "Dimitar", new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Daskalov", "1755365882", 1612m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Kristina.Dimitrov@gmail.com", "Kristina", new DateTime(2020, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Dimitrov", "1078959969", 1666m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Elena.Kolev@gmail.com", "Elena", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Kolev", "9959321806", 1405m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Maria.Kovachev@gmail.com", "Maria", new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Kovachev", "8062872819", 1520m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { null, "Maria.Dimitrov@gmail.com", "Maria", new DateTime(2020, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Dimitrov", "3535324774", 1534m });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "IsDeleted" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

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
    }
}
