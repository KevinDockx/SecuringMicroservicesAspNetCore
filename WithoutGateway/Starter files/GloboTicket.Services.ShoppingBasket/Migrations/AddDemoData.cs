using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboTicket.Services.ShoppingBasket.Migrations
{
    public partial class AddDemoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Baskets");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Date", "Name" },
                values: new object[,]
                {
                    { new Guid("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"), new DateTime(2021, 5, 3, 16, 55, 48, 899, DateTimeKind.Local).AddTicks(6159), "John Egbert Live" },
                    { new Guid("3448d5a4-0f72-4dd7-bf15-c14a46b26c00"), new DateTime(2021, 8, 3, 16, 55, 48, 902, DateTimeKind.Local).AddTicks(3646), "The State of Affairs: Michael Live!" },
                    { new Guid("b419a7ca-3321-4f38-be8e-4d7b6a529319"), new DateTime(2021, 3, 3, 16, 55, 48, 902, DateTimeKind.Local).AddTicks(3679), "Clash of the DJs" },
                    { new Guid("62787623-4c52-43fe-b0c9-b7044fb5929b"), new DateTime(2021, 3, 3, 16, 55, 48, 902, DateTimeKind.Local).AddTicks(3686), "Spanish guitar hits with Manuel" },
                    { new Guid("1babd057-e980-4cb3-9cd2-7fdd9e525668"), new DateTime(2021, 9, 3, 16, 55, 48, 902, DateTimeKind.Local).AddTicks(3690), "Techorama 2021" },
                    { new Guid("adc42c09-08c1-4d2c-9f96-2d15bb1af299"), new DateTime(2021, 7, 3, 16, 55, 48, 902, DateTimeKind.Local).AddTicks(3695), "To the Moon and Back" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("1babd057-e980-4cb3-9cd2-7fdd9e525668"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("3448d5a4-0f72-4dd7-bf15-c14a46b26c00"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("62787623-4c52-43fe-b0c9-b7044fb5929b"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("adc42c09-08c1-4d2c-9f96-2d15bb1af299"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("b419a7ca-3321-4f38-be8e-4d7b6a529319"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("ee272f8b-6096-4cb6-8625-bb4bb2d89e8b"));

            migrationBuilder.AddColumn<Guid>(
                name: "CouponId",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
