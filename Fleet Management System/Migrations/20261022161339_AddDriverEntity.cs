using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "DriverAssignments");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "DriverAssignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LicenseNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverAssignments_DriverId",
                table: "DriverAssignments",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverAssignments_Drivers_DriverId",
                table: "DriverAssignments",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverAssignments_Drivers_DriverId",
                table: "DriverAssignments");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_DriverAssignments_DriverId",
                table: "DriverAssignments");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "DriverAssignments");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "DriverAssignments",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
