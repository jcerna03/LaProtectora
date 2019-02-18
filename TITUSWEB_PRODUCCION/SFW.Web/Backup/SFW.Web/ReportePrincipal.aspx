<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePrincipal.aspx.cs" Inherits="SFW.Web.ReportePrincipal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>REPORTE PRINCIPAL</title>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="MYG" />

    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" media="all" />
    <link type="text/css" href="font-awesome-4.1.0/css/font-awesome.min.css" rel="stylesheet" />
    <link type="text/css" href="Content/MYG.css" rel="stylesheet" media="all" />

    <!--[if IE 8]>
        <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <![endif]-->

    <!--[if IE 9]>
        <script type="text/javascript" src="Scripts/jquery-2.1.1.min.js"></script>
    <![endif]-->

    <!--[if IE 10]>
        <script type="text/javascript" src="Scripts/jquery-2.1.1.min.js"></script>
    <![endif]-->

    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <link href="Content/jasny-bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/jasny-bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-md-12 centrar content">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="col-lg-12">
                        <div class="panel panel-blue sombra02">
                            <div class="panel-heading col-md-12">
                                <div class="col-md-4">
                                    <h4><i class="fa fa-phone-square fa-1x text-white"></i>&nbsp;Reporte Principal</h4>
                                </div>
                                <div class="col-md-8 text-right">
                                    <div class="form-inline">
                                        <asp:DropDownList ID="ddlAnio" runat="server" class="form-control input-sm" onselectedindexchanged="ddlAnio_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="lnkDescargarExcel" runat="server" class="btn btn-default btn-sm" onclick="lnkDescargarExcel_Click"> 
                                            <i class="fa fa-download fa-1x text-success"></i>&nbsp;Descargar Excel 
                                        </asp:LinkButton> 
                                        <asp:LinkButton ID="LnkRegresar" runat="server" class="btn btn-default btn-sm"> 
                                            <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;Regresar 
                                        </asp:LinkButton>  
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body centrar">
                               <div class="row centrar">
                                    <div class="col-md-12 centrar">
                                        <div runat="server" id="correcto" class="alert alert-success" visible="false">
                                            <button type="button" class="close" data-dismiss="alert">×</button>
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                        </div>
                                        <div runat="server" id="orror" class="alert alert-warning" role="alert" visible="false">
                                            <button type="button" class="close" data-dismiss="alert">×</button>
                                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <div class="table-responsive">
                                                        <strong><asp:Label ID="lblCantidadGestion" runat="server" class="input-sm text-blue"></asp:Label></strong>
                                                        <asp:GridView ID="gvReportePrincipal" runat="server" class="table table-bordered table-condensed table-striped input-sm" 
                                                            AllowPaging="True" PageSize="20" onrowcommand="gvReportePrincipal_RowCommand" DataKeyNames="IDMES" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-search fa-1x text-white"></i>' ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="VER" />
                                                                <asp:BoundField DataField="IDMES" HeaderText="IDMES" Visible="false" />
                                                                <asp:BoundField DataField="MES" HeaderText="MES" />
                                                                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" />
                                                                <asp:BoundField DataField="MEF" HeaderText="MEF" />
                                                                <asp:BoundField DataField="CNPC" HeaderText="CNPC" />
                                                                <asp:BoundField DataField="PETRO" HeaderText="PETRO" />
                                                                <asp:BoundField DataField="SEDAPAL" HeaderText="SEDAPAL" />
                                                                <asp:BoundField DataField="OTROS" HeaderText="OTROS" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hfIDMES" runat="server" />
                                                    </div> 
                                                    <br />
                                                    <asp:Label ID="lblErrorReg" runat="server" class="text-danger input-sm"></asp:Label>
                                                </div> 
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <div class="col-md-6">
                                                        <div class="panel panel-blue">
                                                            <div class="panel-heading">
                                                                <i class="fa fa-dashboard fa-1x text-white"></i>&nbsp;Reporte IAFAS
                                                            </div>
                                                            <div class="panel-body">
                                                                <asp:GridView ID="gvReporte01" runat="server" class="table table-bordered table-condensed table-striped input-sm" AllowPaging="True" PageSize="20">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="panel panel-blue">
                                                            <div class="panel-heading">
                                                                <i class="fa fa-dashboard fa-1x text-white"></i>&nbsp;Reporte Usuario
                                                            </div>
                                                            <div class="panel-body">
                                                                <asp:GridView ID="gvReporte02" runat="server" class="table table-bordered table-condensed table-striped input-sm" AllowPaging="True" PageSize="20">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <div class="col-md-6">
                                                        <div class="panel panel-blue">
                                                            <div class="panel-heading">
                                                                <i class="fa fa-dashboard fa-1x text-white"></i>&nbsp;Reporte Gestión
                                                            </div>
                                                            <div class="panel-body">
                                                                <asp:GridView ID="gvReporte03" runat="server" class="table table-bordered table-condensed table-striped input-sm" AllowPaging="True" PageSize="20">
                                                                </asp:GridView> 
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="panel panel-blue">
                                                            <div class="panel-heading">
                                                                <i class="fa fa-dashboard fa-1x text-white"></i>&nbsp;Reporte Llamada
                                                            </div>
                                                            <div class="panel-body">
                                                                <asp:GridView ID="gvReporte04" runat="server" class="table table-bordered table-condensed table-striped input-sm" AllowPaging="True" PageSize="20">
                                                                </asp:GridView> 
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlAnio" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="gvReportePrincipal" EventName="RowCommand" />
                    <asp:PostBackTrigger ControlID="lnkDescargarExcel" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress id="updateProgress" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/image/cargando.gif" AlternateText="Loading ..." ToolTip="Loading ..." style="padding: 10px;position:fixed;top:30%;left:40%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>

     <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jasny-bootstrap.min.js"></script>

    <script type = "text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>

    <script type="text/javascript">
        (function () {
            function checkTime(i) {
                return (i < 10) ? "0" + i : i;
            }

            function startTime() {
                var today = new Date(),
        h = checkTime(today.getHours()),
        m = checkTime(today.getMinutes()),
        s = checkTime(today.getSeconds());
                document.getElementById('time').innerHTML = h + ":" + m + ":" + s;
                t = setTimeout(function () {
                    startTime()
                }, 500);
            }
            startTime();
        })();
    </script>

    <script type="text/javascript">

        function confirmDelete() {
            if (confirm("¿Usted va eliminar un registro de llamada,desea continuar?") == true)
                return true;
            else
                return false;
        }
    </script>

    <script language="javascript" type="text/javascript">

        //Session timeout in minute.
        var sessionTimeout = "<%= Session.Timeout %>";

        //Session timeout warning before 2 minute.
        var sessionTimeoutWarning = parseInt(sessionTimeout) - 5;

        //Session timeout in millisecond.
        var sTimeout = parseInt(sessionTimeout) * 60 * 1000;

        setTimeout('SessionWarning()', sTimeout);

        function SessionWarning() {
            //Calculating minutes before timeout in millisecond.
            var minutesForExpiry = (parseInt(sessionTimeout) - parseInt(sessionTimeoutWarning));

            var message = "Su sesión esta a punto de expirar en " + minutesForExpiry + " minutos. Asegúrese de guardar sus cambios.";
            alert(message);

            setTimeout('Redirect()', (minutesForExpiry * 1000 * 60));

        }

        function Redirect() {
            alert("Su sesión expiró. Usted será redireccionado(a) a la página de Login.");
            window.location = "Sesion.aspx";
        }
    </script>

</body>
</html>
