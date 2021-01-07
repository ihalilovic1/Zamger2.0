using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zamger2._0.Data
{
    public class Document
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string ContentType { get; set; }
        [Required]
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
