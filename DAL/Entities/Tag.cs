using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Tag
    {
        public Tag()
        {
            Groups = new HashSet<Group>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }

        [Required]
        public string ParamTag { get; set; }
        public Param Param { get; set; } 

        public virtual ICollection<Group> Groups { get; set; }

    }
}
