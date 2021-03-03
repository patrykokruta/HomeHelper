using HomeHelper.Domain.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeHelper.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        private DateTime createdDate = DateTime.Now;

        public DateTime CreatedDate
        {
            get { return createdDate; }
            private set { }
        }
    }
}
