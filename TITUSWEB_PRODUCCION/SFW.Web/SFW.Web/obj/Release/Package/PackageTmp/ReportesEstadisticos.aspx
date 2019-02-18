<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportesEstadisticos.aspx.cs" Inherits="SFW.Web.ReportesEstadisticos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="row-eq-height">
        <iframe id="ReportesEstadisticosFrame" name="ReportesEstadisticosFrame" runat="server" width="100%" height="1000" frameborder="0">
        </iframe>   
        <center>
            <asp:Image ID="Image1" runat="server" ImageUrl="image/404.jpg" Visible="false" />
        </center>
    </div>
    </form>
</body>
</html>
