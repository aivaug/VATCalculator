using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models.Entities
{
    [Table("Members")]
    public class Member : BaseEntity
    {
        public bool IsCompany { get; set; }
        public bool IsVATPayer { get; set; }

        [Required(ErrorMessage = "Please specify country of origin")]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
