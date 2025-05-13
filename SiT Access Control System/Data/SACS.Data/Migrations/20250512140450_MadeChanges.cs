using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SACS.Data.Migrations
{
    /// <inheritdoc />
    public partial class MadeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSchedules_Employees_EmployeeId",
                table: "EmployeeSchedules");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeSchedules",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSchedules_EmployeeId",
                table: "EmployeeSchedules",
                newName: "IX_EmployeeSchedules_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_UserId",
                table: "EmployeeSchedules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_UserId",
                table: "EmployeeSchedules");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmployeeSchedules",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSchedules_UserId",
                table: "EmployeeSchedules",
                newName: "IX_EmployeeSchedules_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSchedules_Employees_EmployeeId",
                table: "EmployeeSchedules",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
