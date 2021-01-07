using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Zamger2._0.Data
{
    public class Subject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IdentityUser Profesor { get; set; }
        public IList<Homework> Homeworks { get; set; }

        public IList<Exam> Exams { get; set; }
    }
}
