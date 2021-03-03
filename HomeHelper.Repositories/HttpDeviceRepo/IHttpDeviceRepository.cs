using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.HttpDeviceRepo
{
    public interface IHttpDeviceRepository : IBaseRepository<HttpDevice>
    {
        void Update(HttpDevice httpDevice);
    }
}
