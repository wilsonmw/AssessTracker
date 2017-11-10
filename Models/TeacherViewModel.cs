using System;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class TeacherViewModel
    {
        [Required]
        public string Prefix {get; set;}
        [Required]
        [Display(Name="First Name:")]
        public string FirstName {get; set;}
        [Required]
        [Display(Name="Last Name:")]
        public string LastName {get; set;}
        [Required]
        [Display(Name="Grade Level:")]
        public string GradeLevel {get; set;}
        
    }
}