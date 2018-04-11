namespace StudentSystem.App
{
    using StudentSystem.Data;
    using System;
    using System.Text;

    public class DataPrinter
    {
        private StudentSystemDbContext db;
        
        public DataPrinter(StudentSystemDbContext db)
        {
            this.db = db;
        }

        public string PrintData(string data)
        {
            var sb = new StringBuilder();

            switch (data)
            {
                case "Students":
                    sb.AppendLine("Students");
                    foreach(var student in db.Students)
                    {
                        sb.AppendLine($"{student.Id}. Name: {student.Name} | Phone: {student.Phone} | Registration Date: {student.RegisteredOn}");
                    }
                    break;
                case "Courses":
                    sb.AppendLine("Courses:");
                    foreach(var course in db.Courses)
                    {
                        sb.AppendLine($"{course.Id}. Name: {course.Name} | Price: {course.Price} | Duration: {course.EndDate.Subtract(course.StartDate)}");
                    }
                    break;
                default:
                    sb.AppendLine("Invalid entity - " + data + "try again");
                    break;
            }
            return sb.ToString().Trim();
        }
    }
}