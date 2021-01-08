using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Zamger2._0.Data
{
    public class SubmitedHomework
    {
        [Required]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public virtual IdentityUser Student { get; set; }
        public virtual Homework Homework { get; set; }

        public int HomeworkId { get; set; }

        public DateTime Time { get; set; }
    }
}
