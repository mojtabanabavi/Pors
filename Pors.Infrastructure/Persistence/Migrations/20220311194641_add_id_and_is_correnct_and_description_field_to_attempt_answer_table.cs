using Microsoft.EntityFrameworkCore.Migrations;

namespace Pors.Infrastructure.Persistence.Migrations
{
    public partial class add_id_and_is_correnct_and_description_field_to_attempt_answer_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttemptAnswers",
                table: "AttemptAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "AttemptId",
                table: "AttemptAnswers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AttemptAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AttemptAnswers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "AttemptAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttemptAnswers",
                table: "AttemptAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptAnswers_AttemptId",
                table: "AttemptAnswers",
                column: "AttemptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttemptAnswers",
                table: "AttemptAnswers");

            migrationBuilder.DropIndex(
                name: "IX_AttemptAnswers_AttemptId",
                table: "AttemptAnswers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AttemptAnswers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AttemptAnswers");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "AttemptAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "AttemptId",
                table: "AttemptAnswers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttemptAnswers",
                table: "AttemptAnswers",
                columns: new[] { "AttemptId", "QuestionId", "OptionId" });
        }
    }
}
