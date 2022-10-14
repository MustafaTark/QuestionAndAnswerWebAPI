using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowAPI.Migrations
{
    public partial class UpdaateForien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Questions_QuestionObjectId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_QuestionObjectId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "QuestionObjectId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuestionId",
                table: "Comments",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Questions_QuestionId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_QuestionId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionObjectId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuestionObjectId",
                table: "Comments",
                column: "QuestionObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Questions_QuestionObjectId",
                table: "Comments",
                column: "QuestionObjectId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
