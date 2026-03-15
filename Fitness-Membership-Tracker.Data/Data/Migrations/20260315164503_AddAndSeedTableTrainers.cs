using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAndSeedTableTrainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Kristina.Ivanov@gmail.com", "Kristina", new DateTime(2020, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivanov", "7405733177", 1680m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Stoyanov@gmail.com", "Stefan", new DateTime(2020, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stoyanov", "8472323815", 1638m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Vasilev@gmail.com", "Elena", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "0885923981", 1565m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Zahariev@gmail.com", "Alexander", new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "8880576832", 1560m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Kovachev@gmail.com", "Mihail", new DateTime(2020, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "3423082246", 1572m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Kovachev@gmail.com", new DateTime(2020, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "3094851204", 1432m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Daskalov@gmail.com", "Boris", new DateTime(2020, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "1932942000", 1606m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Radoslavov@gmail.com", "Hristo", new DateTime(2020, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "4573062397", 1490m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Simeonov@gmail.com", "Katerina", new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simeonov", "9709316723", 1682m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Georgi.Kovachev@gmail.com", "Georgi", new DateTime(2020, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "5928079702", 1633m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Daskalov@gmail.com", "Alexander", new DateTime(2020, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "8841338095", 1664m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Petar.Radoslavov@gmail.com", "Petar", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "1569708847", 1622m });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "DeletedAt", "Email", "FirstName", "HireDate", "IsDeleted", "LastName", "LocationId", "PhoneNumber", "Salary", "Specialization" },
                values: new object[,]
                {
                    { 1, null, "Maria.Ivanova@gym.com", "Maria", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Ivanova", 1, "0881000001", 1600m, "Yoga" },
                    { 2, null, "Georgi.Petrov@gym.com", "Georgi", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Petrov", 1, "0881000002", 1800m, "Personal Training" },
                    { 3, null, "Elena.Dimitrova@gym.com", "Elena", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Dimitrova", 2, "0881000003", 1550m, "Pilates" },
                    { 4, null, "Nikola.Hristov@gym.com", "Nikola", new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Hristov", 2, "0881000004", 1900m, "CrossFit" },
                    { 5, null, "Viktoria.Stoyanova@gym.com", "Viktoria", new DateTime(2021, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Stoyanova", 3, "0881000005", 1650m, "Group Fitness" },
                    { 6, null, "Stefan.Vasilev@gym.com", "Stefan", new DateTime(2020, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vasilev", 3, "0881000006", 1750m, "Strength & Conditioning" },
                    { 7, null, "Katerina.Nikolova@gym.com", "Katerina", new DateTime(2023, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Nikolova", 4, "0881000007", 1500m, "Nutrition" },
                    { 8, null, "Hristo.Todorov@gym.com", "Hristo", new DateTime(2018, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Todorov", 4, "0881000008", 2000m, "Martial Arts" },
                    { 9, null, "Desislava.Angelova@gym.com", "Desislava", new DateTime(2022, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Angelova", 1, "0881000009", 1580m, "Cardio" },
                    { 10, null, "Alexander.Kolev@gym.com", "Alexander", new DateTime(2021, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Kolev", 2, "0881000010", 1700m, "Swimming" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_LocationId",
                table: "Trainers",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Todorov@gmail.com", "Stefan", new DateTime(2020, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "7357190214", 1476m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Bozhkov@gmail.com", "Viktoria", new DateTime(2020, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "1927273821", 1673m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Desislava.Petrov@gmail.com", "Desislava", new DateTime(2020, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "4559897316", 1652m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Svetlana.Mihaylov@gmail.com", "Svetlana", new DateTime(2020, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihaylov", "6414266893", 1536m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Nikolov@gmail.com", "Stefan", new DateTime(2020, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "9031831963", 1451m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Dimitrov@gmail.com", new DateTime(2020, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "1962804283", 1420m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Mihaylov@gmail.com", "Hristo", new DateTime(2020, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihaylov", "2480721017", 1411m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Stefan.Angelov@gmail.com", "Stefan", new DateTime(2020, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "4874288005", 1602m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Radoslav.Petrov@gmail.com", "Radoslav", new DateTime(2020, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "1005405715", 1548m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Stoyanov@gmail.com", "Maria", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stoyanov", "7348371450", 1693m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Nikolaev@gmail.com", "Katerina", new DateTime(2020, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolaev", "9872358633", 1617m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Hristov@gmail.com", "Alexander", new DateTime(2020, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hristov", "8551408953", 1685m });
        }
    }
}
