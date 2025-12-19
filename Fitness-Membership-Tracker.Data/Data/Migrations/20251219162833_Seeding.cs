using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "City", "Country" },
                values: new object[,]
                {
                    { 1, "бул. Черни връх 47, Младост, 1303", "Sofia", "Bulgaria" },
                    { 2, "ул. Пирин 12, Люлин, 1324", "Sofia", "Bulgaria" },
                    { 3, "ул. Христо Ботев 23, Център, 1000", "Sofia", "Bulgaria" },
                    { 4, "ул. Васил Левски 45", "Sofia", "Bulgaria" }
                });

            migrationBuilder.InsertData(
                table: "MembershipTiers",
                columns: new[] { "Id", "Description", "MaxSessionsPerMonth", "MonthlyPrice", "Tier" },
                values: new object[,]
                {
                    { 1, "Access to gym facilities during staffed hours.", 8, 9.99m, "Basic" },
                    { 2, "Access to gym facilities during staffed hours.", 12, 15.99m, "Advanced" },
                    { 3, "Access to gym facilities during staffed hours.", 18, 21.99m, "elite" },
                    { 4, "Access to gym facilities during staffed hours.", 24, 29.99m, "Ultimate" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "FirstName", "HireDate", "LastName", "LocationId", "PhoneNumber", "Salary" },
                values: new object[,]
                {
                    { 1, "Boris.Dimitrov@gmail.com", "Boris", new DateTime(2020, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", 1, "0150498755", 1579m },
                    { 2, "Dimitar.Vasilev@gmail.com", "Dimitar", new DateTime(2020, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", 1, "1214631667", 1572m },
                    { 3, "Mihail.Angelov@gmail.com", "Mihail", new DateTime(2020, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", 1, "9268465789", 1571m },
                    { 4, "Elena.Zahariev@gmail.com", "Elena", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", 2, "0792019548", 1517m },
                    { 5, "Maria.Todorov@gmail.com", "Maria", new DateTime(2020, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", 2, "3074066239", 1403m },
                    { 6, "Todor.Ivanov@gmail.com", "Todor", new DateTime(2020, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", 2, "1586247994", 1535m },
                    { 7, "Nikola.Petrov@gmail.com", "Nikola", new DateTime(2020, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", 3, "1270570055", 1606m },
                    { 8, "Katerina.Angelov@gmail.com", "Katerina", new DateTime(2020, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", 3, "3288615843", 1695m },
                    { 9, "Maria.Kolev@gmail.com", "Maria", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kolev", 3, "9430787880", 1451m },
                    { 10, "Todor.Nikolaev@gmail.com", "Todor", new DateTime(2020, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolaev", 4, "8030859187", 1497m },
                    { 11, "Todor.Dimitrov@gmail.com", "Todor", new DateTime(2020, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", 4, "7558721670", 1559m },
                    { 12, "Elena.Georgiev@gmail.com", "Elena", new DateTime(2020, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgiev", 4, "7540878841", 1623m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MembershipTiers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
