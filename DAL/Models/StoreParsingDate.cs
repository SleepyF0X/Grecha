using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public sealed class StoreParsingDate
    {
        public int Id { get; set; }
        [Required] public string StoreName { get; set; }

        public DateTimeOffset LastParsingDate { get; set; }
    }
}