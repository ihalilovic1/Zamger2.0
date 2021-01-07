using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Zamger2._0.Data
{
    public class ExamSignUp
    {

        [Required]
        public int Id { get; set; }
        public IdentityUser Student { get; set; }
        public Exam Exam { get; set; }

        public DateTime Time { get; set; }
    }
}
