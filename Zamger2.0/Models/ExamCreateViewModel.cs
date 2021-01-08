using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zamger2._0.Models
{
    public class ExamCreateViewModel
    {
        [BindProperty]
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
        [BindProperty]
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public string Subject { get; set; }
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        
    }
    
}
