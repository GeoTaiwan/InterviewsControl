using InterviewsControl.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewsControl.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<InterviewEvent> InterviewEvents { get; set; }
    }
}
