﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SyleniumApi.DbContexts;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    [DbContext(typeof(SyleniumDbContext))]
    [Migration("20250110230101_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SyleniumApi.Models.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("AccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.FinancialCategory", b =>
                {
                    b.Property<int>("FinancialCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FinancialCategoryId"));

                    b.Property<string>("FinancialCategoryName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("FinancialCategoryId");

                    b.ToTable("FinancialCategory");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Ledger", b =>
                {
                    b.Property<int>("LedgerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LedgerId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LedgerName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("LedgerId");

                    b.ToTable("Ledger");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<bool>("Cleared")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<decimal>("Inflow")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Outflow")
                        .HasColumnType("numeric");

                    b.Property<int?>("TransactionCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("TransactionCategoryId");

                    b.HasIndex("VendorId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.TransactionCategory", b =>
                {
                    b.Property<int>("TransactionCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionCategoryId"));

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("TransactionCategoryName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("TransactionCategoryId");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("TransactionCategoryName", "ParentCategoryId")
                        .IsUnique();

                    b.ToTable("TransactionCategory");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Vendor", b =>
                {
                    b.Property<int>("VendorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VendorId"));

                    b.Property<string>("VendorName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("VendorId");

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Transaction", b =>
                {
                    b.HasOne("SyleniumApi.Models.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SyleniumApi.Models.Entities.TransactionCategory", "TransactionCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId");

                    b.HasOne("SyleniumApi.Models.Entities.Vendor", "Vendor")
                        .WithMany("Transactions")
                        .HasForeignKey("VendorId");

                    b.Navigation("Account");

                    b.Navigation("TransactionCategory");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.TransactionCategory", b =>
                {
                    b.HasOne("SyleniumApi.Models.Entities.TransactionCategory", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.TransactionCategory", b =>
                {
                    b.Navigation("SubCategories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SyleniumApi.Models.Entities.Vendor", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
