using GAMF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GAMF.Core
{
    public class GAMFDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        public GAMFDbContext(DbContextOptions<GAMFDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") },
                new Student { Id = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { Id = 3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2003-09-01") },
                new Student { Id = 4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { Id = 5, FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { Id = 6, FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2001-09-01") },
                new Student { Id = 7, FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2003-09-01") },
                new Student { Id = 8, FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2005-09-01") }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1050, Title = "Chemistry", Credits = 3, },
                new Course { CourseId = 4022, Title = "Microeconomics", Credits = 3, },
                new Course { CourseId = 4041, Title = "Macroeconomics", Credits = 3, },
                new Course { CourseId = 1045, Title = "Calculus", Credits = 4, },
                new Course { CourseId = 3141, Title = "Trigonometry", Credits = 4, },
                new Course { CourseId = 2021, Title = "Composition", Credits = 3, },
                new Course { CourseId = 2042, Title = "Literature", Credits = 4, }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { EnrollmentId = 1, StudentId = 1, CourseId = 1050, Grade = Grade.A },
                new Enrollment { EnrollmentId = 2, StudentId = 1, CourseId = 4022, Grade = Grade.C },
                new Enrollment { EnrollmentId = 3, StudentId = 1, CourseId = 4041, Grade = Grade.B },
                new Enrollment { EnrollmentId = 4, StudentId = 2, CourseId = 1045, Grade = Grade.B },
                new Enrollment { EnrollmentId = 5, StudentId = 2, CourseId = 3141, Grade = Grade.F },
                new Enrollment { EnrollmentId = 6, StudentId = 2, CourseId = 2021, Grade = Grade.F },
                new Enrollment { EnrollmentId = 7, StudentId = 3, CourseId = 1050 },
                new Enrollment { EnrollmentId = 8, StudentId = 4, CourseId = 1050, },
                new Enrollment { EnrollmentId = 9, StudentId = 4, CourseId = 4022, Grade = Grade.F },
                new Enrollment { EnrollmentId = 10, StudentId = 5, CourseId = 4041, Grade = Grade.C },
                new Enrollment { EnrollmentId = 11, StudentId = 6, CourseId = 1045 },
                new Enrollment { EnrollmentId = 12, StudentId = 7, CourseId = 3141, Grade = Grade.A }
            );
        }
    }

    public class GAMFDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<GAMFDbContext>
    {
        public GAMFDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), true)
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<GAMFDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("GAMFDbConnection"));

            return new GAMFDbContext(optionsBuilder.Options);
        }
    }
}
