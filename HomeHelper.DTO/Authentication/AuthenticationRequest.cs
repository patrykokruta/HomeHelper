using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeHelper.DTO.Authentication
{
    public class AuthenticationRequest : IBaseRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
