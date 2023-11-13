using GAMF.Core;
using GAMF.Core.Models;
using GAMF.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public IActionResult Index2() => View();

        public async Task<DataTablesViewModel<EnrollmentListViewModel>> GetEnrollments(int draw, [FromQuery(Name = "search[value]")] string? searchString, int start, int length,
            [FromQuery(Name = "order[0][column]")] int orderColumn, [FromQuery(Name = "order[0][dir]")] OrderDirection orderDirection)
        {
            IQueryable<Enrollment> enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student);

            if (!string.IsNullOrEmpty(searchString) && searchString.ToUpper() is var searchStringUpper)
            {
                enrollments = enrollments.Where(s => s.Student!.LastName.Contains(searchStringUpper) || s.Course!.Title.ToUpper().Contains(searchStringUpper));
            }

            var orderColumnName = Request.Query[$"columns[{orderColumn}][data]"].ToString();
            IQueryable<Enrollment> enrollmentsSortedAndPaged = orderColumnName switch
            {
                "studentFullName" => orderDirection switch { OrderDirection.Desc => enrollments.OrderByDescending(e => e.Student!.LastName).ThenByDescending(e => e.Student!.FirstMidName), _ => enrollments.OrderBy(e => e.Student!.LastName).ThenBy(e => e.Student!.FirstMidName) },
                "grade" => orderDirection switch { OrderDirection.Desc => enrollments.OrderByDescending(e => e.Grade), _ => enrollments.OrderBy(e => e.Grade) },
                _ => orderDirection switch { OrderDirection.Desc => enrollments.OrderByDescending(e => e.Course!.Title), _ => enrollments.OrderBy(e => e.Course!.Title) }
            };

            enrollmentsSortedAndPaged = enrollmentsSortedAndPaged.Skip(start).Take(length);

            return new DataTablesViewModel<EnrollmentListViewModel>
            {
                Draw = draw,
                RecordsFiltered = await enrollments.CountAsync(),
                RecordsTotal = await _context.Enrollments.CountAsync(),
                Data = await enrollmentsSortedAndPaged.Select(e => new EnrollmentListViewModel
                {
                    CourseTitle = e.Course!.Title,
                    StudentFullName = $"{e.Student!.LastName} {e.Student.FirstMidName}",
                    Grade = e.Grade.ToString()!
                }).ToListAsync()
            };
        }
    }
}
