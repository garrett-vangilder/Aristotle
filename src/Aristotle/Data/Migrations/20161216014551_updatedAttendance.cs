using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aristotle.Data.Migrations
{
    public partial class updatedAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentlyPresent",
                table: "Attendance");

            migrationBuilder.AddColumn<bool>(
                name: "CurrentlyAbsent",
                table: "Attendance",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentlyAbsent",
                table: "Attendance");

            migrationBuilder.AddColumn<bool>(
                name: "CurrentlyPresent",
                table: "Attendance",
                nullable: false,
                defaultValue: false);
        }
    }
}
