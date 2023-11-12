using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAMF.Core.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseId { get; set; }

        [Display(Name = nameof(Resources.Title), ResourceType = typeof(Resources))]
        public required string Title { get; set; }

        [Display(Name = nameof(Resources.Credits), ResourceType = typeof(Resources))]
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
