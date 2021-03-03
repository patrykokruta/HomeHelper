using HomeHelper.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Domain
{
    public class Room : BaseEntityWithLog
    {
        public string Name { get; set; }
    }
}
