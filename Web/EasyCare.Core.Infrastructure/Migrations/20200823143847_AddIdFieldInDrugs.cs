using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddIdFieldInDrugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeniorId",
                table: "Drugs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Drugs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeniorId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Drugs");
        }
    }
}
