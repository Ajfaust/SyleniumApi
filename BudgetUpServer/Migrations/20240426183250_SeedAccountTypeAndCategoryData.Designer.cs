﻿// <auto-generated />
using System;
using BudgetUpServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgetUpServer.Migrations
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20240426183250_SeedAccountTypeAndCategoryData")]
    partial class SeedAccountTypeAndCategoryData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BudgetUpServer.Entity.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.AccountCategory", b =>
                {
                    b.Property<int>("AccountCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountCategoryId"));

                    b.Property<string>("AccountCategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountCategoryId");

                    b.ToTable("AccountCategory");

                    b.HasData(
                        new
                        {
                            AccountCategoryId = 1,
                            AccountCategoryName = "Asset"
                        },
                        new
                        {
                            AccountCategoryId = 2,
                            AccountCategoryName = "Liability"
                        });
                });

            modelBuilder.Entity("BudgetUpServer.Entity.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountTypeId"));

                    b.Property<int>("AccountCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("AccountTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountTypeId");

                    b.HasIndex("AccountCategoryId");

                    b.ToTable("AccountType");

                    b.HasData(
                        new
                        {
                            AccountTypeId = 1,
                            AccountCategoryId = 1,
                            AccountTypeName = "Checking"
                        },
                        new
                        {
                            AccountTypeId = 2,
                            AccountCategoryId = 1,
                            AccountTypeName = "Savings"
                        },
                        new
                        {
                            AccountTypeId = 3,
                            AccountCategoryId = 1,
                            AccountTypeName = "Investment"
                        },
                        new
                        {
                            AccountTypeId = 4,
                            AccountCategoryId = 2,
                            AccountTypeName = "Credit Card"
                        },
                        new
                        {
                            AccountTypeId = 5,
                            AccountCategoryId = 2,
                            AccountTypeName = "Loan"
                        });
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProfileId"));

                    b.HasKey("ProfileId");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer");

                    b.Property<bool>("Cleared")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("Inflow")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<decimal>("Outflow")
                        .HasColumnType("numeric");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<int?>("TransactionCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("VendorId")
                        .HasColumnType("integer");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("ProfileId");

                    b.HasIndex("TransactionCategoryId");

                    b.HasIndex("VendorId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.TransactionCategory", b =>
                {
                    b.Property<int>("TransactionCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionCategoryId"));

                    b.Property<int?>("ParentTransactionCategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<string>("TransactionCategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TransactionCategoryId");

                    b.HasIndex("ParentTransactionCategoryId");

                    b.HasIndex("ProfileId");

                    b.ToTable("TransactionCategory");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Vendor", b =>
                {
                    b.Property<int>("VendorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("VendorId"));

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<string>("VendorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("VendorId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Account", b =>
                {
                    b.HasOne("BudgetUpServer.Entity.AccountType", "AccountType")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUpServer.Entity.Profile", "Profile")
                        .WithMany("Accounts")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountType");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.AccountType", b =>
                {
                    b.HasOne("BudgetUpServer.Entity.AccountCategory", "AccountCategory")
                        .WithMany("AccountTypes")
                        .HasForeignKey("AccountCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountCategory");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Transaction", b =>
                {
                    b.HasOne("BudgetUpServer.Entity.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("BudgetUpServer.Entity.Profile", "Profile")
                        .WithMany("Transactions")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetUpServer.Entity.TransactionCategory", "TransactionCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId");

                    b.HasOne("BudgetUpServer.Entity.Vendor", "Vendor")
                        .WithMany("Transactions")
                        .HasForeignKey("VendorId");

                    b.Navigation("Account");

                    b.Navigation("Profile");

                    b.Navigation("TransactionCategory");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.TransactionCategory", b =>
                {
                    b.HasOne("BudgetUpServer.Entity.TransactionCategory", "ParentTransactionCategory")
                        .WithMany("ChildTransactionCategories")
                        .HasForeignKey("ParentTransactionCategoryId");

                    b.HasOne("BudgetUpServer.Entity.Profile", "Profile")
                        .WithMany("Categories")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentTransactionCategory");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Vendor", b =>
                {
                    b.HasOne("BudgetUpServer.Entity.Profile", "Profile")
                        .WithMany("Vendors")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.AccountCategory", b =>
                {
                    b.Navigation("AccountTypes");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.AccountType", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Profile", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Categories");

                    b.Navigation("Transactions");

                    b.Navigation("Vendors");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.TransactionCategory", b =>
                {
                    b.Navigation("ChildTransactionCategories");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("BudgetUpServer.Entity.Vendor", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
