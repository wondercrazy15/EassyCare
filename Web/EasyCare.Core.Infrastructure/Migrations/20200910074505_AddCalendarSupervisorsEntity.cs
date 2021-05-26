using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddCalendarSupervisorsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarSupervisors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CalenderEventId = table.Column<Guid>(nullable: false),
                    SupervisorId = table.Column<Guid>(nullable: false),
                    InvitedDate = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarSupervisors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarSupervisors");
        }
    }
}
