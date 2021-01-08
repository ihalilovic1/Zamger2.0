using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zamger2._0.Data;

namespace Zamger2._0.Models
{
    public class ExamProfesorViewModel
    {
        
        public List<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();

        public List<Exam> Exams { get; set; } = new List<Exam>();

        
    }
    
}
