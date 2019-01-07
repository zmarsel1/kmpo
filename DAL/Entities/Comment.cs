using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Author { get; set; }
        public DateTime? Posted { get; set; }
        public string Text { get; set; }
        [Required]
        public int TagKey { get; set; }

        public TagValue TagValue { get; set; }
    }
}
