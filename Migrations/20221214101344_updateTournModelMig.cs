using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class updateTournModelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Faqs",
                table: "Faqs");

            migrationBuilder.RenameTable(
                name: "Faqs",
                newName: "FAQs");

            migrationBuilder.RenameColumn(
                name: "Faqid",
                table: "FAQs",
                newName: "FAQId");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanEndDate",
                table: "Tournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanStartDate",
                table: "Tournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionEn",
                table: "FAQs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionAr",
                table: "FAQs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerEn",
                table: "FAQs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerAr",
                table: "FAQs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FAQs",
                table: "FAQs",
                column: "FAQId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FAQs",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "PlanEndDate",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PlanStartDate",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "FAQs",
                newName: "Faqs");

            migrationBuilder.RenameColumn(
                name: "FAQId",
                table: "Faqs",
                newName: "Faqid");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionEn",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionAr",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerEn",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerAr",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faqs",
                table: "Faqs",
                column: "Faqid");
        }
    }
}
