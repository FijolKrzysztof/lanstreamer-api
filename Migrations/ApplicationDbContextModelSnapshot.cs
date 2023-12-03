﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using lanstreamer_api.Data.Context;

#nullable disable

namespace lanstreamer_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("lanstreamer_api.Data.Configuration.ConfigurationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("lanstreamer_api.Data.Modules.IpLocation.IpLocationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<string>("Loc")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("loc");

                    b.Property<string>("Org")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("org");

                    b.Property<string>("Postal")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("postal");

                    b.Property<string>("Readme")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("readme");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("region");

                    b.Property<string>("Timezone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("timezone");

                    b.HasKey("Id");

                    b.ToTable("IpLocations");
                });

            modelBuilder.Entity("lanstreamer_api.Data.Modules.User.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessCode")
                        .HasColumnType("text")
                        .HasColumnName("access_code");

                    b.Property<float>("AppVersion")
                        .HasColumnType("real")
                        .HasColumnName("app_version");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("google_id");

                    b.Property<int?>("IpLocationId")
                        .HasColumnType("integer")
                        .HasColumnName("ip_location_id");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login");

                    b.HasKey("Id");

                    b.HasIndex("IpLocationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("lanstreamer_api.Entities.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("Downloads")
                        .HasColumnType("integer")
                        .HasColumnName("downloads");

                    b.Property<int?>("IpLocationId")
                        .HasColumnType("integer")
                        .HasColumnName("ip_location_id");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("default_language");

                    b.Property<string>("OperatingSystem")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("operating_system");

                    b.Property<string>("ReferrerWebsite")
                        .HasColumnType("text")
                        .HasColumnName("referrer_website");

                    b.Property<TimeSpan>("TimeOnSite")
                        .HasColumnType("interval")
                        .HasColumnName("time_on_site");

                    b.Property<DateTime>("VisitTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("visit_time");

                    b.HasKey("Id");

                    b.HasIndex("IpLocationId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("lanstreamer_api.Entities.FeedbackEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer")
                        .HasColumnName("client_id");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("lanstreamer_api.Data.Modules.User.UserEntity", b =>
                {
                    b.HasOne("lanstreamer_api.Data.Modules.IpLocation.IpLocationEntity", "IpLocation")
                        .WithMany()
                        .HasForeignKey("IpLocationId");

                    b.Navigation("IpLocation");
                });

            modelBuilder.Entity("lanstreamer_api.Entities.ClientEntity", b =>
                {
                    b.HasOne("lanstreamer_api.Data.Modules.IpLocation.IpLocationEntity", "IpLocation")
                        .WithMany()
                        .HasForeignKey("IpLocationId");

                    b.Navigation("IpLocation");
                });

            modelBuilder.Entity("lanstreamer_api.Entities.FeedbackEntity", b =>
                {
                    b.HasOne("lanstreamer_api.Entities.ClientEntity", "Client")
                        .WithMany("Feedbacks")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("lanstreamer_api.Entities.ClientEntity", b =>
                {
                    b.Navigation("Feedbacks");
                });
#pragma warning restore 612, 618
        }
    }
}
