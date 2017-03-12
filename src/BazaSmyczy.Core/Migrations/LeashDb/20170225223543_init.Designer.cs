using BazaSmyczy.Core.Stores.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BazaSmyczy.Migrations.LeashDb
{
    [DbContext(typeof(LeashDbContext))]
    [Migration("20170225223543_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
