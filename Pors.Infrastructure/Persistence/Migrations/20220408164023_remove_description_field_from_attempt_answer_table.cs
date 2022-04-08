using Microsoft.EntityFrameworkCore.Migrations;

namespace Pors.Infrastructure.Persistence.Migrations
{
    public partial class remove_description_field_from_attempt_answer_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AttemptAnswers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AttemptAnswers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
