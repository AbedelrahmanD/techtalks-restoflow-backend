using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestoFlow.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni", 1, null, "admin" },
                    { 2, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni", 2, null, "kitchenstaff" },
                    { 3, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni", 3, null, "billingstaff" },
                    { 4, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$iRectLPBh18dzcNU9eq7FeU2Bt54RyHThvmg67i6rRXbKHR0W6hni", 4, null, "feedbackstaff" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
