using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain_Transactions_Diplom.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartContracts",
                columns: table => new
                {
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublicKeyCreator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicKeyExecutor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdExercise = table.Column<int>(type: "int", nullable: true),
                    ContractValue = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartContracts", x => x.ContractId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartContracts");
        }
    }
}
