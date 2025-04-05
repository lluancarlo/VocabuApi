using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabu.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJobLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastRun = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastRunSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_Id",
                schema: "dbo",
                table: "JobLogs",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobLogs",
                schema: "dbo");
        }
    }
}
