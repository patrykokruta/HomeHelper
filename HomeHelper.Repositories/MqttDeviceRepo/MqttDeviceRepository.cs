using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.MqttDeviceRepo
{
    public class MqttDeviceRepository : BaseRepository<MqttDevice>, IMqttDeviceRepository
    {
        private readonly ApplicationDbContext _db;

        public MqttDeviceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MqttDevice mqttDevice)
        {
            _db.Update(mqttDevice);
        }
    }
}
