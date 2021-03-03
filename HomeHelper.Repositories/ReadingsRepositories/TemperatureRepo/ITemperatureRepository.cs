using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.TemperatureRepo
{
    public interface ITemperatureRepository : IBaseRepository<TemperatureReading>
    {
        void Update(TemperatureReading temperature);
    }
}
