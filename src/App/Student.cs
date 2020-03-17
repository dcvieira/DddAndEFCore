using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Student : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public virtual Course FavoriteCourse { get; set; }
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
        protected Student()
        {

        }

        public Student(string name, string email, Course favoriteCourse)
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }

        public void EnrollIn(Course course, Grade grade)
        {
            _enrollments.Add(new Enrollment(course, this, grade));
        }
    }
}
