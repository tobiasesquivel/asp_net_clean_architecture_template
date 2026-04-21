using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RepliesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentcommentId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ParentCommendId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommendId",
                table: "Comment",
                column: "ParentCommendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommendId",
                table: "Comment",
                column: "ParentCommendId",
                principalTable: "Comment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommendId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentCommendId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ParentCommendId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "ParentcommentId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
