using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Student : Entity
    {
        public virtual Name Name { get; private set; }
        public Email Email { get; private set; }
        public virtual Course FavoriteCourse { get; set; }
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
        protected Student()
        {

        }

        public Student(Name name, Email email, Course favoriteCourse)
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }

        public string EnrollIn(Course course, Grade grade)
        {
            if (_enrollments.Any(x => x.Course == course))
                return $"Student is already enroll in {course.Name}";
            
            _enrollments.Add(new Enrollment(course, this, grade));

            return "Ok";
        }

        public void Disenroll(Course course)
        {
            var enrollment = _enrollments.FirstOrDefault(x => x.Course == course);

            if (enrollment == null)
                return;

            _enrollments.Remove(enrollment);
        }

        public void EditPersonalInfo(Name name, Email email, Course favoriteCourse)
        {
            if(email != Email)
            {
                RaiseDomainEvent(new StudentEmailChangedEvent(Id, email));
            }

            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }
    }
}
