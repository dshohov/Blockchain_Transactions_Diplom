using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Transactions_Diplom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Exercises");
        }
    }
}
