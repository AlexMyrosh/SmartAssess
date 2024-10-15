using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class updateddeletedproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserExamAttempts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExamQuestions");

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Exams",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Courses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_DeletedById",
                table: "Exams",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DeletedById",
                table: "Courses",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DeletedById",
                table: "AspNetUsers",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                table: "AspNetUsers",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_DeletedById",
                table: "Courses",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_DeletedById",
                table: "Exams",
                column: "DeletedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_DeletedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_DeletedById",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_DeletedById",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_DeletedById",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DeletedById",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DeletedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserExamAttempts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExamQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
