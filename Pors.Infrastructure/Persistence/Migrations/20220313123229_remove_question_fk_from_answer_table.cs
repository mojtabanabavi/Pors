using Microsoft.EntityFrameworkCore.Migrations;

namespace Pors.Infrastructure.Persistence.Migrations
{
    public partial class remove_question_fk_from_answer_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttemptAnswers_ExamAttempts_AttemptId",
                table: "AttemptAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttemptAnswers_ExamQuestions_QuestionId",
                table: "AttemptAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttemptAnswers_QuestionOptions_OptionId",
                table: "AttemptAnswers");

            migrationBuilder.DropIndex(
                name: "IX_AttemptAnswers_QuestionId",
                table: "AttemptAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "AttemptAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_AttemptAnswers_ExamAttempts_AttemptId",
                table: "AttemptAnswers",
                column: "AttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttemptAnswers_QuestionOptions_OptionId",
                table: "AttemptAnswers",
                column: "OptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttemptAnswers_ExamAttempts_AttemptId",
                table: "AttemptAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttemptAnswers_QuestionOptions_OptionId",
                table: "AttemptAnswers");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "AttemptAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_QuestionId",
                table: "AttemptAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttemptAnswers_ExamAttempts_AttemptId",
                table: "AttemptAnswers",
                column: "AttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttemptAnswers_ExamQuestions_QuestionId",
                table: "AttemptAnswers",
                column: "QuestionId",
                principalTable: "ExamQuestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttemptAnswers_QuestionOptions_OptionId",
                table: "AttemptAnswers",
                column: "OptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id");
        }
    }
}
