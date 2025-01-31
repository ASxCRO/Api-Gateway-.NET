﻿// <auto-generated />
using System;
using AutoStoper.API.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutoStoper.API.Migrations
{
    [DbContext(typeof(AutoStoperDbContext))]
    [Migration("20210527114102_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("AutoStoper")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutoStoper.API.Data.Database.Models.Adresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Odrediste")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Polaziste")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Adrese");
                });

            modelBuilder.Entity("AutoStoper.API.Data.Database.Models.Voznja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdresaId")
                        .HasColumnType("int");

                    b.Property<bool>("AutomatskoOdobrenje")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LjubimciDozvoljeni")
                        .HasColumnType("bit");

                    b.Property<int>("MaksimalnoPutnika")
                        .HasColumnType("int");

                    b.Property<bool>("PusenjeDozvoljeno")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AdresaId");

                    b.ToTable("Voznje");
                });

            modelBuilder.Entity("AutoStoper.API.Data.Database.Models.VoznjaUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("Vozac")
                        .HasColumnType("bit");

                    b.Property<int>("VoznjaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VoznjaId");

                    b.ToTable("VoznjaKorisnik");
                });

            modelBuilder.Entity("AutoStoper.API.Data.Database.Models.Voznja", b =>
                {
                    b.HasOne("AutoStoper.API.Data.Database.Models.Adresa", "Adresa")
                        .WithMany()
                        .HasForeignKey("AdresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adresa");
                });

            modelBuilder.Entity("AutoStoper.API.Data.Database.Models.VoznjaUser", b =>
                {
                    b.HasOne("AutoStoper.API.Data.Database.Models.Voznja", "Voznja")
                        .WithMany()
                        .HasForeignKey("VoznjaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Voznja");
                });
#pragma warning restore 612, 618
        }
    }
}
