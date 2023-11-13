using GAMF.Core;
using GAMF.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GAMF.Web.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly GAMFDbContext _context;

        public EnrollmentController(GAMFDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? sortOrder, string? searchString)
        {
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : string.Empty;
            ViewBag.CourseSortParam = sortOrder == "Course" ? "CourseDesc" : "Course";
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;

            IQueryable<Enrollment> enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student);

            if (!string.IsNullOrEmpty(searchString) && searchString.ToUpper() is var searchStringUpper)
            {
                enrollments = enrollments.Where(s => s.Student!.LastName.Contains(searchStringUpper) || s.Course!.Title.ToUpper().Contains(searchStringUpper));
            }

            enrollments = sortOrder switch
            {
                "Course" => enrollments.OrderBy(e => e.Course!.Title),
                "CourseDesc" => enrollments.OrderByDescending(e => e.Course!.Title),
                "NameDesc" => enrollments.OrderByDescending(e => e.Student!.LastName).ThenByDescending(e => e.Student!.FirstMidName),
                _ => enrollments.OrderBy(e => e.Student!.LastName).ThenBy(e => e.Student!.FirstMidName),
            };
            return View(await enrollments.ToListAsync());
        }
    }
}
