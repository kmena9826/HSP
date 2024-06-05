<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportCrystalTest.aspx.cs" Inherits="CS_HOSPITALARIO_Front_end.Forms.ReportCrystalTest" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form {
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0
        }
    </style>
    <script type="text/javascript" src="../aspnet_client/system_web/2_0_50727/crystalreportviewers13/js/crviewer/crv.js"></script>
    <script type="text/javascript" src="../crystalreportviewers13/js/crviewer/crv.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left:300px">
            <CR:CrystalReportViewer ID="CRViewer" runat="server" Width="100%" Height="50px" AutoDataBind="True" ToolPanelView="None"  ReuseParameterValuesOnRefresh="True"  ToolPanelWidth="200px" EnableDatabaseLogonPrompt="False" />
        </div>
    </form>
    <script type="text/javascript" src="../crystalreportviewers13/js/crviewer/crv.js"></script>
</body>
</html>

