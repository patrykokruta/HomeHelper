using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeHelper.DB.Migrations
{
    public partial class addSensorReadings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatteryReadings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<double>(nullable: true),
                    ConnectedDeviceId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatteryReadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactReadings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<bool>(nullable: true),
                    ConnectedDeviceId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactReadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HumidityReadings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<double>(nullable: true),
                    ConnectedDeviceId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumidityReadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotionReadings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<bool>(nullable: true),
                    ConnectedDeviceId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotionReadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureReadings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Value = table.Column<double>(nullable: true),
                    ConnectedDeviceId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureReadings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatteryReadings");

            migrationBuilder.DropTable(
                name: "ContactReadings");

            migrationBuilder.DropTable(
                name: "HumidityReadings");

            migrationBuilder.DropTable(
                name: "MotionReadings");

            migrationBuilder.DropTable(
                name: "TemperatureReadings");
        }
    }
}
