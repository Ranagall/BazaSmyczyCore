using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BazaSmyczy.Migrations.LeashDb
{
    public partial class changeLeashSizeToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Leash\" ALTER COLUMN \"Size\" TYPE integer USING (\"Size\"::integer)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Leash\" ALTER COLUMN \"Size\" TYPE text USING (\"Size\"::text)");
        }
    }
}
