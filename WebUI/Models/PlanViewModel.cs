using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class SearchModel
    {
        [Display(Name = "Объект")]
        public int GroupID { get; set; }
        [Display(Name = "C:")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [Display(Name = "По:")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
    }
    public class PlanViewModel : SearchModel
    {
        public IEnumerable<DocumentHead> Documents { get; set; }
    }

    public class CreateDocumentModel
    {
        public enum DocSource
        { 
            Plan = 0,
            Forecast,
            Fact
        }
        [Display(Name = "Объект")]
        public int GroupID { get; set; }
        [Display(Name = "Дата документа")]
        public DateTime Date { get; set; }
        [Display(Name = "Основан на")]
        public bool HasParent { get; set; }
        public DocSource Source { get; set; }
        [Display(Name = "Дата документа")]
        public DateTime ParentDocumentDate { get; set; }
    }

    public class DocumentHead
    {
        [Display(Name="Объект")]
        public int GroupID { get; set; }
        [Display(Name = "Дата документа")]
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

    public class Document
    {
        public Document()
        {
            Entities = new List<TagValue>();
        }
        [Display(Name = "Объект")]
        public int GroupID { get; set; }        
        [Display(Name = "Дата документа")]
        public DateTime Date { get; set; }
        public IList<TagValue> Entities { get; set; }
    }
}