﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hospitalizados.aspx.cs" Inherits="SFW.Web.Hospitalizados" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Hospitalizados</title>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="MYG" />
    <link type="text/css" href="Content/MYG.css" rel="stylesheet" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="row-eq-height">
        <iframe id="HospitalizadosFrame" name="HospitalizadosFrame" runat="server" width="100%" height="1000" frameborder="0">
        </iframe>   
        <center>
            <asp:Image ID="Image1" runat="server" ImageUrl="image/404.jpg" Visible="false" />
        </center>
    </div>
    </form>
</body>
</html>