using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniversityDbApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    OfficeLocation = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "OfficeLocation" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Педагогика, білім беру психологиясы және тәрбие жұмысы.", "Педагогика және психология", "Бас корпус, 312-кабинет" },
                    { 2, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Математика, деректер ғылымы және цифрлық технологиялар.", "Математика және информатика", "№ 2 корпус, 214-кабинет" },
                    { 3, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Қазақ тіл білімі, әдебиеттану және академиялық жазылым.", "Қазақ тілі мен әдебиеті", "Бас корпус, 408-кабинет" },
                    { 4, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Биология, экология және химия бағыттары.", "Жаратылыстану ғылымдары", "№ 3 корпус, 126-кабинет" },
                    { 5, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Ағылшын тілі, лингвистика және халықаралық коммуникация.", "Шетел тілдері", "№ 4 корпус, 305-кабинет" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "DepartmentId", "Email", "ExperienceYears", "FullName", "Subject" },
                values: new object[,]
                {
                    { 1, 1, "a.sarsenova@university.kz", 12, "Айгерім Сәрсенова", "Педагогика негіздері" },
                    { 2, 1, "d.kassymov@university.kz", 8, "Данияр Қасымов", "Білім беру психологиясы" },
                    { 3, 2, "e.nurtayev@university.kz", 10, "Ерлан Нұртаев", "Алгоритмдер және бағдарламалау" },
                    { 4, 2, "m.alieva@university.kz", 6, "Мадина Әлиева", "Деректер қоры" },
                    { 5, 2, "a.orazbek@university.kz", 14, "Арман Оразбек", "Дискретті математика" },
                    { 6, 3, "g.bekenova@university.kz", 18, "Гүлнар Бекенова", "Қазақ әдебиетінің тарихы" },
                    { 7, 3, "n.abdirov@university.kz", 5, "Нұрсұлтан Әбдіров", "Қазіргі қазақ тілі" },
                    { 8, 4, "d.myrzakhmet@university.kz", 9, "Динара Мырзахмет", "Экология және тұрақты даму" },
                    { 9, 4, "a.tolegen@university.kz", 7, "Алихан Төлеген", "Жалпы биология" },
                    { 10, 5, "s.thompson@university.kz", 11, "Sarah Thompson", "Academic English" },
                    { 11, 5, "m.ibraeva@university.kz", 4, "Меруерт Ибраева", "Ағылшын тілі грамматикасы" },
                    { 12, 5, "j.wilson@university.kz", 15, "James Wilson", "Intercultural Communication" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
