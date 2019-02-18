<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionVoip.aspx.cs" Inherits="SFW.Web.GestionVoip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>GESTION VOIP</title>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="MYG" />

    <link type="text/css" href="Content/bootstrap.css" rel="stylesheet" media="all" />
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
            <div class="col-lg-12">
                <div class="panel panel-blue sombra02">
                    <div class="panel-heading col-md-12">
                        <div class="col-md-4">
                            <h4><i class="fa fa-phone-square fa-1x text-white"></i>&nbsp;Adminitración de Central de Asistencia</h4>
                        </div>
                        <div class="col-md-8 text-right">
                            <div class="btn-group">
                                <asp:LinkButton ID="lnkReporteAveria" runat="server" class="btn btn-default btn-sm" ToolTip="Reportar Avería" Visible="false" onclick="lnkReporteAveria_Click" OnClientClick="SetTarget();">
                                    <i class="fa fa-laptop fa-1x text-success"></i>&nbsp;
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkRegistrar" runat="server" class="btn btn-default btn-sm" onclick="lnkRegistrar_Click"> 
                                    <i class="fa fa-plus-circle fa-1x text-blue"></i>&nbsp;Nuevo Registro
                                </asp:LinkButton>  
                                <asp:LinkButton ID="lnkHospitalizados" runat="server" class="btn btn-default btn-sm" onclick="lnkHospitalizados_Click" OnClientClick = "SetTarget();"> 
                                    <i class="fa fa-user-md fa-1x text-danger"></i>&nbsp;Hospitalizados 
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkReportesEstadisticos" runat="server" class="btn btn-default btn-sm" onclick="lnkReportesEstadisticos_Click" OnClientClick = "SetTarget();" Visible = "false"> 
                                    <i class="fa fa-dashboard fa-1x text-info"></i>&nbsp;Reportes Estadísticos  
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkReportes" runat="server" class="btn btn-default btn-sm" onclick="lnkReportes_Click">
                                    <i class="fa fa-bar-chart-o fa-1x text-info"></i>&nbsp;Reportes  
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkDescargarExcel" runat="server" class="btn btn-default btn-sm" onclick="lnkDescargarExcel_Click"> 
                                    <i class="fa fa-download fa-1x text-success"></i>&nbsp;Descargar Excel 
                                </asp:LinkButton> 
                                <asp:LinkButton ID="LnkRegresar" runat="server" class="btn btn-default btn-sm" onclick="LnkRegresar_Click"> 
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
                            <div class="col-md-3" style="border-right:4px solid #5788B2;height:500px">
                                <center>
                                    <asp:Calendar ID="cldFechas" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="10px" 
                                        ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="100%" onselectionchanged="cldFechas_SelectionChanged" >
                                        <DayHeaderStyle Font-Bold="True" Font-Size="9px" />
                                        <NextPrevStyle Font-Bold="True" Font-Size="9px" ForeColor="white" VerticalAlign="Bottom" />
                                        <OtherMonthDayStyle ForeColor="#999999" />
                                        <SelectedDayStyle BackColor="LightBlue" ForeColor="Black" />
                                        <TitleStyle BackColor="#5788B2" BorderColor="#5788B2" BorderWidth="4px" Font-Bold="True" Font-Size="10px" ForeColor="white" />
                                        <TodayDayStyle BackColor="#5788B2" />
                                    </asp:Calendar>
                                </center>
                                <hr />
                                <strong><i class="fa fa-calendar-o fa-1x text-blue"></i>&nbsp;<asp:Label ID="lblConexion" runat="server"  class="input-sm text-blue"></asp:Label>
                                <label id="time" class="text-blue input-sm"></label></strong>
                                <br /><br />
                                <asp:HiddenField ID="hfContador" runat="server" />
                                <asp:HiddenField ID="hfTipo" runat="server" />
                                <asp:GridView ID="gvContador" runat="server" 
                                    class="table table-bordered table-condensed table-striped input-sm" 
                                    AutoGenerateColumns="False" onrowcommand="gvContador_RowCommand" 
                                    DataKeyNames="ID" onrowdatabound="gvContador_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="" Visible="False"/>
                                        <asp:BoundField DataField="DESCRIP" HeaderText="ESTADO" />
                                        <asp:ButtonField CommandName="CANTIDADSE" DataTextField="CANTIDADSE" HeaderText="CANT.DÍA" />
                                        <asp:ButtonField CommandName="CANTIDAD" DataTextField="CANTIDAD" HeaderText="CANT.ACUM" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-md-9">
                                <div class="col-md-12 form-inline">
<%--                                    <asp:DropDownList ID="ddlAnio" runat="server" class="form-control input-sm" 
                                        AutoPostBack="True" Font-Size="Small">
                                    </asp:DropDownList>--%>

                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control input-sm" placeholder="Buscar" TabIndex="1">
                                    </asp:TextBox>

                                    <asp:LinkButton ID="btnBusqueda" runat="server" class="btn btn-default btn-sm" ToolTip="Buscar" onclick="btnBusqueda_Click"> 
                                        <i class="fa fa-check-circle fa-1x text-blue"></i>&nbsp;Buscar 
                                    </asp:LinkButton>
                                    
                                    <asp:DropDownList ID="ddlEmpresa" runat="server" class="form-control input-sm" onselectedindexchanged="ddlEmpresa_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>

                                    <asp:DropDownList ID="ddlTablas" runat="server" class="form-control input-sm" AutoPostBack="True" onselectedindexchanged="ddlTablas_SelectedIndexChanged">
                                    </asp:DropDownList>

                                    <asp:DropDownList ID="ddlUniNeg" runat="server" class="form-control input-sm" AutoPostBack="true" onselectedindexchanged="ddlUniNeg_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </div>
                                <div class="col-md-12 table-responsive">
                                    <br />
                                    <strong><asp:Label ID="lblCantidadGestion" runat="server" class="input-sm text-blue"></asp:Label></strong>
                                    <br /><br />
                                    <asp:GridView ID="gvGestion" runat="server" class="table table-bordered table-condensed table-striped input-sm" 
                                        AutoGenerateColumns="False"  AllowPaging="True" DataKeyNames="CODIGO,TIPO_ASEG,ID_ASEG,DESCRIP_ASEG,IDESTADO,ID_USU,RESPUESTA" 
                                        onrowcommand="gvGestion_RowCommand" 
                                        onpageindexchanging="gvGestion_PageIndexChanging" PageSize="20" Font-Size="X-Small">
                                        <Columns>
                                            <asp:BoundField DataField="CODIGO" HeaderText="NRO"/>
                                            <asp:BoundField DataField="FEC_REG" HeaderText="FECHA REGISTRO" />
                                            <asp:BoundField DataField="EMPRESA" HeaderText="EMPRESA" Visible="false"/>
                                            <asp:BoundField DataField="CLI" HeaderText="CLIENTE"/>
                                            <asp:BoundField DataField="UNIDAD_NEGOCIO" HeaderText="UNIDAD DE NEGOCIO" Visible="false"/>
                                            <asp:BoundField DataField="TIPO_LLAMADA" HeaderText="ORIGEN"/>
                                            <asp:BoundField DataField="TIPO_EMI" HeaderText="TIPO EMISOR" />
                                            <asp:BoundField DataField="DESCRIP_EMI" HeaderText="EMISOR" HtmlEncode="false"/>
                                            <asp:BoundField DataField="TIPO_ASEG" HeaderText="TIPO ASEG" Visible="false"/>
                                            <asp:BoundField DataField="DESCRIP_ASEG" HeaderText="ASEGURADO"/>
                                            <asp:BoundField DataField="GESTION" HeaderText="TIPO GESTION"/>
                                            <asp:BoundField DataField="SUBGESTION" HeaderText="SUBGESTION"/>
                                            <asp:BoundField DataField="USUARIO" HeaderText="USUARIO"/>
                                            <asp:BoundField DataField="FEC_RESULT" HeaderText="FECHA RESUELTO" />
                                            <asp:BoundField DataField="RESPUESTA" HeaderText="RESPUESTA" Visible="false"/>
                                            <asp:BoundField DataField="OBSERVACION" HeaderText="OBSERVACION" Visible="false"/>
                                            <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                                            <asp:ButtonField CommandName="Editar" ButtonType="Link" Text='<i class="fa fa-edit fa-1x text-white"></i>' ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Editar" />
                                            <asp:TemplateField HeaderText="Eliminar" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEliminarGestion" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("CODIGO") %>' OnClientClick="return confirmDelete();" class="btn btn-danger btn-sm" ToolTip="Eliminar"> 
                                                            <i class="fa fa-minus-square-o fa-1x text-white"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ButtonField CommandName="Respuesta" ButtonType="Link" Text='<i class="fa fa-comment fa-1x text-white"></i>' ControlStyle-CssClass="btn btn-success btn-sm" HeaderText="Rpta." />
                                        </Columns>
                                    </asp:GridView>
                                </div>    
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="RESPUESTA" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <div class="modal-body">
                        <div class="row-eq-height">
                            <asp:TextBox ID="txtRespuesta" runat="server"  class="form-control input-sm" TextMode="MultiLine" Height="142px" Width="100%" placeholder="Respuesta"></asp:TextBox>           
                        </div>
                    </div>
                    <div class="modal-footer color-blue text-white text-right">
                        <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar" data-dismiss="modal" aria-hidden="true">
                            <i class="fa fa-sign-out fa-1x text-blue"></i> Salir
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
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
