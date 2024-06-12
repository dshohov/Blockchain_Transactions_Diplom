using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Transactions_Diplom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerCreator",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerExecutor",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileAntwort",
                table: "Exercises",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerCreator",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AnswerExecutor",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "FileAntwort",
                table: "Exercises");
        }
    }
}
