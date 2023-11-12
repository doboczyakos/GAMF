using System.ComponentModel.DataAnnotations;
using GAMF.Core;

namespace GAMF.Web.Models
{
    public class EnrollmentDateViewModel
    {
        [Display(Name = nameof(Resources.EnrollmentDate), ResourceType = typeof(Resources))]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = nameof(Resources.StudentCount), ResourceType = typeof(Resources))]
        public int StudentCount { get; set; }
    }
}
