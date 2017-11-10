using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessTracker.Models
{
    public class Kid
    {
        public int id {get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        [Required]
        public DateTime Birthdate {get; set;}
        public Boolean Active {get; set;}
        public int TeacherId {get; set;}
        public Teacher Teacher {get; set;}
        public List<DateTaken> DateTaken {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}


    }
}