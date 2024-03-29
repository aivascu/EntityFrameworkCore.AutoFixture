﻿// <auto-generated />
using System;
using EntityFrameworkCore.AutoFixture.Tests.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.AutoFixture.Tests.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20220803095545_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.27");

            modelBuilder.Entity("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ItemId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Order", b =>
                {
                    b.HasOne("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities.Item", "Item")
                        .WithMany("Orders")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
