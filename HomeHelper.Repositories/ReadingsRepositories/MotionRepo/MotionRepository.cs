using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.MotionRepo
{
    public class MotionRepository : BaseRepository<MotionReading>, IMotionRepository
    {
        private readonly ApplicationDbContext _db;

        public MotionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MotionReading motion)
        {
            _db.Update(motion);
        }
    }
}
