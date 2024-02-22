using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class Paymentmethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Tournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Camps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Camps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "Camps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Tournaments");

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

            migrationBuilder.DropColumn(
                name: "Auth",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "Camps");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Camps");
        }
    }
}
