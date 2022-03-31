using Microsoft.EntityFrameworkCore.Migrations;

namespace Pors.Infrastructure.Persistence.Migrations
{
    public partial class add_option_status_to_attempt_answer_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "AttemptAnswers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AttemptAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AttemptAnswers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "AttemptAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
