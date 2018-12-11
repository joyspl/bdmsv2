<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tuple<SARASWATIPRESSNEW.Models.BinderAllotQuantity, List<SARASWATIPRESSNEW.Models.BinderAllotQuantityDtl>>>" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>vwBarcode</title>
    <script runat="server">  
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Model.Item2 != null && Model.Item2.Count() > default(int))
                {
                    ReportViewer rptViewer = new ReportViewer();
                    rptViewer.LocalReport.ReportPath = Server.MapPath(@"~/BarcodeGenerator/rptBarcode.rdlc");
                    ReportDataSource ds = new ReportDataSource("dsBarcode", Utility.ToDataTable<SARASWATIPRESSNEW.Models.BinderAllotQuantityDtl>(Model.Item2));
                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.LocalReport.DataSources.Add(ds);
                    //rptViewer.DataBind();
                    rptViewer.LocalReport.Refresh();
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
