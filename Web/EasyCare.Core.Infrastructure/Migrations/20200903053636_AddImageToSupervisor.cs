using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class AddImageToSupervisor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Supervisor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Supervisor");
        }
    }
}
