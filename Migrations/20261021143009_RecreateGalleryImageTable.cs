using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SusanOsuolaleCV.Migrations
{
    /// <inheritdoc />
    public partial class RecreateGalleryImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryImages");

            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
            Id = table.Column<Guid>(
                type: "uniqueidentifier",
                nullable: false),

            FileName = table.Column<string>(
                type: "nvarchar(max)",
                nullable: false),

            Title = table.Column<string>(
                type: "nvarchar(max)",
                nullable: false),

            Description = table.Column<string>(
                type: "nvarchar(max)",
                nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_GalleryImages", x => x.Id);
        });
}
    }
}
