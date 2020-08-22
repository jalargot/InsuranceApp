using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsuranceAppWebAPI.Migrations
{
    public partial class InitialDatabaseSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNumber = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    PolicyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Coverage = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    RiskType = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_Policies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "DocNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 1, "123 death star avenue", "1234567890", "jedi@email.com", "Luke", "Skywalker", "987654321" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "DocNumber", "Email", "FirstName", "LastName", "Phone" },
                values: new object[] { 2, "123 death star avenue", "2143658709", "pricess@email.com", "Leia", "Skywalker", "896745231" });

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "PolicyId", "Coverage", "CustomerId", "Description", "Duration", "Name", "Price", "RiskType", "StartDate" },
                values: new object[] { 1, 70, 1, "Description1", 12, "Name1", 1200000.0, 2, new DateTime(2020, 8, 22, 16, 48, 8, 668, DateTimeKind.Local).AddTicks(1562) });

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "PolicyId", "Coverage", "CustomerId", "Description", "Duration", "Name", "Price", "RiskType", "StartDate" },
                values: new object[] { 2, 40, 2, "Description2", 24, "Name2", 4000000.0, 3, new DateTime(2020, 8, 22, 16, 48, 8, 669, DateTimeKind.Local).AddTicks(4708) });

            migrationBuilder.CreateIndex(
                name: "IX_Policies_CustomerId",
                table: "Policies",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
