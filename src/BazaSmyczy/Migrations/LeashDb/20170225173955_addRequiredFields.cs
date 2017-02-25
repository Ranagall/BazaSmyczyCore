using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BazaSmyczy.Migrations.LeashDb
{
    public partial class addRequiredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Leash",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ImageName",
                table: "Leash",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Leash",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Leash",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageName",
                table: "Leash",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Leash",
                nullable: true);
        }
    }
}
