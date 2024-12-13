﻿// <auto-generated />
using System;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Database.Migrations
{
    [DbContext(typeof(PurchaseManagerContext))]
    [Migration("20190509024128_AddRefreshTokenCreationDate")]
    partial class AddRefreshTokenCreationDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Domain.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("OwnerId")
                        .HasColumnName("OwnerID");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Domain.Models.GroupItem", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnName("GroupID");

                    b.Property<int>("ItemId")
                        .HasColumnName("ItemID");

                    b.Property<int>("ItemStatusId")
                        .HasColumnName("ItemStatusID");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Notes")
                        .HasMaxLength(255);

                    b.HasKey("GroupId", "ItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("ItemStatusId");

                    b.ToTable("GroupItems");
                });

            modelBuilder.Entity("Domain.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Domain.Models.ItemStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("ItemStatuses");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            Name = "Out"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Okay"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Almost out"
                        },
                        new
                        {
                            Id = 4,
                            Name = "One time purchase"
                        });
                });

            modelBuilder.Entity("Domain.Models.RefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.Property<string>("Token")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<bool?>("IsInvalidated")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("FALSE");

                    b.HasKey("UserId", "Token");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Models.UserGroup", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("UserID");

                    b.Property<int>("GroupId")
                        .HasColumnName("GroupID");

                    b.Property<bool?>("IsAcceptedByManager")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ManagerAccepted")
                        .HasDefaultValueSql("FALSE");

                    b.Property<bool?>("IsAcceptedByUser")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserAccepted")
                        .HasDefaultValueSql("FALSE");

                    b.Property<bool?>("IsManager")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("FALSE");

                    b.Property<string>("RequestMessage")
                        .HasMaxLength(255);

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Domain.Models.Group", b =>
                {
                    b.HasOne("Domain.Models.User", "Owner")
                        .WithMany("OwnedGroups")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Domain.Models.GroupItem", b =>
                {
                    b.HasOne("Domain.Models.Group", "Group")
                        .WithMany("GroupItems")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.Item", "Item")
                        .WithMany("GroupItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.Models.ItemStatus", "ItemStatus")
                        .WithMany("GroupItems")
                        .HasForeignKey("ItemStatusId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Domain.Models.RefreshToken", b =>
                {
                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Models.UserGroup", b =>
                {
                    b.HasOne("Domain.Models.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Models.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
