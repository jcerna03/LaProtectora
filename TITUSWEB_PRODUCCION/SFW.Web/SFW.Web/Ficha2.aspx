<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ficha2.aspx.cs" Inherits="SFW.Web.Ficha2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Ficha de Datos</title>
<style type="text/css">
.Estilo1 {
	font-family: Arial Narrow;
	font-weight: bold;  
}
.Estilo2 {
	font-family: Arial Narrow;
	font-size: 13px;
	font-weight: bold;
}
.Estilo3 {
	font-family: Arial Narrow;
	font-size: 13px;
}
.Estilo4 {
	font-family: Arial Narrow;
	font-size: 12px;
}
.Estilo5 {
	font-family: Arial Narrow;
	font-size: 10px;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <table width="650" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo2">
        <tr>
            <td width="121" style="border-top:1px solid #000000;border-left:1px solid #000000;">
                <br />
            </td>
            <td width="540" style="border-top:1px solid #000000;border-right:1px solid #000000;">
                <br />
            </td>
        </tr>
        <tr>
            <td width="121" align="center" valign="middle" style="border-left:1px solid #000000;">
                 <asp:Image ID="Image2" runat="server" width="104" ImageUrl="~/image/NuevoLogoS.png" />
            </td>
            <td width="540" align="center" valign="middle" style="border-right:1px solid #000000;">&nbsp;FICHA DE DATOS PERSONALES</td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo2">
        <tr>
            <td width="650" align="center" style="border-left:1px solid #000000;border-right:1px solid #000000;">
            <br />
            <br />
            </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="121" align="center" valign="bottom" style="border-left:1px solid #000000;">
              <asp:Image ID="Image1" runat="server" width="104" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Height="130" ImageUrl="~/image/photo.png"  />
            </td>
            <td width="540" align="justify" valign="top" style="border-right:1px solid #000000;">
                <table width="540" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td bgcolor="#CCCCCC" class="Estilo2">&nbsp;1. Datos del Titular</td>
                    </tr>
                  </table>
                 <table width="540" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
                    <tr>
                      <td style="border-top:1px solid #000000; border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Nombres y Apellidos: <asp:Label ID="lblnombres" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblapellidopaterno" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblapellidomaterno" runat="server"></asp:Label></td>
                    </tr>
                  </table>
                  <table width="540" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
                    <tr>
                      <td style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Doc. de Identidad: <asp:Label ID="lblnumerodedocumento" runat="server"></asp:Label></td>
                      <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">
                        &nbsp;Sexo: M
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                        &nbsp;F
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                      </td>
                      <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;F. Nacimiento: <asp:Label ID="lblfechanacimiento" runat="server"></asp:Label></td>
                    </tr>
                  </table>
                  <table width="540" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
                    <tr>
                      <td style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Dirección: <asp:Label ID="lbldireccion" runat="server"></asp:Label></td>
                    </tr>
                  </table>
                  <table width="540" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
                    <tr>
                      <td width="394" style="border-left:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Distrito: <asp:Label ID="lbldistrito" runat="server"></asp:Label></td>
                      <td width="256" style="border-right:1px solid #000000; border-bottom:1px solid #000000;">Provincia: <asp:Label ID="lblprovincia" runat="server"></asp:Label></td>
                    </tr>
                  </table>
                  <table width="540" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
                    <tr>
                      <td style="border-left:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Departamento: <asp:Label ID="lbldepartamento" runat="server"></asp:Label></td>
                      <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">Teléfonos: <asp:Label ID="lbltelefonofijo" runat="server"></asp:Label>&nbsp;&nbsp;/&nbsp;&nbsp;<asp:Label ID="lbltelefonomovil" runat="server"></asp:Label></td>
                    </tr>
                  </table>
            </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td style="border-left:1px solid #000000; border-bottom:1px solid #000000;">
          <br />
          </td>
          <td style=" border-bottom:1px solid #000000;">
          <br />
          </td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">
          <br />
          </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo2">
        <tr>
          <td bgcolor="#CCCCCC">&nbsp;2. Datos de Afiliación</td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Código Cliente:&nbsp;<asp:Label ID="lblcodigocliente" runat="server"></asp:Label></td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Código Titular:&nbsp;<asp:Label ID="lblcodigotitular" runat="server"></asp:Label>
          </td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Categoría: <asp:Label ID="lblparentesco" runat="server"></asp:Label></td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
            <td style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Centro de Costo: <asp:Label ID="lblcentrodecosto" runat="server"></asp:Label></td>
            <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Estado:&nbsp;<asp:Label ID="lblEstado" runat="server"></asp:Label>
            </td>
            <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Plan:  <asp:Label ID="lblplan" runat="server"></asp:Label></td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Fecha Ingreso:&nbsp;<asp:Label ID="lblfechadealta" runat="server"></asp:Label></td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Fecha Baja:&nbsp;<asp:Label ID="lblfechadebaja" runat="server"></asp:Label>
          </td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">&nbsp;Fecha Carencia: <asp:Label ID="lblfechadecarencia" runat="server"></asp:Label></td>
        </tr>
      </table>
        <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td style="border-left:1px solid #000000; border-bottom:1px solid #000000;">
          <br />
          </td>
          <td style=" border-bottom:1px solid #000000;">
          <br />
          </td>
          <td style="border-right:1px solid #000000; border-bottom:1px solid #000000;">
          <br />
          </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0"  class="Estilo2">
        <tr>
          <td bgcolor="#CCCCCC">&nbsp;3. Dependientes</td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0">
<%--        <tr class="Estilo2">
          <td align="center" valign="top" style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">Apellido Paterno</td>
          <td align="center" valign="top" style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;">Apellido Materno</td>
          <td align="center" valign="top" style="border-right:1px solid #000000; border-bottom:1px solid #000000;">Nombres</td>
          <td align="center" valign="top" style="border-right:1px solid #000000; border-bottom:1px solid #000000;">Parentesco<br>(de ser el caso)</td>
          <td align="center" valign="top" style="border-right:1px solid #000000; border-bottom:1px solid #000000;">% Participaci&oacute;n</td>
        </tr>--%>
        <tr class="Estilo2">
          <td height="90" colspan="5" style="border-left:1px solid #000000; border-right:1px solid #000000; border-bottom:1px solid #000000;" valign="top">
            <asp:Label ID="lblbeneficiarios" runat="server" Text="No existen beneficiarios" Visible="false"></asp:Label>
            <asp:GridView ID="gvBeneficiarios" runat="server" Width="100%" AutoGenerateColumns="False" class="Estilo5" Visible="false">
                <Columns>
                    <asp:BoundField DataField="nombres" HeaderText="NOMBRES" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="parentesco" HeaderText="PARENTESCO" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="fch_naci" HeaderText="FECHA NACIMIENTO" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="sexo" HeaderText="SEXO" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="dni" HeaderText="DOCUMENTO IDENTIDAD" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ingreso" HeaderText="INGRESO" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="baja" HeaderText="BAJA" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="estado" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />

                </Columns>
            </asp:GridView>
          </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo2">
        <tr>
          <td bgcolor="#CCCCCC" align="center">&nbsp;DECLARACIÓN JURADA</td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td width="623" align="justify" valign="top" style="border-right:1px solid #000000;border-left:1px solid #000000;border-bottom:1px solid #000000;">
          Declaro que los datos consignados de mis dependientes son fidedignos, asimismo me comptrometo a informar a la empresa bajo mi responsabilidad si mi(s) hija(s) declarada(s) se encontrara(n)
          en estado de gestación o uno de mis hijos(as) cambie(n) de estado civil para su retiro automático de los programas médicos.<br />
          Asimismo autorizo a la empresa realizar los descuentos necesarios de mis remuneraciones de beneficio de cualquier indole, por concepto de cuotas de aportación, cargos por gastos excluidos,
           excesos de topes de coberturas y/o todo gasto por dependiente sin derecho al programa, de acuerdo al reglamento vigente, a las normas y políticas de descuento.<br />
          En caso de infracción, la empresa está en su facultad de aplicar las sanciones establecidas en el reglamento interno de trabajo y normas legales vigentes.
          </td>
        </tr>
      </table>
      <table width="650" border="0" cellspacing="0" cellpadding="0" class="Estilo3">
        <tr>
          <td width="75" height="70" style="border-left:1px solid #000000;">&nbsp;</td>
          <td width="251" align="center" >&nbsp;</td>
          <td width="66">&nbsp;</td>
          <td width="183" align="center" style="border-bottom:1px solid #000000;">&nbsp;</td>
          <td width="75" style="border-right:1px solid #000000;">&nbsp;</td>
        </tr>
        <tr>
          <td style="border-left:1px solid #000000;border-bottom:1px solid #000000;">&nbsp;</td>
          <td  style="border-bottom:1px solid #000000;">&nbsp;</td>
          <%--<td align="center">La Positiva Vida Seguros y Reaseguros</td>  style="border-bottom:1px solid #000000;"--%>
          <td style="border-bottom:1px solid #000000;">&nbsp;</td>
          <td align="center" style="border-bottom:1px solid #000000;">Firma del Trabajador</td>
          <td style="border-right:1px solid #000000;border-bottom:1px solid #000000;">&nbsp;</td>
        </tr>
      </table>
    </td>
  </tr>
</table>
    </form>
</body>
</html>
