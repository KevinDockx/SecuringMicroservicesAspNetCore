using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboTicket.Services.Discount.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.CouponId);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "Amount", "UserId" },
                values: new object[] { new Guid("12de6791-38ef-474c-abf6-80a0651341a8"), 10, new Guid("e455a3df-7fa5-47e0-8435-179b300d531f") });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "Amount", "UserId" },
                values: new object[] { new Guid("b99910b8-fd95-4f80-84b4-32eaa8159d35"), 20, new Guid("bbf594b0-3761-4a65-b04c-eec4836d9070") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
