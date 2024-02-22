using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class UpdateInCourseandCampMoMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SubStartDate",
                table: "Camps",
                newName: "JoinStart");

            migrationBuilder.RenameColumn(
                name: "SubEndDate",
                table: "Camps",
                newName: "JoinEnd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinStart",
                table: "Camps",
                newName: "SubStartDate");

            migrationBuilder.RenameColumn(
                name: "JoinEnd",
                table: "Camps",
                newName: "SubEndDate");

            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
