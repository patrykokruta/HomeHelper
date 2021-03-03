using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.HumidityRepo
{
    public class HumidityRepository : BaseRepository<HumidityReading>, IHumidityRepository
    {
        private readonly ApplicationDbContext _db;

        public HumidityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(HumidityReading humidity)
        {
            _db.Update(humidity);
        }
    }
}
