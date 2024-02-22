using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Data.Migrations
{
    public partial class Discriminator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "Discriminator",
               table: "AspNetUsers",
               type: "nvarchar(128)",
               nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("AspNetUsers", "Discriminator");


        }
    }
}
