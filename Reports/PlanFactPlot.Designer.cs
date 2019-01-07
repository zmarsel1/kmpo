namespace Reports
{
    partial class PlanFactPlot
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Charting.Styles.ChartMargins chartMargins1 = new Telerik.Reporting.Charting.Styles.ChartMargins();
            Telerik.Reporting.Charting.ChartSeries chartSeries1 = new Telerik.Reporting.Charting.ChartSeries();
            Telerik.Reporting.Charting.ChartSeries chartSeries2 = new Telerik.Reporting.Charting.ChartSeries();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.chart1 = new Telerik.Reporting.Chart();
            this.entityDataSource = new Telerik.Reporting.EntityDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(17D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.chart1});
            this.detail.Name = "detail";
            // 
            // chart1
            // 
            this.chart1.Appearance.Border.Visible = false;
            this.chart1.BitmapResolution = 120F;
            this.chart1.ChartTitle.Appearance.Visible = false;
            this.chart1.ChartTitle.TextBlock.Appearance.Position.AlignedPosition = Telerik.Reporting.Charting.Styles.AlignedPositions.Center;
            this.chart1.ChartTitle.TextBlock.Text = "Отклонение Факта от Заявки";
            this.chart1.ChartTitle.Visible = false;
            this.chart1.DataSource = this.entityDataSource;
            this.chart1.ImageFormat = System.Drawing.Imaging.ImageFormat.Emf;
            this.chart1.Legend.Appearance.ItemMarkerAppearance.Figure = "Diamond";
            this.chart1.Legend.Appearance.Overflow = Telerik.Reporting.Charting.Styles.Overflow.Row;
            this.chart1.Legend.Appearance.Position.AlignedPosition = Telerik.Reporting.Charting.Styles.AlignedPositions.Bottom;
            this.chart1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.chart1.Name = "chart1";
            chartMargins1.Bottom = new Telerik.Reporting.Charting.Styles.Unit(8D, Telerik.Reporting.Charting.Styles.UnitType.Percentage);
            chartMargins1.Left = new Telerik.Reporting.Charting.Styles.Unit(8D, Telerik.Reporting.Charting.Styles.UnitType.Percentage);
            chartMargins1.Right = new Telerik.Reporting.Charting.Styles.Unit(1D, Telerik.Reporting.Charting.Styles.UnitType.Percentage);
            chartMargins1.Top = new Telerik.Reporting.Charting.Styles.Unit(2D, Telerik.Reporting.Charting.Styles.UnitType.Percentage);
            this.chart1.PlotArea.Appearance.Dimensions.Margins = chartMargins1;
            this.chart1.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.Color.Transparent;
            this.chart1.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.Color.Transparent;
            this.chart1.PlotArea.Appearance.Position.AlignedPosition = Telerik.Reporting.Charting.Styles.AlignedPositions.Center;
            this.chart1.PlotArea.DataTable.Appearance.FillStyle.MainColor = System.Drawing.Color.Silver;
            this.chart1.PlotArea.DataTable.Appearance.FillStyle.SecondColor = System.Drawing.Color.Silver;
            this.chart1.PlotArea.EmptySeriesMessage.TextBlock.Text = "Отсутствуют данные";
            this.chart1.PlotArea.XAxis.Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.White;
            this.chart1.PlotArea.XAxis.Appearance.MajorGridLines.Color = System.Drawing.Color.Silver;
            this.chart1.PlotArea.XAxis.Appearance.MajorTick.Color = System.Drawing.Color.Black;
            this.chart1.PlotArea.XAxis.Appearance.TextAppearance.FillStyle.MainColor = System.Drawing.Color.White;
            this.chart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
            this.chart1.PlotArea.XAxis.IsZeroBased = false;
            this.chart1.PlotArea.XAxis.MaxItemsCount = 24;
            this.chart1.PlotArea.XAxis.MinValue = 1D;
            this.chart1.PlotArea.YAxis.Appearance.MajorGridLines.Color = System.Drawing.Color.Silver;
            this.chart1.PlotArea.YAxis.Appearance.MajorGridLines.PenStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.chart1.PlotArea.YAxis.Appearance.MajorTick.Color = System.Drawing.Color.Black;
            this.chart1.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
            this.chart1.PlotArea.YAxis.AxisLabel.Appearance.Visible = true;
            this.chart1.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
            this.chart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "кВт•час";
            this.chart1.PlotArea.YAxis.AxisLabel.Visible = true;
            this.chart1.PlotArea.YAxis.MaxValue = 90D;
            this.chart1.PlotArea.YAxis.Step = 10D;
            chartSeries1.Appearance.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(205)))), ((int)(((byte)(99)))));
            chartSeries1.Appearance.Border.Width = 3F;
            chartSeries1.Appearance.FillStyle.FillSettings.GradientMode = Telerik.Reporting.Charting.Styles.GradientFillStyle.Vertical;
            chartSeries1.Appearance.FillStyle.FillType = Telerik.Reporting.Charting.Styles.FillType.Solid;
            chartSeries1.Appearance.FillStyle.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(231)))), ((int)(((byte)(179)))));
            chartSeries1.Appearance.LabelAppearance.RotationAngle = 270F;
            chartSeries1.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(205)))), ((int)(((byte)(99)))));
            chartSeries1.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            chartSeries1.DataXColumn = "Hour";
            chartSeries1.DataYColumn = "Fact";
            chartSeries1.Name = "Факт";
            chartSeries2.Appearance.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(249)))));
            chartSeries2.Appearance.Border.Width = 3F;
            chartSeries2.Appearance.FillStyle.FillType = Telerik.Reporting.Charting.Styles.FillType.Solid;
            chartSeries2.Appearance.FillStyle.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(239)))), ((int)(((byte)(252)))));
            chartSeries2.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Reporting.Charting.Styles.AlignedPositions.Center;
            chartSeries2.Appearance.LabelAppearance.RotationAngle = 270F;
            chartSeries2.Appearance.LineSeriesAppearance.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            chartSeries2.Appearance.LineSeriesAppearance.Width = 5F;
            chartSeries2.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(249)))));
            chartSeries2.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            chartSeries2.DataXColumn = "Hour";
            chartSeries2.DataYColumn = "Plan";
            chartSeries2.Name = "Заявка";
            this.chart1.Series.AddRange(new Telerik.Reporting.Charting.ChartSeries[] {
            chartSeries1,
            chartSeries2});
            this.chart1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(27.699899673461914D), Telerik.Reporting.Drawing.Unit.Cm(16.80000114440918D));
            // 
            // entityDataSource
            // 
            this.entityDataSource.ConnectionString = "TagDBEntities";
            this.entityDataSource.Name = "entityDataSource";
            this.entityDataSource.ObjectContext = typeof(TagDBContext.TagDBEntities);
            this.entityDataSource.ObjectContextMember = "GetPlanFact";
            this.entityDataSource.Parameters.AddRange(new Telerik.Reporting.ObjectDataSourceParameter[] {
            new Telerik.Reporting.ObjectDataSourceParameter("groupid", typeof(int), "=Parameters.groupid.Value"),
            new Telerik.Reporting.ObjectDataSourceParameter("date", typeof(System.DateTime), "=Parameters.date.Value")});
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.59719771146774292D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.580966949462891D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1188325881958008D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.currentTimeTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.3000000715255737D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.9000002145767212D), Telerik.Reporting.Drawing.Unit.Cm(0.29999992251396179D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox1.Value = "Заявка Факт на";
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:D}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.1001996994018555D), Telerik.Reporting.Drawing.Unit.Cm(0.29999992251396179D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.9997997283935547D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.textBox2.Value = "= Parameters.date.Value";
            // 
            // PlanFactPlot
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.pageFooterSection1,
            this.pageHeaderSection1});
            this.Name = "PlanFactPlot";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "groupid";
            reportParameter1.Text = "groupid";
            reportParameter1.Value = "2";
            reportParameter2.Name = "date";
            reportParameter2.Text = "date";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "26.12.2012";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(27.699899673461914D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.EntityDataSource entityDataSource;
        private Telerik.Reporting.Chart chart1;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
    }
}