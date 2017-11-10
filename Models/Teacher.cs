using System;

namespace AssessTracker.Models
{
    public class Teacher
    {
        public int id {get; set;}
        public string Prefix {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string GradeLevel {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}