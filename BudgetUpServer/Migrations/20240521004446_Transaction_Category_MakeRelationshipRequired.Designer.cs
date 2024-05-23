﻿// <auto-generated />
using System;
using AllostaServer.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllostaServer.Migrations
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20240521004446_Transaction_Category_MakeRelationshipRequired")]
    partial class Transaction_Category_MakeRelationshipRequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AllostaServer.Models.Entities.FinancialAccount", b =>
                {
                    b.Property<int>("FinancialAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FinancialAccountId"));

                    b.Property<int>("FinancialAccountTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FinancialAccountId");

                    b.HasIndex("FinancialAccountTypeId");

                    b.ToTable("FinancialAccount");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.FinancialAccountType", b =>
                {
                    b.Property<int>("FinancialAccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FinancialAccountTypeId"));

                    b.Property<int>("FinancialCategory")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FinancialAccountTypeId");

                    b.ToTable("FinancialAccountType");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<bool>("Cleared")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FinancialAccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Inflow")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<decimal>("Outflow")
                        .HasColumnType("numeric");

                    b.Property<int>("TransactionCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("TransactionId");

                    b.HasIndex("FinancialAccountId");

                    b.HasIndex("TransactionCategoryId");

                    b.HasIndex("VendorId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.TransactionCategory", b =>
                {
                    b.Property<int>("TransactionCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionCategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("TransactionCategoryId");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("Name", "ParentCategoryId")
                        .IsUnique();

                    b.ToTable("TransactionCategory");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.Vendor", b =>
                {
                    b.Property<int>("VendorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VendorId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("VendorId");

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.FinancialAccount", b =>
                {
                    b.HasOne("AllostaServer.Models.Entities.FinancialAccountType", "FinancialAccountType")
                        .WithMany("FinancialAccounts")
                        .HasForeignKey("FinancialAccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialAccountType");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.Transaction", b =>
                {
                    b.HasOne("AllostaServer.Models.Entities.FinancialAccount", "FinancialAccount")
                        .WithMany()
                        .HasForeignKey("FinancialAccountId");

                    b.HasOne("AllostaServer.Models.Entities.TransactionCategory", "TransactionCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AllostaServer.Models.Entities.Vendor", "Vendor")
                        .WithMany("Transactions")
                        .HasForeignKey("VendorId");

                    b.Navigation("FinancialAccount");

                    b.Navigation("TransactionCategory");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.TransactionCategory", b =>
                {
                    b.HasOne("AllostaServer.Models.Entities.TransactionCategory", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.FinancialAccountType", b =>
                {
                    b.Navigation("FinancialAccounts");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.TransactionCategory", b =>
                {
                    b.Navigation("SubCategories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("AllostaServer.Models.Entities.Vendor", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
