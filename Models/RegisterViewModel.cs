using System;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName {get; set;}
        [Required]
        [MinLength(2)]
        public string LastName {get; set;}
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [MinLength(2)]
        public string Organization {get; set;}
        [Required]
        [MinLength(8)]
        public string Password {get; set;}
        [Required]
        [Compare("Password",ErrorMessage="Confirm Password field must match Password field.")]
        public string PWConfirm {get; set;}
        
    }
}