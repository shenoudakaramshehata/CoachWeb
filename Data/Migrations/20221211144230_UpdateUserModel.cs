using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Data.Migrations
{
    public partial class UpdateUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "Pic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "Pic",
                table: "AspNetUsers");

        }
    }
}
