using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class TagValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagKey { get; set; }
        [Required]
        public int TagID { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
