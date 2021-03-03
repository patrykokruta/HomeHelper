using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.DeviceRepo;
using HomeHelper.Repositories.HttpDeviceRepo;
using HomeHelper.Repositories.MqttDeviceRepo;
using HomeHelper.Repositories.MqttPublishingMessageRepo;
using HomeHelper.Repositories.MqttSubscriptioneMessageRepo;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Room = new RoomRepository(_db);
            Device = new DeviceRepository(_db);
            HttpDevice = new HttpDeviceRepository(_db);
            MqttDevice = new MqttDeviceRepository(_db);
            ZigbeeDevice = new ZigbeeDeviceRepository(_db);
            PublishMessage = new MqttPublishingMessageRepository(_db);
            SubscribeMessage = new MqttSubscriptionMessageRepository(_db);
            BatteryReading = new BatteryRepository(_db);
            ContactReading = new ContactRepository(_db);
            HumidityReading = new HumidityRepository(_db);
            MotionReading = new MotionRepository(_db);
            TemperatureReading = new TemperatureRepository(_db);
        }

        public IRoomRepository Room { get; private set; }
        public IDeviceRepository Device { get; private set; }
        public IHttpDeviceRepository HttpDevice { get; private set; }
        public IMqttDeviceRepository MqttDevice { get; private set; }
        public IZigbeeDeviceRepository ZigbeeDevice { get; private set; }
        public IMqttPublishingMessageRepository PublishMessage { get; private set; }
        public IMqttSubscriptionMessageRepository SubscribeMessage { get; private set; }
        public IBatteryRepository BatteryReading { get; private set; }
        public IContactRepository ContactReading { get; private set; }
        public IHumidityRepository HumidityReading { get; private set; }
        public IMotionRepository MotionReading { get; private set; }
        public ITemperatureRepository TemperatureReading { get; private set; }
        public async ValueTask DisposeAsync()
        {
            await _db.DisposeAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        void IDisposable.Dispose()
        {
            _db.Dispose();
        }
    }
}
