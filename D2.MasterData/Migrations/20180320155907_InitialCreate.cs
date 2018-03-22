using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdministrationUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Edit = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    UserKey = table.Column<string>(maxLength: 10, nullable: false),
                    YearOfConstuction = table.Column<DateTime>(nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrationUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdministrationUnitId = table.Column<Guid>(nullable: false),
                    Edit = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false),
                    Address_City = table.Column<string>(nullable: true),
                    Address_Number = table.Column<string>(nullable: true),
                    Address_PostalCode = table.Column<string>(nullable: true),
                    Address_Street = table.Column<string>(nullable: true),
                    Address_Country_Iso2 = table.Column<string>(nullable: true),
                    Address_Country_Iso3 = table.Column<string>(nullable: true),
                    Address_Country_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrances_AdministrationUnits_AdministrationUnitId",
                        column: x => x.AdministrationUnitId,
                        principalTable: "AdministrationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Edit = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EntranceId = table.Column<Guid>(nullable: false),
                    Floor = table.Column<int>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Usage = table.Column<string>(maxLength: 256, nullable: true),
                    xmin = table.Column<uint>(type: "xid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubUnits_Entrances_EntranceId",
                        column: x => x.EntranceId,
                        principalTable: "Entrances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_AdministrationUnitId",
                table: "Entrances",
                column: "AdministrationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SubUnits_EntranceId",
                table: "SubUnits",
                column: "EntranceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubUnits");

            migrationBuilder.DropTable(
                name: "Entrances");

            migrationBuilder.DropTable(
                name: "AdministrationUnits");
        }
    }
}
