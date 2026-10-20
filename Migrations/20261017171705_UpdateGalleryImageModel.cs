using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SusanOsuolaleCV.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGalleryImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "GalleryImages",
                newName: "FileName");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GalleryImages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GalleryImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GalleryImages");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "GalleryImages",
                newName: "FilePath");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GalleryImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
