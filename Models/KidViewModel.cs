using System;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class KidViewModel
    {
        
        [Required]
        [Display(Name="First Name:")]
        public string FirstName {get; set;}
        [Required]
        [Display(Name="Last Name:")]
        public string LastName {get; set;}
        [Required]
        [Display(Name="Birthdate:")]
        public DateTime Birthdate {get; set;}
        [Required]
        public string Teacher {get; set;}


    }
}