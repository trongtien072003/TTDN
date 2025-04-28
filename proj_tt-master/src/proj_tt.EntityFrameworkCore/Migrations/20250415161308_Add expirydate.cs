using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj_tt.Migrations
{
    /// <inheritdoc />
    public partial class Addexpirydate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "AppProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "AppProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "AppProducts");
        }
    }
}
