using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate
        {
            get; set;
        } = DateTime.Now;

        [JsonIgnore]
        public bool IsDeleted { get; set; }

    }
}
