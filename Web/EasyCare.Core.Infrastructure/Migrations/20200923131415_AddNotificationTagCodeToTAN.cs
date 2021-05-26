using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddNotificationTagCodeToTAN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDeviceId",
                table: "TAN");

            migrationBuilder.AddColumn<string>(
                name: "NotificationTagCode",
                table: "TAN",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationTagCode",
                table: "TAN");

            migrationBuilder.AddColumn<string>(
                name: "RegisterDeviceId",
                table: "TAN",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
