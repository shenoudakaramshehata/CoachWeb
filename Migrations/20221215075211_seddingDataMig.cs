using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class seddingDataMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Configurations",
                columns: new[] { "ConfigurationId", "Facebook", "Instgram", "LinkedIn", "Twitter", "WhatsApp" },
                values: new object[] { 1, "https://www.facebook.com/", "https://www.insgram.com/", "https://www.linkedin.com/", "https://www.twitter.com/", "+965241" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Configurations",
                keyColumn: "ConfigurationId",
                keyValue: 1);
        }
    }
}
