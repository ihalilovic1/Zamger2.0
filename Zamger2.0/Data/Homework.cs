using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zamger2._0.Data
{
    public class Homework
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public Subject Subject { get; set; }

        public Document Document { get; set; }
        public IList<SubmitedHomework> SubmitedHomeworks { get; set; }
    }
}
