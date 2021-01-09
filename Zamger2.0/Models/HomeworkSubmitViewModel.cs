using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zamger2._0.Models
{
    public class HomeworkSubmitViewModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        [Required]
        public IFormFile Document { get; set; }
    }
}
