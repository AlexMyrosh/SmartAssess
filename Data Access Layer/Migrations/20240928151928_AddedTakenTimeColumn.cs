using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddedTakenTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TakenTimeToComplete",
                table: "UserExamAttempts",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakenTimeToComplete",
                table: "UserExamAttempts");
        }
    }
}
