using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Transactions_Diplom.Migrations
{
    /// <inheritdoc />
    public partial class UPExerciseFixedProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileAntwort",
                table: "Exercises",
                newName: "FileAnswer");

            migrationBuilder.AddColumn<string>(
                name: "FileNameAnswer",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNameAnswer",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "FileAnswer",
                table: "Exercises",
                newName: "FileAntwort");
        }
    }
}
