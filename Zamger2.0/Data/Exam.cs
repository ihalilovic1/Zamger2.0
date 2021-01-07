using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Zamger2._0.Data
{
    public class Exam
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public IdentityUser Student { get; set; }
        public Subject Subject { get; set; }

        public IList<ExamSignUp> ExamSignUps { get; set; }
    }
}
