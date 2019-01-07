<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.1.12.823, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<form clientidmode="Static" id="frep" runat="server">

<telerik:ReportViewer ID="ReportViewer1" runat="server" Report="Reports.PrintPlan, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Width="100%" height="1000px">
</telerik:ReportViewer>
</form>
<script runat="server">
        
    public override void VerifyRenderingInServerForm(Control control)
    {
        // to avoid the server form (<form runat="server">) requirement
    }

    protected override void OnLoad(EventArgs e)
    {
        // bind the report viewer
        //Настройка параметров
        Telerik.Reporting.ReportSource report = new Reports.PrintForecast();
        //report.Parameters.Add(new Telerik.Reporting.Parameter("TagID", Model.GroupID));
        report.Parameters.Add(new Telerik.Reporting.Parameter("Date", DateTime.Today));
        ReportViewer1.ReportSource = report;
        base.OnLoad(e);
    }
</script>
