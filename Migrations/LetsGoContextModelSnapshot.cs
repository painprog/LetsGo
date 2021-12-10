﻿// <auto-generated />
using System;
using LetsGo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LetsGo.Migrations
{
    [DbContext(typeof(LetsGoContext))]
    partial class LetsGoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("LetsGo.Models.Event", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AgeLimit")
                        .HasColumnType("int");

                    b.Property<string>("Categories")
                        .HasColumnType("longtext");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EventEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EventStart")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LocationId")
                        .HasColumnType("varchar(255)");

                    b.Property<double>("MinPrice")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("OrganizerId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PosterImage")
                        .HasColumnType("longtext");

                    b.Property<int>("Sold")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDescription")
                        .HasColumnType("longtext");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TicketLimit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("OrganizerId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("LetsGo.Models.EventCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("HasParent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("ParentId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EventCategories");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            HasParent = false,
                            Name = "Концерты"
                        },
                        new
                        {
                            Id = "2",
                            HasParent = false,
                            Name = "Фестивали"
                        },
                        new
                        {
                            Id = "3",
                            HasParent = false,
                            Name = "Спектакли"
                        },
                        new
                        {
                            Id = "4",
                            HasParent = false,
                            Name = "Детям"
                        },
                        new
                        {
                            Id = "5",
                            HasParent = false,
                            Name = "Классика"
                        },
                        new
                        {
                            Id = "6",
                            HasParent = false,
                            Name = "Экскурсии"
                        },
                        new
                        {
                            Id = "7",
                            HasParent = false,
                            Name = "Экскурсии"
                        },
                        new
                        {
                            Id = "8",
                            HasParent = false,
                            Name = "Другое"
                        },
                        new
                        {
                            Id = "9",
                            HasParent = true,
                            Name = "Поп-Музыка",
                            ParentId = "1"
                        },
                        new
                        {
                            Id = "10",
                            HasParent = true,
                            Name = "Рок",
                            ParentId = "1"
                        },
                        new
                        {
                            Id = "11",
                            HasParent = true,
                            Name = "Хип-Хоп",
                            ParentId = "1"
                        },
                        new
                        {
                            Id = "12",
                            HasParent = true,
                            Name = "Комедии",
                            ParentId = "3"
                        },
                        new
                        {
                            Id = "13",
                            HasParent = true,
                            Name = "Драмы",
                            ParentId = "3"
                        },
                        new
                        {
                            Id = "14",
                            HasParent = true,
                            Name = "Мелодрамы",
                            ParentId = "3"
                        },
                        new
                        {
                            Id = "15",
                            HasParent = true,
                            Name = "Опера",
                            ParentId = "5"
                        },
                        new
                        {
                            Id = "16",
                            HasParent = true,
                            Name = "Балет",
                            ParentId = "5"
                        },
                        new
                        {
                            Id = "17",
                            HasParent = true,
                            Name = "Вокал",
                            ParentId = "5"
                        });
                });

            modelBuilder.Entity("LetsGo.Models.EventTicketType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("EventId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int>("Sold")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventTicketTypes");
                });

            modelBuilder.Entity("LetsGo.Models.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Categories")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("LocationImage")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Phones")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Address = "21, 11 Аалы Токомбаева көчөсү, Бишкек",
                            Categories = "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]",
                            Description = "Художественный центр в Бишкеке",
                            Name = "Асанбай Центр",
                            Phones = "[\"+996775979500\"]"
                        },
                        new
                        {
                            Id = "2",
                            Address = "24 просп. Мира, Бишкек",
                            Categories = "[{\"Id\":\"7\",\"Name\":\"Клубы\"}, {\"Id\":\"8\",\"Name\":\"Бары\"}]",
                            Description = "Концертный зал",
                            Name = "Ретро-Метро",
                            Phones = "[\"+996705 000 888\"]"
                        },
                        new
                        {
                            Id = "3",
                            Address = "17 ул. Тоголок Молдо, Бишкек",
                            Categories = "[{\"Id\":\"5\",\"Name\":\"Cпортивные комплексы\"}, {\"Id\":\"10\",\"Name\":\"Другое\"}]",
                            Description = "Концертный зал",
                            Name = "Стадион Спартак",
                            Phones = "[\"+996705 000 888\"]"
                        },
                        new
                        {
                            Id = "4",
                            Address = "167 Советская, Бишкек",
                            Categories = "[{\"Id\":\"1\",\"Name\":\"Театры\"}]",
                            Description = "Театр оперы и балета",
                            Name = "Театр Оперы и Балета",
                            Phones = "[\"0312 621 619\"]"
                        });
                });

            modelBuilder.Entity("LetsGo.Models.LocationCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LocationCategories");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Концерты"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Фестивали"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Спектакли"
                        },
                        new
                        {
                            Id = "4",
                            Name = "Детям"
                        },
                        new
                        {
                            Id = "5",
                            Name = "Классика"
                        },
                        new
                        {
                            Id = "6",
                            Name = "Экскурсии"
                        },
                        new
                        {
                            Id = "7",
                            Name = "Другое"
                        });
                });

            modelBuilder.Entity("LetsGo.Models.PurchasedTicket", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("CustomerName")
                        .HasColumnType("longtext");

                    b.Property<string>("CustomerPhone")
                        .HasColumnType("longtext");

                    b.Property<string>("EventTicketTypeId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("QR")
                        .HasColumnType("longtext");

                    b.Property<bool>("Scanned")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("TicketIdentifier")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("EventTicketTypeId");

                    b.ToTable("PurchasedTickets");
                });

            modelBuilder.Entity("LetsGo.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AvatarLink")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LetsGo.Models.Event", b =>
                {
                    b.HasOne("LetsGo.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("LetsGo.Models.User", "Organizer")
                        .WithMany()
                        .HasForeignKey("OrganizerId");

                    b.Navigation("Location");

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("LetsGo.Models.EventTicketType", b =>
                {
                    b.HasOne("LetsGo.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("LetsGo.Models.PurchasedTicket", b =>
                {
                    b.HasOne("LetsGo.Models.EventTicketType", "EventTicketType")
                        .WithMany()
                        .HasForeignKey("EventTicketTypeId");

                    b.Navigation("EventTicketType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LetsGo.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LetsGo.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LetsGo.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LetsGo.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
