﻿// <auto-generated />
using System;
using InsuranceAppWebAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InsuranceAppWebAPI.Migrations
{
    [DbContext(typeof(InsuranceAppContext))]
    partial class InsuranceAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InsuranceAppWebAPI.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerId = 1,
                            Address = "123 death star avenue",
                            DocNumber = "1234567890",
                            Email = "jedi@email.com",
                            FirstName = "Luke",
                            LastName = "Skywalker",
                            Phone = "987654321"
                        },
                        new
                        {
                            CustomerId = 2,
                            Address = "123 death star avenue",
                            DocNumber = "2143658709",
                            Email = "pricess@email.com",
                            FirstName = "Leia",
                            LastName = "Skywalker",
                            Phone = "896745231"
                        });
                });

            modelBuilder.Entity("InsuranceAppWebAPI.Models.Policy", b =>
                {
                    b.Property<int>("PolicyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Coverage")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("RiskType")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PolicyId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Policies");

                    b.HasData(
                        new
                        {
                            PolicyId = 1,
                            Coverage = 70,
                            CustomerId = 1,
                            Description = "Description1",
                            Duration = 12,
                            Name = "Name1",
                            Price = 1200000.0,
                            RiskType = 2,
                            StartDate = new DateTime(2020, 8, 22, 16, 48, 8, 668, DateTimeKind.Local).AddTicks(1562)
                        },
                        new
                        {
                            PolicyId = 2,
                            Coverage = 40,
                            CustomerId = 2,
                            Description = "Description2",
                            Duration = 24,
                            Name = "Name2",
                            Price = 4000000.0,
                            RiskType = 3,
                            StartDate = new DateTime(2020, 8, 22, 16, 48, 8, 669, DateTimeKind.Local).AddTicks(4708)
                        });
                });

            modelBuilder.Entity("InsuranceAppWebAPI.Models.Policy", b =>
                {
                    b.HasOne("InsuranceAppWebAPI.Models.Customer", "Customer")
                        .WithMany("Policies")
                        .HasForeignKey("CustomerId");
                });
#pragma warning restore 612, 618
        }
    }
}
