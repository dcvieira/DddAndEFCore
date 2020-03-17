using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            var result = Execute(x => x.AddEnrollment(1, 2, Grade.A));
            result =  Execute(x => x.CheckStudentFavoriteCourse(1, 2));
        }

        private static string Execute(Func<StudentController, string> func)
        {
            string connectionString = GetConnectionString();
            using (var context = new SchoolContext(connectionString, useConsoleLogger: true))
            {
                var controller = new StudentController(context);
                return func(controller);
            }
        }


        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["ConnectionString"];
        }


    }
}