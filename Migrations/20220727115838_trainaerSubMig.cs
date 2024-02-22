using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class trainaerSubMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "TrainerSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "TrainerSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "TrainerSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "TrainerSubscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "TrainerSubscriptions");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "TrainerSubscriptions");
        }
    }
}
