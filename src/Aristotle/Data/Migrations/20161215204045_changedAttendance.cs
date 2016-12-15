using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aristotle.Data.Migrations
{
    public partial class changedAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "LineItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "LineItem",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "LineItem");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "LineItem",
                nullable: true);
        }
    }
}
