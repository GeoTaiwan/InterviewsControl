using InterviewsControl.Data;
using InterviewsControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using InterviewsControl.Data; // 視你的 ApplicationDbContext 所在位置調整

namespace InterviewsControl.Controllers
{
    [Authorize]
    public class InterviewEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterviewEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InterviewEvents/Create
        // 亮點 1：接收由 JobApplications/Details 傳遞過來的 jobApplicationId
        public async Task<IActionResult> Create(int jobApplicationId)
        {
            // 驗證該筆求職紀錄是否存在
            var jobApp = await _context.JobApplications.FindAsync(jobApplicationId);
            if (jobApp == null)
            {
                return NotFound();
            }

            // 將公司與職缺名稱透過 ViewBag 傳給前端顯示，增加表單易讀性
            ViewBag.JobInfo = $"{jobApp.CompanyName} - {jobApp.JobTitle}";

            // 預先綁定 Foreign Key 與預設時間
            var model = new InterviewEvent
            {
                JobApplicationId = jobApplicationId,
                InterviewDate = DateTime.Now
            };

            return View(model);
        }

        // POST: InterviewEvents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobApplicationId,Round,InterviewDate,InterviewType,Interviewer,Feedback")] InterviewEvent interviewEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interviewEvent);
                await _context.SaveChangesAsync();

                // 亮點 2：新增成功後，利用 route values 帶著 ID 導回職缺主檔明細頁
                return RedirectToAction("Details", "JobApplications", new { id = interviewEvent.JobApplicationId });
            }
            return View(interviewEvent);
        }

        // GET: InterviewEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var interviewEvent = await _context.InterviewEvents
                                               .Include(i => i.JobApplication) // 預載主檔以取得公司名稱
                                               .FirstOrDefaultAsync(i => i.Id == id);

            if (interviewEvent == null) return NotFound();

            ViewBag.JobInfo = $"{interviewEvent.JobApplication?.CompanyName} - {interviewEvent.JobApplication?.JobTitle}";

            return View(interviewEvent);
        }

        // POST: InterviewEvents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobApplicationId,Round,InterviewDate,InterviewType,Interviewer,Feedback")] InterviewEvent interviewEvent)
        {
            if (id != interviewEvent.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interviewEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterviewEventExists(interviewEvent.Id))
                        return NotFound();
                    else
                        throw;
                }
                // 編輯完成後，同樣導回職缺主檔明細頁
                return RedirectToAction("Details", "JobApplications", new { id = interviewEvent.JobApplicationId });
            }
            return View(interviewEvent);
        }

        // GET: InterviewEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var interviewEvent = await _context.InterviewEvents
                .Include(i => i.JobApplication)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (interviewEvent == null) return NotFound();

            return View(interviewEvent);
        }

        // POST: InterviewEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interviewEvent = await _context.InterviewEvents.FindAsync(id);
            int jobAppId = 0;

            if (interviewEvent != null)
            {
                jobAppId = interviewEvent.JobApplicationId; // 刪除前先記住主檔 ID
                _context.InterviewEvents.Remove(interviewEvent);
                await _context.SaveChangesAsync();
            }

            // 刪除完成後導回職缺主檔明細頁
            return RedirectToAction("Details", "JobApplications", new { id = jobAppId });
        }

        private bool InterviewEventExists(int id)
        {
            return _context.InterviewEvents.Any(e => e.Id == id);
        }
    }
}