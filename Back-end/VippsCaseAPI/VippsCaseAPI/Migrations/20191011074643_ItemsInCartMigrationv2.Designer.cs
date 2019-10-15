﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VippsCaseAPI.DataAccess;

namespace VippsCaseAPI.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20191011074643_ItemsInCartMigrationv2")]
    partial class ItemsInCartMigrationv2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VippsCaseAPI.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Status");

                    b.Property<int>("UserId");

                    b.HasKey("CartId");

                    b.ToTable("carts");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int?>("CartId");

                    b.Property<string>("ImageUrl")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Price");

                    b.HasKey("ItemId");

                    b.HasIndex("CartId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Total");

                    b.Property<int>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("ItemId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Password", b =>
                {
                    b.Property<int>("PasswordId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("PasswordId");

                    b.HasIndex("UserId");

                    b.ToTable("Password");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.PaymentInfo", b =>
                {
                    b.Property<int>("PaymentInfoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PaymentToken")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("PaymentInfoId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentInfo");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Firstname")
                        .IsRequired();

                    b.Property<string>("Lastname")
                        .IsRequired();

                    b.Property<string>("PhoneNr");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Item", b =>
                {
                    b.HasOne("VippsCaseAPI.Models.Cart")
                        .WithMany("items")
                        .HasForeignKey("CartId");
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Order", b =>
                {
                    b.HasOne("VippsCaseAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VippsCaseAPI.Models.OrderItem", b =>
                {
                    b.HasOne("VippsCaseAPI.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VippsCaseAPI.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VippsCaseAPI.Models.Password", b =>
                {
                    b.HasOne("VippsCaseAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VippsCaseAPI.Models.PaymentInfo", b =>
                {
                    b.HasOne("VippsCaseAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
