namespace GAMF.Core.Models
{
    public class Student
    {
        public int Id { get; set; }

        public required string LastName { get; set; }

        public required string FirstMidName { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
