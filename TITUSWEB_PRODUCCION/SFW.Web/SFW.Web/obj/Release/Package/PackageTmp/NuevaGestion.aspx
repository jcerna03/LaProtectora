<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevaGestion.aspx.cs" Inherits="SFW.Web.NuevaGestion"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
    <link type="text/css" href="Content/fileinput.min.css" rel="stylesheet" />
    <link href="Content/paging.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/FileInput/fileinput.min.js" type="text/javascript"></script>
    <style>
        .resaltar
        {
            background: #10750e;
            border-radius: 7px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div class="panel panel-blue centrar">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfCliente" />
                    <asp:HiddenField runat="server" ID="hfPerfil" />

                    <asp:HiddenField runat="server" ID="hfIdVoip" />



                    <asp:HiddenField ID="hfCod_Cliente" runat="server" />
                    <asp:HiddenField ID="hfCod_Titula" runat="server" />
                    <asp:HiddenField ID="hfCategoria" runat="server" />

                    <asp:HiddenField ID="hfEstado_Titular" runat="server" />

                    <asp:HiddenField ID="hfOrigen" runat="server" />
                    <asp:HiddenField ID="hfEmisor" runat="server" />
                    <asp:HiddenField ID="hfAsegurado" runat="server" />

                    <asp:HiddenField ID="hfTipoModalAfiliado" runat="server" />
                    <asp:HiddenField ID="hfLibro" runat="server" />
                    <asp:HiddenField ID="hfOrdenEmisor" runat="server" />
                    <asp:HiddenField ID="hfOrdenSubGestion" runat="server" />


                    <asp:HiddenField ID="hfIdEmisor" runat="server" />
                    <asp:HiddenField ID="hfDniEmisor" runat="server" />
                    <asp:HiddenField ID="hfNombreEmisor" runat="server" />

                    <asp:HiddenField runat="server" ID="hfIdAsegurado" />
                    <asp:HiddenField ID="hfDniAsegurado" runat="server" />
                    <asp:HiddenField runat="server" ID="hfTipoAsegurado" />
                    <asp:HiddenField runat="server" ID="hfDescripcionAsegurado" />



                    <asp:HiddenField ID="hfIdUsuarioDeriva" runat="server" />
                    <asp:HiddenField ID="hfDerivadoEjecutivo" runat="server" />
                    <asp:HiddenField ID="hfDatosAdicionales" runat="server" />

                    <div class="panel-body">
                        <div class="row">
                            <div runat="server" id="correcto" class="alert alert-success" visible="false">
                                <button type="button" class="close" data-dismiss="alert">
                                    ×</button>
                                <asp:Label ID="lblalerta" runat="server"></asp:Label>
                            </div>
                            <div runat="server" id="orror" class="alert alert-warning" role="alert" visible="false">
                                <button type="button" class="close" data-dismiss="alert">
                                    ×</button>
                                <asp:Label ID="lblErrorReg" runat="server" Text=""></asp:Label>
                            </div>
                            <asp:Label ID="lblError" runat="server" class="text-danger"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <h4>
                                        <label>
                                            <i class="fa fa-phone-square fa-1x text-success"></i>&nbsp;Datos Generales de Gestión</label></h4>
                                </div>
                                <div class="col-md-6 text-right">
                                    <div class="btn-group">
                                        <asp:LinkButton ID="lnkGuardar" runat="server" class="btn btn-default btn-sm" OnClick="lnkGuardar_Click">
                                        <i class="fa fa-save fa-1x text-blue"></i>&nbsp;Guardar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="LnkRegresar" runat="server" class="btn btn-default btn-sm" OnClick="LnkRegresar_Click">
                                        <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;Regresar
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr class="hr-blue" />
                        <div class="row-eq-height">
                            <div class="panel panel-default col-md-2">
                                <div class="panel-heading row">
                                    <asp:Label class="control-label input-sm text-white" runat="server" Text="(*)Origen:">
                                    </asp:Label>
                                    <asp:Label ID="lblOrigen" runat="server" Text="" class="text-white input-sm text-uppercase"></asp:Label>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">

                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvOrigen" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ID,DESCRIP,ORDEN" ShowHeader="False"
                                                OnRowCommand="gvOrigen_RowCommand" OnRowDataBound="gvOrigen_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-4">
                                <div class="panel-heading row">
                                    <asp:Label class="control-label input-sm text-white" ID="lblTituloEmisor" runat="server" Text="(*)Emisor:">
                                    </asp:Label>
                                    <div runat="server" id="divlblEmisor" style="display: none;" class="resaltar">
                                        <asp:Label ID="lblEmisor" runat="server" Text="" class="text-white input-sm text-uppercase"></asp:Label>
                                    </div>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <div class="form-inline">
                                        </div>
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvEmisor" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID,DESCRIP,ORDEN"
                                                ShowHeader="False" OnRowCommand="gvEmisor_RowCommand" OnRowDataBound="gvEmisor_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:BoundField DataField="DESCRIP" HeaderText="" />
                                                    <asp:ButtonField CommandName="BUSCAR" HeaderText="" Text="BUSCAR" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-6">
                                <div class="panel-heading row">
                                    <asp:Label class="control-label input-sm text-white" runat="server" Text="(*)Asegurado / Cliente:">
                                    </asp:Label>
                                    <div runat="server" id="divlblAsegurado" style="display: none;" class="resaltar">
                                        <asp:Label ID="lblAsegurado" runat="server" Text="" class="text-white input-sm text-uppercase"></asp:Label>
                                    </div>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <div class="form-inline">
                                            <!--hidden-->
                                            <asp:HiddenField ID="hfComparacion" runat="server" />

                                            <asp:HiddenField ID="hfComodinCerrar" runat="server" />
                                            <asp:HiddenField ID="hfidEmpresa" runat="server" />
                                            <asp:HiddenField ID="hfidPoliza" runat="server" />
                                            <asp:HiddenField ID="hfidUnidad_Negocio" runat="server" />
                                            <asp:HiddenField ID="hfUnidad_Negocio" runat="server" />
                                            <asp:HiddenField ID="hfpoli" runat="server" />
                                            <asp:HiddenField ID="hfNroPoliza" runat="server" />
                                            <asp:HiddenField ID="hfSiniestroReg" runat="server" />
                                            <!--siniestros-->
                                            <asp:HiddenField ID="hfNombreCliente" runat="server" />
                                            <asp:HiddenField ID="hfBienOAsegurado" runat="server" />
                                            <asp:HiddenField ID="hfServicioSiniestro" runat="server" />
                                            <asp:HiddenField ID="hfIdAseguradoPoliza" runat="server" />
                                            <!--insertar asegurado siniestrado-->
                                            <asp:HiddenField ID="hftxtAsegurado" runat="server" />
                                            <asp:HiddenField ID="hftxtDependiente" runat="server" />
                                            <asp:HiddenField ID="hfddlTipoAsegurado" runat="server" />
                                            <asp:HiddenField ID="hfTipoAseg" runat="server" />
                                            <asp:HiddenField ID="hftxtNombreTitular" runat="server" />
                                            <asp:HiddenField ID="hftxtApellidoTitular" runat="server" />
                                            <asp:HiddenField ID="hftxtTitular" runat="server" />
                                            <asp:HiddenField ID="hftxtNombre" runat="server" />
                                            <asp:HiddenField ID="hftxtApellidos" runat="server" />
                                            <asp:HiddenField ID="hftxtAseguradoNombres" runat="server" />
                                            <asp:HiddenField ID="hftxtClienteContratante" runat="server" />
                                            <!--insertar vehiculo siniestrado-->
                                            <asp:HiddenField ID="hftxtConductor" runat="server" />
                                            <asp:HiddenField ID="hfddlTallerAsignado" runat="server" />
                                            <asp:HiddenField ID="hftxtFechaIngresoTaller" runat="server" />
                                            <asp:HiddenField ID="hftxtFechaInspeccion" runat="server" />
                                            <asp:HiddenField ID="hftxtFechaSalidaTaller" runat="server" />
                                            <asp:HiddenField ID="hftxtMontoPresupuesto" runat="server" />
                                            <asp:HiddenField ID="hftxtMontoRespCivil" runat="server" />
                                            <asp:HiddenField ID="hftxtMontoDañoOcupantes" runat="server" />
                                            <asp:HiddenField ID="hftxtLugarSiniestro2" runat="server" />
                                            <asp:HiddenField ID="hftxtDescripcionSiniestro2" runat="server" />
                                            <asp:HiddenField ID="hfckRespCivil" runat="server" />
                                            <asp:HiddenField ID="hfckPerdidaTotal" runat="server" />
                                            <asp:HiddenField ID="hfckDañoOcupantes" runat="server" />
                                            <asp:HiddenField ID="hfckTodoRiesgo" runat="server" />
                                            <asp:HiddenField ID="hftxtMontoTodoRiesgo" runat="server" />
                                            <asp:HiddenField ID="hftxtContactoTaller" runat="server" />
                                            <!--Envio de Correo-->
                                            <asp:HiddenField ID="hfEjecutivoCuenta" runat="server" />
                                            <asp:HiddenField ID="hfStrinCiaseg" runat="server" />
                                            <!--GuardarSniestro o Siniestro Servicio-->
                                            <asp:HiddenField ID="hfTipoSini" runat="server" />
                                            <asp:HiddenField ID="hfLugarSini" runat="server" />
                                            <asp:HiddenField ID="hfDescripcionSini" runat="server" />
                                            <asp:HiddenField ID="hfEjecutivoSini" runat="server" />
                                        </div>
                                        <div class="centrar" id="generaldiv">
                                            <asp:GridView ID="gvDatosAfiliado" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="false" DataKeyNames="cod_cliente2,cod_titula" OnRowCommand="gvDatosAfiliado_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="cod_cliente" HeaderText="CLIENTE" />
                                                    <asp:BoundField DataField="cod_cliente2" HeaderText="CLIENTE" Visible="false" />
                                                    <asp:BoundField DataField="cod_titula" HeaderText="COD.TITULAR" />
                                                    <asp:BoundField DataField="varcate" HeaderText="CATEGORIA" />
                                                    <asp:BoundField DataField="dni" HeaderText="DNI" />
                                                    <asp:BoundField DataField="afiliado" HeaderText="AFILIADO" />
                                                    <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-users fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-success btn-sm" HeaderText="Grupo Fam." />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:GridView ID="gvClientePrincipal" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="false" DataKeyNames="cod_cliente" OnRowCommand="gvClientePrincipal_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="cod_cliente" HeaderText="CLIENTE" Visible="false" />
                                                    <asp:BoundField DataField="dni" HeaderText="DNI" />
                                                    <asp:BoundField DataField="afiliado" HeaderText="CLIENTE" />
                                                    <asp:BoundField DataField="sexo" HeaderText="SEXO" />
                                                    <asp:BoundField DataField="fch_naci" HeaderText="FECHA NACIMIENTO" />
                                                    <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-files-o fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Pólizas" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:GridView ID="gvAsegurado" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID,DESCRIP"
                                                ShowHeader="False" OnRowCommand="gvAsegurado_RowCommand" OnRowDataBound="gvAsegurado_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:BoundField DataField="DESCRIP" HeaderText="" />
                                                    <asp:ButtonField CommandName="BUSCAR" HeaderText="" Text="BUSCAR" />
                                                </Columns>
                                            </asp:GridView>
                                            <div class="form-inline">
                                                <asp:Label ID="lblPolizaReg" runat="server" class="text-danger input-sm"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <h4>
                                        <label>
                                            <i class="fa fa-list-ul fa-1x text-danger"></i>&nbsp;Detalle de Llamada</label></h4>
                                </div>
                                <div class="col-md-6 text-right">
                                </div>
                            </div>
                        </div>
                        <hr class="hr-blue" />
                        <div class="row-eq-height">
                            <div class="panel panel-default col-md-3">
                                <div class="panel-heading row">
                                    <label class="control-label input-sm text-white">
                                        (*)Tipo Gestion:</label>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <asp:HiddenField ID="hfGestion" runat="server" />
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvGestion" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID"
                                                ShowHeader="False" OnRowCommand="gvGestion_RowCommand" OnRowDataBound="gvGestion_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="form-group-sm">
                                        <label class="control-label input-sm text-blue">
                                            (*)Tipo Sub-Gestion:</label>
                                            <asp:Label ID="lblsubgestion" runat="server" class="input-sm text-uppercase"></asp:Label>
                                        <div runat="server" id="divBuscarReclamo" style="display: none;">
                                            <asp:Panel runat="server" DefaultButton="lnkBuscarReclamo">
                                                <asp:TextBox ID="txtBuscarReclamo" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                <asp:LinkButton ID="lnkBuscarReclamo" runat="server" Style="display: none;" OnClick="lnkBuscarReclamo_Click"> </asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                        <asp:HiddenField ID="hfSubGestion" runat="server" />
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvSubGestion" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="12" DataKeyNames="ID,DESCRIP,ORDEN"
                                                ShowHeader="False" OnRowCommand="gvSubGestion_RowCommand" OnRowDataBound="gvSubGestion_RowDataBound" OnPageIndexChanging="gvSubGestion_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-6">
                                <div class="panel-heading row">
                                    <div class="col-md-6">
                                        <label class="control-label input-sm text-white">
                                            Gestión:</label>
                                    </div>
                                    <div class="col-md-6 text-right">
                                        <div class="form-inline">
                                            <asp:LinkButton ID="lnkAgregar" runat="server" class="btn btn-default btn-sm" OnClick="lnkAgregar_Click">
                                            <i class="fa fa-plus fa-1x text-blue"></i>&nbsp;Agregar Registro
                                            </asp:LinkButton>
                                            <div class="text-right" style="display: inline-block;" runat="server" id="botonesSiniestros" visible="false">
                                                <asp:LinkButton ID="lnkNuevoSiniestro" runat="server" class="btn btn-default btn-sm"
                                                    OnClick="lnkNuevoSiniestro_Click">
                                                <i class="fa fa-plus fa-1x text-danger"></i>&nbsp;Siniestro
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkNuevoSiniestroServicio" runat="server"
                                                    class="btn btn-default btn-sm" OnClick="lnkNuevoSiniestroServicio_Click">
                                                <i class="fa fa-plus fa-1x text-success"></i>&nbsp;S.Servicio
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group-sm">
                                    <label class="text-blue input-sm">
                                        (Contacto)Nombres y Apellidos || Teléfono || Correo</label>
                                    <div class="form-inline">
                                        <asp:TextBox ID="txtContactoNombre" runat="server" CssClass="form-control input-sm"
                                            placeholder="Nombres y Apellidos" Width="35%"></asp:TextBox>
                                        <asp:TextBox ID="txtContactoTelefono" runat="server" CssClass="form-control input-sm"
                                            placeholder="Teléfono" Width="20%"></asp:TextBox>
                                        <asp:TextBox ID="txtContactoCorreo" runat="server" CssClass="form-control input-sm" placeholder="Correo"
                                            Width="20%"></asp:TextBox>
                                        <asp:TextBox ID="txtContactoDni" runat="server" CssClass="form-control input-sm" placeholder="Dni"
                                            Width="20%"></asp:TextBox>
                                    </div>
                                </div>

                                <hr />
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm" id="divRespuestaGeneral" runat="server">

                                        <label class="control-label input-sm text-blue">
                                            Detalle de la gestión realizada:</label>
                                        <asp:TextBox ID="txtRespuestaGeneral" runat="server" class="form-control input-sm"
                                            TextMode="MultiLine" Height="142px" Width="100%" placeholder="Descripción"></asp:TextBox>
                                        <hr />
                                    </div>

                                    <div class="form-group-sm">
                                        <div id="divlblRegistro" runat="server">
                                            <label class="control-label input-sm text-blue">
                                                Registro de casos:</label>
                                        </div>

                                        <div class="table-responsive centrar">
                                            <asp:HiddenField ID="hfModificatemp" runat="server" />
                                            <asp:GridView ID="gvGestionResueltos" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="false" OnRowCommand="gvGestionResueltos_RowCommand" DataKeyNames="NRO,ORIGEN,DESCRIPORIGEN,IDSALIENTE,DESCRIPSALIENTE,DESCRIP_CONTACTO,RESPUESTA,OBS,USU_REG,ARCHIVO" OnRowDataBound="gvGestionResueltos_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Archivo">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="btn btn-success btn-sm" ID="lnkArchivo" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="ARCHIVO">
                                                                <i class="fa fa-file-o fa-1x text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:ButtonField CommandName="ACTUALIZAR" ButtonType="Link" Text='<i class="fa fa-edit fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Actualizar" />
                                                    <asp:ButtonField Visible="false" CommandName="ELIMINAR" ButtonType="Link" Text='<i class="fa fa-minus fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-danger btn-sm" HeaderText="Eliminar" />
                                                    <asp:TemplateField HeaderText="#">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="NRO" HeaderText="NRO" Visible="false" />
                                                    <asp:BoundField DataField="ORIGEN" HeaderText="ORIGEN" Visible="false" />
                                                    <asp:BoundField DataField="DESCRIPORIGEN" HeaderText="ORIGEN" />
                                                    <asp:BoundField DataField="IDSALIENTE" HeaderText="IDSALIENTE" Visible="false" />
                                                    <asp:BoundField DataField="DESCRIPSALIENTE" HeaderText="SALIENTE" />
                                                    <asp:BoundField DataField="DESCRIP_CONTACTO" HeaderText="CONTACTO" />
                                                    <asp:TemplateField HeaderText="RESPT.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblresp" runat="server" Text='<%# Eval("RESPUESTA") %>'>
                                                              
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OBS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblobs" runat="server" Text='<%# Eval("OBS") %>'>
                                                             
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="USU_REG" HeaderText="USUREG" Visible="false" />
                                                    <asp:BoundField DataField="FEC_REG" HeaderText="FECREG" DataFormatString="{0:dd/MM/yyyy}" />

                                                </Columns>
                                            </asp:GridView>
                                            <asp:GridView ID="gvGestionResueltos2" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="false" DataKeyNames="ID,NRO,ORIGEN,DESCRIPORIGEN,IDSALIENTE,DESCRIPSALIENTE,DESCRIP_CONTACTO,RESPUESTA,OBS,USU_REG,ARCHIVO"
                                                OnRowCommand="gvGestionResueltos2_RowCommand" OnRowDataBound="gvGestionResueltos2_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Archivo">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="btn btn-success btn-sm" ID="lnkArchivo" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="ARCHIVO">
                                                                <i class="fa fa-file-o fa-1x text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:ButtonField CommandName="ACTUALIZAR" ButtonType="Link" Text='<i class="fa fa-edit fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Actualizar" />
                                                    <asp:ButtonField Visible="false" CommandName="ELIMINAR" ButtonType="Link" Text='<i class="fa fa-minus fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-danger btn-sm" HeaderText="Eliminar" />
                                                    <asp:TemplateField HeaderText="#">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="NRO" HeaderText="NRO" Visible="false" />
                                                    <asp:BoundField DataField="ORIGEN" HeaderText="ORIGEN" Visible="false" />
                                                    <asp:BoundField DataField="DESCRIPORIGEN" HeaderText="ORIGEN" />
                                                    <asp:BoundField DataField="IDSALIENTE" HeaderText="IDSALIENTE" Visible="false" />
                                                    <asp:BoundField DataField="DESCRIPSALIENTE" HeaderText="SALIENTE" />
                                                    <asp:BoundField DataField="DESCRIP_CONTACTO" HeaderText="CONTACTO" />
                                                    <asp:TemplateField HeaderText="RESPT.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblresp" runat="server" Text='<%# Eval("RESPUESTA") %>'>
                                                              
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OBS">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblobs" runat="server" Text='<%# Eval("OBS") %>'>
                                                             
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="USU_REG" HeaderText="USUREG" />
                                                    <asp:BoundField DataField="FEC_REG" HeaderText="FECREG" DataFormatString="{0:dd/MM/yyyy}" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-3">
                                <div class="panel-heading row">
                                    <div class="col-md-3">
                                        <label class="control-label input-sm text-white">
                                            Estado:</label>
                                    </div>
                                    <div class="col-md-9 text-right" runat="server" id="divGuardar2">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-default btn-sm" OnClick="lnkGuardar_Click">
                                            <i class="fa fa-save fa-1x text-blue"></i>&nbsp;Guardar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton6" runat="server" class="btn btn-default btn-sm" OnClick="LnkRegresar_Click">
                                            <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;Regresar
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <asp:HiddenField ID="hfEstado" runat="server" />
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvEstado" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID,DESCRIP"
                                                ShowHeader="False" OnRowCommand="gvEstado_RowCommand" OnRowDataBound="gvEstado_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div runat="server" id="divUsuDeriva" class="form-group-sm" style="display: none;">
                                        <label>Derivado a: </label>
                                        &nbsp;     
                                        <asp:Label runat="server" ID="lblUsuaDeriva"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <h4>
                                        <label>
                                            <i class="fa fa-user fa-1x text-warning"></i>&nbsp;Datos del Asegurado / Cliente</label></h4>
                                </div>
                                <div class="col-md-6 text-right">
                                </div>
                            </div>
                        </div>
                        <hr class="hr-blue" />
                        <div class="row-eq-height">
                            <div class="panel panel-default col-md-4">
                                <div class="panel-heading row">
                                    <label class="control-label input-sm text-white">
                                        Datos Generales:</label>

                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <asp:HiddenField ID="hfidDatos" runat="server" />
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvGeneral" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID"
                                                ShowHeader="False" OnRowCommand="gvGeneral_RowCommand" OnRowDataBound="gvGeneral_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:LinkButton ID="lnkEditarAsegurado" runat="server" class="btn btn-default btn-sm"
                                                OnClick="lnkEditarAsegurado_Click" Visible="false">
                                            <i class="fa fa-edit fa-1x text-blue"></i>&nbsp;Editar Asegurado 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-4">
                                <div class="panel-heading row">
                                    <label class="control-label input-sm text-white">
                                        Documentos:</label>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group-sm">
                                        <asp:HiddenField ID="hfidDocum" runat="server" />
                                        <div class="centrar">
                                            <asp:GridView ID="gvDocumentos" runat="server" class="table table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" DataKeyNames="ID,RUTA"
                                                ShowHeader="False" OnRowCommand="gvDocumentos_RowCommand" OnRowDataBound="gvDocumentos_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label ID="lblDocumentos" runat="server" class="text-danger input-sm"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default col-md-4">
                                <div class="panel-heading row">
                                    <div class="col-md-4">
                                        <label class="control-label input-sm text-white">
                                            Avisos:</label>
                                    </div>
                                    <div class="col-md-8 text-right" runat="server" id="divGuardar3">
                                        <div class="btn-group">
                                            <asp:LinkButton ID="LinkButton7" runat="server" class="btn btn-default btn-sm" OnClick="lnkGuardar_Click">
                                            <i class="fa fa-save fa-1x text-blue"></i>&nbsp;Guardar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton8" runat="server" class="btn btn-default btn-sm" OnClick="LnkRegresar_Click">
                                            <i class="fa fa-sign-out fa-1x text-blue"></i>&nbsp;Regresar
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <asp:Label ID="lblNohayAvisos1" runat="server" class="text-danger input-sm"></asp:Label>
                                    <asp:Label ID="lblnoHayObs" runat="server" class="text-danger input-sm"></asp:Label>
                                    <asp:Label ID="lblNoHayDNI" runat="server" class="text-danger input-sm"></asp:Label>
                                    <asp:Label ID="lblFechaNacimiento" runat="server" class="text-danger input-sm"></asp:Label>
                                    <asp:HiddenField ID="hfidAvisos" runat="server" />
                                    <div class="form-group-sm" runat="server" visible="false" id="divAvisos">
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvAvisos" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                AutoGenerateColumns="false" ShowHeader="false">
                                                <Columns>
                                                    <asp:BoundField DataField="Cod Aviso" HeaderText="Cod Aviso" Visible="false" />
                                                    <asp:BoundField DataField="Aviso" HeaderText="Aviso" ItemStyle-ForeColor="Red" />
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" Visible="false" />
                                                    <asp:BoundField DataField="op" HeaderText="op" Visible="false" />
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="false" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="form-group-sm" runat="server" visible="false" id="divObs">
                                        <label class="control-label input-sm text-blue">
                                            Observaciones SUSALUD:</label>
                                        <asp:GridView ID="gvOBSusalud" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                            AutoGenerateColumns="false" ShowHeader="false">
                                            <Columns>
                                                <asp:BoundField DataField="COD_CLIENTE" HeaderText="COD" Visible="false" />
                                                <asp:BoundField DataField="COD_TITULA" HeaderText="CLIENTE" />
                                                <asp:BoundField DataField="CATEGORIA" HeaderText="ERROR" Visible="false" />
                                                <asp:BoundField DataField="COD_ERROR" HeaderText="COD_ERROR" Visible="false" />
                                                <asp:BoundField DataField="DESCRIP_ERROR" HeaderText="OBSERVACION" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="form-group-sm">
                                        <asp:LinkButton ID="lnkEditarAsegura2" runat="server" class="btn btn-default btn-sm"
                                            OnClick="lnkEditarAsegurado_Click" Visible="false">
                                        <i class="fa fa-edit fa-1x text-blue"></i>&nbsp;Editar Asegurado 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvEmisor" EventName="RowCommand" />
                    <asp:PostBackTrigger ControlID="gvGeneral" />
                    <asp:PostBackTrigger ControlID="gvDocumentos" />
                    <asp:PostBackTrigger ControlID="lnkEditarAsegurado" />
                    <asp:PostBackTrigger ControlID="lnkEditarAsegura2" />
                    <asp:AsyncPostBackTrigger ControlID="gvEstado" EventName="RowCommand" />
                    <asp:PostBackTrigger ControlID="gvClientePrincipal" />
                    <asp:AsyncPostBackTrigger ControlID="lnkNuevoSiniestro" EventName="Click" />
                    <asp:PostBackTrigger ControlID="lnkGuardarResuelto" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/image/loading1.gif"
                            AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div id="CORREO" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <div class="panel-heading color-blue">
                        <h5 class="text-center text-white">
                            <label runat="server" id="Label9" class="text-white">
                                DERIVAR</label></h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                        <ContentTemplate>
                            <div class="panel-body">
                                <asp:HiddenField ID="hfenviarcorreo" runat="server" />
                                <div class="row">
                                    <asp:Label ID="lblCorreoError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                    <div class="col-md-8 col-md-offset-2">
                                        <div class="form-group-sm">
                                            <label class="control-label input-sm text-blue">
                                                Ejecutivo:</label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtEjecutivo01" runat="server" class="form-control" Width="70%" ReadOnly="true"></asp:TextBox>
                                                <asp:LinkButton ID="lnkBuscarejecutivo" runat="server" class="btn btn-default btn-sm"
                                                    OnClick="lnkBuscarejecutivo_Click">
                                                    <i class="fa fa-search fa-1x text-blue"></i>&nbsp;Buscar Ejecutivo 
                                                </asp:LinkButton>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtEjecutivo01" Display="Dynamic" ValidationGroup="vgDerivarCorreo"
                                                    Text="Seleccione Ejecutivo.*" ValidateEmptyText="true" SetFocusOnError="true" />


                                            </div>
                                        </div>
                                        <div class="form-group-sm">
                                            <asp:CheckBox CssClass="form-control col-md-8" Text="Enviar Correo" runat="server" ID="chkCorreo" OnCheckedChanged="chkCorreo_CheckedChanged" Checked="false" AutoPostBack="true" />
                                            <div id="divCorreo" runat="server" style="display: none;">

                                                <div class="form-group-sm">
                                                    <label class="control-label input-sm text-blue">
                                                        Correo:</label>
                                                    <asp:TextBox ID="txtEmailCliente" runat="server" class="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEmailCliente" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtEmailCliente" Display="Dynamic" ValidationGroup="vgDerivarCorreo"
                                                    Text="Ingrese Correo.*" ValidateEmptyText="true" SetFocusOnError="true" Enabled="false" />

                                                     </div>
                                                <div class="form-group-sm">
                                                    <label class="control-label input-sm text-blue">
                                                        Asunto:</label>
                                                    <asp:TextBox ID="txtAsunto" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="form-group-sm">
                                                    <label class="control-label input-sm text-blue">
                                                        Mensaje:</label>
                                                    <asp:TextBox ID="txtBody" runat="server" class="form-control input-sm" TextMode="MultiLine"
                                                        Rows="30" Height="100px"></asp:TextBox>
                                                </div>
                                                <div class="form-group-sm">
                                                    <label class="control-label input-sm text-blue">
                                                        <i class="fa fa-upload fa-1x text-blue"></i>&nbsp;Datos Adjuntos</label>
                                                    <input id="file9" name="file9" runat="server" type="file" class="file" multiple="true"
                                                        data-preview-file-type="any" />
                                                    <%--<input id="input-id" type="file" class="file" data-preview-file-type="text" >--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-right">
                                <asp:LinkButton ID="lnkEnviarCorreo" runat="server" Visible="true" class="btn btn-default btn-sm"
                                    OnClick="lnkEnviarCorreo_Click" ValidationGroup="vgDerivarCorreo">
                                    <i class="fa fa-envelope-square fa-1x text-blue"></i>&nbsp;Derivar 
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkCerrarModal" runat="server" class="btn btn-default btn-sm"
                                    data-dismiss="modal" aria-hidden="true">
                                    <i class="fa fa-close fa-1x text-blue"></i>&nbsp;Cerrar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkEnviarCorreo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="Proveedor" class="modal fade bs-example-modal-lg centrar" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="panel-heading color-blue">
                        <h3 class="text-center text-white">
                            <asp:Label ID="lblS_Titulo" runat="server" Text="Busqueda Proveedor"></asp:Label></h3>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-12 col-md-offset-0">
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnBusquedaPro">
                                                <div class="form-inline">
                                                    <label class="control-label input-sm text-blue">
                                                        Contacto:</label>
                                                    <asp:TextBox ID="txtContactoProveedor" runat="server" class="form-control input-sm"
                                                        placeholder="Ingresar contacto">
                                                    </asp:TextBox>
                                                    <asp:Label ID="lblContactoProveedor" runat="server" class="control-label input-sm text-danger"></asp:Label>

                                                    <label class="control-label input-sm text-blue">
                                                        Dni:</label>
                                                    <asp:TextBox ID="txtDniProveedor" runat="server" class="form-control input-sm"
                                                        placeholder="Ingresar dni">
                                                    </asp:TextBox>

                                                </div>
                                                <hr />
                                                <div class="form-inline">
                                                    <asp:TextBox ID="txtBusquedaPro" runat="server" class="form-control input-sm" placeholder="Buscar"
                                                        TabIndex="1">
                                                    </asp:TextBox>
                                                    <asp:LinkButton ID="btnBusquedaPro" runat="server" class="btn btn-default btn-sm"
                                                        ToolTip="Buscar" OnClick="btnBusquedaPro_Click">
                                                        <i class="fa fa-check-circle fa-1x text-blue"></i> Buscar
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive centrar">
                                                <asp:GridView ID="gvProveedor" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                    AutoGenerateColumns="True" AllowPaging="True" PageSize="10" DataKeyNames="idclinica,clinica"
                                                    AutoGenerateSelectButton="true" OnRowDataBound="gvProveedor_RowDataBound"
                                                    OnSelectedIndexChanged="gvProveedor_SelectedIndexChanged" OnPageIndexChanging="gvProveedor_PageIndexChanging">
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="LinkButton5" runat="server" class="btn btn-default btn-sm" data-dismiss="modal"
                                        aria-hidden="true">
                                        <i class="fa fa-close fa-1x text-danger"></i> Cerrar
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvProveedor" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div id="Ejecutivo" class="modal fade bs-example-modal-lg centrar" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="panel-heading color-blue">
                        <h3 class="text-center text-white">
                            <asp:Label ID="Label1" runat="server" Text="Busqueda Ejecutivo"></asp:Label></h3>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-12 col-md-offset-0">
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtEjecutivo" runat="server" class="form-control input-sm" placeholder="Buscar"
                                                    TabIndex="1"></asp:TextBox>
                                                <asp:LinkButton ID="lnkEjecutivo" runat="server" class="btn btn-default btn-sm" ToolTip="Buscar"
                                                    OnClick="lnkEjecutivo_Click">
                                                    <i class="fa fa-check-circle fa-1x text-blue"></i> Buscar
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive centrar">
                                                <asp:GridView ID="gvEjecutivo" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                    AutoGenerateColumns="True" AllowPaging="True" PageSize="10" DataKeyNames="idusu,nombre"
                                                    AutoGenerateSelectButton="true" OnRowDataBound="gvEjecutivo_RowDataBound" OnPageIndexChanging="gvEjecutivo_PageIndexChanging"
                                                    OnSelectedIndexChanged="gvEjecutivo_SelectedIndexChanged">
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="LinkButton4" runat="server" class="btn btn-default btn-sm" data-dismiss="modal"
                                        aria-hidden="true">
                                        <i class="fa fa-close fa-1x text-danger"></i> Cerrar
                                    </asp:LinkButton>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvEjecutivo" EventName="PageIndexChanging" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div id="Afiliado" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-wide modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="panel-heading color-blue row-eq-height">
                                <div class="col-md-6">
                                    <div class="form-inline">
                                        <h3 class="text-center text-white" style="display: inline-block;">
                                            <asp:Label ID="Label14" runat="server" Text="Busqueda Afiliado"></asp:Label></h3>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-inline">
                                        <br />
                                        <asp:LinkButton ID="LinkButton14" runat="server" class="pull-right" data-dismiss="modal"
                                            aria-hidden="true">
                                        <i class="fa fa-times fa-2x text-white"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-inline">
                                            <asp:DropDownList ID="ddlTablas" runat="server" class="form-control input-sm" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTablas_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtBusquedaAfiliado" runat="server" class="form-control input-sm" placeholder="Buscar"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:LinkButton ID="lnkBuscarAfiliado" runat="server" class="btn btn-default btn-sm" ToolTip="Buscar"
                                                OnClick="lnkBuscarAfiliado_Click">
                                                <i class="fa fa-check-circle fa-1x text-blue"></i> Buscar
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row" runat="server" id="afiliados">
                                    <div class="col-md-12">
                                        <div class="table-responsive centrar">
                                            <asp:GridView ID="gvAfiliado" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                AllowPaging="true" PageSize="10" OnRowCommand="gvAfiliado_RowCommand" AutoGenerateColumns="false"
                                                DataKeyNames="cod_cliente,cod_titula,categoria,afiliado,dni,fch_naci,idempresa,idUnidad_Negocio,UNIDAD_NEGOCIO,estado_titular"
                                                OnPageIndexChanging="gvAfiliado_PageIndexChanging">
                                                <PagerStyle CssClass="pagination-ys" />
                                                <Columns>
                                                    <asp:ButtonField CommandName="SELECCIONAR" ButtonType="Link" Text='<i class="fa fa-arrow-circle-right fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-warning btn-sm" HeaderText="Selecc." />
                                                    <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-info fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-success btn-sm" HeaderText="Datos" />
                                                    <asp:BoundField DataField="cod_cliente" HeaderText="#.POLIZA" Visible="false" />
                                                    <asp:BoundField DataField="cod_cliente2" HeaderText="EMPRESA" />
                                                    <asp:BoundField DataField="cod_titula" HeaderText="COD.CLIENTE" />
                                                    <asp:BoundField DataField="dni" HeaderText="DNI" />
                                                    <asp:BoundField DataField="categoria1" HeaderText="PARENTESCO" />
                                                    <asp:BoundField DataField="categoria" HeaderText="CATEGORIA" Visible="false" />
                                                    <asp:BoundField DataField="plan" HeaderText="PLAN" Visible="false" />
                                                    <asp:BoundField DataField="cent_costo" HeaderText="C.COSTO" Visible="false" />
                                                    <asp:BoundField DataField="afiliado" HeaderText="NOMBRES Y APELLIDOS" />
                                                    <asp:BoundField DataField="sexo" HeaderText="SEXO" />
                                                    <asp:BoundField DataField="edad" HeaderText="EDAD" SortExpression="EDAD" />
                                                    <asp:BoundField DataField="fch_naci" HeaderText="F.NACIMIENTO" />
                                                    <asp:BoundField DataField="fch_alta" HeaderText="F.ALTA" />
                                                    <asp:BoundField DataField="fch_baja" HeaderText="F.BAJA" />
                                                    <asp:BoundField DataField="fch_caren" HeaderText="F.CAREN" Visible="false" />
                                                    <asp:BoundField DataField="PLAN|C.COSTO|F.CAREN" HeaderText="Datos Adicionales" />
                                                    <asp:BoundField DataField="Color" HeaderText="ESTADO" HtmlEncode="false" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkBuscarAfiliado" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTablas" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="PageIndexChanging" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="PolizasCliente" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-wide modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="panel-heading color-blue row-eq-height">
                                <div class="col-md-6">
                                    <div class="form-inline">
                                        <h3 class="text-center text-white" style="display: inline-block;">
                                            <asp:Label ID="Label11" runat="server" Text="Pólizas de Cliente"></asp:Label></h3>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-inline">
                                        <br />
                                        <asp:LinkButton ID="LinkButton13" runat="server" class="pull-right" data-dismiss="modal"
                                            aria-hidden="true">
                                        <i class="fa fa-times fa-2x text-white"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive centrar">
                                            <div id="mydiv">
                                                <label id="lblmensajeFade" class="text-success">
                                                </label>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="form-inline">
                                                    <label>
                                                        Búsqueda de Pólizas :</label>
                                                    <asp:TextBox ID="txtBuscadorPoliza" runat="server" class="form-control input-sm"
                                                        placeholder="Buscar..."></asp:TextBox>
                                                    <asp:DropDownList ID="ddlEstadoPoliza" runat="server" class="form-control input-sm">
                                                    </asp:DropDownList>
                                                    <div class="btn-group small">
                                                        <asp:LinkButton ID="lnkBuscaPoliza" runat="server" class="btn btn-default btn-sm"
                                                            OnClick="lnkBuscaPoliza_Click">
                                                        <i class="fa fa-search fa-1x text-success"></i>&nbsp;Buscar
                                                        </asp:LinkButton>
                                                    </div>
                                                    <asp:LinkButton ID="lnkSinAsignar" runat="server" class="btn btn-default btn-sm pull-right"
                                                        OnClick="lnkSinAsignar_Click">
                                                    <i class="fa fa-check fa-1x text-danger"></i>&nbsp;Sin Asignar
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <asp:Label ID="lblPolizasCliente" runat="server" CssClass="text-danger input-sm"></asp:Label>
                                            <asp:GridView ID="gvPolizasCliente" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                AllowPaging="true" PageSize="10" AutoGenerateColumns="false" OnPageIndexChanging="gvPolizasCliente_PageIndexChanging"
                                                OnRowCommand="gvPolizasCliente_RowCommand" DataKeyNames="clientecontratante,idpoliza,NroPoliza,Estado,RUTA,aseguradora">
                                                <PagerStyle CssClass="pagination-ys" />
                                                <Columns>
                                                    <asp:ButtonField CommandName="Detalle" ButtonType="Link" Text='<i class="fa fa-info-circle fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-danger btn-sm" HeaderText="Detalle" />
                                                    <asp:ButtonField CommandName="Seleccionar" ButtonType="Link" Text='<i class="fa fa-long-arrow-right fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-primary btn-sm" HeaderText="Seleccionar" />
                                                    <asp:BoundField DataField="clientecontratante" HeaderText="cliente" Visible="false" />
                                                    <asp:BoundField DataField="idpoliza" HeaderText="" Visible="false" />
                                                    <asp:BoundField DataField="ciaSeg" HeaderText="" Visible="false" />
                                                    <asp:BoundField DataField="aseguradora" HeaderText="ASEG" />
                                                    <asp:BoundField DataField="NroPoliza" HeaderText="Nº POLIZA" />
                                                    <asp:BoundField DataField="FECVIG" HeaderText="FECHA DE VIGENCIA" />
                                                    <asp:BoundField DataField="Riesgo" HeaderText="RIESGO" />
                                                    <asp:BoundField DataField="UnidadNegocio_cli" HeaderText="UNI.NEG.CLI" Visible="false" />
                                                    <asp:BoundField DataField="UNIDAD_NEGOCIO" HeaderText="UNIDAD DE NEGOCIO" />
                                                    <asp:BoundField DataField="FUNCIONARIO" HeaderText="EJECUTIVO DE CUENTA" />
                                                    <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                                                    <asp:BoundField DataField="PRIMANETA" HeaderText="PRIMA NETA" />
                                                    <asp:BoundField DataField="PRIMATOTAL" HeaderText="PRIMA TOTAL" />
                                                    <asp:BoundField DataField="SINIESTROS" HeaderText="N°SINIESTROS" />
                                                    <asp:BoundField DataField="RUTA" Visible="false" />
                                                    <asp:BoundField DataField="VIGINI" Visible="false" />
                                                    <asp:BoundField DataField="VIGFIN" Visible="false" />
                                                    <asp:BoundField DataField="VIGTIPO" Visible="false" />
                                                    <asp:BoundField DataField="ACCESO_WEB" HeaderText="WEB" />
                                                    <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" HtmlEncode="false" />
                                                    <asp:BoundField DataField="PDF" HeaderText="PDF" HtmlEncode="false" />
                                                    <asp:ButtonField CommandName="estadoCuenta" ButtonType="Link" Text='<i class="fa fa-dollar fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-success btn-sm" HeaderText="Est.Cuenta" Visible="False" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="detapoli" visible="false">
                                    <div class="col-md-12">
                                        <strong><i class="fa fa-file-pdf-o fa-1x text-success"></i>&nbsp;DETALLE POLIZA</strong></h5><hr
                                            class="hr-success" />
                                        <ul class="nav nav-tabs">
                                            <li class="active success"><a href="#detalle_poliza" data-toggle="tab"><i class="fa fa-list-ul fa-1x text-danger"></i>&nbsp;Detalle</a></li>
                                            <li id="aseguradosgrilla" runat="server"><a href="#asegurados_poliza" data-toggle="tab">
                                                <i class="fa fa-users fa-1x text-warning"></i>&nbsp;Asegurados</a></li>
                                            <li id="vehiculogrilla" runat="server"><a href="#vehiculos_poliza" data-toggle="tab">
                                                <i class="fa fa-users fa-1x text-warning"></i>&nbsp;Vehículos</a></li>
                                            <li id="inmueblegrilla" runat="server"><a href="#tab_inmuebles" data-toggle="tab"><i
                                                class="fa fa-users fa-1x text-warning"></i>&nbsp;Inmuebles</a></li>
                                            <li><a href="#endosos_polizas" data-toggle="tab"><i class="fa fa-file-archive-o fa-1x text-gris"></i>&nbsp;Endosos</a></li>
                                            <li><a href="#siniestros_poliza" data-toggle="tab"><i class="fa fa-bomb fa-1x text-blue"></i>&nbsp;Siniestros </a></li>
                                            <li id="li_SeguimientoPoli" runat="server" visible="True"><a href="#seguimiento_poliza"
                                                data-toggle="tab"><i class="fa fa-arrow-circle-right fa-1x text-warning"></i>&nbsp;Seguimiento</a></li>
                                            <li id="li_DocumentoPoli" runat="server" visible="True"><a href="#documentos_poliza"
                                                data-toggle="tab"><i class="fa fa-file-text-o fa-1x text-danger"></i>&nbsp;Documentos</a></li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="detalle_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row table-responsive">
                                                                <asp:GridView ID="gvDetallePoliza" runat="server" class="table table-condensed table-bordered  input-sm"
                                                                    ShowHeader="false" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="COL01" />
                                                                        <asp:BoundField DataField="COL02" />
                                                                        <asp:BoundField DataField="COL03" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="COL04" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="COL05" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="COL06" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="COL07" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="COL08" HtmlEncode="false" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div class="row text-center">
                                                                <asp:Label ID="lblDetallePoliza" runat="server" class="text-danger input-sm"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="asegurados_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtAsegurados" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkAseg" runat="server" class="btn btn-default btn-sm" OnClick="lnkAseg_Click">
                                                                    <i class="fa fa-search fa-1x text-primary"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lblAseguradoPolizas" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gvAsegurados" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    DataKeyNames="idAseguradoPoliza" AllowPaging="True" PageSize="5" AutoGenerateColumns="False"
                                                                    OnPageIndexChanging="gvAsegurados_PageIndexChanging">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idAseguradoPoliza" HeaderText="CODIGO" Visible="FALSE" />
                                                                        <asp:BoundField DataField="codigo" HeaderText="NRO" />
                                                                        <asp:BoundField DataField="dependiente" HeaderText="TIPO DEPENDIENTE" />
                                                                        <asp:BoundField DataField="Titular" HeaderText="TITULAR" />
                                                                        <asp:BoundField DataField="Asegurado" HeaderText="ASEGURADO" />
                                                                        <asp:BoundField DataField="documento" HeaderText="DOCUMENTO" />
                                                                        <asp:BoundField DataField="Sexo" HeaderText="SEXO" />
                                                                        <asp:BoundField DataField="FechaNacimiento" HeaderText="FECHA NACIMIENTO" />
                                                                        <asp:BoundField DataField="FechaInclusion" HeaderText="FECHA INCLUSION" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkAseg" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gvAsegurados" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="vehiculos_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtVehiculos" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkVehAseg" runat="server" class="btn btn-default btn-sm" OnClick="lnkVehAseg_Click">
                                                                <i class="fa fa-search fa-1x text-success"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lblVehiculoAsegurado" runat="server" class="input-sm text-danger"></asp:Label><asp:HiddenField
                                                                            ID="hfVehiculoAsegurado" runat="server" />
                                                                    </strong><strong>
                                                                        <asp:Label ID="lblVehiculosPoliza" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gv_Vehiculos" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    DataKeyNames="idVehAseg" AutoGenerateColumns="false" AllowPaging="True" PageSize="5"
                                                                    OnPageIndexChanging="gv_Vehiculos_PageIndexChanging">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idVehAseg" HeaderText="DOC. PRIMA" Visible="False" />
                                                                        <asp:BoundField DataField="CodigoInciso" HeaderText="NRO" />
                                                                        <asp:BoundField DataField="ClienteContratante" HeaderText="CLIENTE" />
                                                                        <asp:BoundField DataField="IncisoCiaSeg" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="Clase" HeaderText="CLASE" />
                                                                        <asp:BoundField DataField="Marca" HeaderText="MARCA" />
                                                                        <asp:BoundField DataField="Modelo" HeaderText="MODELO" />
                                                                        <asp:BoundField DataField="Placa" HeaderText="PLACA" />
                                                                        <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkVehAseg" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gv_Vehiculos" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="tab_inmuebles">
                                                <asp:UpdatePanel ID="UpdatePanel47" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtFiltroInmueble" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkBuscarInmueble" runat="server" class="btn btn-default btn-sm"
                                                                            OnClick="lnkBuscarInmueble_Click">
                                                                                <i class="fa fa-search fa-1x text-success"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lblInmuebleGrilla" runat="server" class="input-sm text-danger"></asp:Label><asp:HiddenField
                                                                            ID="HiddenField6" runat="server" />
                                                                    </strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gvInmuebles" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvInmuebles_PageIndexChanging">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idInmueble" HeaderText="DOC. PRIMA" Visible="False" />
                                                                        <asp:BoundField DataField="codigoItem" HeaderText="NRO" />
                                                                        <asp:BoundField DataField="Direccion" HeaderText="DIRECCION" />
                                                                        <asp:BoundField DataField="Depa" HeaderText="DEPARTAMENTO" />
                                                                        <asp:BoundField DataField="Provi" HeaderText="PROVINCIA" />
                                                                        <asp:BoundField DataField="Distri" HeaderText="DISTRITO" />
                                                                        <asp:BoundField DataField="anioConstruccion" HeaderText="AÑO CONTRUCCION" />
                                                                        <asp:BoundField DataField="Uso" HeaderText="USO" />
                                                                        <asp:BoundField DataField="tipoEstructura" HeaderText="TIPO ESTRUCTURA" />
                                                                        <asp:BoundField DataField="numPisos" HeaderText="#PISOS" />
                                                                        <asp:BoundField DataField="numSotanos" HeaderText="#SOTANOS" />
                                                                        <asp:BoundField DataField="edificioCosto" HeaderText="COSTO EDIF" />
                                                                        <asp:BoundField DataField="enseres" HeaderText="ENSERES" />
                                                                        <asp:BoundField DataField="maquinaria" HeaderText="MAQUINARIA" />
                                                                        <asp:BoundField DataField="equipoElectronico" HeaderText="EQ.ELECTRONICO" />
                                                                        <asp:BoundField DataField="total" HeaderText="TOTAL" />
                                                                        <asp:BoundField DataField="fechaInclusion" HeaderText="F.INCLUSION" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="fechaExclusion" HeaderText="F.EXCLUSION" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkBuscarInmueble" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gvInmuebles" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="endosos_polizas">
                                                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtEndosos" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkEndosos" runat="server" class="btn btn-default btn-sm" OnClick="lnkEndosos_Click">
                                                                    <i class="fa fa-search fa-1x text-blue"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lbl_Endosos" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gv_Endosos" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    DataKeyNames="idEndoso" AutoGenerateColumns="false" AllowPaging="True" PageSize="5"
                                                                    OnPageIndexChanging="gv_Endosos_PageIndexChanging">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idEndoso" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="NroEndoso" HeaderText="NRO.ENDOSO" />
                                                                        <asp:BoundField DataField="TIPO_ANEXO" HeaderText="TIPO ANEXO" />
                                                                        <asp:BoundField DataField="TIPO_ENDOSO" HeaderText="TIPO ENDOSO" />
                                                                        <asp:BoundField DataField="NRO_LIQ" HeaderText="NRO.LIQ" />
                                                                        <asp:BoundField DataField="DocumentoPrima" HeaderText="NRO.PRIMA" Visible="False" />
                                                                        <asp:BoundField DataField="SumaAsegurada" HeaderText="S.ASEGURADA" Visible="False" />
                                                                        <asp:BoundField DataField="PrimaNeta" HeaderText="PRIMA NETA" />
                                                                        <asp:BoundField DataField="PrimaTotal" HeaderText="PRIMA TOTAL" />
                                                                        <asp:BoundField DataField="vigencia" HeaderText="VIGENCIA" />
                                                                        <asp:BoundField DataField="MemoSlip" HeaderText="MATERIA ASEGURADA" />
                                                                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" />
                                                                        <asp:BoundField DataField="USUREG" HeaderText="REG POR" />
                                                                        <asp:BoundField DataField="USUMOD" HeaderText="MOD POR" />
                                                                        <asp:BoundField DataField="web" HeaderText="ACCESO WEB" />
                                                                        <asp:BoundField DataField="Archivo" HeaderText="DOC. ENDOSO" HtmlEncode="false" />
                                                                        <asp:BoundField DataField="cartaEnvio" HeaderText="REMESA" HtmlEncode="False" />
                                                                        <asp:BoundField DataField="cargo" HeaderText="CARGO" HtmlEncode="False" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkEndosos" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gv_Endosos" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="siniestros_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <strong>
                                                                        <asp:Label ID="lblSiniestroPo" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <asp:GridView ID="gvSiniestros" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                                    DataKeyNames="Siniestro,ClienteContratante,ESTADO,idpoliza" AutoGenerateColumns="false"
                                                                    AllowPaging="True" PageSize="5" OnPageIndexChanging="gvSiniestros_PageIndexChanging"
                                                                    OnRowCommand="gvSiniestros_RowCommand">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-photo fa-1x text-white"></i>'
                                                                            ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Ver" Visible="false" />
                                                                        <asp:BoundField DataField="ClienteContratante" HeaderText="ClienteContratante" Visible="false" />
                                                                        <asp:BoundField DataField="SINIESTRO" HeaderText="SINIESTRO" />
                                                                        <asp:BoundField DataField="CIASEG" HeaderText="CiaSeg" Visible="false" />
                                                                        <asp:BoundField DataField="ASEGURADORA" HeaderText="ASEG" />
                                                                        <asp:BoundField DataField="idpoliza" HeaderText=" idpoliza" Visible="false" />
                                                                        <asp:BoundField DataField="Poliza" HeaderText=" POLIZA" />
                                                                        <asp:BoundField DataField="RIESGO" HeaderText=" RIESGO" />
                                                                        <asp:BoundField DataField="NOMBRE_Cliente" HeaderText="NOMBRE CLIENTE" />
                                                                        <asp:BoundField DataField="MONEDA" HeaderText="MONEDA" />
                                                                        <asp:BoundField DataField="Monto_Perdida" HeaderText="MONTO PERDIDA" />
                                                                        <asp:BoundField DataField="Monto_Deducible" HeaderText="MONTO DEDUCIBLE" />
                                                                        <asp:BoundField DataField="Monto_Indemnizado" HeaderText="MONTO INDEMNIZADO" />
                                                                        <asp:BoundField DataField="FechaIndemnizacion" HeaderText="FECHA INDEMNIZACION" />
                                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvSiniestros" EventName="PageIndexChanging" />
                                                        <asp:AsyncPostBackTrigger ControlID="gvSiniestros" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="coasegurado_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtCoaseguro" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkCoaseguro" runat="server" class="btn btn-default btn-sm" OnClick="lnkCoaseguro_Click">
                                                                    <i class="fa fa-search fa-1x text-turq"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lblCantCoaseg" runat="server" Text=""></asp:Label></strong> <strong>
                                                                            <asp:Label ID="lbl_Coaseguro" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:HiddenField ID="hfidCoaseg" runat="server" />
                                                                <asp:GridView ID="gvCoaseguro" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    AutoGenerateColumns="False" DataKeyNames="idCoaseg,idCiaSeg,CiaSegCoa,Tasa" AllowPaging="True"
                                                                    PageSize="5" OnPageIndexChanging="gvCoaseguro_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idCoaseg" HeaderText="CODIGO" />
                                                                        <asp:BoundField DataField="idCiaSeg" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="CiaSegCoa" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="CiaSeg" HeaderText="CIASEG" />
                                                                        <asp:BoundField DataField="Tasa" HeaderText="%" />
                                                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkCoaseguro" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gvCoaseguro" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="cobertura_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtCobertura" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnkCobertura" runat="server" class="btn btn-default btn-sm" OnClick="lnkCobertura_Click">
                                                                    <i class="fa fa-search fa-1x text-primary"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lbl_Cobertura" runat="server" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:Label ID="lblCantCober" runat="server" Text=""></asp:Label>
                                                                <asp:HiddenField ID="hfidCobertu" runat="server" />
                                                                <asp:GridView ID="gvCobertura" runat="server" class="table table-condensed table-bordered table-striped input-sm"
                                                                    AutoGenerateColumns="False" DataKeyNames="idCobertura,CiaSeg,Codigo,Limite" AllowPaging="True"
                                                                    PageSize="5" OnPageIndexChanging="gvCobertura_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idCobertura" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="CiaSeg" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="Codigo" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="Descrip" HeaderText="DESCRIPCION" />
                                                                        <asp:BoundField DataField="Limite" HeaderText="LIMITE" />
                                                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnkCobertura" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gvCobertura" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="seguimiento_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtSeguimientoRC" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnk_Seguimiento" runat="server" class="btn btn-default btn-sm"
                                                                            OnClick="lnk_Seguimiento_Click">
                                                                    <i class="fa fa-search fa-1x text-gris"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lbl_Seguimiento" runat="server" Text="" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gv_seguimiento" runat="server" AutoGenerateColumns="false" class="table table-condensed table-bordered table-striped input-sm"
                                                                    DataKeyNames="FEC_ACT,UNI_NEG,ID_FUNCIONARIO,IDESTADO,DESCRIPCION,idOperacion"
                                                                    AllowPaging="True" PageSize="5" OnPageIndexChanging="gv_seguimiento_PageIndexChanging">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="idOperacion" HeaderText="" />
                                                                        <asp:BoundField DataField="UNI_NEG" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="ID_FUNCIONARIO" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="FUNCIONARIO" HeaderText="EJECUTIVO" />
                                                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" />
                                                                        <asp:BoundField DataField="FEC_ACT" HeaderText="FEC.ACTIVIDAD" />
                                                                        <asp:BoundField DataField="IDESTADO" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                                                                        <asp:BoundField DataField="USUARIO" HeaderText="USUARIO" />
                                                                        <asp:BoundField DataField="FEC_RESU" HeaderText="FEC.REG. || FEC.RESUELTO" />
                                                                        <asp:BoundField DataField="idClasificacion" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="respuesta" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="OBSERVACION" HeaderText="" Visible="False" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="lnk_Seguimiento" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="gv_seguimiento" EventName="PageIndexChanging" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="documentos_poliza">
                                                <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-12 centrar">
                                                            <div class="row">
                                                                <div class="form-inline">
                                                                    <label>
                                                                        Filtros:</label>
                                                                    <asp:TextBox ID="txtDocumentosRC" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                    <div class="btn-group small">
                                                                        <asp:LinkButton ID="lnk_Documentos" runat="server" class="btn btn-default btn-sm"
                                                                            OnClick="lnk_Documentos_Click">
                                                                    <i class="fa fa-search fa-1x text-warning"></i>&nbsp;Buscar
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <strong>
                                                                        <asp:Label ID="lbl_Documentos" runat="server" Text="" class="text-danger input-sm"></asp:Label></strong>
                                                                </div>
                                                                <br />
                                                                <asp:GridView ID="gv_Documentos" runat="server" AutoGenerateColumns="false" class="table table-condensed table-bordered table-striped input-sm"
                                                                    DataKeyNames="ruta,nombre_archivo,ext,tipo,idDocumento,IDESTADO" AllowPaging="True"
                                                                    PageSize="5" OnPageIndexChanging="gv_Documentos_PageIndexChanging" OnRowCommand="gv_Documentos_RowCommand">
                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                    <Columns>
                                                                        <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-photo fa-1x text-white"></i>'
                                                                            ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Ver" />
                                                                        <asp:BoundField DataField="idDocumento" HeaderText="Nº DOCUMENTO" />
                                                                        <asp:BoundField DataField="tipo" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="VALTIP" HeaderText="TIPO" Visible="True" />
                                                                        <asp:BoundField DataField="nombre_archivo" HeaderText="ARCHIVO" />
                                                                        <asp:BoundField DataField="ruta" HeaderText="RUTA" Visible="False" />
                                                                        <asp:BoundField DataField="IDESTADO" HeaderText="" Visible="False" />
                                                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                                                        <asp:BoundField DataField="web" HeaderText="ACCESO WEB" />
                                                                        <asp:BoundField DataField="codigo" HeaderText="CODIGO [CPS]" Visible="False" />
                                                                        <asp:BoundField DataField="FechaReg" DataFormatString="{0:d}" HeaderText="FECHA REGISTRO" />
                                                                        <asp:BoundField DataField="usuReg" HeaderText="USUARIO" />
                                                                        <asp:BoundField DataField="ext" HeaderText="EXTENSION" Visible="False" />
                                                                        <asp:BoundField DataField="DESCRIP" HeaderText="EXTENSION" Visible="False" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gv_Documentos" EventName="PageIndexChanging" />
                                                        <asp:AsyncPostBackTrigger ControlID="gv_Documentos" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="lnk_Documentos" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkBuscarAfiliado" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="PageIndexChanging" />
                            <asp:PostBackTrigger ControlID="gvPolizasCliente" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="DocumentosCliente" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-wide modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                        <ContentTemplate>
                            <div class="panel-heading color-blue row-eq-height">
                                <div class="col-md-6">
                                    <h3 class="text-center text-white" style="display: inline-block;">
                                        <asp:Label ID="Label12" runat="server" Text="Documentos de Cliente"></asp:Label></h3>
                                </div>
                                <div class="col-md-6">
                                    <div class="pull-right">
                                        <br />
                                        <asp:LinkButton ID="LinkButton11" runat="server" data-dismiss="modal" aria-hidden="true">
                                        <i class="fa fa-times fa-2x text-white"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-inline">
                                            <label>
                                                Filtros:</label>
                                            <asp:TextBox ID="txtDocumentosCliente" runat="server" class="form-control input-sm"></asp:TextBox>
                                            <div class="btn-group small">
                                                <asp:LinkButton ID="lnkDocumentosCliente" runat="server" class="btn btn-default btn-sm"
                                                    OnClick="lnkDocumentosCliente_Click">
                                                <i class="fa fa-search fa-1x text-warning"></i>&nbsp;Buscar
                                                </asp:LinkButton>
                                            </div>
                                            <strong>
                                                <asp:Label ID="Label13" runat="server" Text="" class="text-danger input-sm"></asp:Label></strong>
                                        </div>
                                        <div class="table-responsive centrar">
                                            <asp:Label ID="lblDocumentosCliente" runat="server" CssClass="text-danger input-sm"></asp:Label>
                                            <asp:GridView ID="gvDocumentosCliente" runat="server" class="table table-bordered table-condensed table-striped input-sm"
                                                AllowPaging="true" PageSize="10" AutoGenerateColumns="false" OnPageIndexChanging="gvDocumentosCliente_PageIndexChanging"
                                                OnRowCommand="gvDocumentosCliente_RowCommand" DataKeyNames="ext,ruta">
                                                <PagerStyle CssClass="pagination-ys" />
                                                <Columns>
                                                    <asp:ButtonField CommandName="VER" ButtonType="Link" Text='<i class="fa fa-photo fa-1x text-white"></i>'
                                                        ControlStyle-CssClass="btn btn-info btn-sm" HeaderText="Ver" />
                                                    <asp:BoundField DataField="idDocumento" HeaderText="Nº DOCUMENTO" />
                                                    <asp:BoundField DataField="tipo" HeaderText="" Visible="False" />
                                                    <asp:BoundField DataField="nombre_archivo" HeaderText="ARCHIVO" />
                                                    <asp:BoundField DataField="ruta" HeaderText="RUTA" Visible="False" />
                                                    <asp:BoundField DataField="estado" HeaderText="" Visible="False" />
                                                    <asp:BoundField DataField="codigo" HeaderText="CODIGO [CPS]" Visible="False" />
                                                    <asp:BoundField DataField="FechaReg" DataFormatString="{0:d}" HeaderText="FECHA REGISTRO" />
                                                    <asp:BoundField DataField="ext" HeaderText="EXTENSION" Visible="False" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkBuscarAfiliado" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="gvAfiliado" EventName="PageIndexChanging" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="Familiar" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel-masOpciones-red">
                    <div class="panel-heading color-blue">
                        <h3 class="text-center text-white">
                            <asp:Label ID="Label4" runat="server" Text="Familiar"></asp:Label></h3>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panel-body">
                                <div class="col-md-10 col-lg-offset-1">
                                    <div class="form-group">
                                        <label class="text-blue input-sm">
                                            Nombres y Apellidos</label>
                                        <asp:TextBox ID="txtFamiliar" runat="server" CssClass="form-control input-sm" placeholder="Nombres y Apellidos"></asp:TextBox>
                                    </div>
                                    <hr />
                                    <div class="form-group">
                                        <div class="form-inline">
                                            <div class="form-group">
                                                <label class="text-blue input-sm">
                                                    Teléfono</label>
                                                <asp:TextBox ID="txtTelefonoContactoFamiliar" runat="server" CssClass="form-control input-sm"
                                                    placeholder="Teléfono" Width="100%"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="text-blue input-sm">
                                                    Correo</label>
                                                <asp:TextBox ID="txtCorreoContactoFamiliar" runat="server" CssClass="form-control input-sm"
                                                    placeholder="Correo" Width="100%"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="text-blue input-sm">
                                                    Dni</label>
                                                <asp:TextBox ID="txtDniContactoFamiliar" runat="server" CssClass="form-control input-sm"
                                                    placeholder="Dni" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="lnkRegistrarFami" runat="server" class="btn btn-default btn-sm"
                                    OnClick="lnkRegistrarFami_Click">
                                    <i class="fa fa-edit fa-1x text-blue"></i> Registrar
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton10" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar"
                                    data-dismiss="modal" aria-hidden="true">
                                    <i class="fa fa-sign-out fa-1x text-blue"></i> Salir
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkRegistrarFami" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="Otros" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel-masOpciones-blue">
                    <div class="panel-heading color-blue">
                        <h3 class="text-center text-white">
                            <asp:Label ID="Label3" runat="server" Text="Otros"></asp:Label></h3>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panel-body">
                                <div class="col-md-10 col-lg-offset-1">
                                    <div runat="server" id="frmdatosAdicionalesAsegurado">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="text-blue input-sm">
                                                    Dni</label>
                                                <asp:TextBox ID="txtDniOtrosAsegurado" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="text-blue input-sm">
                                                    Nombres y Apellidos</label>
                                                <asp:TextBox ID="txOtrosAsegurado" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="frmdatosAdicionales">
                                        <div class="form-group">
                                            <label class="text-blue input-sm">
                                                Nombres y Apellidos</label>
                                            <asp:TextBox ID="txtOtros" runat="server" CssClass="form-control input-sm" placeholder="Nombres y Apellidos"></asp:TextBox>
                                        </div>
                                        <hr />
                                        <div class="form-group">
                                            <div class="form-inline">
                                                <div class="form-group">
                                                    <label class="text-blue input-sm">
                                                        Teléfono</label>
                                                    <asp:TextBox ID="txtTelefonoContactoOtros" runat="server" CssClass="form-control input-sm"
                                                        placeholder="Teléfono" Width="100%"></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <label class="text-blue input-sm">
                                                        Correo</label>
                                                    <asp:TextBox ID="txtCorreoContactoOtros" runat="server" CssClass="form-control input-sm"
                                                        placeholder="Correo" Width="100%"></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <label class="text-blue input-sm">
                                                        Dni</label>
                                                    <asp:TextBox ID="txtDniContactoOtros" runat="server" CssClass="form-control input-sm"
                                                        placeholder="Dni" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="lnkRegistrarOtros" runat="server" class="btn btn-default btn-sm"
                                    OnClick="lnkRegistrarOtros_Click">
                                    <i class="fa fa-edit fa-1x text-blue"></i> Registrar
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton12" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar"
                                    data-dismiss="modal" aria-hidden="true">
                                    <i class="fa fa-sign-out fa-1x text-blue"></i> Salir
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkRegistrarOtros" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="NUEVOAFILIADO" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-wide modal-lg">
                <div class="modal-content panel panel-popup">
                    <div class="panel-heading color-blue text-white">
                        <div class="form-inline">
                            <i class="fa fa-pencil fa-1x"></i>
                            <asp:Label ID="lblAfiliado12" runat="server" class="input-sm text-white"></asp:Label><asp:Label
                                ID="lblEstado12" runat="server" class="input-sm text-white"></asp:Label>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#info-tab" data-toggle="tab"><i class="fa fa-user fa-1x text-blue"></i>Información de Afiliado</a></li>
                                <li><a href="#address-tab" data-toggle="tab"><i class="fa fa-file-photo-o fa-1x text-blue"></i>Datos Generales</a></li>
                                <li runat="server" id="CartasTab"><a href="#cartas-tabb" data-toggle="tab"><i class="fa fa-envelope-square fa-1x text-blue"></i>Impresión de Documentos <i class="fa"></i></a></li>
                                <li runat="server" id="RecordConsumoTab"><a id="A1" href="#record-tabb" data-toggle="tab"
                                    runat="server"><i class="fa fa-dashboard fa-1x text-blue"></i>Record de Consumo
                                </a></li>
                                <li runat="server" id="AvisosTab"><a href="#avisos-tabb" data-toggle="tab"><i class="fa fa-file-archive-o fa-1x text-blue"></i>Avisos </a></li>
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
                                            <div class="fileinput fileinput-new" data-provides="fileinput">
                                                <div class="fileinput-new thumbnail" style="width: 150px; height: 150px;">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/image/photo.png" class="img-circle"
                                                        Height="150px" Width="150px" />
                                                </div>
                                                <div class="fileinput-preview fileinput-exists thumbnail" style="max-width: 200px; max-height: 150px;">
                                                </div>
                                                <br />
                                                <div class="form-inline">
                                                    <span class="btn btn-sm btn-file btn-default btn-sm"><span class="fileinput-new"
                                                        title="Cambiar"><i class="fa fa-edit fa-fw"></i></span><span class="fileinput-exists"
                                                            title="Cambiar"><i class="fa fa-edit fa-fw"></i></span>
                                                        <input type="file" name="..." id="file2" runat="server" /></span>
                                                </div>
                                            </div>
                                            <div class="form-group-sm">
                                                <label class="text-blue input-sm text-blue">
                                                    Consultas:</label>
                                                <asp:HyperLink ID="HyperLink2" class="btn btn-sm btn-link" runat="server" Target="_blank"
                                                    NavigateUrl="http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias"><i class="fa fa-stack-1x fa-1x text-blue"></i> RUC</asp:HyperLink>
                                                <asp:HyperLink ID="HyperLink3" class="btn btn-sm btn-link" runat="server" Target="_blank"
                                                    NavigateUrl="http://ww4.essalud.gob.pe:7777/acredita/"><i class="fa fa-stack-1x fa-1x text-blue"></i> ESSALUD</asp:HyperLink>
                                            </div>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <div class="col-sm-4">
                                                        <div class="panel-default">
                                                            <div class="panel-body form-horizontal">
                                                                <div class="form-group-sm">
                                                                    <label class="control-label input-sm text-blue">
                                                                        Código Cliente (*):</label>
                                                                    <asp:TextBox ID="txtNumeroPoli" type="text" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm">
                                                                    <label class="control-label input-sm text-blue">
                                                                        Código Titular (*):</label>
                                                                    <asp:TextBox ID="txtCodigoTitu" ClientIDMode="Static" name="txtCodigoTitu" type="text"
                                                                        runat="server" class="form-control input-sm" onkeypress="return isNumberKey(event)"
                                                                        MaxLength="6"></asp:TextBox>
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
                                                                    <asp:DropDownList ID="ddlPlan" runat="server" class="form-control input-sm">
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
                                                                </div>
                                                                <div class="form-inline">
                                                                    <asp:TextBox ID="txtDNI" ClientIDMode="Static" name="txtDNI" runat="server" class="form-control input-sm"
                                                                        onkeypress="return isNumberKey(event)" MaxLength="15"></asp:TextBox>
                                                                </div>
                                                                <asp:Label ID="lblBD" runat="server" Font-Size="XX-Small"></asp:Label>
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
                                                                    <asp:TextBox ID="txtCodPaciente" runat="server" class="form-control input-sm"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group-sm" runat="server" id="id_paciente">
                                                                    <label class="control-label input-sm text-blue">
                                                                        ID. Hijo (*):</label>
                                                                    <asp:TextBox ID="txtIdPaciente" runat="server" class="form-control input-sm" MaxLength="2"
                                                                        AutoPostBack="true" OnTextChanged="txtIdPaciente_TextChanged"></asp:TextBox>
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
                                                            Contraseña:</label>
                                                        <input type="text" name="prevent_autofill" id="Text1" value="" style="display: none;" />
                                                        <input type="password" name="password_fake" id="password1" value="" style="display: none;" />
                                                        <asp:TextBox ID="txtContraseña" runat="server" class="form-control input-sm" TextMode="Password"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Edad:</label>
                                                        <asp:TextBox ID="txtEdad" runat="server" class="form-control input-sm" disabled></asp:TextBox>
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
                                    <asp:UpdatePanel ID="UpdatePanel77" runat="server">
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
                                            <asp:AsyncPostBackTrigger ControlID="lnkCagarCartas" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvImpresionCartasDetalle" EventName="PageIndexChanging" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="updateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                <asp:Image ID="imgUpdateProgress111" runat="server" ImageUrl="~/image/loading1.gif"
                                                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div class="tab-pane" id="record-tabb">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div class="col-sm-12 centrar">
                                                <div class="form-inline">
                                                    <asp:LinkButton ID="lnkCargarRecord" runat="server" class="btn btn-default btn-sm"
                                                        OnClick="lnkCargarRecord_Click">
                                                        <i class="fa fa-refresh fa-1x text-blue"></i> Cargar Record de Consumo
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
                                            <asp:AsyncPostBackTrigger ControlID="lnkCargarRecord" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvDatosDetalle3" EventName="PageIndexChanging" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="updateProgress2" runat="server">
                                        <ProgressTemplate>
                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                <asp:Image ID="imgUpdateProgress1" runat="server" ImageUrl="~/image/loading1.gif"
                                                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div class="tab-pane" id="avisos-tabb">
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <div class="text-center centrar">
                                                <asp:HiddenField ID="idAvisohf" runat="server" />
                                                <asp:Label ID="lblAvisos" runat="server" class="text-blue input-sm"></asp:Label>
                                                <asp:Label ID="lblNohayAvisos" runat="server" class="text-danger input-sm"></asp:Label>
                                            </div>
                                            <div class="col-sm-12" runat="server" visible="false" id="avisos123">
                                                <asp:GridView ID="gv_Avisos" runat="server" class="table table-striped table-bordered table-condensed input-sm"
                                                    AutoGenerateColumns="false" DataKeyNames="op,Aviso,Cod Aviso" OnPageIndexChanging="gv_Avisos_PageIndexChanging">
                                                    <Columns>
                                                        <asp:ButtonField CommandName="Editar" Text="Editar" />
                                                        <asp:ButtonField CommandName="Eliminar" Text="Eliminar" />
                                                        <asp:BoundField DataField="Cod Aviso" HeaderText="Cod Aviso" />
                                                        <asp:BoundField DataField="Aviso" HeaderText="Aviso" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:BoundField DataField="op" HeaderText="op" Visible="false" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvAvisos" EventName="PageIndexChanging" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="updateProgress3" runat="server">
                                        <ProgressTemplate>
                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/image/loading1.gif"
                                                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div class="tab-pane" id="informes_medicos">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <div class="col-sm-12 centrar">
                                                <div class="form-inline">
                                                    <asp:LinkButton ID="lnkCargarInformes" runat="server" class="btn btn-default btn-sm"
                                                        OnClick="lnkCargarInformes_Click">
                                                    <i class="fa fa-refresh fa-1x text-blue"></i> Cargar Informes Médicos
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
                                                                    <asp:Label ID="Label5" runat="server" class="input-sm text-info">REPORTE ANUAL DE INFORMES MÉDICOS</asp:Label></legend>
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
                                                                    <asp:Label ID="Label6" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
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
                                                                    <asp:Label ID="Label7" runat="server" class="input-sm text-info">REPORTE MENSUAL DE INFORMES MÉDICOS</asp:Label></legend>
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
                                                                    <asp:Label ID="Label8" runat="server" class="input-sm text-info">REPRESENTACIÓN GRÁFICA</asp:Label></legend>
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
                                            <asp:AsyncPostBackTrigger ControlID="lnkCargarInformes" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvInformesMedicosDetalle" EventName="PageIndexChanging" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="updateProgress4" runat="server">
                                        <ProgressTemplate>
                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                <asp:Image ID="imgUpdateProgress3" runat="server" ImageUrl="~/image/loading1.gif"
                                                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                                <div class="tab-pane" id="ultimos_movimientos">
                                    <asp:UpdatePanel ID="UpdatePanel99" runat="server" UpdateMode="Always">
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
                                                            <asp:BoundField DataField="FECREG" HeaderText="FECREG" />
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
                                    <asp:UpdateProgress ID="updateProgress5" runat="server">
                                        <ProgressTemplate>
                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #fff; opacity: 0.7;">
                                                <asp:Image ID="imgUpdateProgress4" runat="server" ImageUrl="~/image/loading1.gif"
                                                    AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer color-blue text-white text-center">
                        <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-default btn-sm" ToolTip="Cerrar"
                            data-dismiss="modal" aria-hidden="true">
                        <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div id="DOCUMENTO" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <div class="modal-body">
                        <div class="row-eq-height">
                            <iframe id="documentoFrame" runat="server" frameborder="0" width="100%" height="760"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer color-blue text-white text-right">
                        <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-default btn-sm" ToolTip="Regresar"
                            data-dismiss="modal" aria-hidden="true">
                            <i class="fa fa-sign-out fa-1x text-blue"></i> Salir
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div id="AFILIADORESUMEN" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panel-heading text-white">
                                <div class="form-inline">
                                    <i class="fa fa-pencil fa-1x"></i>Datos del Afiliado
                                </div>
                            </div>
                            <div class="panel-body color-white">
                                <div class="col-md-12">
                                    <div class="text-center">
                                        <strong>
                                            <asp:Label ID="Label10" runat="server" class="text-success"></asp:Label></strong>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <div class="panel-default">
                                                    <div class="panel-body form-horizontal">
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Código Cliente (*):</label>
                                                            <asp:TextBox ID="txtCodigoClienteAfi" type="text" runat="server" class="form-control input-sm"
                                                                ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Código Titular (*):</label>
                                                            <asp:TextBox ID="txtCodigoTitularAfi" ClientIDMode="Static" name="txtCodigoTitu"
                                                                type="text" runat="server" class="form-control input-sm" onkeypress="return isNumberKey(event)"
                                                                MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Parentesco (*):</label>
                                                            <asp:DropDownList ID="ddlParentescoAfi" runat="server" class="form-control input-sm"
                                                                AutoPostBack="true" ReadOnly="true">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hfParentescoAfi" runat="server" />
                                                            <asp:Label ID="lblCate" runat="server" Text="" Visible="false"></asp:Label>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Nro. Documento (*):</label>
                                                        </div>
                                                        <div class="form-inline">
                                                            <asp:TextBox ID="txtDocumentoAfi" ClientIDMode="Static" name="txtDNI" runat="server"
                                                                class="form-control input-sm" onkeypress="return isNumberKey(event)" MaxLength="15"></asp:TextBox>
                                                            <asp:LinkButton ID="lnkBD" runat="server" class="btn btn-default btn-sm" Visible="false">
                                                                <i class="fa fa-check-circle fa-1x text-blue"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkTraer" runat="server" class="btn btn-default btn-sm" Visible="false">
                                                                <i class="fa fa-check-circle fa-1x text-verde"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="panel-default">
                                                    <div class="panel-body form-horizontal">
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Nombres (*):</label>
                                                            <asp:TextBox ID="txtNombresAfi" ClientIDMode="Static" runat="server" name="txtNombres"
                                                                class="form-control input-sm uppercase"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label class="control-label input-sm text-blue">
                                                                Apellido Paterno (*):</label>
                                                            <asp:TextBox ID="txtApellidoPaternoAfi" runat="server" ClientIDMode="Static" name="txtApellidop"
                                                                class="form-control input-sm uppercase"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="amount" class="control-label input-sm text-blue">
                                                                Apellido Materno (*):</label>
                                                            <asp:TextBox ID="txtApellidoMaternoAfi" runat="server" ClientIDMode="Static" name="txtApellidom"
                                                                class="form-control input-sm uppercase"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="amount" class="control-label input-sm text-blue">
                                                                Fecha Nacimiento (*):</label>
                                                            <asp:TextBox ID="txtFechaNacimientoAfi" runat="server" class="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group-sm">
                                                            <label for="date" class="control-label input-sm text-blue">
                                                                Sexo:</label>
                                                            <asp:DropDownList ID="ddlSexoAfi" runat="server" name="sexo" class="form-control input-sm">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="btnGuardarModificar" runat="server" class="btn btn-default btn-sm"
                                    OnClientClick="return confirm('¿Esta seguro que desea guardar configuración actual');"
                                    OnClick="btnGuardarModificar_Click">
                                    <i class="fa fa-save fa-1x text-blue"></i> Guardar Cambios
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCerrar" runat="server" class="btn btn-default btn-sm" data-dismiss="modal">
                                    <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnGuardarModificar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="RESUELTOS" class="modal fade bs-example-modal-lg" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog"
            aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panel-heading text-white">
                                <div class="form-inline">
                                    <i class="fa fa-pencil fa-1x"></i>Agregar Registro de Gestión
                                </div>
                            </div>
                            <div class="panel-body color-white">
                                <div class="row text-center">
                                    <asp:Label ID="lblErrorModal" runat="server" class="text-danger input-sm"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px; padding-bottom: 0px">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            (*)Origen:</label>
                                                        <asp:HiddenField ID="hfOrigenModal" runat="server" />
                                                        <asp:HiddenField ID="hfOrigenDescripModal" runat="server" />
                                                        <div class="table-responsive centrar">
                                                            <asp:GridView ID="gvOrigenModal" runat="server" class="table table-bordered table-condensed input-sm"
                                                                AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ID,DESCRIP" ShowHeader="False"
                                                                OnRowCommand="gvOrigenModal_RowCommand" OnRowDataBound="gvOrigenModal_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px; padding-bottom: 0px;">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            (*)Solicitante:</label>
                                                        <asp:HiddenField ID="hfEmisorModal" runat="server" />
                                                        <asp:HiddenField ID="hfEmisorDescripModal" runat="server" />
                                                        <div class="table-responsive centrar">
                                                            <asp:GridView ID="gvEmisorModal" runat="server" class="table table-bordered table-condensed input-sm"
                                                                AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ID,DESCRIP" ShowHeader="False"
                                                                OnRowCommand="gvEmisorModal_RowCommand" OnRowDataBound="gvEmisorModal_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="" Visible="False" />
                                                                    <asp:ButtonField CommandName="Seleccionar" DataTextField="DESCRIP" HeaderText="" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px">
                                                    <div class="form-group-sm">
                                                        <asp:FileUpload runat="server" ID="fuArchivoResuelto" CssClass="btn btn-sm" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            (*)Contacto:</label>
                                                        <asp:TextBox ID="txtContactoNombreModal" runat="server" class="form-control input-sm"
                                                            placeholder="Contacto"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            (*)Detalle de la incidencia:</label>
                                                        <asp:TextBox ID="txtRespuesta" runat="server" class="form-control input-sm" TextMode="MultiLine"
                                                            Height="142px" Width="100%" placeholder="Descripción"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="panel-default">
                                                <div class="panel-body form-horizontal" style="padding-top: 0px">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Observación:</label>
                                                        <asp:TextBox ID="TxtObs" runat="server" class="form-control input-sm" TextMode="MultiLine"
                                                            Height="142px" Width="100%" placeholder="Observación"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="lnkGuardarResuelto" runat="server" class="btn btn-default btn-sm"
                                    OnClick="lnkGuardarResuelto_Click" Visible="false">
                                    <i class="fa fa-save fa-1x text-blue"></i> Guardar
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnkModificarResuelto" runat="server" class="btn btn-default btn-sm"
                                    OnClick="lnkModificarResuelto_Click" Visible="false">
                                    <i class="fa fa-save fa-1x text-blue"></i> Modificar Cambios
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton15" runat="server" class="btn btn-default btn-sm" data-dismiss="modal">
                                    <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkGuardarResuelto" />
                            <asp:PostBackTrigger ControlID="lnkModificarResuelto" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="GRUPOFAMILIAR" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panel-heading color-blue text-white">
                                <div class="form-inline">
                                    <i class="fa fa-calendar-o fa-1x"></i>GRUPO FAMILIAR
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12 table-responsive">
                                    <asp:GridView ID="gvGrupoFamiliar" runat="server" AutoGenerateColumns="False" class="table table-bordered table-condensed table-striped input-sm"
                                        OnRowDataBound="gvGrupoFamiliar_RowDataBound" OnSelectedIndexChanged="gvGrupoFamiliar_SelectedIndexChanged"
                                        DataKeyNames="cod_cliente,cod_titula,categoria,afiliado,IDEMPRESA">
                                        <Columns>
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
                                            <asp:BoundField DataField="Color" ShowHeader="false" HtmlEncode="false" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="lnkCerrarGrupoFamiliar" runat="server" class="btn btn-default btn-sm"
                                    ToolTip="Regresar" OnClick="lnkCerrarGrupoFamiliar_Click">
                                <i class="fa fa-sign-out fa-1x text-blue"></i> Cerrar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="gvGrupoFamiliar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="Siniestro" class="modal modal-wide fade bs-example-modal-lg" tabindex="-1"
            role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog width-60 modal-lg">
                <div class="modal-content panel panel-popup">
                    <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                        <ContentTemplate>
                            <div class="panel-heading color-blue text-white">
                                <div class="form-inline">
                                    <i class="fa fa-pencil fa-1x"></i>
                                    <h5 style="display: inline-block;">Nuevo Siniestro / S.Servicio</h5>
                                    <asp:LinkButton ID="lnkCerrarSiniestro" runat="server" ToolTip="Cerrar" data-dismiss="modal"
                                        aria-hidden="true" class="pull-right">
                                    <i class="fa fa-times fa-1x text-white"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12">
                                    <div class="text-center">
                                        <strong>
                                            <asp:Label ID="Label16" runat="server" class="text-success"></asp:Label></strong>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblValidacionSiniestro" runat="server" class="text-danger input-sm"></asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Cliente(*)</label>
                                                        <asp:TextBox ID="txtCliente_" runat="server" class="form-control input-sm" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Ejecutivo de Siniestro(*)</label>
                                                        <asp:DropDownList ID="ddlEjecutivoSiniestro" runat="server" class="form-control input-sm">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-horizontal">
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Tipo Siniestro(*)</label>
                                                        <asp:DropDownList ID="ddlTipoStro" runat="server" class="form-control input-sm">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlTipoStroServicio" runat="server" class="form-control input-sm">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group-sm">
                                                        <label class="control-label input-sm text-blue">
                                                            Lugar de Siniestro(*):</label>
                                                        <asp:TextBox ID="txtLugarSiniestro" ClientIDMode="Static" name="txtLugarSiniestro" type="text"
                                                            runat="server" class="form-control input-sm" TextMode="MultiLine" Rows="5" Height="50px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group-sm" runat="server" id="DivAsegurado_" visible="false">
                                                <label class="control-label input-sm text-blue">Asegurado(*)</label>
                                                <div class="form-inline">
                                                    <asp:TextBox ID="txtAsegurado_" runat="server" class="form-control input-sm"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkBuscarAsegurado_" runat="server" class="btn btn-primary"
                                                        OnClick="lnkBuscarAsegurado__Click">
                                                    <span class="fa fa-search fa-1x text-white"></span>
                                                    </asp:LinkButton>
                                                    <asp:CheckBox ID="chkNohayAsegurado" runat="server"
                                                        CssClass="input-sm" Text="Sin Asignar"
                                                        OnCheckedChanged="chkNohayAsegurado_CheckedChanged" AutoPostBack="true" />
                                                </div>
                                                <br />
                                                <asp:GridView ID="gvAsegurado_" runat="server"
                                                    class="table table-condensed table-bordered table-striped input-sm"
                                                    AutoGenerateColumns="false" OnRowDataBound="gvAsegurado__RowDataBound"
                                                    OnSelectedIndexChanged="gvAsegurado__SelectedIndexChanged"
                                                    AutoGenerateSelectButton="True" DataKeyNames="idAseguradoPoliza"
                                                    AllowPaging="True" PageSize="5"
                                                    OnPageIndexChanging="gvAsegurado__PageIndexChanging">
                                                    <PagerStyle CssClass="pagination-ys" />
                                                    <Columns>
                                                        <asp:BoundField DataField="idAseguradoPoliza" HeaderText="CODIGO" Visible="FALSE" />
                                                        <asp:BoundField DataField="codigo" HeaderText="NRO" />
                                                        <asp:BoundField DataField="dependiente" HeaderText="TIPO DEPENDIENTE" />
                                                        <asp:BoundField DataField="Titular" HeaderText="TITULAR" />
                                                        <asp:BoundField DataField="Asegurado" HeaderText="ASEGURADO" />
                                                        <asp:BoundField DataField="Sexo" HeaderText="SEXO" />
                                                        <asp:BoundField DataField="FechaNacimiento" HeaderText="FECHA NACIMIENTO" />
                                                        <asp:BoundField DataField="FechaInclusion" HeaderText="FECHA INCLUSION" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div class="form-group-sm" runat="server" id="DivBienAsegurado_" visible="false">
                                                <label class="control-label input-sm text-blue">Vehículo Asegurado(*)</label>
                                                <div class="form-inline">
                                                    <asp:TextBox ID="txtBienAsegurado_" runat="server" class="form-control input-sm"></asp:TextBox>
                                                    <asp:LinkButton ID="lnkBuscarBienAsegurado_" runat="server"
                                                        class="btn btn-primary" OnClick="lnkBuscarBienAsegurado__Click">
                                                    <span class="fa fa-search fa-1x text-white"></span>
                                                    </asp:LinkButton>
                                                    <asp:CheckBox ID="chkNohaybienAsegurado" runat="server" CssClass="input-sm" AutoPostBack="true"
                                                        Text="Sin Asignar" OnCheckedChanged="chkNohaybienAsegurado_CheckedChanged" />
                                                </div>
                                                <br />
                                                <asp:GridView ID="gvBienAsegurado_" runat="server"
                                                    class="table table-condensed table-bordered table-striped input-sm"
                                                    AutoGenerateColumns="false" OnRowDataBound="gvBienAsegurado__RowDataBound" AutoGenerateSelectButton="True"
                                                    OnSelectedIndexChanged="gvBienAsegurado__SelectedIndexChanged"
                                                    DataKeyNames="idVehAseg" AllowPaging="true" PageSize="5"
                                                    OnPageIndexChanging="gvBienAsegurado__PageIndexChanging">
                                                    <PagerStyle CssClass="pagination-ys" />
                                                    <Columns>
                                                        <asp:BoundField DataField="idVehAseg" HeaderText="CODIGO" />
                                                        <asp:BoundField DataField="ClienteContratante" HeaderText="CLIENTE" />
                                                        <asp:BoundField DataField="Placa" HeaderText="PLACA" />
                                                        <asp:BoundField DataField="Clase" HeaderText="CLASE" />
                                                        <asp:BoundField DataField="Marca" HeaderText="MARCA" />
                                                        <asp:BoundField DataField="Modelo" HeaderText="MODELO" />
                                                        <asp:BoundField DataField="ANIO" HeaderText="AÑO" />
                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div class="form-group-sm">
                                                <label class="control-label input-sm text-blue">
                                                    Descripción:</label>
                                                <asp:TextBox ID="txtDescripcion" ClientIDMode="Static" name="txtDescripcion" type="text" runat="server" class="form-control input-sm" TextMode="MultiLine" Rows="5" Height="100px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer color-blue text-white text-center">
                                <asp:LinkButton ID="lnkGuardarAsegurado_" runat="server"
                                    class="btn btn-default btn-sm" OnClick="lnkGuardarAsegurado__Click">
                                <i class="fa fa-save fa-1x text-primary"></i> Guardar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkBuscarAsegurado_" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkBuscarBienAsegurado_" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkGuardarAsegurado_" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvBienAsegurado_" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="gvAsegurado_" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="gvBienAsegurado_" EventName="PageIndexChanging" />
                            <asp:AsyncPostBackTrigger ControlID="gvAsegurado_" EventName="PageIndexChanging" />
                            <asp:AsyncPostBackTrigger ControlID="chkNohaybienAsegurado" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkNohayAsegurado" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="ModalCliente" class="modal fade bs-example-modal-lg centrar" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                        <ContentTemplate>
                            <div class="panel-heading color-red">
                                <h3 class="text-center text-white">
                                    <asp:Label ID="lblC_Contacto" runat="server" Text="Actualizar Datos"></asp:Label></h3>
                            </div>
                            <div class="panel-body">
                                <br />
                                <div class="row">
                                    <div>
                                        <div class="form-inlina">
                                            <strong>
                                                <asp:Label ID="lblCorrectoContacto" runat="server" Text="" CssClass="control-label input-sm text-success"></asp:Label></strong>
                                            <strong>
                                                <asp:Label ID="lblErrorContacto" runat="server" Text="" CssClass="control-label input-sm text-danger"></asp:Label></strong>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <div class="form-group-sm">
                                                <label class="control-label input-sm text-blue">Teléfono 1 / Teléfono 2:</label>
                                                <div class="form-inline">
                                                    <asp:TextBox ID="txtTlfContacto1" type="text" runat="server" class="form-control input-sm" Width="49%"></asp:TextBox>
                                                    <asp:TextBox ID="txtTlfContacto2" type="text" runat="server" class="form-control input-sm" Width="49%"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group-sm">
                                                <label class="control-label input-sm text-blue">Celular:</label>
                                                <asp:TextBox ID="txtCelularContacto" type="text" runat="server" class="form-control input-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">

                                            <div class="form-group-sm">
                                                <label class="control-label input-sm text-blue">Email:</label>
                                                <asp:TextBox ID="txtEmailContacto" type="text" runat="server" class="form-control input-sm"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <div class="panel-footer text-right color-red">
                                <asp:LinkButton ID="lnkGuardarDatos" runat="server" class="btn btn-default btn-sm">
                            <i class="fa fa-save fa-1x text-blue"></i>&nbsp;Guardar
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButton9" runat="server" class="btn btn-default btn-sm" data-dismiss="modal" aria-hidden="true">
                            <i class="fa fa-minus-square fa-1x text-danger"></i>&nbsp;Cerrar
                                </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkGuardarDatos" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="mdlConfirmacion" class="modal fade bs-example-modal-lg centrar" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="panel-heading color-blue">
                        <h3 class="text-center text-white">
                            <asp:Label ID="Label2" runat="server" Text="Mensaje de confirmación"></asp:Label></h3>
                    </div>
                    <div class="panel-body">
                        <br />
                        <div class="text-center row">
                            <h4>Se ha realizado la actualización con éxito</h4>
                        </div>
                        <br />
                    </div>
                    <div class="panel-footer text-center  color-blue">

                        <asp:LinkButton ID="lnkConfirmar" runat="server" CssClass="btn btn-default center" OnClick="lnkConfirmar_Click">
                          Cerrar
                        </asp:LinkButton>
                    </div>

                </div>
            </div>
        </div>
        <div id="mdlVisualizador" class="modal" style="overflow: hidden; width: 105%; top: -55px;" data-backdrop="static">
            <div class="modal-dialog modal-lg" style="margin: 54px auto;">
                <div class="modal-content panel">
                    <div class="close-modal-per">
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                            <iframe id="ifArchivo" runat="server" frameborder="0" width="100%" height="500px" src=""></iframe>
                        </div>
                    </div>
                    <div class="panel-footer color-blue text-center">
                        <asp:LinkButton ID="LinkButton16" runat="server" class="btn btn-default btn-sm"
                            data-dismiss="modal" aria-hidden="true">
                                    <i class="fa fa-close fa-1x text-blue"></i>&nbsp;Cerrar
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $("#input-id").fileinput();
        $("#input-id").fileinput({ 'showUpload': false, 'previewFileType': 'any' });
    </script>
    <script type="text/javascript">
        function ChangeColor(GridViewId, SelectedRowId) {
            var GridViewControl = document.getElementById(GridViewId);
            if (GridViewControl != null) {
                var GridViewRows = GridViewControl.rows;
                if (GridViewRows != null) {
                    var SelectedRow = GridViewRows[SelectedRowId];
                    //Remove Selected Row color if any
                    for (var i = 0; i < GridViewRows.length; i++) {
                        var row = GridViewRows[i];
                        if (row == SelectedRow) {
                            //Apply Yellow color to selected Row
                            row.style.backgroundColor = "lightblue";
                        }
                        else {
                            //Apply White color to rest of rows
                            row.style.backgroundColor = "#ffffff";
                        }
                    }

                }
            }

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
