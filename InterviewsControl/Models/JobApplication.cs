using InterviewsControl.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InterviewsControl.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "公司名稱為必填")]
        [StringLength(100)]
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "職缺名稱為必填")]
        [StringLength(100)]
        [Display(Name = "職缺名稱")]
        public string JobTitle { get; set; } = string.Empty;

        [Url(ErrorMessage = "請輸入有效的網址")]
        [StringLength(500)]
        [Display(Name = "職缺連結")]
        public string? JobUrl { get; set; }

        [Display(Name = "投遞日期")]
        [DataType(DataType.Date)]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [Display(Name = "目前狀態")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        [Display(Name = "備註")]
        public string? Notes { get; set; }

        // 導覽屬性：建立 1 對多關聯 (一個職缺對應多個面試關卡)
        public ICollection<InterviewEvent> InterviewEvents { get; set; } = new List<InterviewEvent>();
    }
}
