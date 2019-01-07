using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Entities;

namespace WebUI.Models
{
    public class MonitoringViewModel
    {
        public MonitoringViewModel()
        {
            ColumnsCount = 1;
        }
        public int ColumnsCount { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Group> Units { get; set; }
    }

    public class MonitoringSingleViewModel
    {
        public Group Group { get; set; }
        public IEnumerable<Group> Units { get; set; }
    }

    public class PlotViewModel
    {
        public Group Group { get; set; }
        public Param Param { get; set; }
    }
}