using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BazaSmyczy.Data;

namespace BazaSmyczy.Migrations.TokenDb
{
    [DbContext(typeof(TokenDbContext))]
    [Migration("20170224205658_CreateTokensTable")]
    partial class CreateTokensTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("BazaSmyczy.Models.Token", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Email");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.ToTable("Tokens");
                });
        }
    }
}
