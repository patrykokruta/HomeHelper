using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.BatteryRepo
{
    public interface IBatteryRepository : IBaseRepository<BatteryReading>
    {
        void Update(BatteryReading battery);
    }
}
