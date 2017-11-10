using System;
using System.Collections.Generic;

namespace AssessTracker.Models
{
    public class DateTaken
    {
        public int id {get; set;}
        public DateTime Date {get; set;}
        public int Score {get; set;}
        public string Progress {get; set;}
        public string Comment {get; set;}
        public int AssessmentId {get; set;}
        public Assessment Assessment {get; set;}
        public int KidId {get; set;}
        public Kid Kid {get; set;}
        
    }
}