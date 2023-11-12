using System.ComponentModel.DataAnnotations;

namespace GAMF.Core.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = nameof(Resources.LastName), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = nameof(Resources.LastName_Required), ErrorMessageResourceType = typeof(Resources))]
        public required string LastName { get; set; }

        [Display(Name = nameof(Resources.FirstMidName), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = nameof(Resources.FirstMidName_Required), ErrorMessageResourceType = typeof(Resources))]
        public required string FirstMidName { get; set; }

        [Display(Name = nameof(Resources.EnrollmentDate), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = nameof(Resources.EnrollmentDate_Required), ErrorMessageResourceType = typeof(Resources))]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = nameof(Resources.Enrollments), ResourceType = typeof(Resources))]
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
