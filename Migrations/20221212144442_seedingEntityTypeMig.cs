using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class seedingEntityTypeMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EntityTypes",
                columns: new[] { "EntityTypeId", "EntityTypeTlar", "EntityTypeTlen" },
                values: new object[,]
                {
                    { 1, "Trainer", "Trainer" },
                    { 2, "Camp", "Camp" },
                    { 3, "Tournment", "Tournment" },
                    { 4, "Course", "Course" },
                    { 5, "URL", "URL" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EntityTypes",
                keyColumn: "EntityTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EntityTypes",
                keyColumn: "EntityTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EntityTypes",
                keyColumn: "EntityTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EntityTypes",
                keyColumn: "EntityTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EntityTypes",
                keyColumn: "EntityTypeId",
                keyValue: 5);
        }
    }
}
