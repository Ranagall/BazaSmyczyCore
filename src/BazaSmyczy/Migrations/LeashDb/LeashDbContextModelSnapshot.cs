using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BazaSmyczy.Data;

namespace BazaSmyczy.Migrations.LeashDb
{
    [DbContext(typeof(LeashDbContext))]
    partial class LeashDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("BazaSmyczy.Models.Leash", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color")
                        .IsRequired();

                    b.Property<string>("Desc");

                    b.Property<string>("ImageName");

                    b.Property<int>("Size");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Leash");
                });
        }
    }
}
