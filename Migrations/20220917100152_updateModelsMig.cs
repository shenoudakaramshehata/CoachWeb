using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class updateModelsMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_TrainerPlans_TrainerPlanId1",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_TrainerPlanId1",
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
                name: "TrainerPlanId1",
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

            migrationBuilder.AddColumn<string>(
                name: "Auth",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ref",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackID",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranID",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ispaid",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Ref",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TranID",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ispaid",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "Subscriptions");

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

            migrationBuilder.AddColumn<int>(
                name: "TrainerPlanId1",
                table: "Trainers",
                type: "int",
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

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_TrainerPlanId1",
                table: "Trainers",
                column: "TrainerPlanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_TrainerPlans_TrainerPlanId1",
                table: "Trainers",
                column: "TrainerPlanId1",
                principalTable: "TrainerPlans",
                principalColumn: "TrainerPlanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
