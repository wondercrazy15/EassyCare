using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddYealryFieldsToCalendarEventSchedulers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is2Weekly",
                table: "CalendarEventSchedulers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsYearly",
                table: "CalendarEventSchedulers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "CalendarEventSchedulers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is2Weekly",
                table: "CalendarEventSchedulers");

            migrationBuilder.DropColumn(
                name: "IsYearly",
                table: "CalendarEventSchedulers");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "CalendarEventSchedulers");
        }
    }
}
