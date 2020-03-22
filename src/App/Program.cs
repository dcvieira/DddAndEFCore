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
            //var result5 = Execute(x => x.RegisterStudent("Diego", "dcvieira@email.com", 1));
            var result4 = Execute(x => x.EditPersonalInfo(4, "Diego Camilo", "Martins Viera 3", 2, "dcvieira2@email.com.br", 1));

            // var result3 = Execute(x => x.DisenrollStudent(1, 2));
            // var result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));
            // var result1 = Execute(x => x.CheckStudentFavoriteCourse(1, 2));
        }

        private static string Execute(Func<StudentController, string> func)
        {
            string connectionString = GetConnectionString();

            var bus = new MessageBus(new Bus());
            EventDispatcher dispatcher = new EventDispatcher(bus);
            using (var context = new SchoolContext(connectionString, useConsoleLogger: true, dispatcher))
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