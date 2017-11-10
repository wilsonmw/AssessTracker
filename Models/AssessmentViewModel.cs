using System;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class AssessmentViewModel
    {
        [Required]
        public string Name {get; set;}

    }
}