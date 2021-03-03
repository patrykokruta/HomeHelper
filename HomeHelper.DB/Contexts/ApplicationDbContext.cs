using HomeHelper.Domain;
using HomeHelper.Domain.Readings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.DB
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<MqttDevice> MqttDevices { get; set; }
        public DbSet<HttpDevice> HttpDevices { get; set; }
        public DbSet<ZigbeeDevice> ZigbeeDevices { get; set; }
        public DbSet<MqttPublishingMessage> PublishMessages { get; set; }
        public DbSet<MqttSubscriptionMessage> SubscribeMessages { get; set; }
        public DbSet<BatteryReading> BatteryReadings { get; set; }
        public DbSet<ContactReading> ContactReadings { get; set; }
        public DbSet<HumidityReading> HumidityReadings { get; set; }
        public DbSet<MotionReading> MotionReadings { get; set; }
        public DbSet<TemperatureReading> TemperatureReadings { get; set; }
    }
}
