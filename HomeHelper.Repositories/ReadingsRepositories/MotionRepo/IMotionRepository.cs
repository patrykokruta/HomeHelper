using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.MotionRepo
{
    public interface IMotionRepository : IBaseRepository<MotionReading>
    {
        void Update(MotionReading motion);
    }
}
