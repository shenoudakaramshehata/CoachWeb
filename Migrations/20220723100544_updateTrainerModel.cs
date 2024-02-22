using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class updateTrainerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerPlanId1",
                table: "Trainers",
                type: "int",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_TrainerPlans_TrainerPlanId1",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_TrainerPlanId1",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TrainerPlanId1",
                table: "Trainers");
        }
    }
}
