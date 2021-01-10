using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zamger2._0.Models
{
    public class HomeworkViewModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-.\s]{4,30}$",
         ErrorMessage = "Name must be between 4 and 30 characters long. Only letters, numbers, spaces, - and . are allowed.")]
        public string Name { get; set; }
        [Required]
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        [Required]
        public int SubjectId { get; set; }

        public SelectList Subjects { get; set; }

        public List<HomeworkReviewViewModel> Submits { get; set; } = new List<HomeworkReviewViewModel>();
        public string SubjectName { get; set; }
    }
}
