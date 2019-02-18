<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sesion.aspx.cs" Inherits="SFW.Web.Sesion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sistema DBF</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="MYG" />
    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" media="all" />
    <link type="text/css" href="Content/MYG.css" rel="stylesheet" media="all" />

</head>
<body>
    <form id="form1" runat="server">
        <br /><br /><br /><br />
    <div class="container">
        <div class="row">
        <div class="col-md-3 col-md-offset-4">
            <div class="panel panel-blue text-center sombra02">
                <div class="panel-heading">
                    TITUS WEB
                </div>
                <div class="panel-body">
                    <div class="logo text-center">
                        <img src="image/login.png" class="img-responsive"/>
                    </div>
                    <br />
                    <div class="form-signin" data-toggle="validator" action="#">
                    <div class="form-group">
                        <asp:TextBox ID="txtUsuario" runat="server" class="form-control" placeholder="Usuario" required/>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Password"  />
                    </div>
                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" 
                        class="btn btn-lg btn-block" onclick="btnIngresar_Click" />
   
                    <asp:Label ID="lblalerta" runat="server" class="text-danger"></asp:Label>
                
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

</body>
</html>
