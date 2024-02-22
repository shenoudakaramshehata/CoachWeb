using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class seedingDataMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PageContents",
                columns: new[] { "PageContentId", "ContentAr", "ContentEn", "PageTitleAr", "PageTitleEn" },
                values: new object[,]
                {
                    { 1, "من نحن", "About Page", "من نحن", "About" },
                    { 2, "الشروط والاحكام", "Condition and Terms Page", "الشروط والاحكام", "Condition and Terms" },
                    { 3, "سياسة الخصوصية", "Privacy Policy Page", "سياسة الخصوصية", "Privacy Policy" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "PaymentMethodTlEn", "PaymentMethodTlar" },
                values: new object[,]
                {
                    { 1, "CASH", "CASH" },
                    { 2, "KNET", "KNET" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PageContents",
                keyColumn: "PageContentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PageContents",
                keyColumn: "PageContentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PageContents",
                keyColumn: "PageContentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodId",
                keyValue: 2);
        }
    }
}
