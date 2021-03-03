using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.TemperatureRepo
{
    public class TemperatureRepository : BaseRepository<TemperatureReading>, ITemperatureRepository
    {
        private readonly ApplicationDbContext _db;

        public TemperatureRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(TemperatureReading temperature)
        {
            _db.Update(temperature);
        }
    }
}
