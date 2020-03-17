using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class StudentController
    {
        private SchoolContext _contex;

        public StudentController(SchoolContext contex)
        {
            _contex = contex;
        }
        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
            Student student = _contex.Students.Find(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

           return student.FavoriteCourse == course ? "Yes" : "No";
        }

        public string AddEnrollment(long studentId, long courseId, Grade grade)
        {
            Student student = _contex.Students.Find(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

            student.EnrollIn(course, grade);

            _contex.SaveChanges();

            return "OK";
        }
    }
}
