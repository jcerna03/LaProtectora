<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfMantenimiento.aspx.cs"
    Inherits="SFW.Web.wfMantenimiento" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panel Principal</title>
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
    <link href="Content/chosen.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        @media screen
        {
            #printSection
            {
                display: none;
            }
        }
        
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            #printSection, #printSection *
            {
                visibility: visible;
            }
            #printSection
            {
                position: absolute;
                left: 0;
                top: 0;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" method="post" autocomplete="off">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="col-md-12 centrar content">
        <asp:HiddenField ID="hfNombres" runat="server" />
        <asp:HiddenField ID="hfApellidoPaterno" runat="server" />
        <asp:HiddenField ID="hfApellidoMaterno" runat="server" />
        <asp:HiddenField ID="hfDNI" runat="server" />
        <div class="col-lg-12">
            <div class="panel panel-blue sombra02">
                <div class="panel-heading text-center">
                    <h4>
                        Sistema de Administración de Datos de Afiliados TituFoxWeb V.10.08.16<label style="visibility: hidden;">----</label></h4>
                </div>
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="text-center">
                            <asp:Label ID="lblalerta" runat="server" class="text-blue"></asp:Label>
                            <div runat="server" id="duplicado" class="alert alert-danger" visible="false">
                                <button type="button" class="close" data-dismiss="alert">
                                    <span aria-hidden="true">×</span> <span class="sr-only">Close</span>
                                </button>
                                <h4>
                                    ¡¡Ocurrió un error de duplicidad</h4>
                                <p>
                                    <strong>
                                        <asp:Label ID="lbldupli" runat="server"></asp:Label></strong>
                                </p>
                            </div>
                            <asp:Label ID="lblErrorReg" runat="server" class="text-danger"></asp:Label>
                        </div>
                        <div class="text-left">
                            <strong class="text-success">Bienvenido(a)<asp:Label ID="lblRol" runat="server" class="text-primary"
                                Text=" Ejecutivo(a)"></asp:Label>
                                <asp:Label ID="lblUsuario" runat="server" class="text-primary"></asp:Label>
                            </strong>
                        </div>
                        <div class="pull-right">
                            <asp:LinkButton ID="lnkAlertas" runat="server" class="btn btn-sm" ToolTip="Alertas"
                                OnClick="lnkAlertas_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/image/alertas.gif" Height="10px"
                                    Width="10px" />
                                <strong>
                                    <asp:Label ID="lblTotal" runat="server" class="text-danger"></asp:Label></strong></asp:LinkButton>
                            <asp:LinkButton ID="lnkCerrarSesion" runat="server" class="btn btn-sm" ToolTip="Cerrar Sesión"
                                OnClientClick="return confirm('Seguro que desea Cerrar su sesión?')" OnClick="lnkCerrarSesion_Click"> <i class="fa fa-sign-out fa-1x text-blue"></i><label class="text-blue">Cerrar Sesión</label> </asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <hr />
                    <br />
                    <div class="col-lg-12">
                        <div class="form-inline">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBusqueda">
                                <asp:DropDownList ID="ddlTablas" runat="server" class="form-control input-sm" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlTablas_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtBusqueda" runat="server" class="form-control input-sm" placeholder="Buscar por DNI y Apellidos"
                                    TabIndex="1"></asp:TextBox>
                                <asp:LinkButton ID="btnBusqueda" runat="server" class="btn btn-default btn-sm" OnClick="btnBusqueda_Click"
                                    ToolTip="Buscar">
                                        <i class="fa fa-check-circle fa-1x text-success"></i> Buscar
                                </asp:LinkButton>
                            </asp:Panel>
                            <asp:LinkButton ID="lnkReporteAveria" runat="server" ToolTip="Reportar Avería" class="pull-right"
                                Visible="false" OnClick="lnkReporteAveria_Click"> <i class="fa fa-laptop fa-1x text-success"></i></asp:LinkButton>
                        </div>
                    </div>
                    <br />
                    <div class="col-sm-12">
                        <br />
                        <strong>
                            <asp:Label ID="lblContador" runat="server" class="text-info"></asp:Label></strong>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <br />
                                <div class="btn-group pull-right">
                                    <asp:LinkButton ID="lnkTramaSoat" runat="server" class="btn btn-default btn-sm" TabIndex="-3"
                                        OnClick="lnkTramaSoat_Click"> <i class="fa fa-car fa-1x text-danger"></i>&nbsp;TRAMA SOAT </asp:LinkButton>
                                    <asp:LinkButton ID="btnVOIP" runat="server" class="btn btn-default btn-sm" TabIndex="-3"
                                        OnClick="btnVOIP_Click"> <i class="fa fa-search fa-1x text-success"></i>&nbsp;VOIP </asp:LinkButton>
                                    <asp:LinkButton ID="lnkBajaTitus" runat="server" class="btn btn-default btn-sm" TabIndex="-3"
                                        OnClick="lnkBajaTitus_Click"> <i class="fa fa-upload fa-1x text-success"></i>DescargaTitus </asp:LinkButton>
                                    <asp:LinkButton ID="btnImportar" runat="server" class="btn btn-default btn-sm" TabIndex="-3"
                                        Visible="false" OnClick="btnImportar_Click"> <i class="fa fa-upload fa-1x text-success"></i>&nbsp;Carga Financia </asp:LinkButton>
                                    <asp:LinkButton ID="btnActualizar" runat="server" class="btn btn-default btn-sm"
                                        TabIndex="-3" OnClick="btnActualizar_Click" Visible="false"> <i class="fa fa-refresh fa-1x text-success"></i>&nbsp;Actualizar </asp:LinkButton>
                                    <asp:LinkButton ID="btnNuevo" runat="server" class="btn btn-default btn-sm" TabIndex="-5"
                                        OnClick="btnNuevo_Click"> <i class="fa fa-plus-circle fa-1x text-info"></i>&nbsp;Nuevo Titular </asp:LinkButton>
                                    <asp:LinkButton ID="btnReportes" runat="server" class="btn btn-default btn-sm" Visible="false"
                                        TabIndex="-7" OnClick="btnReportes_Click"> <i class="fa fa-list fa-1x text-primary"></i>&nbsp;Reportes </asp:LinkButton>
                                    <asp:LinkButton ID="btnMovimientos" runat="server" class="btn btn-default btn-sm"
                                        Visible="false" TabIndex="-8"> <i class="fa fa-history fa-1x text-primary"></i>&nbsp;Historial </asp:LinkButton>
                                    <asp:LinkButton ID="btnSusalud" runat="server" class="btn btn-default btn-sm" TabIndex="-9"
                                        OnClick="btnSusalud_Click"> <i class="fa fa-history fa-1x text-primary"></i>&nbsp;SUSALUD </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12 table-responsive">
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="categoria,cod_titula,fch_baja,cod_cliente,dni,cod_cliente2"
                                    AllowPaging="True" class="table table-bordered table-condensed table-striped input-sm"
                                    TabIndex="2" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowCommand="GridView2_RowCommand">
                                    <Columns>
                                        <asp:ButtonField CommandName="Editar" Text="Editar" />
                                        <asp:BoundField DataField="cod_cliente" HeaderText="#.POLIZA" SortExpression="#.POLIZA"
                                            Visible="false" />
                                        <asp:BoundField DataField="cod_cliente2" HeaderText="#.POLIZA" SortExpression="#.POLIZA" />
                                        <asp:BoundField DataField="cod_titula" HeaderText="CO.TITU" SortExpression="CO.TITU" />
                                        <asp:BoundField DataField="dni" HeaderText="DNI" SortExpression="DNI" />
                                        <asp:BoundField DataField="categoria1" HeaderText="PARENTESCO" SortExpression="CATEGORIA" />
                                        <asp:BoundField DataField="categoria" HeaderText="CATEGORIA" SortExpression="CATEGORIA"
                                            Visible="false" />
                                        <asp:BoundField DataField="plan" HeaderText="PLAN" SortExpression="PLAN" />
                                        <asp:BoundField DataField="cent_costo" HeaderText="C.COSTO" SortExpression="C.COSTO" />
                                        <asp:BoundField DataField="afiliado" HeaderText="NOMBRES Y APELLIDOS" SortExpression="NOMBRES Y APELLIDOS" />
                                        <asp:BoundField DataField="sexo" HeaderText="SEXO" SortExpression="SEXO" />
                                        <asp:BoundField DataField="edad" HeaderText="EDAD" SortExpression="EDAD" />
                                        <asp:BoundField DataField="fch_naci" HeaderText="F.NACIMIENTO" SortExpression="F.NACIMIENTO" />
                                        <asp:BoundField DataField="fch_alta" HeaderText="F.ALTA" SortExpression="F.ALTA" />
                                        <asp:BoundField DataField="fch_baja" HeaderText="F.BAJA" SortExpression="F.BAJA" />
                                        <asp:BoundField DataField="fch_caren" HeaderText="F.CAREN" SortExpression="F.PRO" />
                                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" SortExpression="F.PRO" Visible="false" />
                                        <asp:BoundField DataField="Color" HeaderText="ESTADO" HtmlEncode="false" SortExpression="F.PRO" />
                                        <asp:BoundField DataField="Reporte" HeaderText="SUSALUD" HtmlEncode="false" />
                                    </Columns>
                                    <PagerStyle BackColor="White" ForeColor="Black" VerticalAlign="Top" />
                                </asp:GridView>
                                <asp:HiddenField ID="hfNombreEmpresa" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="NUEVOAFILIADO" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog modal-wide modal-lg">
            <div class="modal-content panel panel-popup">
                <div class="panel-heading color-blue text-white">
                    <div class="form-inline">
                        <i class="fa fa-pencil fa-1x"></i>
                        <asp:Label ID="lblAfiliado" runat="server" class="input-sm text-white"></asp:Label><asp:Label
                            ID="lblEstado" runat="server" class="input-sm text-white"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <ul class="nav nav-tabs">
                                    <li class="active"><a href="#info-tab" data-toggle="tab"><i class="fa fa-user fa-1x text-blue">
                                    </i>Información de Afiliado</a></li>
                                    <li><a href="#address-tab" data-toggle="tab"><i class="fa fa-file-photo-o fa-1x text-blue">
                                    </i>Datos Generales</a></li>
                                    <li runat="server" id="CartasTab"><a href="#cartas-tabb" data-toggle="tab"><i class="fa fa-envelope-square fa-1x text-blue">
                                    </i>Impresión de Documentos <i class="fa"></i></a></li>
                                    <li runat="server" id="RecordConsumoTab"><a href="#record-tabb" data-toggle="tab"
                                        runat="server"><i class="fa fa-dashboard fa-1x text-blue"></i>Record de Consumo
                                    </a></li>
                                    <li runat="server" id="AvisosTab"><a href="#avisos-tabb" data-toggle="tab"><i class="fa fa-file-archive-o fa-1x text-blue">
                                    </i>Avisos </a></li>
                                    <li runat="server" id="informesmedicos"><a href="#informes_medicos" data-toggle="tab">
                                        <i class="fa fa-file-archive-o fa-1x text-blue"></i>Informes Médicos </a></li>
                                    <li runat="server" id="ultimosmovimientos"><a href="#ultimos_movimientos" data-toggle="tab">
                                        <i class="fa fa-file-archive-o fa-1x text-blue"></i>Últimos Movimientos </a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane active" id="info-tab">
                                        <div class="text-center">
                                            <strong>
                                                <asp:Label ID="lblverificacion" runat="server" class="text-success"></asp:Label></strong>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2 text-center">
                                                <%--                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>--%>
                                                <div class="fileinput fileinput-new" data-provides="fileinput">
                                                    <div class="fileinput-new thumbnail" style="width: 150px; height: 150px;">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/image/photo.png" class="img-circle"
                                                            Height="150px" Width="150px" />
                                                    </div>
                                                    <div class="fileinput-preview fileinput-exists thumbnail" style="max-width: 200px;
                                                        max-height: 150px;">
                                                    </div>
                                                    <br />
                                                    <div class="form-inline">
                                                        <span class="btn btn-sm btn-file btn-default btn-sm"><span class="fileinput-new"
                                                            title="Cambiar"><i class="fa fa-edit fa-fw"></i></span><span class="fileinput-exists"
                                                                title="Cambiar"><i class="fa fa-edit fa-fw"></i></span>
                                                            <input type="file" name="..." id="file2" runat="server" /></span> <a href="#" class="btn btn-default btn-sm fileinput-exists"
                                                                data-dismiss="fileinput" title="Quitar"><i class="fa fa-minus-circle fa-fw"></i>
                                                            </a>
                                                        <asp:LinkButton ID="lnkFoto" runat="server" class="btn btn-sm btn-default" OnClick="lnkFoto_Click"
                                                            Visible="false"><i class="fa fa-upload fa-fw"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <%--                                               <asp:Label ID="lblFotoError" runat="server"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkFoto" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                                <div class="form-group-sm">
                                                    <label class="text-blue input-sm text-blue">
                                                        Opciones:</label>
                                                    <ul class="list-group">
                                                        <asp:HyperLink ID="HyperLink2" class="list-group-item" runat="server" Target="_blank"
                                                            NavigateUrl="http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias"><i class="fa fa-building fa-fw"></i><label class="text-blue input-sm">RUC</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:HyperLink>
                                                        <asp:HyperLink ID="HyperLink3" class="list-group-item" runat="server" Target="_blank"
                                                            NavigateUrl="http://ww4.essalud.gob.pe:7777/acredita/"><i class="fa fa-hospital-o fa-fw"></i><label class="text-blue input-sm">ESSALUD</label></asp:HyperLink>
                                                        <asp:LinkButton ID="lnkActualizacionTitu2" runat="server" class="list-group-item btn-danger"
                                                            OnClick="lnkActualizacionTitu2_Click"><i class="fa fa-refresh fa-fw text-danger"></i><label class="text-danger input-sm">Verificación de Datos</label></asp:LinkButton>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-sm-4">
                                                            <div class="panel-default">
                                                                <div class="panel-body form-horizontal">
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Código Cliente (*):</label>
                                                                        <div class="form-inline">
                                                                            <asp:TextBox ID="txtNumeroPoli" type="text" runat="server" class="form-control input-sm"
                                                                                Width="30%"></asp:TextBox>
                                                                            <asp:TextBox ID="txtNombreEmpresa" type="text" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Código Titular (*):</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtCodigoTitu" ClientIDMode="Static" name="txtCodigoTitu" type="text"
                                                                                runat="server" class="form-control input-sm" onkeypress="return isNumberKey(event)"
                                                                                MaxLength="6" OnTextChanged="txtCodigoTitu_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <label for="uLogin" class="input-group-addon color-blue text-white input-sm">
                                                                                <asp:LinkButton ID="lnkBuscarTitular" runat="server" OnClick="lnkBuscarTitular_Click"
                                                                                    Visible="false">
                                                                        <i class="glyphicon glyphicon-search color-blue text-white fa-1x"></i>
                                                                                </asp:LinkButton>
                                                                            </label>
                                                                        </div>
                                                                        <asp:Label ID="lblCodigoTitularCarac" runat="server" class="input-sm text-danger"></asp:Label>
                                                                        <asp:Label ID="lblCodigoTitularCorrecto" runat="server" class="input-sm text-success"></asp:Label>
                                                                        <asp:Label ID="lblCodigoTitularNoHay" runat="server" class="input-sm text-danger"></asp:Label>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Parentesco (*):</label>
                                                                        <asp:DropDownList ID="ddlCategoria" runat="server" class="form-control input-sm"
                                                                            OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="categoriaHidden" runat="server" />
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Plan:</label>
                                                                        <asp:DropDownList ID="ddlPlan" runat="server" class="form-control input-sm" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Centro de Costo:</label>
                                                                        <asp:DropDownList ID="ddlCentro" runat="server" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Tipo Documento:</label>
                                                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Nro. Documento (*):</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtDNI" ClientIDMode="Static" name="txtDNI" runat="server" type="text"
                                                                                class="form-control input-sm" onkeypress="return isNumberKey(event)" MaxLength="15"></asp:TextBox>
                                                                            <label for="uLogin" class="input-group-addon color-blue text-white input-sm">
                                                                                <asp:LinkButton ID="lnkBD" runat="server" OnClick="lnkBD_Click">
                                                                        <i class="glyphicon glyphicon-search color-blue text-white fa-1x"></i>
                                                                                </asp:LinkButton>
                                                                            </label>
                                                                        </div>
                                                                        <asp:Label ID="lblBD" runat="server" Font-Size="XX-Small"></asp:Label>
                                                                    </div>
                                                                    <div class="form-group-sm" id="iPassword" runat="server" visible="false">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Contraseña para Acceso Web:</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtContraseña" ClientIDMode="Static" runat="server" type="text"
                                                                                class="form-control input-sm" MaxLength="15" TextMode="Password" ReadOnly="true"></asp:TextBox>
                                                                            <label for="uLogin" class="input-group-addon color-blue text-white input-sm">
                                                                                <asp:LinkButton ID="lnkReset" runat="server" OnClick="lnkReset_Click">
                                                                        <i class="fa fa-refresh color-blue text-white fa-1x"></i>
                                                                                </asp:LinkButton>
                                                                            </label>
                                                                        </div>
                                                                        <%--
                                                            <input type="text" name="prevent_autofill" id="Text1" value="" style="display:none;" />
                                                            <input type="password" name="password_fake" id="password1" value="" style="display:none;" />
                                                            <asp:TextBox ID="txtContraseña" runat="server" class="form-control input-sm" TextMode="Password"></asp:TextBox>--%>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="documentoSIMA" visible="false">
                                                                        <label class="control-label input-sm text-warning">
                                                                            Documento SIMA:</label>
                                                                        <asp:TextBox ID="txtDocumento" ClientIDMode="Static" runat="server" name="txtNombres"
                                                                            class="form-control input-sm"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="pad" visible="false">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Prog. Atención Dental:</label>
                                                                        <asp:DropDownList ID="ddlPad" runat="server" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="cod_paciente" visible="false">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Parentesco Petro (*):</label>
                                                                        <asp:TextBox ID="txtCodPaciente" runat="server" class="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="id_paciente">
                                                                        <label class="control-label input-sm text-blue">
                                                                            ID. Hijo (*):</label>
                                                                        <asp:TextBox ID="txtIdPaciente" runat="server" class="form-control input-sm" ReadOnly="true"
                                                                            MaxLength="2" AutoPostBack="true" OnTextChanged="txtIdPaciente_TextChanged"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <div class="panel-default">
                                                                <div class="panel-body form-horizontal">
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Nombres (*):</label>
                                                                        <asp:TextBox ID="txtNombres" ClientIDMode="Static" runat="server" name="txtNombres"
                                                                            class="form-control input-sm uppercase"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Apellido Paterno (*):</label>
                                                                        <asp:TextBox ID="txtApellidop" runat="server" ClientIDMode="Static" name="txtApellidop"
                                                                            class="form-control input-sm uppercase"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="amount" class="control-label input-sm text-blue">
                                                                            Apellido Materno (*):</label>
                                                                        <asp:TextBox ID="txtApellidom" runat="server" ClientIDMode="Static" name="txtApellidom"
                                                                            class="form-control input-sm uppercase"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="amount" class="control-label input-sm text-blue">
                                                                            Fecha Nacimiento (*):</label>
                                                                        <asp:TextBox ID="txtNacimiento" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="txtNacimiento_CalendarExtender" runat="server" Enabled="True"
                                                                            TargetControlID="txtNacimiento" Format="dd/MM/yyyy">
                                                                        </asp:CalendarExtender>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="date" class="control-label input-sm text-blue">
                                                                            Sexo:</label>
                                                                        <asp:DropDownList ID="ddlSexo" runat="server" name="sexo" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="date" class="control-label input-sm text-blue">
                                                                            Estado Civil:</label>
                                                                        <asp:DropDownList ID="ddlEstadoCivil" runat="server" name="estado" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="date" class="control-label input-sm text-blue">
                                                                            Correo:</label>
                                                                        <asp:TextBox ID="txtObservar" ClientIDMode="Static" runat="server" name="txtObservar"
                                                                            class="form-control input-sm" value="@correo.com" onfocus="if(this.value=='@correo.com') this.value=''"
                                                                            onblur="if(this.value=='') this.value='@correo.com'"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="date" class="control-label input-sm text-blue">
                                                                            Correo 1:</label>
                                                                        <asp:TextBox ID="txtCorreo1" ClientIDMode="Static" runat="server" name="txtCorreo1"
                                                                            class="form-control input-sm" value="@correo.com" onfocus="if(this.value=='@correo.com') this.value=''"
                                                                            onblur="if(this.value=='') this.value='@correo.com'"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="date" class="control-label input-sm text-blue">
                                                                            Correo 2:</label>
                                                                        <asp:TextBox ID="txtCorreo2" ClientIDMode="Static" runat="server" name="txtCorreo2"
                                                                            class="form-control input-sm" value="@correo.com" onfocus="if(this.value=='@correo.com') this.value=''"
                                                                            onblur="if(this.value=='') this.value='@correo.com'"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="campo2" visible="false">
                                                                        <label class="control-label input-sm text-blue">
                                                                            Financia:</label>
                                                                        <asp:TextBox ID="txtCampo2" runat="server" class="form-control input-sm" MaxLength="1"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="rol" visible="false">
                                                                        <label for="status" class="control-label text-blue input-sm">
                                                                            Rol:
                                                                        </label>
                                                                        <asp:TextBox ID="txtRol" ClientIDMode="Static" name="txtRol" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group-sm" runat="server" id="dpto" visible="false">
                                                                        <label class="control-label text-blue input-sm">
                                                                            Dependencia:</label>
                                                                        <asp:TextBox ID="txtDpto" ClientIDMode="Static" name="txtDpto" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkBD" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlCategoria" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtIdPaciente" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="col-sm-4">
                                                    <div class="panel-default">
                                                        <div class="panel-body form-horizontal">
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="form-group-sm">
                                                                        <label for="status" class="control-label text-blue input-sm">
                                                                            Departamento:
                                                                        </label>
                                                                        <asp:DropDownList ID="ddlDepartamento" runat="server" class="form-control input-sm"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="status" class="control-label text-blue input-sm">
                                                                            Provincia:
                                                                        </label>
                                                                        <asp:DropDownList ID="ddlProvincia" runat="server" class="form-control input-sm"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group-sm">
                                                                        <label for="status" class="control-label text-blue input-sm">
                                                                            Distrito:
                                                                        </label>
                                                                        <asp:DropDownList ID="ddlDistrito" runat="server" class="form-control input-sm">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <div class="form-group-sm">
                                                                <label for="concept" class="control-label input-sm text-blue">
                                                                    Dirección:</label>
                                                                <asp:TextBox ID="txtDireccion" ClientIDMode="Static" runat="server" name="txtDireccion"
                                                                    class="form-control input-sm uppercase"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group-sm">
                                                                <label for="date" class="control-label input-sm text-blue">
                                                                    Teléfono Fijo:</label>
                                                                <asp:TextBox ID="txtTelefono1" runat="server" name="txtTelefono1" onkeypress="return isNumberKey(event)"
                                                                    class="form-control input-sm"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group-sm">
                                                                <label class="control-label input-sm text-blue">
                                                                    Teléfono Móvil:</label>
                                                                <asp:TextBox ID="txtTelefono2" runat="server" name="txtTelefono2" onkeypress="return isNumberKey(event)"
                                                                    class="form-control input-sm"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group-sm">
                                                                <label for="date" class="control-label input-sm text-blue">
                                                                    Fecha Ingreso:</label>
                                                                <asp:TextBox ID="txtAlta" ClientIDMode="Static" runat="server" name="txtAlta" class="form-control input-sm"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtAlta_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtAlta" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </div>
                                                            <div class="form-group-sm">
                                                                <label for="date" class="control-label text-warning input-sm">
                                                                    Carencia:</label>
                                                                <input type="text" name="prevent_autofill" id="prevent_autofill" value="" style="display: none;" />
                                                                <input type="password" name="password_fake" id="password_fake" value="" style="display: none;" />
                                                                <asp:TextBox ID="txtCarencia" runat="server" ClientIDMode="Static" name="txtCarencia"
                                                                    class="form-control input-sm uppercase"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtCarencia_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtCarencia" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </div>
                                                            <div class="form-group-sm">
                                                                <label for="date" class="control-label input-sm text-blue">
                                                                    Fecha Baja:</label>
                                                                <asp:TextBox ID="txtBaja" ClientIDMode="Static" runat="server" name="txtBaja" class="form-control input-sm"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtBaja_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtBaja" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hfCodigoCliente" runat="server" />
                                        <asp:HiddenField ID="hfCodigoTitular" runat="server" />
                                    </div>
                                    <div class="tab-pane" id="address-tab">
                                        <div class="text-center">
                                            <strong>
                                                <asp:Label ID="lblverificacion1" runat="server" class="text-success"></asp:Label></strong>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4">
                                                <div class="panel-default">
                                                    <div class="panel-body form-horizontal">
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                ¿Persona Discapacitada?:</label>
                                                            <asp:DropDownList ID="ddlDiscapacit" runat="server" class="form-control input-sm">
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Edad:</label>
                                                            <asp:TextBox ID="txtEdad" runat="server" class="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                Peso:</label>
                                                            <asp:TextBox ID="txtPeso" runat="server" class="form-control input-sm" placeholder="kg."></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="amount" class="control-label input-sm text-blue">
                                                                Estatura:</label>
                                                            <asp:TextBox ID="txtEstatura" runat="server" class="form-control input-sm" placeholder="cm."></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                Grupo Sanguíneo:</label>
                                                            <asp:TextBox ID="txtSangre" runat="server" class="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                ¿Consume Alcohol?:</label>
                                                            <asp:DropDownList ID="ddlAlcohol" runat="server" class="form-control input-sm text-center">
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                ¿Consume Drogas?:</label>
                                                            <asp:DropDownList ID="ddlDrogas" runat="server" class="form-control input-sm text-center">
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4" runat="server" id="ocultos2" visible="false">
                                                <div id="Div2" runat="server" class="panel-default" visible="true">
                                                    <div class="panel-body form-horizontal">
                                                        <div class="form-group-sm" runat="server" id="onco" visible="false">
                                                            <label for="amount" class="control-label input-sm text-blue">
                                                                ONCO:</label>
                                                            <asp:DropDownList ID="ddlOnco" runat="server" class="form-control input-sm">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm" runat="server" id="basico" visible="false">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                Básico:</label>
                                                            <asp:DropDownList ID="ddlBasico" runat="server" class="form-control input-sm">
                                                                <asp:ListItem Value="0">----Opciones----</asp:ListItem>
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm" runat="server" id="segundacapa" visible="false">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                Segunda Capa:</label>
                                                            <asp:DropDownList ID="ddlCapa" runat="server" class="form-control input-sm">
                                                                <asp:ListItem Value="0">----Opciones----</asp:ListItem>
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="form-group-sm" runat="server" id="programa" visible="false">
                                                            <label class="control-label input-sm text-blue">
                                                                Programa Especial</label>
                                                            <asp:DropDownList ID="ddlPespecial" runat="server" class="form-control input-sm">
                                                                <asp:ListItem Value="0">----Opciones----</asp:ListItem>
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="S">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="cartas-tabb">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div class="col-sm-12 centrar">
                                                    <div class="form-inline">
                                                        <asp:DropDownList ID="ddlTipoOrdenAtencion" runat="server" class="form-control input-sm">
                                                            <asp:ListItem Value="">Tipo</asp:ListItem>
                                                            <asp:ListItem Value="1">Órdenes de Atención</asp:ListItem>
                                                            <asp:ListItem Value="2">Cartas de Garantía</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="lnkCagarCartas" runat="server" class="btn btn-default btn-sm"
                                                            OnClick="lnkCagarCartas_Click">
                                                <i class="fa fa-search fa-1x text-blue"></i> Consultar Cartas
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkExportarIC" runat="server" class="btn btn-default btn-sm pull-right"
                                                            OnClick="lnkExportarIC_Click">
                                                <i class="fa fa-download fa-1x text-blue"></i> Exportar Cartas
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <asp:Label ID="lblNohayCarta" runat="server" class="text-danger input-sm"></asp:Label>
                                                <div class="row centrar" runat="server" id="cartas123" visible="false">
                                                    <div class="col-sm-12 table-responsive">
                                                        <div class="col-md-12 table-responsive" runat="server" id="icanual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblEvoIC" runat="server" class="input-sm text-info">EVOLUCIÓN ANUAL DE CARTAS</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvImpresionCartasAnio" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    DataKeyNames="ANIO" AllowSorting="True" PageSize="7" AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkICAnio" runat="server" Text="Ver" OnClick="PostgvICAnio" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                                                                        <asp:BoundField DataField="CARTAS" HeaderText="CANTIDAD" />
                                                                        <asp:BoundField DataField="GASTO" HeaderText="MONTO" DataFormatString="{0:N0}" />
                                                                        <asp:BoundField DataField="PORG" HeaderText="PORG" Visible="false" />
                                                                        <asp:BoundField DataField="PORB" HeaderText="PORB" Visible="false" />
                                                                        <asp:BoundField DataField="X" HeaderText="X" Visible="false" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblGraficoIC" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="ChartAnioIC" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="BlueViolet" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 table-responsive" runat="server" id="icmensual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblDetaEvoIC" runat="server" class="input-sm text-info">EVOLUCIÓN MENSUAL DE CARTAS</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvImpresionCartasMes" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    DataKeyNames="COD_MES,MES" AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkICMes" runat="server" Text="Ver" OnClick="PostgvICMes" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="COD_MES" HeaderText="COD_MES" Visible="false" />
                                                                        <asp:BoundField DataField="MES" HeaderText="MES" />
                                                                        <asp:BoundField DataField="CARTAS" HeaderText="CANTIDAD" />
                                                                        <asp:BoundField DataField="GASTO" HeaderText="MONTO" DataFormatString="{0:N0}" />
                                                                        <asp:BoundField DataField="PORG" HeaderText="PORG" Visible="false" />
                                                                        <asp:BoundField DataField="PORB" HeaderText="PORB" Visible="false" />
                                                                        <asp:BoundField DataField="X" HeaderText="X" Visible="false" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblGraficaIC2" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="ChartMesIC" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="DarkGray" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 table-responsive" id="icdetalle" runat="server" visible="false">
                                                            <div class="form-inline">
                                                                <legend>
                                                                    <asp:Label ID="lblDetalleIC" runat="server" class="input-sm text-info"></asp:Label></legend>
                                                            </div>
                                                            <asp:GridView ID="gvImpresionCartasDetalle" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                AllowPaging="True" AllowSorting="True" PageSize="8" DataKeyNames="solben_id,atencion_id,cod_cliente"
                                                                AutoGenerateColumns="false" OnPageIndexChanging="gvImpresionCartasDetalle_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDetalleIC" runat="server" Text="Ver" OnClick="FullPostBack" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ampliacion" HeaderText="ID" Visible="false" />
                                                                    <asp:BoundField DataField="solben_id" HeaderText="ID" />
                                                                    <asp:BoundField DataField="atencion_nombre" HeaderText="ATENCION" />
                                                                    <asp:BoundField DataField="especialidad" HeaderText="ESPECIALIDAD" />
                                                                    <asp:BoundField DataField="atencion_id" HeaderText="ATENCION_ID" Visible="false" />
                                                                    <asp:BoundField DataField="cod_cliente" HeaderText="COD.CLI" Visible="false" />
                                                                    <asp:BoundField DataField="cod_titula" HeaderText="COD.TI" Visible="false" />
                                                                    <asp:BoundField DataField="categoria" HeaderText="CATEGORIA" Visible="false" />
                                                                    <asp:BoundField DataField="clinica_nombre" HeaderText="CLINICA" />
                                                                    <asp:BoundField DataField="afiliado" HeaderText="AFILIADO" />
                                                                    <asp:BoundField DataField="solben_monto" HeaderText="MONTO" DataFormatString="{0:N0}" />
                                                                    <asp:BoundField DataField="fecha" HeaderText="FECHA" Visible="false" />
                                                                    <asp:BoundField DataField="parentesco" HeaderText="PARENTESCO" />
                                                                    <asp:BoundField DataField="solben_fecha" HeaderText="S.FECHA" />
                                                                </Columns>
                                                                <HeaderStyle CssClass="input-sm" />
                                                                <PagerSettings Position="TopAndBottom" PageButtonCount="3" />
                                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkExportarIC" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkCagarCartas" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvImpresionCartasDetalle" EventName="PageIndexChanging" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="updateProgress" runat="server">
                                            <ProgressTemplate>
                                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                                                    right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/image/loading1.gif"
                                                        AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed;
                                                        top: 45%; left: 50%;" />
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div class="tab-pane" id="record-tabb">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="col-sm-12 centrar">
                                                    <div class="form-inline">
                                                        <asp:LinkButton ID="lnkCargarRecord" runat="server" class="btn btn-default btn-sm"
                                                            OnClick="lnkCargarRecord_Click">
                                                    <i class="fa fa-refresh fa-1x text-blue"></i> Cargar Record de Consumo
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkExportarRecord" runat="server" class="btn btn-default btn-sm pull-right"
                                                            OnClick="lnkExportarRecord_Click">
                                                    <i class="fa fa-download fa-1x text-blue"></i> Exportar Record de Consumo
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <asp:Label ID="lblNoHayRecord" runat="server" class="text-danger input-sm"></asp:Label>
                                                <div class="row centrar" runat="server" id="record123" visible="false">
                                                    <div class="col-sm-12 table-responsive">
                                                        <div class="col-md-12 table-responsive" runat="server" id="RCanual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblEvo" runat="server" class="input-sm text-info">EVOLUCIÓN ANUAL RECORD DE CONSUMO</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvDatosDetalle1" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    AutoGenerateColumns="False" DataKeyNames="ANIO">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDetallegv1" runat="server" Text="Ver" OnClick="PartialPostgv1" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                                                                        <asp:BoundField DataField="CASOS" HeaderText="CASOS <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" SortExpression="CASOS" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="GASTO" HeaderText="GASTO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" DataFormatString="{0:N0}" SortExpression="GASTO" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="BENEFICIO" DataFormatString="{0:N0}" HeaderText="BENEFICIO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" SortExpression="BENEFICIO" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="PORG" HeaderText="% REC." ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="PORB" HeaderText="% INC." ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="X" HeaderText="X" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="input-sm" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive ">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblGraficaRC" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA ANUAL</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="gAnio" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            IsXValueIndexed="false" Color="Blue" IsValueShownAsLabel="false">
                                                                            <SmartLabelStyle Enabled="False" />
                                                                        </asp:Series>
                                                                        <asp:Series Name="BENEFICIOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="Red" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="RCmensual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblEvoDet" runat="server" class="input-sm text-info">EVOLUCIÓN MENSUAL RECORD DE CONSUMO</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvDatosDetalle2" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    AutoGenerateColumns="False" DataKeyNames="COD_MES,MES">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDetallegv2" runat="server" Text="Ver" OnClick="PartialPostgv2" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="COD_MES" HeaderText="COD_MES" Visible="false" />
                                                                        <asp:BoundField DataField="MES" HeaderText="MES" />
                                                                        <asp:BoundField DataField="CASOS" HeaderText="CASOS <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" SortExpression="CASOS" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="GASTO" HeaderText="GASTO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" DataFormatString="{0:N0}" SortExpression="GASTO" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="BENEFICIO" HeaderText="BENEFICIO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                            HtmlEncode="false" DataFormatString="{0:N0}" SortExpression="BENEFICIO" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="PORG" HeaderText="% GASTO" Visible="False" ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="PORB" HeaderText="% INC." ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundField DataField="X" HeaderText="X" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="input-sm" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive ">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="lblGraficaRC2" runat="server" class="input-sm text-info">EVOLUCIÓN MENSUAL RECORD DE CONSUMO</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="gMeses" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            IsXValueIndexed="false" Color="Blue" IsValueShownAsLabel="false">
                                                                            <SmartLabelStyle Enabled="False" />
                                                                        </asp:Series>
                                                                        <asp:Series Name="BENEFICIOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="Red" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 table-responsive" runat="server" id="detalleRC" visible="false">
                                                            <div class="form-inline">
                                                                <legend>
                                                                    <asp:Label ID="lblFinal" runat="server" class="input-sm text-info"></asp:Label></legend>
                                                            </div>
                                                            <asp:GridView ID="gvDatosDetalle3" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="NRO_INTER,NRO_PLANI"
                                                                AllowSorting="True" OnPageIndexChanging="gvDatosDetalle3_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDetallegv3" runat="server" Text="Ver" OnClick="PartialPostgv3" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="PACIENTE" HeaderText="PACIENTE" />
                                                                    <asp:BoundField DataField="PARENTESCO" HeaderText="PARENTESCO" />
                                                                    <asp:BoundField DataField="proveedor" HeaderText="CLINICA" />
                                                                    <asp:BoundField DataField="cie10" HeaderText="CIE10" />
                                                                    <asp:BoundField DataField="GASTO" HeaderText="GASTO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                        HtmlEncode="false" DataFormatString="{0:N2}" SortExpression="GASTO" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="BENEFICIO" HeaderText="BENEFICIO <i class='fa fa-sort fa-1x text-blue'></i>"
                                                                        HtmlEncode="false" DataFormatString="{0:N2}" SortExpression="BENEFICIO" ItemStyle-HorizontalAlign="Right" />
                                                                    <asp:BoundField DataField="NRO_INTER" HeaderText="NRO INTER" Visible="false" />
                                                                    <asp:BoundField DataField="NRO_PLANI" HeaderText="NRO PLANI" Visible="false" />
                                                                    <asp:BoundField DataField="atencion" HeaderText="FECHA ATENCIÓN" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="proceso" HeaderText="FECHA PROCESO" ItemStyle-HorizontalAlign="Center" />
                                                                </Columns>
                                                                <HeaderStyle CssClass="input-sm" />
                                                                <PagerSettings Position="TopAndBottom" PageButtonCount="3" />
                                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkExportarRecord" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkCargarRecord" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDatosDetalle3" EventName="PageIndexChanging" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="updateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                                                    right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                    <asp:Image ID="imgUpdateProgress1" runat="server" ImageUrl="~/image/loading1.gif"
                                                        AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed;
                                                        top: 45%; left: 50%;" />
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div class="tab-pane" id="avisos-tabb">
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12 centrar" runat="server" id="BuscaAviso">
                                                    <div class="form-inline">
                                                        <asp:Label ID="lblAvisoDesc" runat="server" Text="Ingresar Descripción de Aviso:"
                                                            class="text-blue input-sm" Visible="False"></asp:Label>
                                                        <asp:TextBox ID="txtAfiliado" runat="server" class="form-control input-sm" placeholder="Buscar Aviso"
                                                            Width="250"></asp:TextBox>
                                                        <asp:LinkButton ID="btnBuscar" runat="server" class="btn btn-default btn-sm" ToolTip="Buscar"
                                                            OnClick="btnBuscar_Click"> 
                                                <i class="fa fa-1x text-blue"></i>Buscar
                                                        </asp:LinkButton>
                                                        <asp:Label ID="lblNohayAvisos" runat="server" class="text-danger input-sm"></asp:Label>
                                                        <asp:LinkButton ID="btnNuevoAviso2" runat="server" class="btn btn-default btn-sm pull-right"
                                                            OnClick="btnNuevoAviso2_Click">
                                                <i class="fa fa-plus-square-o fa-1x text-blue"></i> Nuevo
                                                        </asp:LinkButton>
                                                    </div>
                                                    <asp:HiddenField ID="idAviso" runat="server" />
                                                    <asp:Label ID="lblAvisos" runat="server" class="text-blue input-sm"></asp:Label>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false" id="ListaAvisos">
                                                    <asp:GridView ID="gvAvisos" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                        AutoGenerateColumns="false" DataKeyNames="op,Aviso,Cod Aviso,idClasif,Desde,Hasta,Fecha,estado,limite"
                                                        OnRowCommand="gvAvisos_RowCommand" OnPageIndexChanging="gvAvisos_PageIndexChanging"
                                                        AllowPaging="True">
                                                        <Columns>
                                                            <asp:ButtonField CommandName="Editar" Text="Editar" />
                                                            <asp:ButtonField CommandName="Eliminar" Text="Eliminar" />
                                                            <asp:BoundField DataField="Cod Aviso" HeaderText="COD" SortExpression="NRO" />
                                                            <asp:BoundField DataField="Aviso" HeaderText="Aviso" SortExpression="NRO" />
                                                            <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="NRO" Visible='false' />
                                                            <asp:BoundField DataField="Clasif" HeaderText="Clasificación" SortExpression="NRO" />
                                                            <asp:BoundField DataField="Desde" HeaderText="Desde" SortExpression="NRO" />
                                                            <asp:BoundField DataField="Hasta" HeaderText="Hasta" SortExpression="NRO" />
                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha Reg." SortExpression="NRO" />
                                                            <asp:BoundField DataField="fecha_mod" HeaderText="Fecha Mod." SortExpression="NRO" />
                                                            <asp:BoundField DataField="limite" HeaderText="Limite" SortExpression="NRO" Visible="false" />
                                                            <asp:BoundField DataField="estado" HeaderText="Estado" SortExpression="NRO" Visible="false" />
                                                            <asp:BoundField DataField="sinlimite_des" HeaderText="Limite" SortExpression="NRO"
                                                                HtmlEncode="false" />
                                                            <asp:BoundField DataField="estado_des" HeaderText="Estado" SortExpression="NRO" HtmlEncode="false" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <div class="col-md-12" id="NuevoAviso" runat="server" visible="false">
                                                    <div class="col-md-6 col-lg-offset-3">
                                                        <br />
                                                        <div class="panel-default">
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <div class="form-inline">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" class="text-blue input-sm" Text="Aviso Nuevo"></asp:Label></strong>
                                                                        <div class="form-inline pull-right">
                                                                            <asp:LinkButton ID="btnGuardarAviso" runat="server" class="btn btn-default btn-sm"
                                                                                OnClick="btnGuardarAviso_Click">
                                                                    <i class="fa fa-floppy-o fa-1x text-blue"></i> Guardar 
                                                                            </asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkGuardarCambios" runat="server" class="btn btn-default btn-sm"
                                                                                Visible="false" OnClick="lnkGuardarCambios_Click">
                                                                    <i class="fa fa-floppy-o fa-1x text-blue"></i> Guardar Cambios
                                                                            </asp:LinkButton>
                                                                            <asp:LinkButton ID="btnCancelAviso" runat="server" class="btn btn-default btn-sm"
                                                                                OnClick="btnCancelAviso_Click">
                                                                    <i class="fa fa-times fa-1x text-blue"></i> Cancelar
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                    <hr class="hr-blue" />
                                                                </div>
                                                            </div>
                                                            <div class="panel-body form-horizontal">
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label13" runat="server" class="col-sm-3 text-blue input-sm">Clasificacion (*):</asp:Label>
                                                                        <div class="col-sm-6">
                                                                            <asp:DropDownList ID="Ddl_ClasifAviso" runat="server" AutoPostBack="true" CssClass="form-control input-sm"
                                                                                OnSelectedIndexChanged="Ddl_ClasifAviso_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label7" runat="server" class="col-sm-3 text-blue input-sm">Ingresar Descripción de Aviso :</asp:Label>
                                                                        <div class="col-sm-6">
                                                                            <asp:TextBox ID="txtAvisoDescrip" runat="server" class="form-control input-sm" placeHolder='AVISO A REGISTRAR'
                                                                                TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label6" runat="server" class=" col-sm-3 text-blue input-sm">Periodo (*):</asp:Label>
                                                                        <div class="col-sm-3">
                                                                            <asp:TextBox ID="txtdesde" type="text" runat="server" CssClass="form-control input-sm"
                                                                                placeholder="DESDE" onkeydown="return (event.keyCode!=13);" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtdesde"
                                                                                Format="yyyy-MM-dd">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <asp:TextBox ID="txthasta" type="text" runat="server" CssClass="form-control input-sm"
                                                                                placeholder="HASTA" onkeydown="return (event.keyCode!=13);" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txthasta"
                                                                                Format="yyyy-MM-dd">
                                                                            </asp:CalendarExtender>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <asp:CheckBox ID="chkSinLimite" runat="server" class="input-sm text-blue" Text="&nbsp;Sin Límite"
                                                                                OnCheckedChanged="chkSinLimite_CheckedChanged" AutoPostBack="true" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lblAvisoNuevo" runat="server" class="text-blue input-sm"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvAvisos" EventName="PageIndexChanging" />
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardarAviso" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Ddl_ClasifAviso" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="gvAvisos" EventName="PageIndexChanging" />
                                                <asp:AsyncPostBackTrigger ControlID="chkSinLimite" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="updateProgress2" runat="server">
                                            <ProgressTemplate>
                                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                                                    right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                    <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/image/loading1.gif"
                                                        AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed;
                                                        top: 45%; left: 50%;" />
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div class="tab-pane" id="informes_medicos">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="col-sm-12 centrar">
                                                    <div class="form-inline">
                                                        <asp:LinkButton ID="lnkCargarInformes" runat="server" class="btn btn-default btn-sm"
                                                            OnClick="lnkCargarInformes_Click">
                                                <i class="fa fa-refresh fa-1x text-blue"></i> Cargar Informes Médicos
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkExportarInformesMedicos" runat="server" class="btn btn-default btn-sm pull-right"
                                                            OnClick="lnkExportarInformesMedicos_Click">
                                                <i class="fa fa-download fa-1x text-blue"></i> Exportar Informes Médicos
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <asp:Label ID="lblNoHayInformes" runat="server" class="text-danger input-sm"></asp:Label>
                                                <div class="row centrar" runat="server" visible="false" id="informes123">
                                                    <div class="col-sm-12 table-responsive">
                                                        <div class="col-md-12" runat="server" id="IManual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="Label4" runat="server" class="input-sm text-info">REPORTE ANUAL DE INFORMES MÉDICOS</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvInformesMedicosAnio" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    DataKeyNames="ANIO" AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkIMAnio" runat="server" Text="Ver" OnClick="PostgvIMAnio" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                                                                        <asp:BoundField DataField="INF_CANT_TOT" HeaderText="TOTAL" />
                                                                        <asp:BoundField DataField="INF_CERRADOS" HeaderText="CERRADOS" />
                                                                        <asp:BoundField DataField="INF_PENDIENTES" HeaderText="PENDIENTES" />
                                                                        <asp:BoundField DataField="INF_TEMP_CANT_TOT" HeaderText="TEMP TOTAL" />
                                                                        <asp:BoundField DataField="INF_TEMP_CERRADOS" HeaderText="TEMP CERRADOS" />
                                                                        <asp:BoundField DataField="INF_TEMP_PENDIENTES" HeaderText="TEMP PENDIENTES" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="input-sm" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive ">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="Label1" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="ChartIM1" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            IsXValueIndexed="false" Color="Blue" IsValueShownAsLabel="false">
                                                                            <SmartLabelStyle Enabled="False" />
                                                                        </asp:Series>
                                                                        <asp:Series Name="BENEFICIOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="Red" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" runat="server" id="IMmensual" visible="false">
                                                            <div class="col-md-6 table-responsive">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="Label5" runat="server" class="input-sm text-info">REPORTE MENSUAL DE INFORMES MÉDICOS</asp:Label></legend>
                                                                </div>
                                                                <asp:GridView ID="gvInformesMedicosMes" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                    DataKeyNames="COD_MES,MES" AutoGenerateColumns="false">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkIMMes" runat="server" Text="Ver" OnClick="PostgvIMMes" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="COD_MES" HeaderText="COD_MES" Visible="false" />
                                                                        <asp:BoundField DataField="MES" HeaderText="TOTAL" />
                                                                        <asp:BoundField DataField="INF_CANT_TOT" HeaderText="TOTAL" />
                                                                        <asp:BoundField DataField="INF_CERRADOS" HeaderText="CERRADOS" />
                                                                        <asp:BoundField DataField="INF_PENDIENTES" HeaderText="PENDIENTES" />
                                                                        <asp:BoundField DataField="INF_TEMP_CANT_TOT" HeaderText="TEMP TOTAL" />
                                                                        <asp:BoundField DataField="INF_TEMP_CERRADOS" HeaderText="TEMP CERRADOS" />
                                                                        <asp:BoundField DataField="INF_TEMP_PENDIENTES" HeaderText="TEMP PENDIENTES" />
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="input-sm" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="col-md-6 table-responsive ">
                                                                <div class="form-inline">
                                                                    <legend>
                                                                        <asp:Label ID="Label2" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
                                                                </div>
                                                                <asp:Chart ID="ChartIM2" runat="server" Width="440px" Height="220px" EnableViewState="True"
                                                                    Style="text-align: right" Palette="Fire">
                                                                    <Series>
                                                                        <asp:Series Name="GASTOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            IsXValueIndexed="false" Color="Blue" IsValueShownAsLabel="false">
                                                                            <SmartLabelStyle Enabled="False" />
                                                                        </asp:Series>
                                                                        <asp:Series Name="BENEFICIOS" YValuesPerPoint="4" Legend="Legend1" ChartArea="ChartArea1"
                                                                            Color="Red" IsValueShownAsLabel="false">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1">
                                                                            <AxisX Interval="1">
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend Name="Legend1">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                </asp:Chart>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 table-responsive" id="IMdetalle" runat="server" visible="false">
                                                            <div class="form-inline">
                                                                <legend>
                                                                    <asp:Label ID="lblIMDetalle" runat="server" class="input-sm text-info"></asp:Label></legend>
                                                            </div>
                                                            <asp:GridView ID="gvInformesMedicosDetalle" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                                AllowPaging="True" AllowSorting="True" DataKeyNames="solben" OnPageIndexChanging="gvInformesMedicosDetalle_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDetalleIM" runat="server" Text="Ver" OnClick="PostIMDetalle" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="input-sm" />
                                                                <PagerSettings Position="TopAndBottom" PageButtonCount="3" />
                                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkExportarInformesMedicos" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkCargarInformes" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvInformesMedicosDetalle" EventName="PageIndexChanging" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="updateProgress3" runat="server">
                                            <ProgressTemplate>
                                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                                                    right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                    <asp:Image ID="imgUpdateProgress3" runat="server" ImageUrl="~/image/loading1.gif"
                                                        AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed;
                                                        top: 45%; left: 50%;" />
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                    <div class="tab-pane" id="ultimos_movimientos">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div class="text-center centrar">
                                                    <div class="form-inline">
                                                        <label class="text-blue input-sm">
                                                            AÑO:</label>
                                                        <asp:DropDownList ID="ddlAnio" runat="server" class="form-control input-sm">
                                                        </asp:DropDownList>
                                                        <label class="text-blue input-sm">
                                                            MES:</label>
                                                        <asp:DropDownList ID="ddlMes" runat="server" class="form-control input-sm">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnBuscarCG" runat="server" Text="Buscar" class="btn btn-primary btn-sm"
                                                            OnClick="btnBuscarCG_Click" />
                                                    </div>
                                                    <asp:Label ID="lblNoHayMovimientos" runat="server" class="text-danger input-sm"></asp:Label>
                                                </div>
                                                <div class="row" runat="server" id="movimientos123" visible="false">
                                                    <div class="col-md-12 table-responsive">
                                                        <asp:GridView ID="gvMovimientos" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                            AutoGenerateColumns="false" OnPageIndexChanging="gvMovimientos_PageIndexChanging">
                                                            <Columns>
                                                                <asp:BoundField DataField="FECREG" HeaderText="FECREG" DataFormatString="{0:d}" />
                                                                <asp:BoundField DataField="MOVIMIENTO" HeaderText="MOVIMIENTO" />
                                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" />
                                                                <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnBuscarCG" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="PageIndexChanging" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:UpdateProgress ID="updateProgress4" runat="server">
                                            <ProgressTemplate>
                                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                                                    right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                    <asp:Image ID="imgUpdateProgress4" runat="server" ImageUrl="~/image/loading1.gif"
                                                        AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed;
                                                        top: 45%; left: 50%;" />
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPlan" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="panel-footer color-blue text-white text-center">
                    <%--http://www.solben.net/loginIII.php?u=%3Cusu%3E&p=%3Cpass%3E&d=3&p2=%3Cregistro%3E--%>
                    <asp:LinkButton ID="lnkImpreOrden" runat="server" class="btn btn-default btn-sm"
                        OnClick="lnkImpreOrden_Click"> <i class="fa fa-print fa-1x text-blue"></i>&nbsp;Generación de Órdenes de Atención </asp:LinkButton>
                    <asp:LinkButton ID="lnkFichaPersonal" runat="server" class="btn btn-default btn-sm"
                        OnClick="lnkFichaPersonal_Click"> <i class="fa fa-print fa-1x text-blue"></i>&nbsp;Imprimir Ficha </asp:LinkButton>
                    <%--<button id="btnPrint" class="btn btn-default btn-sm"><i class="fa fa-print fa-1x text-blue"></i> Imprimir Datos</button>--%>
                    <asp:LinkButton ID="btnActivarAfiliado" runat="server" class="btn btn-default btn-sm"
                        Visible="false" OnClick="btnActivarAfiliado_Click"> <i class="fa fa-plus-circle fa-1x text-blue"></i>&nbsp;Activar Dependiente </asp:LinkButton>
                    <asp:LinkButton ID="btnBajaModal" runat="server" class="btn btn-default btn-sm" OnClick="btnBajaModal_Click"
                        Visible="false"> <i class="fa fa-plus-circle fa-1x text-warning"></i>&nbsp;Dar de Baja </asp:LinkButton>
                    <asp:LinkButton ID="btnHistorialEdicionTitular" runat="server" class="btn btn-default btn-sm"
                        Visible="False"> <i class="fa fa-history fa-1x text-info"></i>&nbsp;Historial </asp:LinkButton>
                    <asp:LinkButton ID="btnGuardarModificar" runat="server" class="btn btn-default btn-sm"
                        OnClientClick="return confirm('¿Esta seguro que desea guardar configuración actual');"
                        OnClick="btnGuardarModificar_Click" Visible="false"> <i class="fa fa-save fa-1x text-blue"></i>&nbsp;Guardar Cambios </asp:LinkButton>
                    <asp:LinkButton ID="btnGuardarRegistrar" runat="server" class="btn btn-default btn-sm"
                        OnClick="btnGuardarRegistrar_Click" Visible="false"> <i class="fa fa-edit fa-1x text-blue"></i>&nbsp;Registrar Afiliado </asp:LinkButton>
                    <asp:LinkButton ID="btnCerrar" runat="server" class="btn btn-default btn-sm" OnClick="btnCerrar_Click"> <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;Cerrar </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div id="BAJA" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content panel panel-popup">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="panel-heading color-blue text-white">
                            <div class="form-inline">
                                <i class="fa fa-calendar-o fa-1x"></i>Seleccionar Fecha de Baja
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-8 col-md-offset-1">
                                <div class="form-inline">
                                    <label class="text-blue input-sm">
                                        Ingrese fecha de baja:
                                    </label>
                                    <asp:TextBox ID="TextBox1" runat="server" class="form-control input-sm"></asp:TextBox>
                                    <asp:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="TextBox1" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:LinkButton ID="btnBaja01" runat="server" class="btn btn-default btn-sm" OnClick="btnBaja01_Click">
                                        <i class="fa fa-check-circle fa-1x text-success"></i> Dar de Baja
                                    </asp:LinkButton>
                                </div>
                                <br />
                                <div id="bajapetro" runat="server" visible="false">
                                    <legend>
                                        <label class="text-blue input-sm">
                                            Seleccione los clientes en los que desea dar de baja al afiliado:
                                        </label>
                                    </legend>
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Vertical" class="input-sm text-blue">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer color-blue text-white text-center">
                            <asp:LinkButton ID="btnSali" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar"
                                OnClick="btnSali_Click">
                                    <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                            </asp:LinkButton>
                            <%--data-dismiss="modal" aria-hidden="true" --%>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnBaja01" />
                        <asp:PostBackTrigger ControlID="btnSali" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="GRUPOFAMILIAR" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content panel panel-popup">
                <div class="panel-heading color-blue text-white">
                    <div class="form-inline">
                        <i class="fa fa-calendar-o fa-1x"></i>GRUPO FAMILIAR
                    </div>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="col-md-12 table-responsive">
                                <asp:GridView ID="gvGrupoFamiliar" runat="server" AutoGenerateColumns="False" DataKeyNames="cod_cliente,cod_titula,categoria"
                                    class="table table-bordered table-condensed table-striped input-sm" OnRowCommand="gvGrupoFamiliar_RowCommand">
                                    <Columns>
                                        <asp:ButtonField CommandName="Editar" Text="Editar" />
                                        <asp:BoundField DataField="cod_cliente" HeaderText="CLIENTE" SortExpression="#.POLIZA" />
                                        <asp:BoundField DataField="cod_cliente2" HeaderText="#.POLIZA" SortExpression="#.POLIZA"
                                            Visible="false" />
                                        <asp:BoundField DataField="cod_titula" HeaderText="CO.TITU" SortExpression="CO.TITU" />
                                        <asp:BoundField DataField="dni" HeaderText="DNI" SortExpression="DNI" />
                                        <asp:BoundField DataField="parentesco" HeaderText="PARENTESCO" SortExpression="CATEGORIA" />
                                        <asp:BoundField DataField="categoria" HeaderText="CATEGORIA" SortExpression="CATEGORIA" />
                                        <asp:BoundField DataField="plan" HeaderText="PLAN" SortExpression="PLAN" />
                                        <asp:BoundField DataField="centro_costo" HeaderText="C.COSTO" SortExpression="C.COSTO" />
                                        <asp:BoundField DataField="afiliado" HeaderText="NOMBRES Y APELLIDOS" SortExpression="NOMBRES Y APELLIDOS" />
                                        <asp:BoundField DataField="sexo" HeaderText="SEXO" SortExpression="SEXO" />
                                        <asp:BoundField DataField="edad" HeaderText="EDAD" SortExpression="EDAD" />
                                        <asp:BoundField DataField="fch_naci" HeaderText="F.NACIMIENTO" SortExpression="F.NACIMIENTO" />
                                        <asp:BoundField DataField="fch_alta" HeaderText="F.ALTA" SortExpression="F.ALTA" />
                                        <asp:BoundField DataField="fch_baja" HeaderText="F.BAJA" SortExpression="F.BAJA" />
                                        <asp:BoundField DataField="fch_caren" HeaderText="F.CAREN" SortExpression="F.PRO" />
                                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" SortExpression="F.PRO" Visible="false" />
                                        <asp:BoundField DataField="Color" ShowHeader="false" HeaderText="ESTADO" HtmlEncode="false" />
                                        <asp:BoundField DataField="Reporte" HeaderText="SUSALUD" HtmlEncode="false" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <strong>
                                    <asp:Label ID="lblCorrecto" runat="server" class="text-success"></asp:Label></strong>
                                <strong>
                                    <asp:Label ID="lblError" runat="server" class="text-danger"></asp:Label></strong>
                                <strong>
                                    <asp:Label ID="lblDuplicidad" runat="server" class="text-danger"></asp:Label></strong>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="gvGrupoFamiliar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="panel-footer color-blue text-white text-center">
                    <asp:LinkButton ID="lnkCartasGarantia" runat="server" class="btn btn-default btn-sm"
                        OnClick="lnkCartasGarantia_Click"> 
                        <i class="fa fa-print fa-1x text-blue"></i>&nbsp;Generar Cartas de Garantía 
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDependienteNuevo" runat="server" class="btn btn-default btn-sm"
                        OnClick="btnDependienteNuevo_Click">
                        <i class="fa fa-plus-circle fa-1x text-danger"></i> Agregar Dependiente
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar"
                        data-dismiss="modal" aria-hidden="true">
                        <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div id="xLOAD" class="modal fade bs-example-modal-lg centrar" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content panel panel-popup">
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <div class="panel-heading color-blue text-white">
                            <div class="form-inline">
                                <i class="fa fa-file fa-1x"></i>IMPORTAR ARCHIVOS
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="col-md-8">
                                    <asp:FileUpload ID="flpImp" runat="server" class="input-sm text-blue" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnSubir" runat="server" Text="Importar" class="btn btn-primary btn-sm"
                                        OnClick="btnSubir_Click" />
                                    <asp:Button ID="btnSubir2" runat="server" Text="Importar2" class="btn btn-primary btn-sm"
                                        OnClick="btnSubir2_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer color-blue text-white text-center">
                            <button class="btn btn-default btn-sm" data-dismiss="modal" aria-hidden="true">
                                Cerrar</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnSubir" />
                        <asp:PostBackTrigger ControlID="btnSubir2" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="ACTTITU" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-lg">
            <div class="modal-content panel panel-popup">
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div class="panel-heading color-blue text-white">
                            <div class="form-inline">
                                <i class="fa fa-pencil fa-1x"></i>
                                <asp:Label ID="lblTitulo" runat="server" Text="DATOS DE PACIENTE ELEGIDO"></asp:Label>
                                <asp:Label ID="lblIdAfiliado" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <legend class="input-sm">Datos Actuales</legend>
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Tipo Documento:</label>
                                                        <asp:DropDownList ID="ddlTipoDocu" runat="server" class="form-control disabled-button"
                                                            Width="200" ReadOnly="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Nro. Documento:</label>
                                                        <asp:TextBox ID="txtNroDocu" runat="server" class="form-control" placeholder="NRO. DOCUMENTO"
                                                            Width="200" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            E-Mail:</label>
                                                        <asp:TextBox ID="txtEMail" runat="server" class="form-control" placeholder="E-MAIL"
                                                            Width="200" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Telf. Fijo:</label>
                                                        <asp:TextBox ID="txtTelFijo" runat="server" class="form-control" placeholder="TELEFONO FIJO"
                                                            Width="200" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Telf. Movil:</label>
                                                        <asp:TextBox ID="txtTelMovil" runat="server" class="form-control" placeholder="TELEFONO MOVIL"
                                                            Width="200" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <legend class="input-sm">Datos Nuevos</legend>
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Tipo Documento:</label>
                                                        <div class="form-inline">
                                                            <asp:DropDownList ID="ddlTipoDocu2" runat="server" class="form-control disabled-button"
                                                                Width="200" ReadOnly="true">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                            <asp:CheckBox ID="CheckTipoDocu2" class="input-sm" runat="server" Checked="false"
                                                                Text="Actualizar" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Nro. Documento:</label>
                                                        <div class="form-inline">
                                                            <asp:TextBox ID="txtNroDocu2" runat="server" class="form-control" placeholder="NRO. DOCUMENTO"
                                                                Width="200" ReadOnly="true">
                                                            </asp:TextBox>&nbsp;
                                                            <asp:CheckBox ID="CheckNroDocu2" class="input-sm" runat="server" Checked="false"
                                                                Text="&nbsp;Actualizar" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            E-Mail:</label>
                                                        <div class="form-inline">
                                                            <asp:TextBox ID="txtEMail2" runat="server" class="form-control" placeholder="E-MAIL"
                                                                Width="200" ReadOnly="true">
                                                            </asp:TextBox>&nbsp;
                                                            <asp:CheckBox ID="CheckEMail2" class="input-sm" runat="server" Checked="false" Text="&nbsp;Actualizar" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Telf. Fijo:</label>
                                                        <div class="form-inline">
                                                            <asp:TextBox ID="txtTelFijo2" runat="server" class="form-control" placeholder="TELEFONO FIJO"
                                                                Width="200" ReadOnly="true">
                                                            </asp:TextBox>&nbsp;
                                                            <asp:CheckBox ID="CheckTelFijo2" class="input-sm" runat="server" Checked="false"
                                                                Text="&nbsp;Actualizar" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Telf. Movil:</label>
                                                        <div class="form-inline">
                                                            <asp:TextBox ID="txtTelMovil2" runat="server" class="form-control" placeholder="TELEFONO MOVIL"
                                                                Width="200" ReadOnly="true">
                                                            </asp:TextBox>&nbsp;
                                                            <asp:CheckBox ID="CheckTelMovil2" class="input-sm" runat="server" Checked="false"
                                                                Text="&nbsp;Actualizar" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <strong>
                                        <asp:Label ID="lblMsg" runat="server" class="text-blue input-sm"></asp:Label></strong>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer color-blue text-white text-center">
                            <asp:LinkButton ID="LnkActualizar_Modal" runat="server" class="btn btn-default btn-sm"
                                OnClick="LnkActualizar_Modal_Click">
                                <i class="fa fa-search fa-1x text-blue"></i> Actualizar 
                            </asp:LinkButton>
                            <asp:LinkButton ID="LnkCerrar_Modal" runat="server" class="btn btn-default btn-sm"
                                OnClick="LnkCerrar_Modal_Click">
                                <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LnkActualizar_Modal" EventName="Click" />
                        <asp:PostBackTrigger ControlID="LnkCerrar_Modal" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="CARTASMODAL" class="modal fade modal-wide bs-example-modal-lg" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content panel panel-popup">
                <div class="panel-heading color-blue text-white">
                    <div class="form-inline">
                        <i class="fa fa-pencil fa-1x"></i>
                        <asp:Label ID="Label8" runat="server" Text="CARTAS DE GARANTÍA"></asp:Label>
                        <asp:Label ID="Label9" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <script type="text/javascript">

                                $(document).ready(function () {
                                    // Do something exciting
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    //Cargamos nuevamente el calendario
                                    prm.add_endRequest(function () {
                                        $(function () {
                                            $(".chosen-select").chosen({
                                                no_results_text: "No se encontraron resultados",
                                                placeholder_text_single: "Seleccione una opción"
                                            });
                                        });
                                    });
                                });
                            </script>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblVerificacion22" runat="server" class="text-blue"></asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <legend class="input-sm">DATOS DEL SINIESTRO</legend>
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            NRO SINIESTRO:</label>
                                                        <asp:TextBox ID="txtNroSiniestro" runat="server" class="form-control input-sm" placeholder="N°Siniestro"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            PÓLIZA:</label>
                                                        <asp:TextBox ID="txtPoliza" runat="server" class="form-control input-sm" placeholder="Póliza"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            N° CARTA GARANTÍA:</label>
                                                        <asp:TextBox ID="txtCartaGarantia" runat="server" class="form-control input-sm" placeholder="Carta de Garantía"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            TIPO ATENCIÓN:</label>
                                                        <asp:DropDownList ID="ddlTipoAtencion" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            DIAGNÓSTICO:</label>
                                                        <asp:DropDownList ID="ddlDiagnostico" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            MONTO:</label>
                                                        <asp:TextBox ID="txtMonto" runat="server" class="form-control input-sm" placeholder="Monto"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            CLÍNICA:</label>
                                                        <asp:DropDownList ID="ddlClinica" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            TIPO SINIESTRO:</label>
                                                        <asp:DropDownList ID="ddlTipoSiniestro" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            TIPO SINIESTRO:</label>
                                                        <asp:DropDownList ID="ddlCobertura" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <legend class="input-sm"></legend>
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            NOMBRE CONDUCTOR:</label>
                                                        <asp:TextBox ID="txtNombreConductor" runat="server" class="form-control input-sm"
                                                            placeholder="Nombre Conductor"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            APELLIDO PATERNO CONDUCTOR:</label>
                                                        <asp:TextBox ID="txtApellidoPaterno" runat="server" class="form-control input-sm"
                                                            placeholder="Apellido Paterno"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            APELLIDO MATERNO CONDUCTOR:</label>
                                                        <asp:TextBox ID="txtApellidoMaterno" runat="server" class="form-control input-sm"
                                                            placeholder="Apellido Materno"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            PLACA VEHÍCULO:</label>
                                                        <asp:TextBox ID="txtPlaca" runat="server" class="form-control input-sm" placeholder="Placa"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            NOMBRE PERSONA (CONTACTO):</label>
                                                        <asp:TextBox ID="txtPersonaLlamaNombre" runat="server" class="form-control input-sm"
                                                            placeholder="Nombre Persona"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            TELÉFONO PERSONA (CONTACTO):</label>
                                                        <asp:TextBox ID="txtPersonaLlamaTelefono" runat="server" class="form-control input-sm"
                                                            placeholder="Teléfono Persona"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            UBIGEO:</label>
                                                        <asp:DropDownList ID="ddlUBIGEO" runat="server" class="chosen-select" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <legend class="input-sm"></legend>
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            COMISARÍA:</label>
                                                        <asp:TextBox ID="txtComisaria" runat="server" class="form-control input-sm" placeholder="Nombre de Comisaría"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            FECHA SINIESTRO:</label>
                                                        <asp:TextBox ID="txtFechaSiniestro" runat="server" class="form-control input-sm"
                                                            placeholder="FECHA SINIESTRO" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            OCUPANTE:</label>
                                                        <asp:DropDownList ID="ddlOcupante" runat="server" class="chosen-select" Width="100%"
                                                            OnSelectedIndexChanged="ddlFallecido_SelectedIndexChanged">
                                                            <asp:ListItem Value="O" Text="OCUPANTE"></asp:ListItem>
                                                            <asp:ListItem Value="P" Text="PEATÓN"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            FALLECIDO:</label>
                                                        <asp:DropDownList ID="ddlFallecido" runat="server" class="chosen-select" Width="100%"
                                                            OnSelectedIndexChanged="ddlFallecido_SelectedIndexChanged">
                                                            <asp:ListItem Value="N" Text="NO"></asp:ListItem>
                                                            <asp:ListItem Value="S" Text="SI"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm" runat="server" id="fechaFallecido">
                                                        <label class="control-label input-sm text-blue">
                                                            FECHA FALLECIDO:</label>
                                                        <asp:TextBox ID="txtFechaFallecido" runat="server" class="form-control input-sm"
                                                            placeholder="Fecha Fallecido" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr />
                                    </div>
                                </div>
                                <div class="row">
                                    <strong>
                                        <asp:Label ID="Label10" runat="server" class="text-blue input-sm"></asp:Label></strong>
                                </div>
                            </div>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel-footer color-blue text-white text-center">
                    <asp:LinkButton ID="lnkRegistrarCarta" runat="server" class="btn btn-default btn-sm"
                        OnClick="lnkRegistrarCarta_Click">
                                <i class="fa fa-search fa-1x text-blue"></i>&nbsp;REGISTRAR 
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-default btn-sm" OnClick="LinkButton3_Click">
                                <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;CERRAR
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="Scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jasny-bootstrap.min.js"></script>
    <script src="Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="Scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            document.forms[0].target = "_self";
        });        
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
    <script type="text/javascript">
        $(document).ready(function () {

            $(".chosen-select").chosen();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $(".chosen-select").chosen();
            }

        });

    </script>
</body>
</html>
