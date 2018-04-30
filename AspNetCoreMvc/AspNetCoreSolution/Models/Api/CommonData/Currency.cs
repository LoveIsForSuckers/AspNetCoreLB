using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.CommonData
{
    public class Currency
    {
        [Required]
        [Range(0, 9000)]
        public int Soft { get; set; } = 0;
        [Required]
        [Range(0, 9000)]
        public int Hard { get; set; } = 0;
    }
}
