using System.ComponentModel.DataAnnotations;
using GAMF.Core;

namespace GAMF.Web.Models
{
    public class EnrollmentListViewModel
    {
        [Display(Name = nameof(Resources.Course), ResourceType = typeof(Resources))]
        public required string CourseTitle { get; set; }

        [Display(Name = nameof(Resources.Student), ResourceType = typeof(Resources))]
        public required string StudentFullName { get; set; }

        [Display(Name = nameof(Resources.Grade), ResourceType = typeof(Resources))]
        public required string Grade { get; set; }
    }
}
