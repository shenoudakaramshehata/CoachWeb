using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class updateConfigurationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Configurations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
