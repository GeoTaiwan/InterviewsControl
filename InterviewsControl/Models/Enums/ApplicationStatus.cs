using System.ComponentModel.DataAnnotations;
namespace InterviewsControl.Models.Enums
{
    public enum ApplicationStatus
    {
        [Display(Name = "已投遞")]
        Applied = 1,

        [Display(Name = "履歷審查中")]
        Reviewing = 2,

        [Display(Name = "面試進行中")]
        Interviewing = 3,

        [Display(Name = "錄取 (Offer)")]
        Offered = 4,

        [Display(Name = "未錄取 / 感謝信")]
        Rejected = 5
    }
}
