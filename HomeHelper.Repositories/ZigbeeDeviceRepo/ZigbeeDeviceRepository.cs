using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ZigbeeDeviceRepo
{
    public class ZigbeeDeviceRepository : BaseRepository<ZigbeeDevice>, IZigbeeDeviceRepository
    {
        private readonly ApplicationDbContext _db;

        public ZigbeeDeviceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ZigbeeDevice zigbeeDevice)
        {
            _db.Update(zigbeeDevice);
        }
    }
}
