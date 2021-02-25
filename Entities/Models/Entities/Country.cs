using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Entities
{
    [Table("Countries")]
    public class Country : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify if country belongs to EU")]
        public bool IsEU { get; set; }

        [Required(ErrorMessage = "Please specify VAT")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal VAT { get; set; }

        public decimal VATPercent => 1 + VAT / 100;
    }
}
