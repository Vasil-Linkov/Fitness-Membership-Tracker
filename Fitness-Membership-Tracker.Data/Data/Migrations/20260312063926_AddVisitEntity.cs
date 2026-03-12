using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visits_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visits_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id");
                });

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
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Bozhkov@gmail.com", new DateTime(2020, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "1927273821", 1673m });

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
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Dimitrov@gmail.com", "Boris", new DateTime(2020, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "1962804283", 1420m });

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

            migrationBuilder.CreateIndex(
                name: "IX_Visits_LocationId",
                table: "Visits",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_MemberId",
                table: "Visits",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_MembershipId",
                table: "Visits",
                column: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");

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
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Mihaylov@gmail.com", new DateTime(2020, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mihaylov", "3595356532", 1413m });

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
    }
}
