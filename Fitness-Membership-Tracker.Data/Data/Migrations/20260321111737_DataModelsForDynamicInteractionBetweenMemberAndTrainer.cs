using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Membership_Tracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataModelsForDynamicInteractionBetweenMemberAndTrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerCapacities",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxTrainees = table.Column<int>(type: "int", nullable: false),
                    TrainerId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerCapacities", x => x.TrainerId);
                    table.ForeignKey(
                        name: "FK_TrainerCapacities_Trainers_TrainerId1",
                        column: x => x.TrainerId1,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerSchedules_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerTrainees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerTrainees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerTrainees_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerTrainees_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemberMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainerResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingRequests_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingRequests_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: true),
                    Reps = table.Column<int>(type: "int", nullable: true),
                    WeightKg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkoutLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_WorkoutLogs_WorkoutLogId",
                        column: x => x.WorkoutLogId,
                        principalTable: "WorkoutLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Radoslav.Radoslavov@gmail.com", "Radoslav", new DateTime(2020, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radoslavov", "1690599085", 1676m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Todorov@gmail.com", "Mihail", new DateTime(2020, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Todorov", "0408046739", 1639m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Viktoria.Petrov@gmail.com", "Viktoria", new DateTime(2020, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "2291636215", 1515m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Hristo.Zahariev@gmail.com", "Hristo", new DateTime(2020, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "2216206523", 1406m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Daskalov@gmail.com", "Alexander", new DateTime(2020, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "7515980835", 1495m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Dimitrov@gmail.com", "Dimitar", new DateTime(2020, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dimitrov", "4410796088", 1557m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Angelov@gmail.com", new DateTime(2020, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelov", "6850184536", 1441m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Todor.Zahariev@gmail.com", "Todor", new DateTime(2020, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zahariev", "0604083743", 1417m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Bozhkov@gmail.com", new DateTime(2020, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "8878504377", 1641m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Mihail.Bozhkov@gmail.com", "Mihail", new DateTime(2020, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bozhkov", "3869889558", 1549m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Dimitar.Petrov@gmail.com", "Dimitar", new DateTime(2020, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petrov", "2565512649", 1657m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Vasilev@gmail.com", "Alexander", new DateTime(2020, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vasilev", "1766991909", 1428m });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerCapacities_TrainerId1",
                table: "TrainerCapacities",
                column: "TrainerId1");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSchedules_TrainerId",
                table: "TrainerSchedules",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerTrainees_MemberId",
                table: "TrainerTrainees",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerTrainees_TrainerId",
                table: "TrainerTrainees",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRequests_MemberId",
                table: "TrainingRequests",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRequests_TrainerId",
                table: "TrainingRequests",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutLogId",
                table: "WorkoutExercises",
                column: "WorkoutLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_MemberId",
                table: "WorkoutLogs",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerCapacities");

            migrationBuilder.DropTable(
                name: "TrainerSchedules");

            migrationBuilder.DropTable(
                name: "TrainerTrainees");

            migrationBuilder.DropTable(
                name: "TrainingRequests");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "WorkoutLogs");

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
                columns: new[] { "Email", "FirstName", "HireDate", "PhoneNumber", "Salary" },
                values: new object[] { "Alexander.Zahariev@gmail.com", "Alexander", new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "8880576832", 1560m });

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
                columns: new[] { "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Kovachev@gmail.com", "Boris", new DateTime(2020, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kovachev", "3094851204", 1432m });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Boris.Daskalov@gmail.com", new DateTime(2020, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daskalov", "1932942000", 1606m });

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
                columns: new[] { "Email", "HireDate", "LastName", "PhoneNumber", "Salary" },
                values: new object[] { "Katerina.Simeonov@gmail.com", new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simeonov", "9709316723", 1682m });

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
        }
    }
}
