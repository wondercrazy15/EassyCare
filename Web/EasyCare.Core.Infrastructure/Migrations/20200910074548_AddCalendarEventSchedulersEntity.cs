using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddCalendarEventSchedulersEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarEventSchedulers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CalenderEventId = table.Column<Guid>(nullable: false),
                    IsMonthly = table.Column<bool>(nullable: false),
                    MonthDate = table.Column<DateTime>(nullable: false),
                    IsWeekly = table.Column<bool>(nullable: false),
                    WeekDay = table.Column<string>(nullable: true),
                    IsDaily = table.Column<bool>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    OneTimeDate = table.Column<DateTime>(nullable: false),
                    PeriodOfTime = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEventSchedulers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEventSchedulers");
        }
    }
}
