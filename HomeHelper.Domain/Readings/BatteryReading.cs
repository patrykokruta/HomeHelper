using HomeHelper.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Domain.Readings
{
    public class BatteryReading : BaseEntity
    {
        public double? Value { get; set; }
        public string ConnectedDeviceId { get; set; }
        private DateTime createdDate = DateTime.Now;
        public DateTime CreatedDate
        {
            get { return createdDate; }
            private set { }
        }
    }
}
