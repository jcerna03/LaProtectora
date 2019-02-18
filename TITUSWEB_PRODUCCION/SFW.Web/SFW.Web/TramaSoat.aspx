<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TramaSoat.aspx.cs" Inherits="SFW.Web.TramaSoat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="Link1" type="text/css" href="Content/bootstrap.css" rel="stylesheet" runat="server" />
    <link id="Link2" type="text/css" href="Content/sb-admin-2.css" rel="stylesheet" runat="server" />
    <link id="Link3" type="text/css" href="font-awesome-4.1.0/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link id="Link4" type="text/css" href="Content/MYG.css" rel="stylesheet" runat="server" />
    <link href="Content/jasny-bootstrap.css" rel="stylesheet" type="text/css" /> 
    <link href="Content/paging.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="orror" class="alert alert-warning" role="alert" visible="false">
        <asp:LinkButton ID="lnkbotonCerrar" runat="server" class="close" 
            onclick="lnkbotonCerrar_Click">x
        </asp:LinkButton>
        <%--  <button type="button" class="close" data-dismiss="alert">×</button>--%>
        <asp:Label ID="lblErrorReg" runat="server" Text=""></asp:Label>
    </div>
    <div class="panel-default" runat="server" id="siniestrosAsegurados">
    <div class="col-md-12"><h5><strong><i class="fa fa-users fa-1x text-warning"></i>&nbsp;Envio de Tramas / Protecta</strong></h5></div>
                    <hr class="hr-danger" />
                    <div class="panel-heading row-eq-height">
                        <div class="col-md-12 text-left">
                            <div class="form-inline">  
                                <label>Filtros:</label>  
                                <asp:TextBox ID="txtsini" runat="server" PlaceHolder="Nº Siniestro"></asp:TextBox>
                                <asp:TextBox ID="txtcerti" runat="server" PlaceHolder="Nº Certificado"></asp:TextBox>
                                <asp:TextBox ID="txtplaca" runat="server" PlaceHolder="Nº Placa"></asp:TextBox>
                                <asp:TextBox ID="txtdni" runat="server" PlaceHolder="Nº Dni"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Filtrar" 
                                    onclick="btnBuscar_Click"/>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="row table-responsive"> 
                            <div class="col-md-12">
                                <asp:GridView ID="gvData" runat="server" 
                                    class="table table-condensed table-bordered table-striped input-sm" 
                                    AutoGenerateColumns="True" AllowPaging="True" PageSize="10" onpageindexchanging="gvData_PageIndexChanging">
                                </asp:GridView>
                            </div>
                        </div>
                       
                    </div>
                </div>
    <div>
        
        <asp:Button ID="btnTramaApertura" runat="server" Text="Trama Apertura" onclick="btnTramaApertura_Click" />
        <asp:Button ID="btnTramaReserva" runat="server" Text="Trama Reserva" onclick="btnTramaReserva_Click" />
        <asp:Button ID="btnTramaLiqui" runat="server" Text="Trama Liquidacion" onclick="btnTramaLiqui_Click" />
    
    </div>
    
    </form>
</body>
</html>
