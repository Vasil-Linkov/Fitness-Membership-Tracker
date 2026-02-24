using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOneToManyBEtweenLocationToMembershipAndMemberToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Payments_PaymentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PaymentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocationRegistered",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Kolev@gmail.com", "Alexander", new DateTime(2020, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "5263430484", 1416m });

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
                name: "IX_Memberships_LocationId",
                table: "Memberships",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Locations_LocationId",
                table: "Memberships",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Locations_LocationId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_LocationId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Memberships");

            migrationBuilder.AddColumn<string>(
                name: "LocationRegistered",
                table: "Memberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Radoslavov@gmail.com", "Alexander", new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "1850224504", 1541m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Svetlana.Radoslavov@gmail.com", "Svetlana", new DateTime(2020, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "6763625111", 1630m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Petar.Todorov@gmail.com", "Petar", new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "5448845351", 1698m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Bozhkov@gmail.com", "Dimitar", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "8280392452", 1624m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Nikolov@gmail.com", "Mihail", new DateTime(2020, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikolov", "5044757937", 1673m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Ivan.Daskalov@gmail.com", "Ivan", new DateTime(2020, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "7007214308", 1400m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Iva.Angelov@gmail.com", "Iva", new DateTime(2020, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "7131109770", 1432m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Daskalov@gmail.com", "Dimitar", new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "1755365882", 1612m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Kristina.Dimitrov@gmail.com", "Kristina", new DateTime(2020, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "1078959969", 1666m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Elena.Kolev@gmail.com", "Elena", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "9959321806", 1405m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Kovachev@gmail.com", "Maria", new DateTime(2020, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "8062872819", 1520m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Maria.Dimitrov@gmail.com", "Maria", new DateTime(2020, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "3535324774", 1534m });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PaymentId",
                table: "AspNetUsers",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Payments_PaymentId",
                table: "AspNetUsers",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }
    }
}
