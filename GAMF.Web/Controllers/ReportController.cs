using GAMF.Core;
using GAMF.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GAMF.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly GAMFDbContext _context;

        public ReportController(GAMFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> EnrollmentDateReport()
        {
            var query = from student in _context.Students
                        group student by student.EnrollmentDate into dateGroup
                        select new EnrollmentDateViewModel
                        {
                            EnrollmentDate = dateGroup.Key,
                            StudentCount = dateGroup.Count()
                        };

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> StudentCreditsReport()
        {
            var query = from creditsByStudentId in from enrollment in _context.Enrollments
                                                   where enrollment.Grade != null && enrollment.Grade != Core.Models.Grade.F
                                                   group new { enrollment.Course!.Credits } by enrollment.StudentId into studentGroup
                                                   select new
                                                   {
                                                       StudentId = studentGroup.Key,
                                                       Credits = studentGroup.Sum(s => s.Credits)
                                                   }
                        join student in _context.Students on creditsByStudentId.StudentId equals student.Id
                        select new StudentCreditsViewModel
                        {
                            Name = $"{student.LastName} {student.FirstMidName}",
                            Credits = creditsByStudentId.Credits
                        };

            return View(await query.ToListAsync());
        }
    }
}
