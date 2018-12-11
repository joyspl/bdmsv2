<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vwBarcodeNew.aspx.cs" Inherits="SARASWATIPRESSNEW.BarcodeGenerator.vwBarcodeNew" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script runat="server">  
  
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="background-color: #ff0000;">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
