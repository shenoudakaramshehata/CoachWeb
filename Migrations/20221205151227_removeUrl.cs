using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class removeUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Camps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
