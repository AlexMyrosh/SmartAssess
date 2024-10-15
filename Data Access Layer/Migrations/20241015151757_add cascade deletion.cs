using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class addcascadedeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_UserExamAttemptId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamAttempts_AspNetUsers_UserId",
                table: "UserExamAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_UserExamAttemptId",
                table: "UserAnswers",
                column: "UserExamAttemptId",
                principalTable: "UserExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamAttempts_AspNetUsers_UserId",
                table: "UserExamAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_UserExamAttemptId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamAttempts_AspNetUsers_UserId",
                table: "UserExamAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_UserExamAttemptId",
                table: "UserAnswers",
                column: "UserExamAttemptId",
                principalTable: "UserExamAttempts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamAttempts_AspNetUsers_UserId",
                table: "UserExamAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
