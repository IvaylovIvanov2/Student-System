namespace StudentSystem.App
{
    using Microsoft.EntityFrameworkCore;
    using StudentSystem.Data;
    using StudentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        private static Random random = new Random();

        public static void Main()
        {
            using (var db = new StudentSystemDbContext())
            {
                db.Database.Migrate();
                var printer = new DataPrinter(db);
                string command;

                while ((command = Console.ReadLine()) != "END")
                {
                    var commandArgs = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (commandArgs[0] == "Print")
                    {
                        Console.WriteLine(printer.PrintData(commandArgs[1]));
                    }
                    else
                    {
                        Console.WriteLine("Invalid command, try again.");
                    }
                }

                //SeedData(db);
            }
        }

        private static void SeedData(StudentSystemDbContext db)
        {
            const int totalStudents = 25;
            const int totalCourses = 10;
            var currentDate = DateTime.Now;

            //Students
            for (int i = 0; i < totalStudents; i++)
            {
                db.Students.Add(new Student
                {
                    Name = $"Student No: {i}",
                    RegisteredOn = currentDate.AddDays(i),
                    Birthday = currentDate.AddYears(-20).AddDays(i),
                    Phone = $"Random phone {i}"
                });
            }

            db.SaveChanges();

            //Courses
            var addedCourses = new List<Course>();

            for (int i = 0; i < totalCourses; i++)
            {
                var course = new Course
                {
                    Name = $"Course {i}",
                    Description = $"Course Details {i}",
                    Price = 10 * i,
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddDays(20 + i)
                };

                db.Courses.Add(course);

                addedCourses.Add(course);
            }

            db.SaveChanges();


            //Students in courses
            var studentIds = db
                .Students
                .Select(s => s.Id)
                .ToList();

            for (int i = 0; i < totalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourse = random.Next(2, totalStudents / 2);

                for (int j = 0; j < studentsInCourse; j++)
                {
                    var studentId = studentIds[random.Next(0, studentIds.Count)];

                    if (!currentCourse.Students.Any(s => s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentCourse
                        {
                            StudentId = studentId
                        });
                    }
                    else
                    {
                        j--;
                    }
                }

                var resourcesInCourse = random.Next(2, 20);
                var types = new[] { 0, 1, 2, 999 };

                for (int j = 0; j < resourcesInCourse; j++)
                {
                    currentCourse.Resources.Add(new Resource
                    {
                        Name = $"Resource {i} {j}",
                        Url = $"Url {i} {j}",
                        Type = (ResourceType)types[random.Next(0, types.Length)]
                    });
                }
            }

            db.SaveChanges();

            for (int i = 0; i < totalCourses; i++)
            {
                var currentCourse = addedCourses[i];

                var studentsInCourseIds = currentCourse
                    .Students
                    .Select(s => s.StudentId)
                    .ToList();

                for (int j = 0; j < studentsInCourseIds.Count; j++)
                {
                    var totalHomeworks = random.Next(2, 5);

                    for (int k = 0; k < totalHomeworks; k++)
                    {
                        db.Homeworks.Add(new Homework
                        {
                            Content = $"Content {i}",
                            SubmissionDate = currentDate.AddDays(-i),
                            Type = ContentType.Zip,
                            StudentId = studentsInCourseIds[j],
                            CourseId = currentCourse.Id
                        });
                    }
                }
            }

            db.SaveChanges();
        }
    }
}
