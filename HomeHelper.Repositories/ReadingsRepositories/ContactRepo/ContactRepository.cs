using HomeHelper.DB;
using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.ContactRepo
{
    public class ContactRepository : BaseRepository<ContactReading>, IContactRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ContactReading contact)
        {
            _db.Update(contact);
        }
    }
}
