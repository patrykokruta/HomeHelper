using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.HumidityRepo
{
    public interface IHumidityRepository : IBaseRepository<HumidityReading>
    {
        void Update(HumidityReading humidity);
    }
}
