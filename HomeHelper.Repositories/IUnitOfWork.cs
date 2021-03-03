using HomeHelper.Repositories.DeviceRepo;
using HomeHelper.Repositories.HttpDeviceRepo;
using HomeHelper.Repositories.MqttDeviceRepo;
using HomeHelper.Repositories.MqttPublishingMessageRepo;
using HomeHelper.Repositories.MqttSubscriptionMessageRepo;
using HomeHelper.Repositories.ReadingsRepositories.BatteryRepo;
using HomeHelper.Repositories.ReadingsRepositories.ContactRepo;
using HomeHelper.Repositories.ReadingsRepositories.HumidityRepo;
using HomeHelper.Repositories.ReadingsRepositories.MotionRepo;
using HomeHelper.Repositories.ReadingsRepositories.TemperatureRepo;
using HomeHelper.Repositories.RoomRepo;
using HomeHelper.Repositories.ZigbeeDeviceRepo;
using System;
using System.Threading.Tasks;

namespace HomeHelper.Repositories
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IRoomRepository Room { get; }
        IDeviceRepository Device { get; }
        IHttpDeviceRepository HttpDevice { get; }
        IMqttDeviceRepository MqttDevice { get; }
        IZigbeeDeviceRepository ZigbeeDevice { get; }
        IMqttPublishingMessageRepository PublishMessage { get; }
        IMqttSubscriptionMessageRepository SubscribeMessage { get; }
        IBatteryRepository BatteryReading { get; }
        IContactRepository ContactReading { get; }
        IHumidityRepository HumidityReading { get; }
        IMotionRepository MotionReading { get; }
        ITemperatureRepository TemperatureReading { get; }
        Task SaveAsync();
    }
}
