namespace StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public List<StudentCourse> Courses { get; set; } = new List<StudentCourse>();

        public List<Homework> Homeworks { get; set; } = new List<Homework>();

    }
}
