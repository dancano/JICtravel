using Microsoft.EntityFrameworkCore.Migrations;

namespace JICtravel.Web.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensivesType_TripDetails_TripDetailsId",
                table: "ExpensivesType");

            migrationBuilder.DropIndex(
                name: "IX_ExpensivesType_TripDetailsId",
                table: "ExpensivesType");

            migrationBuilder.DropColumn(
                name: "TripDetailsId",
                table: "ExpensivesType");

            migrationBuilder.AlterColumn<decimal>(
                name: "Expensive",
                table: "TripDetails",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<int>(
                name: "ExpensiveTypeId",
                table: "TripDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripDetails_ExpensiveTypeId",
                table: "TripDetails",
                column: "ExpensiveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripDetails_ExpensivesType_ExpensiveTypeId",
                table: "TripDetails",
                column: "ExpensiveTypeId",
                principalTable: "ExpensivesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripDetails_ExpensivesType_ExpensiveTypeId",
                table: "TripDetails");

            migrationBuilder.DropIndex(
                name: "IX_TripDetails_ExpensiveTypeId",
                table: "TripDetails");

            migrationBuilder.DropColumn(
                name: "ExpensiveTypeId",
                table: "TripDetails");

            migrationBuilder.AlterColumn<double>(
                name: "Expensive",
                table: "TripDetails",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "TripDetailsId",
                table: "ExpensivesType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpensivesType_TripDetailsId",
                table: "ExpensivesType",
                column: "TripDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensivesType_TripDetails_TripDetailsId",
                table: "ExpensivesType",
                column: "TripDetailsId",
                principalTable: "TripDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
