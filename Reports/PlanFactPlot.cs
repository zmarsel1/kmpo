namespace Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for PlanFactPlot.
    /// </summary>
    public partial class PlanFactPlot : Telerik.Reporting.Report
    {
        public PlanFactPlot()
        {
            //
            // Required for telerik Reporting designer support
            //



            InitializeComponent();
            
            chart1.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            chart1.BitmapResolution = 96;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}