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
        public virtual Subject Subject { get; set; }

        public int SubjectId { get; set; }

        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }
        public virtual IList<SubmitedHomework> SubmitedHomeworks { get; set; }
    }
}
