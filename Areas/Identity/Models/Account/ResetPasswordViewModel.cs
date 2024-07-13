// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspMVC.Areas.Identity.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
            [Required(ErrorMessage = "{0} is required")]
            [EmailAddress(ErrorMessage= "Please enter a valid {0}")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Password confirm")]
            [Compare("Password", ErrorMessage = "Password doesn't match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }

    }
}
