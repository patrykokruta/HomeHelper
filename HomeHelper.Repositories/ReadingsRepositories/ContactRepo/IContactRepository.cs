using HomeHelper.Domain.Readings;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ReadingsRepositories.ContactRepo
{
    public interface IContactRepository : IBaseRepository<ContactReading>
    {
        void Update(ContactReading contact);
    }
}
