using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace lanstreamer_api.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "IpLocations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "text", nullable: false),
                    region = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    loc = table.Column<string>(type: "text", nullable: false),
                    org = table.Column<string>(type: "text", nullable: false),
                    postal = table.Column<string>(type: "text", nullable: false),
                    timezone = table.Column<string>(type: "text", nullable: false),
                    readme = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpLocations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip_location_id = table.Column<int>(type: "integer", nullable: true),
                    visit_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    operating_system = table.Column<string>(type: "text", nullable: false),
                    default_language = table.Column<string>(type: "text", nullable: false),
                    time_on_site = table.Column<TimeSpan>(type: "interval", nullable: false),
                    referrer_website = table.Column<string>(type: "text", nullable: true),
                    downloads = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clients_IpLocations_ip_location_id",
                        column: x => x.ip_location_id,
                        principalTable: "IpLocations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip_location_id = table.Column<int>(type: "integer", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    google_id = table.Column<string>(type: "text", nullable: false),
                    app_version = table.Column<float>(type: "real", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    access_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_IpLocations_ip_location_id",
                        column: x => x.ip_location_id,
                        principalTable: "IpLocations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ip_location_id",
                table: "Clients",
                column: "ip_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_client_id",
                table: "Feedbacks",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ip_location_id",
                table: "Users",
                column: "ip_location_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "IpLocations");
        }
    }
}
