using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagDBContext.Models
{
    public class PlanFactRecord
    {
        public int Hour { get; set; }
        public DateTime DateTime { get; set; }
        public double Plan { get; set; }
        public double Fact { get; set; }
        public double AbsDev { get; set; }
        public double RelDev { get; set;}
    }
}
