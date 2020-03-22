using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class StudentController
    {
        private readonly StudentRepository _studentRepo;
        private readonly SchoolContext _contex;

        public StudentController(SchoolContext contex)
        {
            _studentRepo = new StudentRepository(contex);
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

        public string EnrollStudent(long studentId, long courseId, Grade grade)
        {
            Student student = _studentRepo.GetById(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

            var result = student.EnrollIn(course, grade);

            _contex.SaveChanges();

            return result;
        }

        public string DisenrollStudent(long studentId, long courseId)
        {
            Student student = _studentRepo.GetById(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

            student.Disenroll(course);

            _contex.SaveChanges();

            return "OK";
        }

        public string RegisterStudent(string firstName, string lastName, long nameSuffixId, string email, long favoriteCourseId)
        {
            Course course = Course.FromId(favoriteCourseId);
            if (course == null)
                return "Course not found";

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            Suffix suffix = Suffix.FromId(nameSuffixId);

            Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
            if (nameResult.IsFailure)
                return nameResult.Error;

            var student = new Student(nameResult.Value, emailResult.Value, course);

            _contex.Attach(student);

            _contex.SaveChanges();

            return "OK";

        }

        public string EditPersonalInfo(long studentId, string firstName, string lastName, long nameSuffixId, string email, long favoriteCourseId)
        {
            Course course = Course.FromId(favoriteCourseId);
            if (course == null)
                return "Course not found";

            var student = _studentRepo.GetById(studentId);

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            Suffix suffix = Suffix.FromId(nameSuffixId);

            Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
            if (nameResult.IsFailure)
                return nameResult.Error;

            student.EditPersonalInfo(nameResult.Value, emailResult.Value, course);

            _contex.SaveChanges();

            return "OK";

        }
    }
}
