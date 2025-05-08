using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SACS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesRFIDCards_Employees_EmployeeId",
                table: "EmployeesRFIDCards");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeesRFIDCards",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeesRFIDCards_EmployeeId",
                table: "EmployeesRFIDCards",
                newName: "IX_EmployeesRFIDCards_UserId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Summaries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "EmployeeSchedules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonalIdentificationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_UserId",
                table: "Summaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_ApplicationUserId",
                table: "EmployeeSchedules",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonalIdentificationId",
                table: "AspNetUsers",
                column: "PersonalIdentificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PersonalIdentifications_PersonalIdentificationId",
                table: "AspNetUsers",
                column: "PersonalIdentificationId",
                principalTable: "PersonalIdentifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_ApplicationUserId",
                table: "EmployeeSchedules",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesRFIDCards_AspNetUsers_UserId",
                table: "EmployeesRFIDCards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_AspNetUsers_UserId",
                table: "Summaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PersonalIdentifications_PersonalIdentificationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSchedules_AspNetUsers_ApplicationUserId",
                table: "EmployeeSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesRFIDCards_AspNetUsers_UserId",
                table: "EmployeesRFIDCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_AspNetUsers_UserId",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_UserId",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSchedules_ApplicationUserId",
                table: "EmployeeSchedules");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PersonalIdentificationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalIdentificationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmployeesRFIDCards",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeesRFIDCards_UserId",
                table: "EmployeesRFIDCards",
                newName: "IX_EmployeesRFIDCards_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesRFIDCards_Employees_EmployeeId",
                table: "EmployeesRFIDCards",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
