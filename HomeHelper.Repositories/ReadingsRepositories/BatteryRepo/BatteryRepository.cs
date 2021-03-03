using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.BatteryRepo
{
    public class BatteryRepository : BaseRepository<BatteryReading>, IBatteryRepository
    {
        private readonly ApplicationDbContext _db;

        public BatteryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(BatteryReading battery)
        {
            _db.Update(battery);
        }
    }
}
