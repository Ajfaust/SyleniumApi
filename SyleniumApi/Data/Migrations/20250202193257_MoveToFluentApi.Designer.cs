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
    [Migration("20250202193257_MoveToFluentApi")]
    partial class MoveToFluentApi
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SyleniumApi.Data.Entities.ActiveLedger", b =>
                {
                    b.Property<int>("LedgerId")
                        .HasColumnType("integer");

                    b.HasKey("LedgerId");

                    b.ToTable("ActiveLedger");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FinancialAccountCategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("LedgerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("FinancialAccountCategoryId");

                    b.HasIndex("LedgerId");

                    b.ToTable("FinancialAccounts", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccountCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LedgerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LedgerId");

                    b.ToTable("FinancialAccountCategories", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Ledger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Ledger", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Cleared")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("FinancialAccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Inflow")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Outflow")
                        .HasColumnType("numeric");

                    b.Property<int>("TransactionCategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TransactionCategoryId");

                    b.HasIndex("VendorId");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.TransactionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LedgerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LedgerId");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("Name", "ParentCategoryId")
                        .IsUnique();

                    b.ToTable("TransactionCategories", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LedgerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("LedgerId");

                    b.ToTable("Vendors", (string)null);
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.ActiveLedger", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.Ledger", "Ledger")
                        .WithMany()
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ledger");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccount", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.FinancialAccountCategory", "FinancialAccountCategory")
                        .WithMany("FinancialAccounts")
                        .HasForeignKey("FinancialAccountCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SyleniumApi.Data.Entities.Ledger", "Ledger")
                        .WithMany("FinancialAccounts")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAccountCategory");

                    b.Navigation("Ledger");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccountCategory", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.Ledger", "Ledger")
                        .WithMany("FinancialAccountCategories")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ledger");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Transaction", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.FinancialAccount", "FinancialAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SyleniumApi.Data.Entities.TransactionCategory", "TransactionCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SyleniumApi.Data.Entities.Vendor", "Vendor")
                        .WithMany("Transactions")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAccount");

                    b.Navigation("TransactionCategory");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.TransactionCategory", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.Ledger", "Ledger")
                        .WithMany("TransactionCategories")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SyleniumApi.Data.Entities.TransactionCategory", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("Ledger");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Vendor", b =>
                {
                    b.HasOne("SyleniumApi.Data.Entities.Ledger", "Ledger")
                        .WithMany("Vendors")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ledger");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccount", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.FinancialAccountCategory", b =>
                {
                    b.Navigation("FinancialAccounts");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Ledger", b =>
                {
                    b.Navigation("FinancialAccountCategories");

                    b.Navigation("FinancialAccounts");

                    b.Navigation("TransactionCategories");

                    b.Navigation("Vendors");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.TransactionCategory", b =>
                {
                    b.Navigation("SubCategories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SyleniumApi.Data.Entities.Vendor", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
