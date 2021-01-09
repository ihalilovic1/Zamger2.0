using Microsoft.EntityFrameworkCore.Migrations;

namespace Zamger2._0.Migrations
{
    public partial class HomeworkDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Documents_DocumentId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_DocumentId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Homeworks");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "SubmitedHomeworks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubmitedHomeworks_DocumentId",
                table: "SubmitedHomeworks",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmitedHomeworks_Documents_DocumentId",
                table: "SubmitedHomeworks",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitedHomeworks_Documents_DocumentId",
                table: "SubmitedHomeworks");

            migrationBuilder.DropIndex(
                name: "IX_SubmitedHomeworks_DocumentId",
                table: "SubmitedHomeworks");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "SubmitedHomeworks");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Homeworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_DocumentId",
                table: "Homeworks",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Documents_DocumentId",
                table: "Homeworks",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
