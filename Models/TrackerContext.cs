using Microsoft.EntityFrameworkCore;

namespace AssessTracker.Models
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options){}

        public DbSet<User> Users {get; set;}
        public DbSet<Kid> Kids {get; set;}
        public DbSet<Assessment> Assessments {get; set;}
        public DbSet<DateTaken> DateTaken {get; set;}
        public DbSet<Teacher> Teachers {get; set;}
        
    }
}