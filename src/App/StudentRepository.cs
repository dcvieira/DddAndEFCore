using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class StudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public Student GetById(long studentId)
        {
            var student = _context.Students.Find(studentId);
            if (student == null)
                return null;

            _context.Entry(student).Collection(x => x.Enrollments).Load();

            return student;

        }
    }
}
