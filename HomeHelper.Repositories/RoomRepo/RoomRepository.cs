using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.RoomRepo
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _db;

        public RoomRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Room room)
        {
            _db.Update(room);
        }
    }
}
