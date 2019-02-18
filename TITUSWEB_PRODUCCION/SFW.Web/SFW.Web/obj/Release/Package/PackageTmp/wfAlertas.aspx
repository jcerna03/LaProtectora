<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfAlertas.aspx.cs" Inherits="SFW.Web.wfAlertas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Alertas</title>
    <meta charset="UTF-8">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1 , maximum-scale=1, user-scalable= no ">--%>
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no, width=device-width" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" media="all" />
    <link type="text/css" href="Content/Iconos.css" rel="stylesheet" media="all" />
<%--    <script type="text/javascript" src="Scripts/jquery.responsivetable.min.js"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
        <div class="row text-center">
            <h3 class="text-danger">Alertas Mensuales</h3>
        </div>
        <hr />
        <hr />
        <div class="row">
    	<div class="col-md-12">
            <div class="panel with-nav-tabs panel-default">
                <div class="panel-heading">
                        <ul class="nav nav-tabs">
                            <li class="active" runat="server" id="meftab" visible="false">
                                <a href="#tab1default" data-toggle="tab">
                                    <span class="glyphicon glyphicon-user"></span>
                                    MEF 
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadmef" runat="server"></asp:Label>
                                    </span>
                                </a>
                            </li>
                            <li runat="server" id="minceturtab" visible="false">
                                <a href="#tab2default" data-toggle="tab">
                                    <span class="glyphicon glyphicon-user"></span>
                                    MINCETUR
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadmincetur" runat="server"></asp:Label>
                                    </span>
                                </a>
                            </li>
                            <li runat="server" id="petrobrastab" visible="false">
                                <a href="#tab3default" data-toggle="tab">
                                    <span class="glyphicon glyphicon-certificate"></span>
                                    PETROBRAS
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadpetrobras" runat="server"></asp:Label>
                                    </span>
                                     
                                </a>
                            </li>
                            <li runat="server" id="petroperutab" visible="false">
                                <a href="#tab4default" data-toggle="tab">
                                     <span class="glyphicon glyphicon-asterisk"></span>
                                     PETROPERU
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadpetroperu" runat="server"></asp:Label>
                                    </span>
                                </a>
                            </li>
                            <li runat="server" id="sedapaltab" visible="false">
                                <a href="#tab5default" data-toggle="tab">
                                     <span class="glyphicon glyphicon-zoom-in"></span>
                                     SEDAPAL
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadsedapal" runat="server"></asp:Label>                                    
                                    </span>
                                </a>
                            </li>
                            <li runat="server" id="simatab" visible="false">
                                <a href="#tab6default" data-toggle="tab">
                                     <span class="glyphicon glyphicon-zoom-in"></span>
                                     SIMA
                                    <span class="bg-success">
                                        <asp:Label ID="cantidadsima" runat="server"></asp:Label>                                    
                                    </span>
                                </a>
                            </li>
                        </ul>
                </div>
                <div class="panel-body">
                    <div class="tab-content">
                        <div class="tab-pane fade in" id="tab1default">
                            <div class="row">
                                <div class="panel panel-default text-center">
                                    
                                    <asp:Label ID="lblMef" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LAS EDADES DE  23 Y  70 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                        <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasMEF23" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True"
                                                PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small"/>
                                        </asp:GridView>

                                        <hr />

                                        <asp:GridView ID="gvAlertasMEF70" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True" 
                                                PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>

                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab2default">
                            <div class="row">
                                <div class="panel panel-default text-center">

                                    <asp:Label ID="lblmincetur" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LA EDAD DE 25 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha1" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                    <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasMINCETUR25" runat="server" class="table table-condensed" 
                                            AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True" 
                                            PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab3default">
                            <div class="row">
                                <div class="panel panel-default text-center">

                                    <asp:Label ID="lblpetrobras" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LA EDAD DE 25 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha2" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                        <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasPETROBRAS25" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab4default">
                            <div class="row">
                                <div class="panel panel-default text-center">

                                    <asp:Label ID="lblpetroperu" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LAS EDADES DE 18 Y 25 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha3" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                        <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasPETROPERU18" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>

                                        <br />

                                        <asp:GridView ID="gvAlertasPETROPERU25" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>

                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab5default">
                            <div class="row">
                                <div class="panel panel-default text-center">

                                    <asp:Label ID="lblsedapal" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LA EDAD DE 26 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha4" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                        <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasSEDAPAL26" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane fade" id="tab6default">
                            <div class="row">
                                <div class="panel panel-default text-center">

                                    <asp:Label ID="lblsima" runat="server" Text="REPORTE DE CUMPLEAÑOS DE AFILIADOS CON LA EDAD DE 25 AÑOS" class="text-info pull-left"></asp:Label>
                                    <asp:Label ID="lblFecha5" runat="server" class="pull-right"></asp:Label>

                                    <div class="col-xs-12 table-responsive">
                                        <div class="panel-body form-horizontal payment-form">
                                        <asp:GridView ID="gvAlertasSIMA25" runat="server" class="table table-condensed" 
                                                AutoGenerateColumns="False" DataKeyNames="cod_titula" AllowPaging="True">
                                            <Columns>
                                                <asp:BoundField DataField="cod_titula" HeaderText="Código" SortExpression="#" />
                                                <asp:BoundField DataField="categoria" HeaderText="Categoria" SortExpression="NOMBRE" />
                                                <asp:BoundField DataField="afiliado" HeaderText="Afiliado" SortExpression="APELLIDO" />
                                                <asp:BoundField DataField="plan" HeaderText="Plan" SortExpression="USUARIO" />
                                                <asp:BoundField DataField="cent_costo" HeaderText="Centro" SortExpression="PASSWORD"/>
                                                <asp:BoundField DataField="fch_naci" HeaderText="Nacimiento" SortExpression="ROL"/>
                                                <asp:BoundField DataField="edad" HeaderText="Edad" SortExpression="ESTADO"/>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="X-Small" />
                                            <RowStyle Font-Size="X-Small" />
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
    </form>

          <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
<%--    <script type="text/javascript" src="Scripts/jquery.responsivetable.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script> 

   <%--     <script type="text/javascript">
            //        $(document).ready(function () {
            //            $('table').responsiveTable();
            //        });
            $(document).ready(function () {
                // custom settings
                $('table').responsiveTable({
                    staticColumns: 0,
                    scrollRight: true,
                    scrollHintEnabled: true,
                    scrollHintDuration: 2000
                });
            }); 
    </script>--%>

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
