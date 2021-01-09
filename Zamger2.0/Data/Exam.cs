using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Zamger2._0.Models;

namespace Zamger2._0.Data
{
    public class Exam :IValidatableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
        [Required]
        [CustomDateRange]
        public DateTime Time { get; set; }
        [Required]
        [CustomDateRange]
        public DateTime Deadline { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual IList<ExamSignUp> ExamSignUps { get; set; }
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (Time < Deadline)
            {
                ValidationResult mss = new ValidationResult("Time date must be greater than or equal to Deadline");
                res.Add(mss);

            }
            return res;
        }
    }
}
