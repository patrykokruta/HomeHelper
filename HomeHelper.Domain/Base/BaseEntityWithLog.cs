using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Domain.Base
{
    public class BaseEntityWithLog : BaseEntity
    {
        private DateTime createdDate = DateTime.Now;

        public DateTime CreatedDate
        {
            get { return createdDate; }
            private set { }
        }
        public DateTime LastModifiedDate { get; set; }
    }
}
