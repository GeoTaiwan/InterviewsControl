using System.Collections.Generic;

namespace InterviewsControl.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalApplications { get; set; }
        public int OfferedCount { get; set; }
        public int InterviewingCount { get; set; }
        public int RejectedCount { get; set; }

        // 用來顯示近期即將到來的面試行程
        public List<InterviewEvent> UpcomingInterviews { get; set; } = new List<InterviewEvent>();
    }
}