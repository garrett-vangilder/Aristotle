using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aristotle.Data.Migrations
{
    public partial class changedAttendanceAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LineItem",
                table: "LineItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendance",
                table: "LineItem",
                column: "AttendanceId");

            migrationBuilder.RenameTable(
                name: "LineItem",
                newName: "Attendance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendance",
                table: "Attendance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineItem",
                table: "Attendance",
                column: "AttendanceId");

            migrationBuilder.RenameTable(
                name: "Attendance",
                newName: "LineItem");
        }
    }
}
