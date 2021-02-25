using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEU = table.Column<bool>(type: "bit", nullable: false),
                    VAT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false),
                    IsVATPayer = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "IsEU", "Name", "VAT" },
                values: new object[] { 1, new DateTime(2021, 2, 25, 22, 45, 55, 47, DateTimeKind.Local).AddTicks(6415), false, true, "Lithuania", 21m });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "IsEU", "Name", "VAT" },
                values: new object[] { 2, new DateTime(2021, 2, 25, 22, 45, 55, 51, DateTimeKind.Local).AddTicks(1894), false, false, "Russia", 20m });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "IsEU", "Name", "VAT" },
                values: new object[] { 3, new DateTime(2021, 2, 25, 22, 45, 55, 51, DateTimeKind.Local).AddTicks(1943), false, false, "United kingdom", 20m });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "CountryId", "CreatedDate", "IsCompany", "IsDeleted", "IsVATPayer" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2021, 2, 25, 22, 45, 55, 52, DateTimeKind.Local).AddTicks(7449), true, false, true },
                    { 4, 1, new DateTime(2021, 2, 25, 22, 45, 55, 52, DateTimeKind.Local).AddTicks(9550), false, false, false },
                    { 2, 2, new DateTime(2021, 2, 25, 22, 45, 55, 52, DateTimeKind.Local).AddTicks(9529), true, false, true },
                    { 3, 3, new DateTime(2021, 2, 25, 22, 45, 55, 52, DateTimeKind.Local).AddTicks(9546), false, false, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_CountryId",
                table: "Members",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
