using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SACS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStarTimeAndEndTimeInEmployeeScheduleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "EmployeeSchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "EmployeeSchedules",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "EmployeeSchedules");
        }
    }
}
