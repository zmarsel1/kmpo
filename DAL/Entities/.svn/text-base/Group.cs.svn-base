using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual Group Parent { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
