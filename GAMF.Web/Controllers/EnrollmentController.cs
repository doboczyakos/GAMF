using GAMF.Core;
using GAMF.Core.Models;
using GAMF.Web.Extensions;
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

        public async Task<DataTablesViewModel<EnrollmentListViewModel>> GetEnrollments([FromQuery] DataTablesRequest dataTablesRequest)
        {
            IQueryable<Enrollment> enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student);

            if (!string.IsNullOrEmpty(dataTablesRequest.SearchString) && dataTablesRequest.SearchString.ToUpper() is var searchStringUpper)
            {
                enrollments = enrollments.Where(s => s.Student!.LastName.Contains(searchStringUpper) || s.Course!.Title.ToUpper().Contains(searchStringUpper));
            }

            IQueryable<Enrollment> enrollmentsSortedAndPaged = enrollments;
            if (dataTablesRequest.Order != null && dataTablesRequest.Columns != null)
            {
                foreach (var order in dataTablesRequest.Order)
                {
                    enrollmentsSortedAndPaged = dataTablesRequest.Columns[order.Column].Data switch
                    {
                        "studentFullName" => enrollmentsSortedAndPaged.AddOrderBy(e => e.Student!.LastName, order.Direction).AddOrderBy(e => e.Student!.FirstMidName, order.Direction),
                        "grade" => enrollmentsSortedAndPaged.AddOrderBy(e => e.Grade, order.Direction),
                        _ => enrollmentsSortedAndPaged.AddOrderBy(e => e.Course!.Title, order.Direction)
                    };
                }
            }

            enrollmentsSortedAndPaged = enrollmentsSortedAndPaged.Skip(dataTablesRequest.Start).Take(dataTablesRequest.Length);

            return new DataTablesViewModel<EnrollmentListViewModel>
            {
                Draw = dataTablesRequest.Draw,
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
