using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class TrainerPayMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Trainers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Trainers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SubscriptionCost",
                table: "Trainers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "Trainers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Auth",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "SubscriptionCost",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Trainers");
        }
    }
}
