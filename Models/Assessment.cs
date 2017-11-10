using System;
using System.Collections.Generic;

namespace AssessTracker.Models
{
    public class Assessment
    {
        public int id {get; set;}
        public string Name {get; set;}
        public List<DateTaken> DateTaken {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

    }
}
