using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;

namespace HomeHelper.Repositories.DeviceRepo
{
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        private readonly ApplicationDbContext _db;

        public DeviceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Device device)
        {
            _db.Update(device);
        }
    }
}
