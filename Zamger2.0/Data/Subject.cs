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
        public string ProfesorId { get; set; }
        public virtual IdentityUser Profesor { get; set; }
        public virtual IList<Homework> Homeworks { get; set; }

        public virtual IList<Exam> Exams { get; set; }
    }
}
