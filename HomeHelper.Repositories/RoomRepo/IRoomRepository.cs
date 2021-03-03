using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.RoomRepo
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        void Update(Room room);
    }
}
