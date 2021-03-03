using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeHelper.DTO.Account
{
    public class RegisterRequest : IBaseRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        [Remote(action: "VerifyEmail", controller: "Account", areaName: "Identity", ErrorMessage = "Given address already exists.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and maximally {1} characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and maximally {1} characters long", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Database password")]
        public string DbPassword { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and maximally {1} characters long", MinimumLength = 2)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

    }
}
