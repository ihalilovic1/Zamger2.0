using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zamger2._0.Models
{
    public class CustomDateRange: RangeAttribute
    {
        public CustomDateRange() : base(typeof(DateTime), DateTime.Now.ToString(), DateTime.Now.AddYears(1).ToString())
        { }
    }
}
