using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BazaSmyczy.Core.Stores.Data;

namespace BazaSmyczy.Migrations.LeashDb
{
    [DbContext(typeof(LeashDbContext))]
    [Migration("20170317193415_AddCreationTime")]
    partial class AddCreationTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("BazaSmyczy.Core.Models.Leash", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color")
                        .IsRequired();

                    b.Property<DateTime>("CreationTime");

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
