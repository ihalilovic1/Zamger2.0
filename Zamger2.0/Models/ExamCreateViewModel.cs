using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zamger2._0.Models
{
    public class ExamCreateViewModel :IValidatableObject
    {
        [BindProperty]
        [Required]
        //[MinLength(4)]
        [RegularExpression(@"^[a-zA-Z0-9-.\s]{4,30}$",
         ErrorMessage = "Name must be between 4 and 30 characters long. Only letters, numbers, spaces, - and . are allowed.")]

        public string Name { get; set; }
        [BindProperty, Required, CustomDateRange, DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        
        [BindProperty, Required, CustomDateRange,DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
   
        public DateTime Time { get; set; }
        [Required]
        public string Subject { get; set; }
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();

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
