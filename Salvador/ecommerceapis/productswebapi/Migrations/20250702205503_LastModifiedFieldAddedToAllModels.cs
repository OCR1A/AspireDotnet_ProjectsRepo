using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace productswebapi.Migrations
{
    /// <inheritdoc />
    public partial class LastModifiedFieldAddedToAllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Products",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Categories",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Categories");
        }
    }
}
