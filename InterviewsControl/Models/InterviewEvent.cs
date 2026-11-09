using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewsControl.Models
{
    public class InterviewEvent
    {
        [Key]
        public int Id { get; set; }

        // 外鍵 (Foreign Key)
        [Required]
        public int JobApplicationId { get; set; }

        [Required(ErrorMessage = "請輸入面試關卡，例如：一面、人資面談")]
        [StringLength(50)]
        [Display(Name = "面試關卡")]
        public string Round { get; set; } = string.Empty;

        [Display(Name = "面試時間")]
        public DateTime InterviewDate { get; set; }

        [StringLength(50)]
        [Display(Name = "面試方式")] // 例如：實體面試、Microsoft Teams
        public string? InterviewType { get; set; }

        [Display(Name = "面試官/聯絡人")]
        [StringLength(50)]
        public string? Interviewer { get; set; }

        [Display(Name = "心得與考題紀錄")]
        public string? Feedback { get; set; }

        // 導覽屬性：指回所屬的求職紀錄
        [ForeignKey("JobApplicationId")]
        public JobApplication? JobApplication { get; set; }
    }
}
