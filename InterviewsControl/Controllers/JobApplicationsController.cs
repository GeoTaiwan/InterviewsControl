using InterviewsControl.Data;
using InterviewsControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InterviewsControl.Controllers
{
    [Authorize]
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // 透過依賴注入 (DI) 取得 DbContext
        public JobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index()
        {
            var applications = await _context.JobApplications
                                             .OrderByDescending(j => j.AppliedDate)
                                             .ToListAsync();
            return View(applications);
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // 亮點：使用 Include (Eager Loading) 一併抓出關聯的「面試關卡」紀錄
            var jobApplication = await _context.JobApplications
                .Include(j => j.InterviewEvents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobApplication == null) return NotFound();

            return View(jobApplication);
        }

        // GET: JobApplications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,JobTitle,JobUrl,AppliedDate,Status,Notes")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null) return NotFound();

            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,JobTitle,JobUrl,AppliedDate,Status,Notes")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobApplication == null) return NotFound();

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}