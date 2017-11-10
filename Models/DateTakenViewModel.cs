using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class DateTakenViewModel
    { 
        public string Asst {get; set;}
        [Required]
        [Display(Name="Date:")]
        public DateTime Date {get; set;}
        [Required]
        [Display(Name="Score:")]
        public int Score {get; set;}
        public string Progress {get; set;}
        public string Comment {get; set;} 
    }
}