<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="SFW.Web.PDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FILE</title>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="MYG" />
    <link type="text/css" href="Content/MYG.css" rel="stylesheet" media="all" />
</head>
<body>
    <form id="form1" runat="server">


        <div class="row">
            <div class="col-lg-9 col-md-8 col-sm-12">
                <img src="images/lp_unete.png" alt="Alternate Text" />
            </div>

            <div class="col-lg-3 col-md-8 col-sm-12">
                <div class="text">TOP 8</div>
                <div>CORREDORES DE SEGUROS</div>
            </div>
        </div>

        <div class="row">
            <div class="text">boleti</div>
            <div class="text">nes</div>
            &nbsp;
                <div>de S</div>
            <div>alud</div>
        </div>

        <div class="row">
            <div>
                <a style="background-image: url(/images/15-Consecuencias-De-La-Automedicación-01-600x400.png)" href=""></a>
                <a class="button" href="">Leer más...</a>
            </div>
            <div>
                <a style="background-image: url(/images/14-Sarampion-01-600x380.png)" href=""></a>
                <a class="button" href="">Leer más...</a>
            </div>
        </div>





        <div class="row-eq-height">
            <iframe id="pdfiframe" name="pdfiframe" runat="server" width="100%" height="760"></iframe>
            <center>
                <asp:Image ID="Image1" runat="server" ImageUrl="image/404.jpg" Visible="false" />
            </center>
        </div>
    </form>
</body>
</html>
