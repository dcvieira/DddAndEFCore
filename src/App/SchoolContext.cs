﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App
{
    public sealed class SchoolContext : DbContext
    {
        private readonly bool _useConsoleLogger;

        private readonly string _connectionString;

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public SchoolContext(string connectionString, bool useConsoleLogger)
        {
            _connectionString = connectionString;
            _useConsoleLogger = useConsoleLogger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = CreateLoggerFactory();

            optionsBuilder
                .UseSqlServer(_connectionString).UseLazyLoadingProxies();
            if (_useConsoleLogger)
            {
                optionsBuilder.UseLoggerFactory(loggerFactory)
                 .EnableSensitiveDataLogging();
            }

        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(x =>
            {
                x.ToTable("Student").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("StudentID");
                x.Property(p => p.Email);
                x.Property(p => p.Name);
                x.HasOne(p => p.FavoriteCourse).WithMany();
                x.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            modelBuilder.Entity<Course>(x =>
            {
                x.ToTable("Course").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("CourseID");
                x.Property(p => p.Name);
            });
            modelBuilder.Entity<Enrollment>(x =>
               {
                   x.ToTable("Enrollment").HasKey(k => k.Id);
                   x.Property(p => p.Id).HasColumnName("EnrollmentID");
                   x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
                   x.HasOne(p => p.Course).WithMany();
                   x.Property(p => p.Grade);
               });
        }
    }
}