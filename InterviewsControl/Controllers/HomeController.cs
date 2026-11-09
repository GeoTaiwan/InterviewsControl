using InterviewsControl.Data;
using InterviewsControl.Models;
using InterviewsControl.Models.Enums;
using InterviewsControl.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InterviewsControl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // 亮點：將資料庫連線實體注入到首頁控制器
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel();

            // 亮點 1：使用 LINQ 進行非同步統計
            vm.TotalApplications = await _context.JobApplications.CountAsync();
            vm.OfferedCount = await _context.JobApplications.CountAsync(j => j.Status == ApplicationStatus.Offered);
            vm.InterviewingCount = await _context.JobApplications.CountAsync(j => j.Status == ApplicationStatus.Interviewing);
            vm.RejectedCount = await _context.JobApplications.CountAsync(j => j.Status == ApplicationStatus.Rejected);

            // 亮點 2：取得「未來」即將發生的前 5 場面試，並預載(Include)主檔以顯示公司名稱
            vm.UpcomingInterviews = await _context.InterviewEvents
                .Include(i => i.JobApplication)
                .Where(i => i.InterviewDate >= DateTime.Now)
                .OrderBy(i => i.InterviewDate)
                .Take(5)
                .ToListAsync();

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//Scaffold-DBContext "Data Source=localhost;Database=Application;TrustServerCertificate=True;User ID=abc;Password=123" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -NoOnConfiguring -UseDatabaseNames -NoPluralize -Force
