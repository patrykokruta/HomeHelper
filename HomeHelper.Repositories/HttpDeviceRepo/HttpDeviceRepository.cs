using HomeHelper.DB;
using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.HttpDeviceRepo
{
    public class HttpDeviceRepository : BaseRepository<HttpDevice>, IHttpDeviceRepository
    {
        private readonly ApplicationDbContext _db;

        public HttpDeviceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(HttpDevice httpDevice)
        {
            _db.Update(httpDevice);
        }
    }
}
