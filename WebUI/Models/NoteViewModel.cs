using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace WebUI.Models
{
    public class NoteViewModel
    {
        public IEnumerable<Group> Groups { get; set; }
        public Group Group { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public IEnumerable<OptimalFactRecord> Data { get; set; }
    }

    public class OptimalFactRecord
    {
        public int TagKey { get; set; }
        public DateTime Date { get; set; }
        public Double OptimalValue { get; set; }
        public Double FactValue { get; set; }
        public Double AbsDeviation { get { return OptimalValue - FactValue; } }
        public Double RelDeviation { get { return 100*AbsDeviation/OptimalValue; } }
    }

    public class CommentsViewModel
    {
        public int TagKey { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}