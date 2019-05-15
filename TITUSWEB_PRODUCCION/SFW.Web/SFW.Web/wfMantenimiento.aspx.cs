using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SFW.BL;
using SFW.BE;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;
using System.Net;
using System.Collections;
using System.Threading;
using Tamir.SharpSsh;
using Tamir.Streams;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using IBM.WMQ;
using System.Xml.Serialization;

using pe.gob.susalud.jr.transaccion.susalud.bean;
using pe.gob.susalud.jr.transaccion.susalud.service.imp;
using pe.gob.susalud.jr.transaccion.susalud.validator;
using pe.gob.susalud.jr.transaccion.susalud.util;
using pe.gob.susalud.jr.transaccion.susalud.trama;
using java.util;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SFW.Web
{
    public partial class wfMantenimiento : System.Web.UI.Page
    {
        int resultado;
        int contador;
        string idUsuario;
        ReporteSUSALUD rs = new ReporteSUSALUD();
        Usuario usu = new Usuario();
        Datos dat = new Datos();
        public string NOMBRECOMPLETO = "";
        string rGuardar = HttpContext.Current.Server.MapPath("~/SUBIDOS2/");
        DataTable dt2 = new DataTable();
        DataTable dtab1, dtab2, dtab3 = new DataTable();

        DataTable dtEstructuraAfiliadoError = new DataTable
        {
            Columns = { new DataColumn("Afiliado", typeof(string)),
                        new DataColumn("Errores", typeof(string))
            }

        };


        void alertas()
        {
            string alrtas = "CALL SP_alertHijos('1','','1','','','" + DateTime.Today.ToString("yyyy-MM-dd") + "','');";
            DataTable dttt = dat.mysql(alrtas);
            lblTotal.Text = dttt.Rows.Count.ToString();
        }

        DataTable GenerarDataTable(string ruta)
        {

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {

                using (var stream = File.OpenRead(ruta))
                {
                    pck.Load(stream);
                }
                dynamic ws = pck.Workbook.Worksheets.First();
                System.Data.DataTable tbl = new System.Data.DataTable();
                bool hasHeader = true;

                // adjust it accordingly( i've mentioned that this is a simple approach)
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                dynamic startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    dynamic wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    dynamic row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }
        public void InicializarBotones()
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            txtBaja_CalendarExtender.StartDate = DateTime.Now.AddDays(-5);
            txtFechaBajaModal.Text = Convert.ToString(DateTime.Today);

            lblUsuario.Text = usu.NOMBRE;
            lblTotal.Text = "";

            lnkReporteAveria.Visible = false;
            btnImportar.Visible = false;
            lnkTramaSoat.Visible = false;
            btnVOIP.Visible = false;

            if (usu.AVERIA == "1")
            {
                lnkReporteAveria.Visible = true;
            }
            if (usu.ROL == "100")
            {
                btnReportes.Visible = true;
                lblRol.Text = " Administrador(a)";
            }

            if (usu.ROL == "50")
            {
                btnMovimientos.Visible = true;
                btnReportes.Visible = true;
                lblRol.Text = " ";
            }
            if (ddlTablas.SelectedValue == "15")
            {
                btnImportar.Visible = true;
            }
            if (ddlTablas.SelectedValue == "37")
            {
                lnkTramaSoat.Visible = true;
            }

            string validacionVoIP = "CALL validaciones_sp('1','" + usu.PERFIL + "','" + usu.ID + "','0');";
            DataTable dt = dat.mysql(validacionVoIP);
            if (dt.Rows.Count > 0)
            {
                btnVOIP.Visible = Convert.ToBoolean(dt.Rows[0]["Voip"]);
            }

            string validacionROL = "CALL validaciones_sp('2','" + usu.ROL + "','0','0');";
            DataTable dt2 = dat.mysql(validacionROL);

            if (dt2.Rows.Count > 0)
            {
                btnReportes.Visible = Convert.ToBoolean(dt2.Rows[0]["btnReportes"]);
                btnNuevo.Visible = Convert.ToBoolean(dt2.Rows[0]["btnNuevo"]);
                btnMovimientos.Visible = Convert.ToBoolean(dt2.Rows[0]["btnMovimientos"]);
                lblRol.Text = dt2.Rows[0]["lblRol"].ToString();
            }

            alertas();
            Cargarcombos(Session["USUARIO"].ToString());

            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue), Session["USUARIO"].ToString());

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USUARIO"] == null || Session["afiliado"] == null || Session["DATOS"] == null)
            {
                Response.Redirect("Sesion.aspx?usu=&pass=");
            }
            if (!Page.IsPostBack)
            {
                InicializarBotones();
                ddlMes.SelectedValue = DateTime.Now.Month.ToString();
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                if (Session["afiliado"].ToString() != "0")
                {
                    grupofamiliar(Session["afiliado"].ToString().Substring(2, 6), Session["afiliado"].ToString().Substring(0, 2));
                    hfCodigoCliente.Value = Session["afiliado"].ToString().Substring(0, 2);
                    hfCodigoTitular.Value = Session["afiliado"].ToString().Substring(2, 6);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#GRUPOFAMILIAR').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    Session["afiliado"] = "";
                }
            }
        }

        protected void Cargarcombos(string usu)
        {

            ddlTablas.DataSource = dat.mysql("call sp_fill(51,'" + usu + "',0,0)");
            ddlTablas.DataTextField = "descrip";
            ddlTablas.DataValueField = "valor";
            ddlTablas.DataBind();

            ddlDepartamento.DataSource = dat.mysql("call sp_fill(40,0,0,0)");
            ddlDepartamento.DataTextField = "descrip";
            ddlDepartamento.DataValueField = "valor";
            ddlDepartamento.DataBind();

            ddlAnio.DataSource = dat.mysql("call sp_fill(68,0,0,0)");
            ddlAnio.DataTextField = "descrip";
            ddlAnio.DataValueField = "descrip";
            ddlAnio.DataBind();

            ddlMes.DataSource = dat.mysql("call sp_fill(67,0,0,0)");
            ddlMes.DataTextField = "descrip";
            ddlMes.DataValueField = "valor";
            ddlMes.DataBind();

            //=======================================================================

            ddlEstadoCivil.DataSource = dat.mysql("call sp_fill(44,0,0,0)");
            ddlEstadoCivil.DataValueField = "valor";
            ddlEstadoCivil.DataTextField = "descrip";
            ddlEstadoCivil.DataBind();

            //=======================================================================

            ddlTipoDocumento.DataSource = dat.mysql("call sp_fill(45,0,0,0)");
            ddlTipoDocumento.DataValueField = "valor";
            ddlTipoDocumento.DataTextField = "descrip";
            ddlTipoDocumento.DataBind();

            //=======================================================================

            ddlOnco.DataSource = dat.mysql("call sp_fill(46,0,0,0)");
            ddlOnco.DataValueField = "valor";
            ddlOnco.DataTextField = "descrip";
            ddlOnco.DataBind();

            //=======================================================================

            ddlCategoria.DataSource = dat.mysql("call sp_fill(47,0,0,0)");
            ddlCategoria.DataValueField = "valor";
            ddlCategoria.DataTextField = "descrip";
            ddlCategoria.DataBind();

            //=======================================================================

            ddlSexo.DataSource = dat.mysql("call sp_fill(49,0,0,0)");
            ddlSexo.DataValueField = "valor";
            ddlSexo.DataTextField = "descrip";
            ddlSexo.DataBind();

            //=======================================================================

            ddlPad.DataSource = dat.mysql("call sp_fill(61,0,0,0)");
            ddlPad.DataValueField = "VALOR";
            ddlPad.DataTextField = "DESCRIP";
            ddlPad.DataBind();

            ddlTipoDocu.DataSource = dat.mysql("CALL SP_UpdateTitu(4,'','','','','','','','','','','','','','','')");
            ddlTipoDocu.DataValueField = "VALOR";
            ddlTipoDocu.DataTextField = "DESCRIP";
            ddlTipoDocu.DataBind();

            ddlTipoDocu2.DataSource = dat.mysql("CALL SP_UpdateTitu(4,'','','','','','','','','','','','','','','')");
            ddlTipoDocu2.DataValueField = "VALOR";
            ddlTipoDocu2.DataTextField = "DESCRIP";
            ddlTipoDocu2.DataBind();

            ddlClasificacion.DataSource = dat.mysql("CALL SP_UpdateTitu(7,'','','','','','','','','','','','','','','')");
            ddlClasificacion.DataValueField = "VALOR";
            ddlClasificacion.DataTextField = "DESCRIP";
            ddlClasificacion.DataBind();


        }

        protected void limpiar()
        {
            lblverificacion.Text = "";
            lblBD.Text = "";
            txtApellidop.Text = "";
            txtApellidom.Text = "";
            txtNacimiento.Text = "";
            txtNombres.Text = "";
            txtDireccion.Text = "";
            txtTelefono1.Text = "";
            txtTelefono2.Text = "";
            txtDNI.Text = "";
            txtObservar.Text = "";
            txtAlta.Text = "";
            txtBaja.Text = "";
            txtContraseña.Text = "";
            txtSangre.Text = "";
            txtEdad.Text = "";
            txtPeso.Text = "";
            txtEstatura.Text = "";
            lblverificacion.Text = "";
            lblverificacion1.Text = "";

            txtCodPaciente.Text = "";
            txtIdPaciente.Text = "";
            ddlTipoDocumento.SelectedValue = "0";
            ddlEstadoCivil.SelectedValue = "0";
            txtCorreo1.Text = "";
            txtCorreo2.Text = "";
            txtCarencia.Text = "";

        }

        protected void borrarmensajes()
        {
            lblalerta.Text = "";
            lblCorrecto.Text = "";
            lblError.Text = "";
            lblErrorReg.Text = "";
            duplicado.Visible = false;
            lbldupli.Text = "";
            lblCodigoTitularCarac.Text = "";
            lblCodigoTitularCorrecto.Text = "";
            lblCodigoTitularNoHay.Text = "";
        }

        protected void CombosCliente(string cliente, string plan)
        {
            //===========================================================================

            ddlPlan.DataSource = dat.mysql("call sp_fill(43,'" + cliente + "','" + plan + "','')");
            ddlPlan.DataValueField = "valor";
            ddlPlan.DataTextField = "descrip";
            ddlPlan.DataBind();

            //=======================================================================

            ddlCentro.DataSource = dat.mysql("call sp_fill(50,'" + cliente + "',0,0)");
            ddlCentro.DataValueField = "valor";
            ddlCentro.DataTextField = "descrip";
            ddlCentro.DataBind();

            Ddl_ClasifAviso.DataSource = dat.mysql("CALL sp_avisos2(6, '" + cliente + "','','','','','','','','')");
            Ddl_ClasifAviso.DataValueField = "VALOR";
            Ddl_ClasifAviso.DataTextField = "VALOR01";
            Ddl_ClasifAviso.DataBind();

            ddlClasificacion.DataSource = dat.mysql("CALL SP_UpdateTitu(7,'','','','','','','','','','','','','','','')");
            ddlClasificacion.DataValueField = "VALOR";
            ddlClasificacion.DataTextField = "DESCRIP";
            ddlClasificacion.DataBind();
        }

        protected void Filtro(string busqueda, string cliente, string usuario)
        {
            DataTable dt = dat.mysql("call sp_fill(7,'" + busqueda + "','" + usuario + "','" + cliente + "')");
            GridView2.DataSource = dt;
            GridView2.DataBind();

            if (GridView2.Rows.Count != 0)
            {
                lblContador.Text = "Registros Mostrados: " + dt.Rows.Count.ToString();
            }
            else
            {
                lblContador.Text = "Registros Mostrados: 0";
            }
        }

        protected void ddlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnImportar.Visible = false;
            lnkTramaSoat.Visible = false;
            if (ddlTablas.SelectedValue == "15")
            {
                btnImportar.Visible = true;
            }
            if (ddlTablas.SelectedValue == "37")
            {
                lnkTramaSoat.Visible = true;
            }
            CombosCliente(ddlTablas.SelectedValue, "0");
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue), Session["USUARIO"].ToString());
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue), Session["USUARIO"].ToString());
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

            if (ddlTablas.SelectedValue == "00")
            {
                lblErrorReg.Text = "ADVERTENCIA ,Seleccione un cliente para poder crear un nuevo registro.";
                return;
            }
            else
            {

                Image1.ImageUrl = "~/image/photo.png";
                borrarmensajes();
                limpiar();
                RecordConsumoTab.Visible = false;
                AvisosTab.Visible = false;
                CartasTab.Visible = false;
                txtNumeroPoli.Text = Convert.ToString(ddlTablas.SelectedValue);
                txtNombreEmpresa.Text = ddlTablas.SelectedItem.Text.ToString().Substring(3);
                txtCodigoTitu.Text = "";
                txtCodigoTitu.ReadOnly = false;
                btnGuardarModificar.Visible = false;
                btnBajaModal.Visible = false;
                ddlSexo.SelectedValue = "M";
                ddlEstadoCivil.SelectedValue = "0";
                ddlCategoria.SelectedIndex = 0;
                if (txtNumeroPoli.Text == "57")
                {
                    divContratante.Visible = true;
                }
                ddlCategoria.Attributes.Add("readonly", "readonly");
                ddlCategoria.CssClass = "form-control input-sm disabled-button";
                divClasificacion.Attributes.Add("style", "display:initial;");
                lblAfiliado.Text = "NUEVO TITULAR";
                btnGuardarRegistrar.Visible = true;
                divchkSusalud.Visible = true;
                txtAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddlDepartamento.SelectedValue = "15";
                ddlDepartamento_SelectedIndexChanged(sender, e);
                lnkBD.Visible = true;

                //=======================================================================

                ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                ddlCategoria.DataValueField = "valor";
                ddlCategoria.DataTextField = "descrip";
                ddlCategoria.DataBind();

                CombosCliente(ddlTablas.SelectedValue, "0");

                id_paciente.Visible = false;
                cod_paciente.Visible = false;
                concubina.Visible = false;
                lnkActualizacionTitu2.Visible = false;

                switch (txtNumeroPoli.Text)
                {
                    case "90":
                    case "96":
                    case "98":
                        ocultos2.Visible = true;
                        rol.Visible = true;
                        dpto.Visible = true;
                        programa.Visible = true;
                        documentoSIMA.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        lnkBuscarTitular.Visible = true;
                        ddlCentro.SelectedIndex = 0;
                        ddlPlan.SelectedIndex = 0;
                        ddlPespecial.SelectedIndex = 0;
                        txtRol.Text = "99";
                        txtDpto.Text = "9999";
                        txtDocumento.Text = "";
                        txtObservar.Text = "";
                        txtTelefono1.Text = "";
                        txtTelefono2.Text = "";
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "11":
                        documentoSIMA.Visible = true;
                        txtCarencia.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                        ocultos2.Visible = false;
                        rol.Visible = false;
                        dpto.Visible = false;
                        programa.Visible = false;
                        pad.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        campo2.Visible = false;
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "26":
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        pad.Visible = false;
                        campo2.Visible = false;
                        txtCarencia.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "57":
                        ocultos2.Visible = true;
                        segundacapa.Visible = true;
                        basico.Visible = true;
                        onco.Visible = true;
                        documentoSIMA.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        pad.Visible = false;
                        campo2.Visible = false;
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "15":
                        txtCarencia.Text = DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
                        campo2.Visible = true;
                        ocultos2.Visible = false;
                        rol.Visible = false;
                        dpto.Visible = false;
                        programa.Visible = false;
                        pad.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "70":
                    case "44":
                        txtCarencia.Text = "";
                        ocultos2.Visible = false;
                        rol.Visible = false;
                        dpto.Visible = false;
                        programa.Visible = false;
                        pad.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        campo2.Visible = false;
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "37":
                        txtCodigoTitu.ReadOnly = true;
                        lnkBuscarTitular.Enabled = false;
                        txtNacimiento.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        lnkImpreOrden.Visible = false;
                        lnkFichaPersonal.Visible = false;
                        break;
                    default:
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        txtCarencia.Text = "";
                        ocultos2.Visible = false;
                        rol.Visible = false;
                        dpto.Visible = false;
                        programa.Visible = false;
                        pad.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
                        campo2.Visible = false;
                        break;

                }

                ddlPlan_SelectedIndexChanged(sender, e);

                if (usu.ROL == "50")
                {
                    btnGuardarModificar.Attributes.Add("disabled", "disabled");
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#NUEVOAFILIADO').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }


        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue), idUsuario);
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProvincia.DataSource = new ComboBL().ListaCombos(41, ddlDepartamento.SelectedValue);
            ddlProvincia.DataValueField = "valor";
            ddlProvincia.DataTextField = "descrip";
            ddlProvincia.DataBind();

            ddlProvincia_SelectedIndexChanged(sender, e);
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDistrito.DataSource = new ComboBL().ListaCombos(42, ddlProvincia.SelectedValue);
            ddlDistrito.DataValueField = "valor";
            ddlDistrito.DataTextField = "descrip";
            ddlDistrito.DataBind();
        }

        string extensionFoto;

        protected void btnGuardarRegistrar_Click(object sender, EventArgs e)
        {
            borrarmensajes();
            extensionFoto = "";
            string resultadoSusalud = "";
            string estadoSuSalud = "1"; //NO REGISTRADO
            string operacion = "";

            string fotoregistro = "";
            string categoriaReal = "";


            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            string strDni = "CALL sp_fill('82','" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + txtDNI.Text + "');";
            DataTable dtficha = dat.mysql(strDni);
            if (Convert.ToInt32(dtficha.Rows[0][0]) > 0)
            {
                lblverificacion.Text = "Duplicidad el afiliado ya existe (DNI duplicado en grupo familiar)";
                lblverificacion1.Text = "Duplicidad el afiliado ya existe (DNI duplicado en grupo familiar)";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#NUEVOAFILIADO').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                return;
            }
            if (txtDNI.Text == "" || txtCodigoTitu.Text == "" || txtApellidop.Text == "" || txtNombres.Text == "" || txtNacimiento.Text == "")
            {
                lblverificacion.Text = "Debe ingresar todos los campos obligatorios(*)";
                lblverificacion1.Text = "Debe ingresar todos los campos obligatorios(*)";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#NUEVOAFILIADO').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                return;
            }
            else
            {
                if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96") || (txtNumeroPoli.Text == "95"))
                {
                    if (ddlCategoria.SelectedValue != "00" && ddlCategoria.SelectedValue != "04")
                    {
                        if (txtCodPaciente.Text == "")
                        {
                            lblverificacion.Text = "Debe ingresar todos los campos obligatorios(*)";
                            lblverificacion1.Text = "Debe ingresar todos los campos obligatorios(*)";
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script type='text/javascript'>");
                            sb.Append("$('#NUEVOAFILIADO').modal('show');");
                            sb.Append("</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                            return;
                        }
                    }
                    if (ddlCategoria.SelectedValue != "00" && ddlCategoria.SelectedValue == "04")
                    {
                        if (txtCodPaciente.Text == "" || txtIdPaciente.Text == "")
                        {
                            lblverificacion.Text = "Debe ingresar todos los campos obligatorios(*)";
                            lblverificacion1.Text = "Debe ingresar todos los campos obligatorios(*)";
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script type='text/javascript'>");
                            sb.Append("$('#NUEVOAFILIADO').modal('show');");
                            sb.Append("</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                            return;
                        }
                    }
                }

                Titular obj = new Titular();
                Titular_Detalle obj_det = new Titular_Detalle();

                obj.cod_cliente = txtNumeroPoli.Text;
                obj.cod_titula = txtCodigoTitu.Text;
                obj.categoria = ddlCategoria.SelectedValue;
                obj.centro_costo = ddlCentro.SelectedValue;
                obj.plan = Convert.ToInt32(ddlPlan.SelectedValue);
                obj.afiliado = txtApellidop.Text + " " + txtApellidom.Text + "," + txtNombres.Text;
                obj.parentesco = ddlCategoria.SelectedItem.Text;
                obj.sexo = ddlSexo.SelectedValue;
                obj.fch_naci = txtNacimiento.Text;
                obj.fch_alta = txtAlta.Text;
                obj.fch_baja = txtBaja.Text;
                obj.fch_proc = Convert.ToString(DateTime.Today.ToShortDateString());
                obj.pass = txtDNI.Text;
                obj.email = txtObservar.Text;
                obj.tipo_doc = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                obj.dni = txtDNI.Text;
                obj.madre = "0";
                obj.actividad = "0";
                obj.ubicacion = "0";
                obj.estado_titular = 1;
                obj.capitados = "0";
                obj.financia = "0";
                obj.oncologico = ddlOnco.SelectedValue;
                obj.dx_onco = "0";
                obj.campo1 = "0";
                obj.campo2 = txtCampo2.Text;
                obj.campo3 = "0";
                obj.fch_caren = txtCarencia.Text;
                obj_det.cod_cliente = txtNumeroPoli.Text;
                obj_det.cod_titula = txtCodigoTitu.Text;
                obj_det.categoria = ddlCategoria.SelectedValue;
                obj_det.depa_id = ddlDepartamento.SelectedValue;
                obj_det.prov_id = ddlProvincia.SelectedValue;
                obj_det.dist_id = ddlDistrito.SelectedValue;
                obj_det.direccion = txtDireccion.Text;
                obj_det.email = txtObservar.Text;
                obj_det.t_fijo = txtTelefono1.Text;
                obj_det.t_movil = txtTelefono2.Text;
                obj_det.estado_civil = Convert.ToInt32(ddlEstadoCivil.SelectedValue);
                obj_det.edad = txtEdad.Text;
                obj_det.peso = txtPeso.Text;
                obj_det.estatura = txtEstatura.Text;
                obj_det.discapacitado = ddlDiscapacit.SelectedValue;
                obj_det.consume_alcohol = ddlAlcohol.SelectedValue;
                obj_det.consume_tabaco = ddlDrogas.SelectedValue;
                obj_det.grupo_sanguineo = txtSangre.Text;
                obj_det.fch_fincarencia = txtCarencia.Text;
                obj_det.pad = ddlPad.SelectedValue;
                obj_det.dpto = txtDpto.Text;
                obj_det.rol = txtRol.Text;
                obj_det.prog_especial = ddlPespecial.SelectedValue;
                obj_det.cod_paciente = txtCodPaciente.Text;
                obj_det.id_paciente = txtIdPaciente.Text;
                obj_det.basico = ddlBasico.SelectedValue;
                obj_det.onco = ddlOnco.SelectedValue;
                obj_det.segunda_capa = ddlCapa.SelectedValue;
                obj_det.docum = txtDocumento.Text;
                obj_det.afi_nombre = txtNombres.Text;
                obj_det.afi_apepat = txtApellidop.Text;
                obj_det.afi_apemat = txtApellidom.Text;
                obj_det.correo1 = txtCorreo1.Text;
                obj_det.correo2 = txtCorreo2.Text;
                obj_det.contrato = obj.cod_cliente + obj.cod_titula;
                obj_det.clasificacion = ddlClasificacion.SelectedValue;
                obj_det.contratante = "0";
                obj_det.categoriaSusalud = ddlCategoria.SelectedValue;
                obj_det.causalBaja = "";
                obj_det.estado_afiliado = "1";
                obj_det.estado_afiliacion = "1";
                obj_det.fallecido = "";

                if (chkContratante.Checked)
                {
                    obj_det.contratante = "1";
                }
                // VALIDACION PARA CATEGORIA HIJO(A) A SUSALUD
                if (obj.categoria == "04")
                {
                    if (obj.cod_cliente == "90")
                    {
                        obj_det.categoriaSusalud = (Convert.ToInt32(txtIdPaciente.Text) + 3).ToString("00");
                    }
                    else
                    {
                        DataTable dt = dat.mysql("call sp_fill_3('1','" + obj.cod_cliente + "','" + obj.cod_titula + "','','','','','','','','')");
                        if (dt.Rows.Count > 0)
                        {
                            obj_det.categoriaSusalud = (Convert.ToInt32(dt.Rows[0][0]) + 1).ToString("00");
                        }
                    }
                }
                obj_det.cod_afiliado = obj.cod_cliente + obj.cod_titula + obj_det.categoriaSusalud;

                // VALIDACION DE CONTRATO PARA PAMI DEPENDIENTES
                if (obj.cod_cliente == "96")
                {
                    if (obj.categoria == "00")
                    {
                        estadoSuSalud = "3";
                        chkSusalud.Checked = false;
                    }
                    else
                    {
                        obj_det.categoriaSusalud = "00";
                        DataTable dt = dat.mysql("call sp_fill_3('2','" + obj.cod_cliente + "','" + obj.cod_titula + "','','','','','','','','')");
                        if (dt.Rows.Count > 0)
                        {
                            obj_det.contrato = obj.cod_cliente + obj.cod_titula + "-" + dt.Rows[0][0].ToString();
                        }
                    }
                }
                // VALIDACION PARA FOTOS 
                if (Page.IsPostBack)
                {
                    fotoregistro = "";
                    if (file2.Value != "")
                    {
                        extensionFoto = Path.GetExtension(file2.Value).ToString();

                        if (ddlCategoria.SelectedValue == "04")
                        {
                            categoriaReal = ddlCategoria.SelectedValue + obj_det.id_paciente;
                        }
                        else
                        {
                            categoriaReal = ddlCategoria.SelectedValue;
                        }

                        if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96"))
                        {
                            fotoregistro = "90" + "-" + obj.cod_titula + "-" + categoriaReal + extensionFoto;
                        }
                        else
                        {
                            fotoregistro = obj.cod_cliente + "-" + obj.cod_titula + "-" + categoriaReal + extensionFoto;
                        }
                    }

                }

                obj_det.foto = fotoregistro;
                hfNombres.Value = txtNombres.Text;
                hfApellidoPaterno.Value = txtApellidop.Text;
                hfApellidoMaterno.Value = txtApellidom.Text;

                // validacion del tipo de operacion para REGISTRAR a SUSALUD
                DataTable dtope = new DataTable();
                string qryope = "CALL sp_fill_3('4','" + obj.cod_cliente + "','" + txtDNI.Text + "','','','','','','','','');";
                dtope = dat.mysql(qryope);
                operacion = dtope.Rows[0]["OP"].ToString();

                if (chkSusalud.Checked == true)
                {
                    resultadoSusalud = rs.EnvioSUSALUD(operacion, obj, obj_det);
                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        lblverificacion.Text = resultadoSusalud;
                        lblverificacion1.Text = resultadoSusalud;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#NUEVOAFILIADO').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                        return;
                    }
                    estadoSuSalud = "5";
                }


                string resultado = new TitularBL().InsertarTitular(obj, obj_det, usu, 1);
                int result = new TitularBL().RegistrarFechaRenovacionAlta(obj, obj_det);
                // ACTUALIZA EL COD AFILIADO, CONTRATO Y ESTADO
                string stroe3 = "CALL sp_fill_3('5','" +
                    obj.cod_cliente + "','" +
                    obj.cod_titula + "','" +
                    obj_det.categoriaSusalud + "','" +
                    obj.dni + "','" +
                    obj_det.contrato + "','" +
                    obj_det.cod_afiliado + "','" +
                    estadoSuSalud + "','','','');";
                DataTable dtab3 = dat.mysql(stroe3);
                // ACTUALIZAR FOTO DE CÓNYUGE
                if (obj.categoria == "01")
                {
                    string codigoTemporal = obj.cod_cliente;

                    if ((obj.cod_cliente == "98") || (obj.cod_cliente == "96"))
                    {
                        codigoTemporal = "90";
                    }

                    string xSQL = "call sp_fill(74,'" + codigoTemporal + "','" + obj.cod_titula + "',0);";
                    DataTable dt = dat.mysql(xSQL);
                    if (dt.Rows[0]["foto"].ToString() != "0" && dt.Rows[0]["foto"].ToString().Trim() != "")
                    {
                        string archivo = codigoTemporal + "-" + obj.cod_titula + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4);
                        string elimino = DeleteFile(codigoTemporal + "-" + obj.cod_titula + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4), txtNumeroPoli.Text);
                    }
                }

                if (resultado == "1")
                {
                    lblverificacion.Text = "Titular registrado satisfactoriamente";
                    lblverificacion2.Text = "Titular registrado satisfactoriamente";
                    limpiar();
                    Filtro(txtBusqueda.Text, obj.cod_cliente, idUsuario);

                    if (obj.cod_cliente == "37")
                    {
                        limpiarCartasModal();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#CARTASMODAL').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                }
                else
                {
                    lblverificacion.Text = resultado;
                    lblverificacion1.Text = resultado;
                    Filtro(txtBusqueda.Text, obj.cod_cliente, idUsuario);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#NUEVOAFILIADO').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }



            }

        }

        public void limpiarCartasModal()
        {
            txtNroSiniestro.Text = "";
            txtPoliza.Text = "";
            txtPlaca.Text = "";
            txtComisaria.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtMonto.Text = "";
            txtPersonaLlamaNombre.Text = "";
            txtPersonaLlamaTelefono.Text = "";
            lblVerificacion22.Text = "";
            lblverificacion.Text = "";
        }

        protected void grupofamiliar(string cod_titula, string cliente)
        {
            gvGrupoFamiliar.DataSource = new TitularBL().ListarGrupoFamiliar(777, cliente, cod_titula);
            gvGrupoFamiliar.DataBind();
        }

        protected void btnGuardarModificar_Click(object sender, EventArgs e)
        {
            extensionFoto = "";
            borrarmensajes();
            string resultadoSusalud = "";
            string estadoSuSalud = ""; //NO REGISTRADO
            List<Titular> lstTitular = new List<Titular>();
            List<Titular_Detalle> lstTitularDetalle = new List<Titular_Detalle>();
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            // ES CAMBIO DE PLAN
            if (planHidden.Value == "1")
            {
                lstTitular = new TitularBL().ListarTitularesGrupo(532, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                lstTitularDetalle = new TitularDetalleBL().ListarDetallesGrupoFamiliar(542, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                List<string> listNuevaBaja = new List<string>();

                if (lstTitular.Count > 0 && lstTitularDetalle.Count > 0 && lstTitular.Count == lstTitularDetalle.Count)
                {
                    for (int i = 0; i < lstTitular.Count; i++)
                    {
                        Titular obj = lstTitular[i];
                        Titular_Detalle obj_deta = lstTitularDetalle[i];
                        listNuevaBaja.Add(obj.fch_baja);
                        obj.fch_baja = txtAlta.Text;
                        obj_deta.causalBaja = "7";
                        obj_deta.estado_afiliado = "1";
                        obj_deta.estado_afiliacion = "2";

                        int resultado = new TitularBL().RegistrarFechaRenovacionBaja(obj);
                        resultadoSusalud = rs.EnvioSUSALUD("21", obj, obj_deta);
                        if (resultadoSusalud.Contains("ERROR"))
                        {
                            continue;
                        }



                    }
                    for (int i = lstTitular.Count - 1; i >= 0; i--)
                    {
                        Titular obj = lstTitular[i];
                        Titular_Detalle obj_deta = lstTitularDetalle[i];
                        Titular OLD_obj = obj;
                        Titular_Detalle OLD_obj_deta = obj_deta;

                        string strCont = "CALL sp_fecha_bajas(3,'" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','" + ddlPlan.SelectedValue.ToString() + "','','','','','','');";
                        DataTable dt = null;
                        dt = dat.mysql(strCont);

                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            obj_deta.cod_afiliado = obj.cod_cliente + obj.cod_titula + obj.categoria + "-" + dt.Rows[0][0].ToString();
                        }

                        if (obj_deta.cod_afiliado == "")
                        {
                            obj_deta.cod_afiliado = obj.cod_cliente + obj.cod_titula + obj.categoria;
                        }
                        if (obj_deta.contrato == "")
                        {
                            obj_deta.contrato = obj.cod_cliente + obj.cod_titula;
                        }

                        obj.fch_baja = listNuevaBaja[i];
                        obj.plan = Convert.ToInt32(ddlPlan.SelectedValue);
                        obj_deta.onco = ddlOnco.SelectedValue;
                        obj_deta.estado_afiliado = "1";
                        obj_deta.estado_afiliacion = "1";


                        DataTable dtv = dat.mysql("CALL sp_fill_3(10,'" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','','','','','','','');");
                        if (dtv.Rows[0][0].ToString() == "1")
                        {
                            resultadoSusalud = rs.EnvioSUSALUD("00", obj, obj_deta);
                        }

                        if (resultadoSusalud.Contains("ERROR"))
                        {
                            continue;
                        }
                        estadoSuSalud = "5";
                        resultado = new TitularBL().RegistrarFechaRenovacionAlta(obj, obj_deta);
                        resultado = new TitularBL().ActualizarTitular(obj, obj_deta, usu, 1, OLD_obj, OLD_obj_deta);
                        if (resultado == 1)
                        {
                            //// actualiza estado ln susalud
                            string stroe2 = "CALL sp_fill_3('5','" +
                                   obj.cod_cliente + "','" +
                                   obj.cod_titula + "','" +
                                   obj.categoria + "','" +
                                   obj.dni + "','" +
                                   obj_deta.contrato + "','" +
                                   obj_deta.cod_afiliado + "','" +
                                   estadoSuSalud + "','','','');";
                            DataTable dtab2 = dat.mysql(stroe2);

                            if ((OLD_obj.plan == 1 && obj.plan == 2) && (!(obj_deta.onco == "C" || obj_deta.onco == "P")) && (obj.cod_cliente == "90" || obj.cod_cliente == "98" || obj.cod_cliente == "96" || obj.cod_cliente == "95"))
                            {
                                string xSQL = "call sp_avisos (2,'" + obj.cod_cliente.ToString() + "','" + obj.cod_titula.ToString() + "','" + obj.categoria.ToString() + "','Afiliado podrá utilizar el nuevo tope de cobertura del Plan B a partir del " + DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy") + " ','" + Session["USUARIO"].ToString() + "','')";
                                DataTable dt2 = dat.mysql(xSQL);
                            }
                        }

                    }
                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        lblverificacion.Text = resultadoSusalud;
                        lblverificacion1.Text = resultadoSusalud;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#NUEVOAFILIADO').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                        return;
                    }
                    else
                    {

                        Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
                        grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                        lblCorrecto.Text = "Cambio de plan satisfactorio";
                        limpiar();
                        gAnio.Visible = false;
                        gMeses.Visible = false;
                        lblFinal.Text = "";
                        lblEvoDet.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "javascript:__doPostBack('btnCerrar','')", true);

                    }
                }

            }
            else
            {

                lstTitular = new TitularBL().ListarTitularesGrupo(53, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                lstTitularDetalle = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

                Titular OLD_obj = lstTitular.First(delegate (Titular objTI) { return (objTI.cod_cliente.Equals(txtNumeroPoli.Text) && (objTI.cod_titula.Equals(txtCodigoTitu.Text) && (objTI.categoria.Equals(categoriaHidden.Value)))); });
                Titular_Detalle OLD_obj_deta = lstTitularDetalle.First(delegate (Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });

                Titular obj = lstTitular.First(delegate (Titular objTI) { return (objTI.cod_cliente.Equals(txtNumeroPoli.Text) && (objTI.cod_titula.Equals(txtCodigoTitu.Text) && (objTI.categoria.Equals(categoriaHidden.Value)))); });
                Titular_Detalle obj_det = lstTitularDetalle.First(delegate (Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });


                obj.cod_cliente = Convert.ToString(txtNumeroPoli.Text);
                obj.cod_titula = txtCodigoTitu.Text;
                obj.categoria = categoriaHidden.Value;
                obj.centro_costo = ddlCentro.SelectedValue;
                obj.plan = Convert.ToInt32(ddlPlan.SelectedValue);
                obj.afiliado = txtApellidop.Text + " " + txtApellidom.Text + "," + txtNombres.Text;
                obj.parentesco = ddlCategoria.SelectedItem.Text;
                obj.sexo = ddlSexo.SelectedValue;
                obj.fch_naci = txtNacimiento.Text;
                obj.fch_alta = txtAlta.Text;
                obj.fch_baja = txtBaja.Text;
                obj.fch_proc = Convert.ToString(DateTime.Today);
                obj.pass = txtContraseña.Text;
                obj.email = txtObservar.Text;
                obj.tipo_doc = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                obj.dni = txtDNI.Text;
                obj.madre = "0";
                obj.actividad = "0";
                obj.ubicacion = "0";
                obj.estado_titular = 1;
                obj.capitados = "0";
                obj.financia = "0";
                obj.oncologico = ddlOnco.SelectedValue;
                obj.dx_onco = "0";
                obj.campo1 = "0";
                obj.campo2 = txtCampo2.Text;
                obj.campo3 = "0";
                obj.fch_caren = txtCarencia.Text;
                obj_det.cod_cliente = Convert.ToString(txtNumeroPoli.Text);
                obj_det.cod_titula = txtCodigoTitu.Text;
                obj_det.categoria = categoriaHidden.Value;
                obj_det.depa_id = ddlDepartamento.SelectedValue;
                obj_det.prov_id = ddlProvincia.SelectedValue;
                obj_det.dist_id = ddlDistrito.SelectedValue;
                obj_det.direccion = txtDireccion.Text;
                obj_det.email = txtObservar.Text;
                obj_det.t_fijo = txtTelefono1.Text;
                obj_det.t_movil = txtTelefono2.Text;
                obj_det.estado_civil = Convert.ToInt32(ddlEstadoCivil.SelectedValue);
                obj_det.edad = txtEdad.Text;
                obj_det.peso = txtPeso.Text;
                obj_det.estatura = txtEstatura.Text;
                obj_det.discapacitado = ddlDiscapacit.SelectedValue;
                obj_det.consume_alcohol = ddlAlcohol.SelectedValue;
                obj_det.consume_tabaco = ddlDrogas.SelectedValue;
                obj_det.grupo_sanguineo = txtSangre.Text;
                obj_det.fch_fincarencia = txtCarencia.Text;
                obj_det.pad = ddlPad.SelectedValue;
                obj_det.dpto = txtDpto.Text;
                obj_det.rol = txtRol.Text;
                obj_det.prog_especial = ddlPespecial.SelectedValue;
                obj_det.cod_paciente = txtCodPaciente.Text;
                obj_det.id_paciente = txtIdPaciente.Text;
                obj_det.basico = ddlBasico.SelectedValue;
                obj_det.onco = ddlOnco.SelectedValue;
                obj_det.segunda_capa = ddlCapa.SelectedValue;
                obj_det.docum = txtDocumento.Text;
                obj_det.afi_nombre = txtNombres.Text;
                obj_det.afi_apepat = txtApellidop.Text;
                obj_det.afi_apemat = txtApellidom.Text;
                obj_det.correo1 = txtCorreo1.Text;
                obj_det.correo2 = txtCorreo2.Text;
                obj_det.clasificacion = ddlClasificacion.SelectedValue;
                if (chkContratante.Checked)
                {
                    obj_det.contratante = "1";
                }
                else
                {
                    obj_det.contratante = "0";
                }


                string fotoActualizacion = "";
                if (Page.IsPostBack)
                {
                    if (file2.Value != "")
                    {
                        extensionFoto = Path.GetExtension(file2.Value).ToString();
                        if (extensionFoto != "")
                        {
                            if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96"))
                            {
                                fotoActualizacion = "90" + "-" + txtCodigoTitu.Text + "-" + categoriaHidden.Value + extensionFoto;
                                obj_det.foto = fotoActualizacion;
                                subeFotos("90", txtCodigoTitu.Text, categoriaHidden.Value);
                            }
                            else
                            {
                                fotoActualizacion = txtNumeroPoli.Text + "-" + txtCodigoTitu.Text + "-" + categoriaHidden.Value + extensionFoto;
                                obj_det.foto = fotoActualizacion;
                                subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                            }
                        }
                        else
                        {
                            fotoActualizacion = obj_det.foto;
                            extensionFoto = fotoActualizacion.Substring(fotoActualizacion.Length - 4, 4);
                        }
                    }
                    else
                    {
                        if (obj_det.foto != "")
                        {
                            fotoActualizacion = obj_det.foto;
                        }
                        else
                        {
                            fotoActualizacion = "";
                        }
                    }
                }
                obj_det.foto = fotoActualizacion;
                obj_det.estado_afiliado = "1";
                obj_det.estado_afiliacion = "1";
                // Si es dependiente de un fallecido
                if (obj_det.contrato.Length > 0)
                {
                    if (obj_det.contrato.Substring(obj_det.contrato.Length - 1, 1) == "F")
                    {
                        obj_det.categoriaSusalud = "00";
                    }
                }
               

                resultadoSusalud = rs.EnvioSUSALUD("10", obj, obj_det);

                if (resultadoSusalud.Contains("ERROR"))
                {
                    lblverificacion.Text = resultadoSusalud;
                    lblverificacion1.Text = resultadoSusalud;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#NUEVOAFILIADO').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    return;
                }

                int resultado = new TitularBL().ActualizarTitular(obj, obj_det, usu, 1, OLD_obj, OLD_obj_deta);

                string call = "CALL sp_fill_3('15','" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','" + obj.fch_baja + "','" + obj.fch_alta + "','','','','','');";
                DataTable dtab = dat.mysql(call);

                if (resultado == 1)
                {

                    if ((OLD_obj.plan == 1 && obj.plan == 2) && (!((obj_det.onco == "C" || obj_det.onco == "P"))) && (obj.cod_cliente == "90" || obj.cod_cliente == "98" || obj.cod_cliente == "96" || obj.cod_cliente == "95"))
                    {
                        string xSQL = "call sp_avisos (2,'" + obj.cod_cliente.ToString() + "','" + obj.cod_titula.ToString() + "','" + categoriaHidden.Value + "','Afiliado podrá utilizar el nuevo tope de cobertura del Plan B a partir del   " + DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy") + " ','" + Session["USUARIO"].ToString() + "','')";
                        DataTable dt2 = dat.mysql(xSQL);
                    }

                    Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
                    grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                    lblCorrecto.Text = "Afiliado modificado satisfactoriamente";
                    limpiar();
                    gAnio.Visible = false;
                    gMeses.Visible = false;
                    lblFinal.Text = "";
                    lblEvoDet.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "javascript:__doPostBack('btnCerrar','')", true);

                }
                else
                {
                    lblError.Text = "Error de Modificación, refresque la página y vuelva a intentarlo, si el error persiste comunicarse con el Administrador del Sistema.";
                    Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
                    grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "javascript:__doPostBack('btnCerrar','')", true);

                }
            }

        }

        protected void lnkFallecido_Click(object sender, EventArgs e)
        {
            List<Titular> lstTitular = null;
            List<Titular_Detalle> lstTitu_deta = null;
            string Operacion = "21", estado_Susalud = "";
            int resultado;
            lstTitular = new TitularBL().ListarTitularesGrupo(531, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            lstTitu_deta = new TitularDetalleBL().ListarDetallesGrupoFamiliar(541, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

            if (lstTitular.Count > 0 && lstTitu_deta.Count > 0 && lstTitular.Count == lstTitu_deta.Count)
            {
                string resultadoSusalud = "";

                for (int i = 0; i < lstTitular.Count; i++)
                {
                    Titular obj = lstTitular[i];
                    Titular_Detalle obj_deta = lstTitu_deta[i];
                    obj.fch_baja = txtFechaTitularFallecido.Text;
                    obj_deta.causalBaja = "7";
                    obj_deta.estado_afiliado = "1";
                    Operacion = "21";
                    estado_Susalud = "6";
                    obj_deta.estado_afiliacion = "2";

                    if (lstTitular.Count == 1)
                    {
                        obj_deta.fch_falleci = txtFechaTitularFallecido.Text;
                        obj_deta.causalBaja = "9";
                        obj_deta.estado_afiliado = "2";
                        Operacion = "20";
                        estado_Susalud = "7";
                    }
                    else
                    {
                        if (obj_deta.categoria == "00")
                        {
                            obj_deta.fch_falleci = txtFechaTitularFallecido.Text;
                            obj_deta.causalBaja = "9";
                            obj_deta.estado_afiliado = "2";
                            Operacion = "20";
                            estado_Susalud = "7";
                        }
                    }


                    DataTable dt = dat.mysql("CALL sp_fill_3(10,'" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','','','','','','','');");

                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        resultadoSusalud = rs.EnvioSUSALUD(Operacion, obj, obj_deta);
                    }

                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        continue;
                    }
                    string stroe2 = "CALL sp_fill_3('5','" +
                          obj.cod_cliente + "','" +
                          obj.cod_titula + "','" +
                          obj.categoria + "','" +
                          obj.dni + "','" +
                          obj_deta.contrato + "','" +
                          obj_deta.cod_afiliado + "','" +
                          estado_Susalud + "','','','');";
                    DataTable dtab2 = dat.mysql(stroe2);
                    resultado = new TitularBL().BAJACOMPLETA(obj, obj_deta, usu, txtFechaTitularFallecido.Text, "3");
                    resultado = new TitularBL().RegistrarFechaRenovacionBaja(obj);
                }

                for (int i = lstTitular.Count - 2; i >= 0; i--)
                {
                    Titular obj = lstTitular[i];
                    Titular_Detalle obj_deta = lstTitu_deta[i];

                    obj_deta.estado_afiliado = "1";
                    obj_deta.estado_afiliacion = "1";
                    obj_deta.contrato = obj.cod_titula + "-F";

                    obj_deta.categoriaSusalud = "00";
                    resultadoSusalud = rs.EnvioSUSALUD("01", obj, obj_deta);
                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        continue;
                    }

                    resultado = new TitularBL().RegistrarFechaRenovacionAlta(obj, obj_deta);
                    string stroe2 = "CALL sp_fill_3('5','" +
                    obj.cod_cliente + "','" +
                    obj.cod_titula + "','" +
                    obj.categoria + "','" +
                    obj.dni + "','" +
                    obj_deta.contrato + "','" +
                    obj_deta.cod_afiliado + "','" +
                    "5" + "','','','');";
                    DataTable dtab2 = dat.mysql(stroe2);
                    resultado = new TitularBL().ACTIVAR(obj, obj_deta, usu, "5");

                }
                string stroe3 = "CALL sp_fill_3('13','" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + "00" + "','','','','','','','');";
                DataTable dtab3 = dat.mysql(stroe3);

                if (resultadoSusalud.Contains("ERROR"))
                {
                    lblverificacion.Text = resultadoSusalud;
                    lblverificacion1.Text = resultadoSusalud;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#NUEVOAFILIADO').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    return;
                }
                else
                {
                    Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
                    grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                    lblCorrecto.Text = "El titular se dió de baja por Fallecimiento";
                    limpiar();
                    gAnio.Visible = false;
                    gMeses.Visible = false;
                    lblFinal.Text = "";
                    lblEvoDet.Visible = false;
                    btnCerrar_Click(sender, e);
                }
            }
        }
        /*
        protected void lnkPlan_Click(object sender, EventArgs e)
        {

            List<Titular> OLD_LIST = null;
            List<Titular_Detalle> OLD_LIST2 = null;

            OLD_LIST = new TitularBL().ListarTitularesGrupo(532, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            OLD_LIST2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(542, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            List<string> listNuevaBaja = new List<string>();
            if (OLD_LIST.Count > 0 && OLD_LIST2.Count > 0 && OLD_LIST.Count == OLD_LIST2.Count)
            {
                string resultadoSusalud = "";
                bool baja = true;

                for (int i = 0; i < OLD_LIST.Count; i++)
                {
                    Titular OLD_objTitu = OLD_LIST[i];
                    Titular_Detalle OLD_objTitudeta = OLD_LIST2[i];

                    listNuevaBaja.Add(OLD_objTitu.fch_baja);
                    OLD_objTitu.fch_baja = DateTime.Now.ToString("dd/MM/yyyy");

                    int resultado = new TitularBL().RegistrarFechaRenovacionBaja(OLD_objTitu);
                    resultadoSusalud = rs.EnvioSUSALUD("21", OLD_objTitu, OLD_objTitudeta);

                    baja = true;
                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        continue;
                    }
                }
                if (baja)
                {
                    for (int i = OLD_LIST.Count - 1; i >= 0; i--)
                    {
                        Titular OLD_objTitu = OLD_LIST[i];
                        Titular_Detalle OLD_objTitudeta = OLD_LIST2[i];
                        Titular obj = OLD_LIST[i];

                        obj.fch_alta = DateTime.Now.ToString("dd/MM/yyyy");
                        obj.fch_baja = listNuevaBaja[i];
                        obj.plan = Convert.ToInt32(ddlPlan.SelectedValue);

                        resultado = new TitularBL().RegistrarFechaRenovacionAlta(obj, OLD_objTitudeta);
                        resultado = new TitularBL().ActualizarTitular(obj, OLD_objTitudeta, usu, 1, OLD_objTitu, OLD_objTitudeta);

                        resultadoSusalud = rs.EnvioSUSALUD("01", obj, OLD_objTitudeta, obj, OLD_objTitudeta);
                        if (resultadoSusalud.Contains("ERROR"))
                        {
                            continue;
                        }
                    }
                }
                if (resultadoSusalud.Contains("ERROR"))
                {
                    lblverificacion.Text = resultadoSusalud;
                    lblverificacion1.Text = resultadoSusalud;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#NUEVOAFILIADO').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    return;
                }
                else
                {

                    Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
                    grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                    lblCorrecto.Text = "Cambio de plan satisfactorio";
                    limpiar();
                    gAnio.Visible = false;
                    gMeses.Visible = false;
                    lblFinal.Text = "";
                    lblEvoDet.Visible = false;
                    btnCerrar_Click(sender, e);
                }
            }
        }
        */
        protected void btnDependienteNuevo_Click(object sender, EventArgs e)
        {
            borrarmensajes();
            limpiar();

            Image1.ImageUrl = "~/image/photo.png";

            RecordConsumoTab.Visible = false;
            AvisosTab.Visible = false;
            CartasTab.Visible = false;
            txtCodigoTitu.Text = gvGrupoFamiliar.Rows[0].Cells[3].Text;
            txtNumeroPoli.Text = gvGrupoFamiliar.Rows[0].Cells[1].Text;
            txtNombreEmpresa.Text = hfNombreEmpresa.Value.ToString().Substring(3);
            txtNumeroPoli.ReadOnly = true;
            txtNombreEmpresa.ReadOnly = true;
            lblAfiliado.Text = "NUEVO DEPENDIENTE";
            ddlCategoria.Attributes.Remove("readonly");
            divClasificacion.Attributes.Add("style", "display:none;");
            ddlCategoria.CssClass = "form-control input-sm";
            btnBajaModal.Visible = false;
            btnActivarAfiliado.Visible = false;
            btnGuardarModificar.Visible = false;
            btnGuardarRegistrar.Visible = true;
            divchkSusalud.Visible = true;
            txtAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlDepartamento.SelectedValue = "15";
            ddlDepartamento_SelectedIndexChanged(sender, e);
            lnkBD.Visible = true;
            id_paciente.Visible = false;
            cod_paciente.Visible = false;
            lnkActualizacionTitu2.Visible = false;
            chkConcubina.Enabled = true;
            CombosCliente(gvGrupoFamiliar.Rows[0].Cells[1].Text, "0");

            switch (gvGrupoFamiliar.Rows[0].Cells[1].Text)
            {
                case "90":
                case "95":
                case "96":
                case "98":
                    //lnkBD.Visible = false;
                    //lnkTraer.Visible = true;
                    ocultos2.Visible = true;
                    rol.Visible = true;
                    dpto.Visible = true;
                    programa.Visible = true;
                    pad.Visible = true;
                    documentoSIMA.Visible = false;
                    id_paciente.Visible = true;
                    cod_paciente.Visible = true;
                    concubina.Visible = true;

                    campo2.Visible = false;
                    List<Titular> list = new TitularBL().ListarTitularesGrupo(53, Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text), gvGrupoFamiliar.Rows[0].Cells[3].Text, "00");
                    Titular objTitu = list.First(delegate (Titular obj)
                    {
                        return (obj.cod_cliente.Equals(Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text))
                                && (obj.cod_titula.Equals(gvGrupoFamiliar.Rows[0].Cells[3].Text)
                                && (obj.categoria.Equals("00"))));
                    });


                    List<Titular_Detalle> list2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text), gvGrupoFamiliar.Rows[0].Cells[3].Text, "00");
                    Titular_Detalle objTitudeta = list2.First(delegate (Titular_Detalle objt)
                    {
                        return (objt.cod_cliente.Equals(Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text))
                                && (objt.cod_titula.Equals(gvGrupoFamiliar.Rows[0].Cells[3].Text)
                                && (objt.categoria.Equals("00"))));
                    });

                    if (objTitudeta != null)
                    {

                        ddlCentro.SelectedValue = objTitu.centro_costo;
                        ddlPlan.SelectedValue = Convert.ToString(objTitu.plan);
                        txtRol.Text = objTitudeta.rol;
                        txtDpto.Text = objTitudeta.dpto;
                        txtDocumento.Text = objTitudeta.docum;
                        ddlPad.SelectedValue = objTitudeta.pad;
                        txtCodPaciente.Text = objTitudeta.cod_paciente;
                        txtIdPaciente.Text = objTitudeta.id_paciente;
                        txtTelefono1.Text = objTitudeta.t_fijo;
                        txtTelefono2.Text = objTitudeta.t_movil;
                        txtDireccion.Text = objTitudeta.direccion;

                        if (objTitudeta.dpto == "") { txtDpto.Text = "9999"; }
                        if (objTitudeta.rol == "") { txtRol.Text = "99"; }

                    }
                    else
                    {
                        ddlCentro.SelectedIndex = 0;
                        ddlPlan.SelectedIndex = 0;
                        ddlPespecial.SelectedIndex = 0;
                        txtRol.Text = "99";
                        txtDpto.Text = "9999";
                        txtDocumento.Text = "";
                        txtObservar.Text = "";
                        txtTelefono1.Text = "";
                        txtTelefono2.Text = "";
                    }

                    break;
                case "11":
                    documentoSIMA.Visible = true;
                    txtCarencia.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                    ocultos2.Visible = false;
                    rol.Visible = false;
                    dpto.Visible = false;
                    programa.Visible = false;
                    pad.Visible = false;
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    campo2.Visible = false;
                    break;
                case "26":
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    txtCarencia.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                    pad.Visible = false;
                    campo2.Visible = false;
                    break;
                //case "55":
                //case "56":
                case "57":
                    ocultos2.Visible = true;
                    segundacapa.Visible = true;
                    basico.Visible = true;
                    onco.Visible = true;
                    documentoSIMA.Visible = false;
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    pad.Visible = false;
                    campo2.Visible = false;
                    break;
                case "15":
                    //carencia.Visible = true;
                    txtCarencia.Text = DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
                    ocultos2.Visible = false;
                    rol.Visible = false;
                    dpto.Visible = false;
                    programa.Visible = false;
                    pad.Visible = false;
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    campo2.Visible = true;
                    break;
                case "70":
                case "44":
                    txtCarencia.Text = "";
                    ocultos2.Visible = false;
                    rol.Visible = false;
                    dpto.Visible = false;
                    programa.Visible = false;
                    pad.Visible = false;
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    pad.Visible = false;
                    campo2.Visible = false;
                    break;
                default:
                    txtCarencia.Text = "";
                    ocultos2.Visible = false;
                    rol.Visible = false;
                    dpto.Visible = false;
                    programa.Visible = false;
                    pad.Visible = false;
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
                    concubina.Visible = false;
                    campo2.Visible = false;
                    break;
            }

            if (usu.ROL == "50")
            {
                btnGuardarModificar.Attributes.Add("disabled", "disabled");
            }

            txtAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlCategoria.Items.Remove(ddlCategoria.Items.FindByText("TITULAR"));
            ddlCategoria_SelectedIndexChanged(sender, e);

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#NUEVOAFILIADO').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {

            limpiar();
            if (lblAfiliado.Text.Equals("NUEVO TITULAR"))
            {
                Filtro(txtBusqueda.Text, Convert.ToString(txtNumeroPoli.Text), Session["USUARIO"].ToString());

                ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                ddlCategoria.DataValueField = "valor";
                ddlCategoria.DataTextField = "descrip";
                ddlCategoria.DataBind();

                StringBuilder sb1 = new StringBuilder();
                sb1.Append("<script type='text/javascript'>");
                sb1.Append("$('#NUEVOAFILIADO').modal('hide');");
                sb1.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb1.ToString(), false);
            }
            else
            {
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);

                ddlCategoria.DataSource = new ComboBL().ListaCombos(48);
                ddlCategoria.DataValueField = "valor";
                ddlCategoria.DataTextField = "descrip";
                ddlCategoria.DataBind();

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#GRUPOFAMILIAR').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
        }

        protected void btnBaja01_Click(object sender, EventArgs e)
        {
            string resultadoSusalud = "";
            string estadoSuSalud = "6"; //NO REGISTRADO
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            List<Titular> lstTitular = new TitularBL().ListarTitularesGrupo(531, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            List<Titular_Detalle> lstTitu_deta = new TitularDetalleBL().ListarDetallesGrupoFamiliar(541, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);


            if ((categoriaHidden.Value == "00") && ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "95") || (txtNumeroPoli.Text == "96") || (txtNumeroPoli.Text == "98")))
            {

                foreach (ListItem listItem in CheckBoxPetro.Items)
                {
                    if (listItem.Selected)
                    {
                        List<Titular> lstTitular_petro = new TitularBL().ListarTitularesGrupo(531, CheckBoxPetro.SelectedValue, txtCodigoTitu.Text, categoriaHidden.Value);
                        List<Titular_Detalle> lstTitu_deta_petro = new TitularDetalleBL().ListarDetallesGrupoFamiliar(541, CheckBoxPetro.SelectedValue, txtCodigoTitu.Text, categoriaHidden.Value);

                        if (lstTitular_petro.Count > 0 && lstTitu_deta_petro.Count > 0 && lstTitular_petro.Count == lstTitu_deta_petro.Count)
                        {
                            for (int i = 0; i < lstTitular_petro.Count; i++)
                            {
                                Titular obj_petro = lstTitular_petro[i];
                                Titular_Detalle obj_det_petro = lstTitu_deta_petro[i];

                                obj_petro.fch_baja = txtFechaBajaModal.Text;
                                obj_det_petro.causalBaja = ddlCausalBaja.SelectedValue;
                                obj_det_petro.estado_afiliado = "1";
                                if (ddlCausalBaja.SelectedValue == "9")
                                {
                                    if (lstTitular_petro.Count == 1)
                                    {
                                        obj_det_petro.fch_falleci = txtFechaBajaModal.Text;
                                        obj_det_petro.estado_afiliado = "2";
                                        obj_det_petro.fallecido = "1";
                                        estadoSuSalud = "7";

                                    }
                                    else
                                    {
                                        if (obj_det_petro.categoria == "00")
                                        {
                                            obj_det_petro.fch_falleci = txtFechaBajaModal.Text;
                                            obj_det_petro.estado_afiliado = "2";
                                            obj_det_petro.fallecido = "1";
                                            estadoSuSalud = "7";
                                        }
                                    }
                                }
                                obj_det_petro.estado_afiliacion = "2";

                                DataTable dtcont = dat.mysql("CALL sp_fill_3(14,'" + obj_petro.cod_cliente + "','" + obj_petro.cod_titula + "','','','','','','','','');");
                                if (dtcont.Rows[0][0].ToString() == "1")
                                {
                                    obj_det_petro.categoriaSusalud = "00";

                                }

                                DataTable dt = dat.mysql("CALL sp_fill_3(10,'" + obj_petro.cod_cliente + "','" + obj_petro.cod_titula + "','" + obj_petro.categoria + "','','','','','','','');");

                                if (dt.Rows[0][0].ToString() == "1")
                                {
                                    resultadoSusalud = rs.EnvioSUSALUD("21", obj_petro, obj_det_petro);
                                }
                                if (resultadoSusalud.Contains("ERROR"))
                                {
                                    lblCorrecto.Text = resultadoSusalud;
                                    lblError.Text = "";
                                    btnCerrar_Click(sender, e);
                                    return;
                                }
                                string stroe2 = "CALL sp_fill_3('5','" +
                                                    obj_petro.cod_cliente + "','" +
                                                    obj_petro.cod_titula + "','" +
                                                    obj_petro.categoria + "','" +
                                                    obj_petro.dni + "','" +
                                                    obj_det_petro.contrato + "','" +
                                                    obj_det_petro.cod_afiliado + "','" +
                                                    estadoSuSalud + "','','','');";
                                DataTable dtab2 = dat.mysql(stroe2);
                                if (obj_det_petro.fallecido == "1")
                                {
                                    string stroe3 = "CALL sp_fill_3('13','" + obj_petro.cod_cliente + "','" + obj_petro.cod_titula + "','" + obj_petro.categoria + "','','','','','','','');";
                                    DataTable dtab3 = dat.mysql(stroe3);
                                }
                                resultado = new TitularBL().RegistrarFechaRenovacionBaja(obj_petro);
                                resultado = new TitularBL().BAJACOMPLETA(obj_petro, obj_det_petro, usu, txtFechaBajaModal.Text, "3");
                            }
                        }
                    }
                }
            }



            if (lstTitular.Count > 0 && lstTitu_deta.Count > 0 && lstTitular.Count == lstTitu_deta.Count)
            {
                for (int i = 0; i < lstTitular.Count; i++)
                {
                    Titular obj = lstTitular[i];
                    Titular_Detalle obj_deta = lstTitu_deta[i];
                    obj.fch_baja = txtFechaBajaModal.Text;
                    obj_deta.causalBaja = ddlCausalBaja.SelectedValue;
                    obj_deta.estado_afiliado = "1";
                    obj_deta.fallecido = "";

                    if (ddlCausalBaja.SelectedValue == "9")
                    {
                        if (lstTitular.Count == 1)
                        {
                            obj_deta.fch_falleci = txtFechaBajaModal.Text;
                            obj_deta.estado_afiliado = "2";
                            obj_deta.fallecido = "1";
                            estadoSuSalud = "7";
                        }
                        else
                        {
                            if (obj_deta.categoria == "00")
                            {
                                obj_deta.fch_falleci = txtFechaBajaModal.Text;
                                obj_deta.estado_afiliado = "2";
                                obj_deta.fallecido = "1";
                                estadoSuSalud = "7";
                            }
                        }
                    }
                    obj_deta.estado_afiliacion = "2";

                    // Es dependiente de un titular fallecido
                    DataTable dtcont = dat.mysql("CALL sp_fill_3(14,'" + obj.cod_cliente + "','" + obj.cod_titula + "','','','','','','','','');");
                    if (dtcont.Rows[0][0].ToString() == "1")
                    {
                        obj_deta.categoriaSusalud = "00";

                    }

                    DataTable dt = dat.mysql("CALL sp_fill_3(10,'" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','','','','','','','');");

                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        resultadoSusalud = rs.EnvioSUSALUD("21", obj, obj_deta);
                    }

                    if (resultadoSusalud.Contains("ERROR"))
                    {
                        lblCorrecto.Text = resultadoSusalud;
                        lblError.Text = "";
                        btnCerrar_Click(sender, e);
                        return;
                    }
                    string stroe2 = "CALL sp_fill_3('5','" +
                                        obj.cod_cliente + "','" +
                                        obj.cod_titula + "','" +
                                        obj.categoria + "','" +
                                        obj.dni + "','" +
                                        obj_deta.contrato + "','" +
                                        obj_deta.cod_afiliado + "','" +
                                        estadoSuSalud + "','','','');";
                    DataTable dtab2 = dat.mysql(stroe2);
                    if (obj_deta.fallecido == "1")
                    {
                        string stroe3 = "CALL sp_fill_3('13','" + obj.cod_cliente + "','" + obj.cod_titula + "','" + obj.categoria + "','','','','','','','');";
                        DataTable dtab3 = dat.mysql(stroe3);
                    }
                    resultado = new TitularBL().RegistrarFechaRenovacionBaja(obj);
                    resultado = new TitularBL().BAJACOMPLETA(obj, obj_deta, usu, txtFechaBajaModal.Text, "3");
                }
            }

            if (resultado == 1)
            {
                lblCorrecto.Text = "Afiliado dado de Baja";
                lblError.Text = "";
                limpiar();

            }
            if (resultado == 0)
            {
                lblError.Text = "Ocurrió un error, no se dió de baja al afiliado, verifique si ingreso una fecha";
                lblCorrecto.Text = "";
            }

            Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
            grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
            btnCerrar_Click(sender, e);

        }

        protected void btnBajaModal_Click(object sender, EventArgs e)
        {
            txtFechaBajaModal.Text = "";

            CheckBoxPetro.Visible = true;

            ddlCausalBaja.DataSource = dat.mysql("call sp_fill(78,0,0,0)");
            ddlCausalBaja.DataValueField = "VALOR";
            ddlCausalBaja.DataTextField = "DESCRIP";
            ddlCausalBaja.DataBind();

            if (categoriaHidden.Value == "00")
            {
                switch (txtNumeroPoli.Text)
                {
                    case "90":
                    case "95":
                    case "96":
                    case "98":

                        bajapetro.Visible = true;

                        string xSQL = "call sp_fill (63,'" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "',0)";
                        DataTable dt = dat.mysql(xSQL);
                        CheckBoxPetro.DataSource = dt;
                        CheckBoxPetro.DataTextField = "cliente";
                        CheckBoxPetro.DataValueField = "codigo";
                        CheckBoxPetro.DataBind();
                        break;

                }
            }
            else
            {
                bajapetro.Visible = false;
            }



            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#BAJA').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkReset_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                txtContraseña.Attributes.Add("Value", txtDNI.Text);
            }
        }

        void avisos(string cliente, string titula, string categoria)
        {
            lblNohayAvisos.Text = "";
            DataTable dt = new DataTable();
            try
            {
                ListaAvisos.Visible = true;
                string SSQLX = null;
                SSQLX = "CALL sp_avisos2(1, '" + cliente + "','" + titula + "','" + categoria + "','" + txtAfiliado.Text + "','','','','','')";
                dt = dat.mysql(SSQLX);

                if (dt.Rows.Count == 0)
                {
                    ListaAvisos.Visible = false;
                    lblNohayAvisos.Text = "No existen avisos registrados";
                }
                else
                {
                    gvAvisos.DataSource = dt;
                    gvAvisos.DataBind();
                }

            }
            catch (Exception ex)
            {
                ListaAvisos.Visible = false;
                lblNohayAvisos.Text = "Ocurrió un error al cargar avisos " + ex.Message.ToString();
            }
        }

        protected void btnNuevoAviso_Click(object sender, EventArgs e)
        {
            lblAvisoDesc.Visible = true;
        }

        protected void btnReportes_Click(object sender, EventArgs e)
        {

            string SSQL = "call sp_fill(79,'" + Convert.ToInt32(Session["USUARIO"].ToString()) + "','78','0')";
            DataTable dt = dat.mysql(SSQL);

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + dt.Rows[0]["Ruta"].ToString() + "');", true);
            }
            else
            {
                return;
            }

        }

        protected void btnActivarAfiliado_Click(object sender, EventArgs e)
        {
            int resultado;
            string estadoSuSalud = "", operacion = "", resultadoSusalud = "";
            DataTable dt = new DataTable();

            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            List<Titular> lstTitular = new TitularBL().ListarTitularesGrupo(53, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular obj = lstTitular.First(delegate (Titular Titu) { return (Titu.cod_cliente.Equals(txtNumeroPoli.Text) && (Titu.cod_titula.Equals(txtCodigoTitu.Text) && (Titu.categoria.Equals(categoriaHidden.Value)))); });

            List<Titular_Detalle> lstTitu_deta = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular_Detalle obj_deta = lstTitu_deta.First(delegate (Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });

            string strCont = "CALL sp_fecha_bajas(3,'" + obj_deta.cod_cliente + "','" + obj_deta.cod_titula + "','" + obj_deta.categoria + "','" + obj.plan + "','','','','','','');";
            dt = dat.mysql(strCont);

            if (dt.Rows[0][0].ToString() != "0")
            {
                obj_deta.cod_afiliado = obj.cod_cliente + obj.cod_titula + obj.categoria + "-" + dt.Rows[0][0].ToString();
            }

            if (obj_deta.cod_afiliado == "")
            {
                obj_deta.cod_afiliado = obj.cod_cliente + obj.cod_titula + obj.categoria;
            }
            if (obj_deta.contrato == "")
            {
                obj_deta.contrato = obj.cod_cliente + obj.cod_titula;
            }

            if (obj.cod_cliente == txtNumeroPoli.Text && obj.cod_titula == txtCodigoTitu.Text && obj.categoria == categoriaHidden.Value)
            {
                obj_deta.afi_nombre = txtNombres.Text;
                obj_deta.afi_apepat = txtApellidop.Text;
                obj_deta.afi_apemat = txtApellidom.Text;
            }
            obj.fch_alta = txtAlta.Text;
            obj.fch_baja = "";
            obj.estado_titular = 1;
            obj_deta.causalBaja = "";
            obj_deta.estado_afiliado = "1";
            obj_deta.estado_afiliacion = "1";
            obj_deta.fallecido = "";
            // validacion del tipo de operacion para REGISTRAR a SUSALUD
            DataTable dtope = new DataTable();
            string qryope = "CALL sp_fill_3('4','" + obj.cod_cliente + "','" + txtDNI.Text + "','','','','','','','','');";
            dtope = dat.mysql(qryope);
            operacion = dtope.Rows[0]["OP"].ToString();

            // validacion de pami 
            if (obj.cod_cliente == "96" && obj.categoria == "00")
            {   //Contratante
                estadoSuSalud = "3";
            }
            else
            {
                resultadoSusalud = rs.EnvioSUSALUD(operacion, obj, obj_deta);
            }

            if (resultadoSusalud.Contains("ERROR"))
            {
                lblverificacion.Text = resultadoSusalud;
                lblverificacion1.Text = resultadoSusalud;
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#NUEVOAFILIADO').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                return;
            }
            resultado = new TitularBL().RegistrarFechaRenovacionAlta(obj, obj_deta);
            estadoSuSalud = "5";
            string stroe2 = "CALL sp_fill_3('5','" +
                obj.cod_cliente + "','" +
                obj.cod_titula + "','" +
                obj_deta.categoriaSusalud + "','" +
                obj.dni + "','" +
                obj_deta.contrato + "','" +
                obj_deta.cod_afiliado + "','" +
                estadoSuSalud + "','','','');";
            DataTable dtab3 = dat.mysql(stroe2);
            resultado = new TitularBL().ACTIVAR(obj, obj_deta, usu, "5");

            if (resultado == 1)
            {
                lblCorrecto.Text = "Afiliado activado";
                limpiar();
            }
            else
            {
                lblError.Text = "Error de activación";
            }
            Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);
            grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
            btnCerrar_Click(sender, e);
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            //Session.Abandon();
            Session.Clear();
            Response.Redirect("Sesion.aspx?usu=&pass=");
        }

        private bool RemoteFileExists(string url)
        {
            bool result = false;
            using (WebClient client = new WebClient())
            {
                try
                {
                    Stream stream = client.OpenRead(url);
                    if (stream != null)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch
                {
                    //Any exception will returns false.
                    result = false;
                }
            }
            return result;
        }

        protected void lnkAlertas_Click(object sender, EventArgs e)
        {
            string SSQL = "call sp_fill(79,'" + Convert.ToInt32(Session["USUARIO"].ToString()) + "','78','0')";
            DataTable dt = dat.mysql(SSQL);

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + dt.Rows[0]["Ruta"].ToString() + "');", true);
            }
            else
            {
                return;
            }
        }

        protected void btnBuscarCG_Click(object sender, EventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            movimientos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value, ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(), usu.ID.ToString());
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            lblErrorReg.Text = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#xLOAD').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void btnSubir_Click(object sender, EventArgs e)
        {

            if (flpImp.FileName != "")
            {
                string fileName = Path.GetFileName(flpImp.PostedFile.FileName);
                flpImp.PostedFile.SaveAs((Server.MapPath("~/SUBIDOS2/") + fileName));


                if (IsPostBack)
                {
                    dt2 = Util.GenerarDataTable(rGuardar + flpImp.FileName);
                }
                foreach (DataRow row in dt2.Rows)
                {
                    try
                    {
                        string xLoadTAb = "INSERT INTO solben_bd.T_FINANCIA (cod_cliente, cod_titula, categoria, financia) VALUES ('" + row["cod_cliente"].ToString() + "','" + row["cod_titula"] + "','" + row["categoria"] + "','" + row["campo2"] + "')";
                        dat.mysql(xLoadTAb);
                    }
                    catch (Exception ex)
                    {
                        lblErrorReg.Text = "ERROR DE ESTRUCTURA :" + ex.Message.ToString();
                        return;
                    }

                }

                string xSQL = "call sp_proc_basic (9,'','')";

                try
                {

                    DataTable dt = dat.mysql(xSQL);
                    lblalerta.Text = "IMPORTACIÓN SATISFACTORIA";

                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(rGuardar);
                FileInfo[] fileInfo = dirInfo.GetFiles("*.*");

                for (int x = 0; x <= fileInfo.Count() - 1; x++)
                {

                    try
                    {
                        File.Delete(rGuardar + fileInfo[x].Name.ToString());
                    }
                    catch (Exception ex)
                    {
                        lblErrorReg.Text = ex.Message.ToString();
                        return;
                    }
                }

            }
            else
            {
                lblErrorReg.Text = "ADVERTENCIA, Debe seleccionar el archivo que desea subir";
                return;
            }




        }

        protected void ddlTipoOrdenAtencion_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void FullPostBack(object sender, EventArgs e)
        {

            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string solben_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["solben_id"].ToString();
            string atencion_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["atencion_id"].ToString();
            string cod_cliente = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["cod_cliente"].ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/loginTitus.php?u=" + Session["USUARIO_USU"] + "&p=" + Session["PASS"] + "&p2=2&t=" + atencion_id + "&n=" + solben_id + "');", true);
        }


        void subeFotos(string cli, string titu, string cate)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (file2.Value != null)
                    {
                        string ext = Path.GetExtension(file2.Value).ToString();

                        if (ext != "")
                        {
                            string pat = Server.MapPath("~/FOTOS/" + cli + "-" + titu + "-" + cate + ext).ToString();
                            string pat2 = Server.MapPath("~/FOTOS/");
                            string virtualpath = Server.MapPath("~/fotos/" + cli + "/") + cli + "-" + titu + "-" + cate + ext;
                            //string virtualpath = "\\\\10.100.100.5\\fotos-solben\\"+ cli +"\\"+ cli + "-" + titu + "-" + cate + ext;
                            HttpPostedFile psf = file2.PostedFile;
                            psf.SaveAs(virtualpath);
                            //sftp(pat2, cli + "-" + titu + "-" + cate + ext,cli);
                            //ftp(psf, pat2, cli + "-" + titu + "-" + cate + ext,cli);
                            //lblalerta.Text = "Foto Subida Satisfactoriamente";
                        }
                        else
                        {
                            lblErrorReg.Text = "No se ha encontrado una extensión de foto válida. Seleccione una foto y vuelva a intentarlo.";
                        }
                    }
                    else
                    {
                        lblErrorReg.Text = "No ha seleccionado una foto. Seleccione una foto y vuelva a intentarlo.";
                    }
                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                }

            }

        }

        protected void lnkFoto_Click(object sender, EventArgs e)
        {

        }

        protected void lnkBD_Click(object sender, EventArgs e)
        {
            if (txtDNI.Text == "")
            {
                lblBD.Text = "El DNI esta vacío.";
                return;
            }

            try
            {
                string xSQL = "call sp_fill (62,'" + txtDNI.Text + "',0,0)";
                DataTable dt2 = dat.mysql(xSQL);

                if (dt2.Rows.Count > 0)
                {
                    txtNombres.Text = dt2.Rows[0][0].ToString();
                    txtApellidop.Text = dt2.Rows[0][1].ToString();
                    txtApellidom.Text = dt2.Rows[0][2].ToString();
                    txtNacimiento.Text = dt2.Rows[0][3].ToString();
                    txtDireccion.Text = dt2.Rows[0][4].ToString();
                    ddlSexo.SelectedValue = dt2.Rows[0][5].ToString();
                    txtObservar.Text = dt2.Rows[0][6].ToString();
                    txtTelefono1.Text = dt2.Rows[0][7].ToString();
                    txtTelefono2.Text = dt2.Rows[0][8].ToString();
                    ddlEstadoCivil.SelectedValue = dt2.Rows[0][9].ToString();
                    ddlDepartamento.SelectedValue = dt2.Rows[0][10].ToString();
                    ddlProvincia.SelectedValue = dt2.Rows[0][11].ToString();
                    ddlDistrito.SelectedValue = dt2.Rows[0][12].ToString();

                    lblBD.Text = "Datos Cargados";
                }
                else
                {
                    xSQL = "VER_TABLAS 1,'" + txtDNI.Text + "'";
                    DataTable dt = dat.SQL(xSQL);

                    if (dt.Rows[0][1].ToString() != "0")
                    {
                        txtNombres.Text = dt.Rows[0][2].ToString();
                        txtApellidop.Text = dt.Rows[0][0].ToString();
                        txtApellidom.Text = dt.Rows[0][1].ToString();
                        txtNacimiento.Text = dt.Rows[0][3].ToString();

                        if (dt.Rows[0][4].ToString() == "1")
                        {
                            ddlSexo.SelectedValue = "M";
                        }
                        else
                        {
                            ddlSexo.SelectedValue = "F";
                        }

                        lblBD.Text = "Datos Cargados";
                    }
                    else
                    {
                        txtNombres.Text = "";
                        txtApellidop.Text = "";
                        txtApellidom.Text = "";
                        txtNacimiento.Text = "";
                        lblBD.Text = dt.Rows[0][0].ToString();
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                lblBD.Text = ex.Message.ToString();
                return;
            }
        }

        protected void lnkFichaPersonal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('Ficha2.aspx?cc=" + txtNumeroPoli.Text + "&ct=" + txtCodigoTitu.Text + "&c=" + categoriaHidden.Value + "');", true);
            return;
        }



        protected void lnkBajaTitus_Click(object sender, EventArgs e)
        {
            if (ddlTablas.SelectedValue.ToString() == "57")
            {
                exportar57();
            }
            else
            {
                exportardbf();

            }
        }

        void exportar57()
        {
            string xSQL = "CALL SP_ESTADO_TITUS('57','1');";
            DataTable dtabexport = dat.mysql(xSQL);

            string serie = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Color colorCode;

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {

                ExcelWorksheet wst = pck.Workbook.Worksheets.Add("REPORTE");

                wst.Cells["A1"].Value = "LA PROTECTORA";
                wst.Cells["A1"].Style.Font.Bold = true;
                wst.Cells["A1:G1"].Merge = true;

                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");

                wst.Cells["A2:G2"].Merge = true;
                wst.Cells["A2:G2"].Style.Font.Bold = true;
                wst.Cells["A2:G2"].Value = "TITUS"; ;
                wst.Cells["A2:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(colorCode);

                wst.Cells["A3:G3"].Merge = true;
                wst.Cells["A3:G3"].Style.Font.Bold = true;
                wst.Cells["A3:G3"].Value = Session["USU_NOM"] + " || " + DateTime.Now.ToString();

                wst.Cells["A4:V4"].Style.Font.Bold = true;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#DCDCDC");
                wst.Cells["A4:V4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A4:V4"].Style.Fill.BackgroundColor.SetColor(colorCode);

                wst.Cells["A4"].LoadFromDataTable(dtabexport, true);

                wst.Cells["A4"].Value = "CLIENTE";
                wst.Cells["B4"].Value = "EMPRESA";
                wst.Cells["C4"].Value = "C.TITULA";
                wst.Cells["D4"].Value = "DNI";
                wst.Cells["E4"].Value = "CATEGORIA";
                wst.Cells["F4"].Value = "PLAN";
                wst.Cells["G4"].Value = "C.COSTO";
                wst.Cells["H4"].Value = "AFILIADO";
                wst.Cells["I4"].Value = "SEXO";
                wst.Cells["J4"].Value = "EDAD";
                wst.Cells["K4"].Value = "F.NACIMIENTO";
                wst.Cells["L4"].Value = "F.ALTA";
                wst.Cells["M4"].Value = "F.BAJA";
                wst.Cells["N4"].Value = "F.CARENCIA";
                wst.Cells["O4"].Value = "T.FIJO";
                wst.Cells["P4"].Value = "T.MOVIL";
                wst.Cells["Q4"].Value = "CORREO";
                wst.Cells["R4"].Value = "CORREO1";
                wst.Cells["S4"].Value = "CORREO2";
                wst.Cells["T4"].Value = "ESTADO";
                wst.Cells["U4"].Value = "ONCO";
                wst.Cells["V4"].Value = "BASICO";



                wst.Cells["A:A"].AutoFitColumns();
                wst.Cells["B:B"].AutoFitColumns();
                wst.Cells["C:C"].AutoFitColumns();
                wst.Cells["D:D"].AutoFitColumns();
                wst.Cells["E:E"].AutoFitColumns();
                wst.Cells["F:F"].AutoFitColumns();
                wst.Cells["G:G"].AutoFitColumns();
                wst.Cells["H:H"].AutoFitColumns();
                wst.Cells["I:I"].AutoFitColumns();
                wst.Cells["J:J"].AutoFitColumns();
                wst.Cells["K:K"].AutoFitColumns();
                wst.Cells["L:L"].AutoFitColumns();
                wst.Cells["M:M"].AutoFitColumns();
                wst.Cells["N:N"].AutoFitColumns();
                wst.Cells["O:O"].AutoFitColumns();
                wst.Cells["P:P"].AutoFitColumns();
                wst.Cells["Q:Q"].AutoFitColumns();
                wst.Cells["R:R"].AutoFitColumns();
                wst.Cells["S:S"].AutoFitColumns();
                wst.Cells["T:T"].AutoFitColumns();
                wst.Cells["U:U"].AutoFitColumns();
                wst.Cells["V:V"].AutoFitColumns();


                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TITUS57_" + serie + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();

            }

        }

        void exportardbf()
        {
            string ruta = "~/TITUS/TITU" + ddlTablas.SelectedValue.ToString() + ".DBF";
            try
            {
                if (ruta != "")
                {
                    string path = Server.MapPath(ruta);
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.WriteFile(file.FullName);
                        Response.End();
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>window.alert('" + ex.Message.ToString() + "')</script>");
                throw;
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xSQL = "call sp_fill (60,'" + Convert.ToInt32(ddlCategoria.SelectedValue) + "',0,0)";
            DataTable dt = dat.mysql(xSQL);
            txtCodPaciente.Text = dt.Rows[0][0].ToString();

            if (ddlCategoria.SelectedValue == "04")
            {
                id_paciente.Visible = true;
                txtIdPaciente.ReadOnly = false;
                concubina.Visible = false;

            }
            else if (ddlCategoria.SelectedValue == "01" && (txtNumeroPoli.Text == "90" || txtNumeroPoli.Text == "96" || txtNumeroPoli.Text == "98"))
            {
                concubina.Visible = true;
                chkConcubina.Checked = false;
            }
            else
            {
                id_paciente.Visible = false;
                txtIdPaciente.Text = "";
                txtIdPaciente.ReadOnly = true;
                concubina.Visible = false;
            }

        }

        protected void txtIdPaciente_TextChanged(object sender, EventArgs e)
        {
            if (txtIdPaciente.Text.Length < 2)
            {
                txtIdPaciente.Text = "0" + txtIdPaciente.Text;
            }
        }

        protected void btnSali_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            btnCerrar_Click(sender, e);
        }

        protected void btnSusalud_Click(object sender, EventArgs e)
        {

            string SSQL = "call sp_fill(79,'" + Convert.ToInt32(Session["USUARIO"].ToString()) + "','102','0')";
            DataTable dt = dat.mysql(SSQL);

            if (dt.Rows.Count > 0)
            {
                //usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
                //string usuario = Convert.ToString(usu.USER);
                //string pass = Convert.ToString(usu.PASS);
                ////Response.Redirect("http://190.102.136.157/AppGer/inicio.aspx?usu=" + usuario + "&pass=" + pass + "");
                //--Response.Redirect(dt.Rows[0]["Ruta"].ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + dt.Rows[0]["Ruta"].ToString() + "');", true);
            }
            else
            {
                return;
            }
        }

        void descargaRecord(DataTable dt1, DataTable dt2, DataTable dt3, string titulo, string asegurado)
        {

            string serie = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Color colorCode;

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                ExcelWorksheet wst = pck.Workbook.Worksheets.Add("REPORTE");

                wst.Cells["A1:E1"].Merge = true;
                wst.Cells["A1:E1"].Style.Font.Bold = true;
                wst.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wst.Cells["A1:E1"].Value = titulo;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["F1:J1"].Merge = true;
                wst.Cells["F1:J1"].Style.Font.Bold = true;
                wst.Cells["F1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wst.Cells["F1:J1"].Value = asegurado;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["F1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["F1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                dt1.Columns.Remove("PORG");
                dt1.Columns.Remove("PORB");
                dt1.Columns.Remove("X");
                wst.Cells["A4"].LoadFromDataTable(dt1, true);
                //wst.Cells("A" & (dtab1.Rows.Count() + 5).ToString & "").Style.Font.Bold = True
                //wst.Cells("C" & (dtab1.Rows.Count() + 5).ToString & "").Style.Font.Bold = True
                //wst.Cells("E" & (dtab1.Rows.Count() + 5).ToString & "").Style.Font.Bold = True
                //colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9")
                //wst.Cells("A" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                //wst.Cells("C" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                //wst.Cells("E" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                //wst.Cells("A" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.BackgroundColor.SetColor(colorCode)
                //wst.Cells("C" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.BackgroundColor.SetColor(colorCode)
                //wst.Cells("E" & (dtab1.Rows.Count() + 3).ToString & "").Style.Fill.BackgroundColor.SetColor(colorCode)
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Merge = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Font.Bold = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                //wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.BackgroundColor.SetColor(colorCode);
                //wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Value = "CANTIDAD";
                //wst.Cells("C" & (dtab1.Rows.Count() + 3).ToString & "").Value = lblCantidad2.Text
                //wst.Cells("E" & (dtab1.Rows.Count() + 3).ToString & "").Value = lblMontoSOATs.Text
                if (dt2 != null)
                {
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Value = "DETALLE DE EVOLUCIÓN";
                    dt2.Columns.Remove("PORG");
                    dt2.Columns.Remove("PORB");
                    dt2.Columns.Remove("X");
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1).ToString() + ""].LoadFromDataTable(dt2, true);
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Merge = true;
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Font.Bold = true;
                    wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                }

                if (dt3 != null)
                {
                    if (dt3.Rows.Count != 0)
                    {
                        wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Fill.BackgroundColor.SetColor(colorCode);
                        wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Value = "LIQUIDACIONES";
                        wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2 + 2 + 1).ToString() + ""].LoadFromDataTable(dt3, true);
                    }

                }
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=RECORD" + serie + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
            }

        }

        //RECORD DE CONSUMO//
        //###########################################################
        void loadDetail()
        {
            try
            {
                record123.Visible = true;
                RCanual.Visible = true;
                RCmensual.Visible = false;
                detalleRC.Visible = false;
                gMeses.Series["GASTOS"].Points.Clear();
                gMeses.Series["BENEFICIOS"].Points.Clear();
                lblDetaEvoIC.Visible = false;
                lblGraficaIC2.Visible = false;
                lblDetalleIC.Visible = false;
                gvDatosDetalle2.DataSource = null;
                gvDatosDetalle2.DataBind();
                gvDatosDetalle3.DataSource = null;
                gvDatosDetalle3.DataBind();

                //string xSQL = "CALL SP_REPORTES_IAFA (4,'" + cliente + "','','','','" + nombre + "','','')";
                string xSQL = "CALL SP_REPORTES_IAFA (76,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + ddlCategoria.SelectedValue.ToString() + "','','','','')";
                DataTable dt = dat.mysql(xSQL);

                int i = 1;
                while (i <= dt.Rows.Count - 1)
                {
                    dt.Rows[i][4] = Convert.ToString(Math.Round(Convert.ToDecimal(dt.Rows[i][3]) * 100 / Convert.ToDecimal(dt.Rows[0][2]), 2)) + "%";
                    dt.Rows[i][5] = Convert.ToString(Math.Round(Convert.ToDecimal(dt.Rows[i][3]) * 100 / Convert.ToDecimal(dt.Rows[0][2]), 2)) + "%";
                    i = i + 1;
                }

                dt.Rows[0][4] = "";
                dt.Rows[0][5] = "100%";

                dt.DefaultView.Sort = "ANIO ASC";
                gvDatosDetalle1.AllowPaging = false;
                gvDatosDetalle1.DataSource = dt;
                gvDatosDetalle1.DataBind();

                cargaAnios();

                dt.DefaultView.Sort = "ANIO DESC";
                gvDatosDetalle1.AllowPaging = true;
                gvDatosDetalle1.DataSource = dt;
                gvDatosDetalle1.DataBind();

                if (gvDatosDetalle1.Rows.Count == 1)
                {
                    RCanual.Visible = false;
                    RCmensual.Visible = false;
                    record123.Visible = false;
                    lblNoHayRecord.Text = "No existe record de consumo para este afiliado";
                    gvDatosDetalle1.DataSource = null;
                    gvDatosDetalle1.DataBind();
                    gvDatosDetalle2.DataSource = null;
                    gvDatosDetalle2.DataBind();
                    gvDatosDetalle3.DataSource = null;
                    gvDatosDetalle3.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblNoHayRecord.Text = "Ocurrió un error al cargar record de consumo " + ex.Message.ToString();
                record123.Visible = false;
            }

        }

        void cargaTablaDetalle2(string anio)
        {
            lblDetaEvoIC.Visible = true;
            lblGraficaIC2.Visible = true;
            //string SSQL = "CALL SP_REPORTES_IAFA (6,'" + cliente + "','','','','" + anio + "','" + nombre + "','')";
            string SSQL = "CALL SP_REPORTES_IAFA (77,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + ddlCategoria.SelectedValue.ToString() + "','" + anio + "','','','')";
            DataTable dt = dat.mysql(SSQL);

            int i = 1;
            while (i <= dt.Rows.Count - 1)
            {
                //dt.Rows(i)(5) = Math.Round(Convert.ToDecimal(dt.Rows(i)(4)) * 100 / Convert.ToDecimal(dt.Rows(0)(3)), 2)
                dt.Rows[i][6] = Convert.ToString(Math.Round(Convert.ToDecimal(dt.Rows[i][4]) * 100 / Convert.ToDecimal(dt.Rows[0][4]), 2)) + "%";
                i = i + 1;
            }

            dt.Rows[0][5] = "";
            dt.Rows[0][6] = "100%";

            dt.DefaultView.Sort = "COD_MES ASC";
            //gvDatosDetalle2.AllowPaging = false;
            gvDatosDetalle2.DataSource = dt;
            gvDatosDetalle2.DataBind();

            cargaMeses();

            //gvDatosDetalle2.AllowPaging = true;
            //gvDatosDetalle2.DataSource = dt;
            //gvDatosDetalle2.DataBind();

        }

        void cargaTablaDetalle3(string cliente, string anio, string mes, string nombre)
        {
            lblDetalleIC.Visible = true;
            detalleRC.Visible = true;
            string SSQL = "CALL SP_REPORTES_IAFA (13,'" + cliente + "','','','','" + anio + "','" + mes + "','" + nombre + "')";
            DataTable dt = dat.mysql(SSQL);

            dt.Rows[0][2] = "CASOS: " + (dt.Rows.Count - 1).ToString();
            dt.Rows[0][4] = dt.Compute("Sum(GASTO)", null);
            dt.Rows[0][5] = dt.Compute("Sum(BENEFICIO)", null);

            gvDatosDetalle3.DataSource = dt;
            gvDatosDetalle3.DataBind();

        }

        void cargaAnios()
        {
            gAnio.Series["GASTOS"].Points.Clear();
            gAnio.Series["BENEFICIOS"].Points.Clear();


            try
            {
                string VAL = "";
                string XY12 = "";
                string XY13 = "";
                bool UO = false;

                foreach (GridViewRow row in gvDatosDetalle1.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[1].Text;
                        XY12 = row.Cells[3].Text;
                        XY13 = row.Cells[4].Text;

                        gAnio.Series["GASTOS"].Points.AddXY(VAL, XY12);
                        gAnio.Series["BENEFICIOS"].Points.AddXY(VAL, XY13);
                    }
                }

            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
            }


        }

        void cargaMeses()
        {
            try
            {
                string VAL = "";
                //string XY21 = "";
                string XY22 = "";
                string XY23 = "";
                bool UO = true;

                foreach (GridViewRow row in gvDatosDetalle2.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[2].Text;
                        XY22 = row.Cells[4].Text;
                        XY23 = row.Cells[5].Text;

                        gMeses.Series["GASTOS"].Points.AddXY(VAL, XY22);
                        gMeses.Series["BENEFICIOS"].Points.AddXY(VAL, XY23);
                    }
                }

            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
            }

        }

        protected void PartialPostgv1(object sender, EventArgs e)
        {
            RCanual.Visible = true;
            RCmensual.Visible = true;
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["ANIORC"] = gvDatosDetalle1.DataKeys[row.RowIndex].Values["ANIO"].ToString();
            NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;
            cargaTablaDetalle2(ViewState["ANIORC"].ToString());
        }

        protected void PartialPostgv2(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["COD_MESRC"] = gvDatosDetalle2.DataKeys[row.RowIndex].Values["COD_MES"].ToString();
            ViewState["MESRC"] = gvDatosDetalle2.DataKeys[row.RowIndex].Values["MES"].ToString();
            NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;
            cargaTablaDetalle3(txtNumeroPoli.Text, ViewState["ANIORC"].ToString(), ViewState["COD_MESRC"].ToString(), NOMBRECOMPLETO);
            lblFinal.Text = "LIQUIDACIONES DE " + ViewState["MESRC"].ToString() + " DEL AÑO " + ViewState["ANIORC"].ToString();
        }

        protected void PartialPostgv3(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string NRO_INTER = gvDatosDetalle3.DataKeys[row.RowIndex].Values["NRO_INTER"].ToString();
            string NRO_PLANI = gvDatosDetalle3.DataKeys[row.RowIndex].Values["NRO_PLANI"].ToString();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/detalle_liquidacion_report.php?nroin=" + NRO_INTER + "&p=" + NRO_PLANI + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/loginTitus.php?u=" + Session["USUARIO_USU"] + "&p=" + Session["PASS"] + "&p2=1&r=" + txtNumeroPoli.Text + txtCodigoTitu.Text + categoriaHidden.Value + "');", true);
        }

        //###########################################################

        //IMPRESION DE CARTAS//
        //###########################################################
        void loadIC(string cliente, string titula, string categoria)
        {
            try
            {
                cartas123.Visible = true;
                icanual.Visible = true;
                icmensual.Visible = false;
                icdetalle.Visible = false;
                ChartAnioIC.Series["GASTOS"].Points.Clear();
                gvImpresionCartasMes.DataSource = null;
                gvImpresionCartasMes.DataBind();
                gvImpresionCartasDetalle.DataSource = null;
                gvImpresionCartasDetalle.DataBind();

                string xSQL = "CALL SP_REPORTES_IAFA (69,'" + cliente + "','" + titula + "','" + categoria + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','','')";
                DataTable dt = dat.mysql(xSQL);
                gvImpresionCartasAnio.DataSource = dt;
                gvImpresionCartasAnio.DataBind();
                ChartAniosIC();

                if (gvImpresionCartasAnio.Rows.Count == 1)
                {

                    icanual.Visible = false;
                    icmensual.Visible = false;
                    cartas123.Visible = false;
                    lblNohayCarta.Text = "No existen cartas para este afiliado";
                    gvImpresionCartasAnio.DataSource = null;
                    gvImpresionCartasAnio.DataBind();
                    gvImpresionCartasMes.DataSource = null;
                    gvImpresionCartasMes.DataBind();
                    gvImpresionCartasDetalle.DataSource = null;
                    gvImpresionCartasDetalle.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblNohayCarta.Text = "Ocurrió un error al cargar cartas " + ex.Message.ToString();
                cartas123.Visible = false;
            }

        }

        void cargarICAnio(string cliente, string anio, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (70,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','')";
            DataTable dt = dat.mysql(SSQL);
            gvImpresionCartasMes.DataSource = dt;
            gvImpresionCartasMes.DataBind();

            ChartMesesIC();
        }

        void cargarICMes(string cliente, string anio, string mes, string titula, string categoria)
        {
            icdetalle.Visible = true;
            string SSQL = "CALL SP_REPORTES_IAFA (71,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + mes + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','')";
            DataTable dt = dat.mysql(SSQL);
            gvImpresionCartasDetalle.DataSource = dt;
            gvImpresionCartasDetalle.DataBind();
        }

        void ChartAniosIC()
        {
            ChartAnioIC.Series["GASTOS"].Points.Clear();

            try
            {
                string VAL = "";
                string XY13 = "";
                bool UO = false;

                foreach (GridViewRow row in gvImpresionCartasAnio.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[1].Text;
                        XY13 = row.Cells[3].Text;

                        ChartAnioIC.Series["GASTOS"].Points.AddXY(VAL, XY13);
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }


        }

        void ChartMesesIC()
        {
            ChartMesIC.Series["GASTOS"].Points.Clear();
            try
            {
                string VAL = "";
                string XY22 = "";
                bool UO = true;

                foreach (GridViewRow row in gvImpresionCartasMes.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[1].Text;
                        XY22 = row.Cells[4].Text;

                        ChartMesIC.Series["GASTOS"].Points.AddXY(VAL, XY22);
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }

        }

        protected void PostgvICAnio(object sender, EventArgs e)
        {
            try
            {
                icanual.Visible = true;
                icmensual.Visible = true;
                LinkButton btn = sender as LinkButton;
                GridViewRow row = btn.NamingContainer as GridViewRow;
                ViewState["ANIO"] = gvImpresionCartasAnio.DataKeys[row.RowIndex].Values["ANIO"].ToString();
                cargarICAnio(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }

        }

        protected void PostgvICMes(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["COD_MES"] = gvImpresionCartasMes.DataKeys[row.RowIndex].Values["COD_MES"].ToString();
            ViewState["MES"] = gvImpresionCartasMes.DataKeys[row.RowIndex].Values["MES"].ToString();
            cargarICMes(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), ViewState["COD_MES"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
            lblDetalleIC.Text = "CARTAS " + ViewState["MES"].ToString() + " DEL AÑO " + ViewState["ANIO"].ToString();
        }

        //###########################################################
        //INFORMES MÉDICOS//
        //###########################################################
        void loadIM(string cliente, string titula, string categoria)
        {
            try
            {
                informes123.Visible = true;
                IManual.Visible = true;
                IMmensual.Visible = false;
                icdetalle.Visible = false;
                ChartIM1.Series["GASTOS"].Points.Clear();
                gvInformesMedicosMes.DataSource = null;
                gvInformesMedicosMes.DataBind();
                gvInformesMedicosDetalle.DataSource = null;
                gvInformesMedicosDetalle.DataBind();

                string xSQL = "CALL SP_REPORTES_IAFA (72,'" + cliente + "','" + titula + "','" + categoria + "','','','','')";

                DataTable dt = dat.mysql(xSQL);
                gvInformesMedicosAnio.DataSource = dt;
                gvInformesMedicosAnio.DataBind();

                ChartAniosIM();

                if (gvInformesMedicosAnio.Rows.Count == 1 || (gvInformesMedicosAnio.Rows[0].Cells[2].Text == "0" && gvInformesMedicosAnio.Rows[0].Cells[5].Text == "0"))
                {
                    IManual.Visible = false;
                    IMmensual.Visible = false;
                    informes123.Visible = false;
                    lblNoHayInformes.Text = "No existen informes médicos para este afiliado";
                    gvInformesMedicosAnio.DataSource = null;
                    gvInformesMedicosAnio.DataBind();
                    gvInformesMedicosMes.DataSource = null;
                    gvInformesMedicosMes.DataBind();
                    gvInformesMedicosDetalle.DataSource = null;
                    gvInformesMedicosDetalle.DataBind();

                }
            }
            catch (Exception ex)
            {
                informes123.Visible = false;
                lblNoHayInformes.Text = "Ocurrió un error al cargar los Informes Médicos " + ex.Message.ToString();
            }

        }

        void cargarIMAnio(string cliente, string anio, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (73,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','','','')";
            DataTable dt = dat.mysql(SSQL);
            gvInformesMedicosMes.DataSource = dt;
            gvInformesMedicosMes.DataBind();

            ChartMesesIM();
        }

        void cargarIMMes(string cliente, string anio, string mes, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (74,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + mes + "','','')";
            DataTable dt = dat.mysql(SSQL);
            gvInformesMedicosDetalle.DataSource = dt;
            gvInformesMedicosDetalle.DataBind();
        }

        void ChartAniosIM()
        {
            ChartIM1.Series["GASTOS"].Points.Clear();

            try
            {
                string VAL = "";
                string XY13 = "";
                bool UO = false;

                foreach (GridViewRow row in gvInformesMedicosAnio.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[1].Text;
                        XY13 = row.Cells[3].Text;

                        ChartIM1.Series["GASTOS"].Points.AddXY(VAL, XY13);
                    }
                }

            }
            catch (Exception ex)
            {
            }


        }

        void ChartMesesIM()
        {
            ChartIM2.Series["GASTOS"].Points.Clear();
            try
            {
                string VAL = "";
                string XY22 = "";
                bool UO = true;

                foreach (GridViewRow row in gvInformesMedicosMes.Rows)
                {
                    if (UO == true)
                    {
                        UO = false;
                        continue;
                    }
                    else
                    {
                        VAL = row.Cells[1].Text;
                        XY22 = row.Cells[4].Text;

                        ChartIM2.Series["GASTOS"].Points.AddXY(VAL, XY22);
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void PostgvIMAnio(object sender, EventArgs e)
        {
            try
            {
                IManual.Visible = true;
                IMmensual.Visible = true;
                LinkButton btn = sender as LinkButton;
                GridViewRow row = btn.NamingContainer as GridViewRow;
                ViewState["ANIO1"] = gvInformesMedicosAnio.DataKeys[row.RowIndex].Values["ANIO"].ToString();

                cargarIMAnio(txtNumeroPoli.Text, ViewState["ANIO1"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }

        }

        protected void PostgvIMMes(object sender, EventArgs e)
        {
            IMdetalle.Visible = true;
            lblIMDetalle.Visible = true;
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["COD_MES1"] = gvInformesMedicosMes.DataKeys[row.RowIndex].Values["COD_MES"].ToString();
            ViewState["MES1"] = gvInformesMedicosMes.DataKeys[row.RowIndex].Values["MES"].ToString();
            cargarIMMes(txtNumeroPoli.Text, ViewState["ANIO1"].ToString(), ViewState["COD_MES1"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
            lblIMDetalle.Text = "INFORMES MÉDICOS DE " + ViewState["MES1"].ToString() + " DEL AÑO " + ViewState["ANIO1"].ToString();
        }

        protected void PostIMDetalle(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string idsolben = gvInformesMedicosDetalle.DataKeys[row.RowIndex].Values["solben"].ToString();
            if (idsolben.Substring(0, 1) != "T")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/infor_med_vista.php?cg=" + idsolben + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/infor_med_imp_temp.php?cg=" + idsolben + "');", true);
            }
        }

        //###########################################################

        void movimientos(string cliente, string titula, string categoria, string mes, string anio, string usu)
        {
            try
            {
                movimientos123.Visible = true;
                string SSQL = "CALL SP_REPORTES_IAFA (75,'" + cliente + "','" + titula + "','" + categoria + "','" + mes + "','" + anio + "','','')";
                DataTable dt = dat.mysql(SSQL);
                gvMovimientos.DataSource = dt;
                gvMovimientos.DataBind();

                if (gvMovimientos.Rows.Count == 0)
                {
                    movimientos123.Visible = false;
                    lblNoHayMovimientos.Text = "No hay movimientos";
                }
                else
                {
                    movimientos123.Visible = true;
                    lblNoHayMovimientos.Text = "";
                }
            }
            catch (Exception ex)
            {
                movimientos123.Visible = false;
                lblNoHayMovimientos.Text = "Ocurrió un error al cargar los movimientos " + ex.Message.ToString();
            }


        }

        protected void lnkCagarCartas_Click(object sender, EventArgs e)
        {
            gvImpresionCartasAnio.DataSource = null;
            gvImpresionCartasAnio.DataBind();
            gvImpresionCartasMes.DataSource = null;
            gvImpresionCartasMes.DataBind();
            gvImpresionCartasDetalle.DataSource = null;
            gvImpresionCartasDetalle.DataBind();
            ViewState["ANIO"] = null;
            ViewState["COD_MES"] = null;
            loadIC(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void lnkCargarRecord_Click(object sender, EventArgs e)
        {
            gvDatosDetalle1.DataSource = null;
            gvDatosDetalle1.DataBind();
            gvDatosDetalle2.DataSource = null;
            gvDatosDetalle2.DataBind();
            gvDatosDetalle3.DataSource = null;
            gvDatosDetalle3.DataBind();
            ViewState["ANIORC"] = null;
            ViewState["COD_MESRC"] = null;
            loadDetail();
        }

        protected void lnkCargarInformes_Click(object sender, EventArgs e)
        {
            gvInformesMedicosAnio.DataSource = null;
            gvInformesMedicosAnio.DataBind();
            gvInformesMedicosMes.DataSource = null;
            gvInformesMedicosMes.DataBind();
            gvInformesMedicosDetalle.DataSource = null;
            gvInformesMedicosDetalle.DataBind();
            ViewState["ANIO1"] = null;
            ViewState["COD_MES1"] = null;
            loadIM(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void lnkExportarIC_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (69,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','','')";
                dtab1 = dat.mysql(xSQL);
                if (ViewState["ANIO"] != null)
                {
                    string SSQL = "CALL SP_REPORTES_IAFA (70,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO"].ToString() + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','')";
                    dtab2 = dat.mysql(SSQL);
                }
                if ((ViewState["ANIO"] != null) && (ViewState["COD_MES"] != null))
                {
                    string YSQL = "CALL SP_REPORTES_IAFA (71,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO"].ToString() + "','" + ViewState["COD_MES"].ToString() + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','')";
                    dtab3 = dat.mysql(YSQL);
                }
            }
            catch (Exception ex)
            {
                lblNohayCarta.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3, "IMPRESION DE CARTAS", txtApellidom.Text + " " + txtApellidop.Text + ", " + txtNombres.Text);
        }

        protected void lnkExportarRecord_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (4,'" + txtNumeroPoli.Text + "','','','','" + txtApellidop.Text + ' ' + txtApellidom.Text + ", " + txtNombres.Text + "','','')";
                dtab1 = dat.mysql(xSQL);

                NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;

                if (ViewState["ANIORC"] != null)
                {
                    string SSQL = "CALL SP_REPORTES_IAFA (6,'" + txtNumeroPoli.Text + "','','','','" + ViewState["ANIORC"].ToString() + "','" + NOMBRECOMPLETO + "','')";
                    dtab2 = dat.mysql(SSQL);

                    NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;
                }

                if ((ViewState["ANIORC"] != null) && (ViewState["COD_MESRC"] != null))
                {
                    string YSQL = "CALL SP_REPORTES_IAFA (13,'" + txtNumeroPoli.Text + "','','','','" + ViewState["ANIORC"].ToString() + "','" + ViewState["COD_MESRC"].ToString() + "','" + NOMBRECOMPLETO + "')";
                    dtab3 = dat.mysql(YSQL);
                }

            }
            catch (Exception ex)
            {
                lblNoHayRecord.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3, "RECORD DE CONSUMO", txtApellidom.Text + " " + txtApellidop.Text + ", " + txtNombres.Text);

        }

        protected void lnkExportarInformesMedicos_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (72,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','','','','')";
                dtab1 = dat.mysql(xSQL);
                if (ViewState["ANIO1"] != null)
                {
                    string SSQL = "CALL SP_REPORTES_IAFA (73,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO1"].ToString() + "','','','')";
                    dtab2 = dat.mysql(SSQL);
                }
                if ((ViewState["ANIO1"] != null) && (ViewState["COD_MES1"] != null))
                {
                    string YSSQL = "CALL SP_REPORTES_IAFA (74,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO1"].ToString() + "','" + ViewState["COD_MES1"].ToString() + "','','')";
                    dtab3 = dat.mysql(YSSQL);
                }

            }
            catch (Exception ex)
            {
                lblNoHayRecord.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3, "INFORMES MÉDICOS", txtApellidom.Text + " " + txtApellidop.Text + ", " + txtNombres.Text);
        }

        protected void btnCancelarAviso_Click(object sender, EventArgs e)
        {
            lblAvisoDesc.Visible = false;
        }

        protected void lnkImpreOrden_Click(object sender, EventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/loginIII.php?u=" + usu.USER.ToString() + "&p=" + usu.PASS.ToString() + "&d=3&p2=" + txtNumeroPoli.Text + txtCodigoTitu.Text + categoriaHidden.Value + "');", true);

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#GRUPOFAMILIAR').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

        }

        protected void btnVOIP_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionVoip.aspx");
        }
        #region








        #endregion
        protected void txtCodigoTitu_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigoTitu.Text.Length < 6)
            {
                txtCodigoTitu.Text = "";
                lblCodigoTitularCarac.Text = "El código titular debe tener 6 caracteres";
                lblCodigoTitularCorrecto.Text = "";
                return;
            }
            else
            {
                lblCodigoTitularCorrecto.Text = "Código Correcto";
                lblCodigoTitularCarac.Text = "";
                return;
            }
        }

        protected void btnSubir2_Click(object sender, EventArgs e)
        {

            if (flpImp.FileName != "")
            {
                string fileName = Path.GetFileName(flpImp.PostedFile.FileName);
                flpImp.PostedFile.SaveAs((Server.MapPath("~/SUBIDOS2/") + fileName));


                if (IsPostBack)
                {
                    dt2 = Util.GenerarDataTable(rGuardar + flpImp.FileName);
                }

                foreach (DataRow row in dt2.Rows)
                {
                    try
                    {
                        string xLoadTAb = "INSERT INTO PRIM_CARG_II(iafas,cod_titula,fecha,periodo,tipoPC,tipoIP,monto,consumo,pago,nuevoSaldo,fecha_reg,usuario)" +
                                          "VALUES ('" + row["valor1"].ToString() + "','" + row["valor2"] + "','" + row["valor3"] + "','" + row["valor4"] + "','" + row["valor5"] + "','','" + row["valor6"] + "','" + row["valor7"] + "','" + row["valor8"] + "','" + row["valor9"] + "','" + DateTime.Now + "','" + row["valor10"] + "')";
                        dat.mysql(xLoadTAb);

                    }
                    catch (Exception ex)
                    {
                        lblErrorReg.Text = "ERROR DE ESTRUCTURA :" + ex.Message.ToString();
                        return;
                    }

                }

                string xSQL = "call sp_proc_basic (9,'','')";

                try
                {

                    DataTable dt = dat.mysql(xSQL);
                    lblalerta.Text = "IMPORTACIÓN SATISFACTORIA";

                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(rGuardar);
                FileInfo[] fileInfo = dirInfo.GetFiles("*.*");

                for (int x = 0; x <= fileInfo.Count() - 1; x++)
                {

                    try
                    {
                        File.Delete(rGuardar + fileInfo[x].Name.ToString());
                    }
                    catch (Exception ex)
                    {
                        lblErrorReg.Text = ex.Message.ToString();
                        return;
                    }
                }

            }
            else
            {
                lblErrorReg.Text = "ADVERTENCIA, Debe seleccionar el archivo que desea subir";
                return;
            }
        }



        void ValidarBotonFallecido(string cliente, string titula, string categoria, int estado)
        {
            DataTable dt = dat.mysql("call sp_fecha_bajas ('5','" + cliente + "','" + titula + "','" + categoria + "','','','','','','','')");

            if (estado == 0)
            {
                lnkFallecido.Visible = false;
                divFallecido.Attributes.Add("style", "display:none;");
                return;
            }
            if (categoria == "00")
            {
                lnkFallecido.Visible = true;
                divFallecido.Attributes.Add("style", "float: left; display: inline-flex;");
            }

            else
            {
                divFallecido.Attributes.Add("style", "display:none;");
                lnkFallecido.Visible = false;
            }
        }

        void CargarAfiliado(string usuario, string clientegrilla, string codigogrilla, string comparacion, string categoriagrilla, object sender, GridViewCommandEventArgs e)
        {

            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(usuario));
            NOMBRECOMPLETO = "";
            lblNohayCarta.Text = "";
            lblNoHayInformes.Text = "";
            lblNoHayRecord.Text = "";
            lblCodigoTitularNoHay.Text = "";

            if (Convert.ToInt32(comparacion) > 4 && Convert.ToInt32(comparacion) <= 26)
            {
                comparacion = "04";
            }

            List<Titular> list = new TitularBL().ListarTitularesGrupo(53, clientegrilla, codigogrilla, categoriagrilla);
            Titular objTitu = list.First(delegate (Titular obj) { return (obj.cod_cliente.Equals(clientegrilla) && (obj.cod_titula.Equals(codigogrilla) && (obj.categoria.Equals(categoriagrilla)))); });


            List<Titular_Detalle> list2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, clientegrilla, codigogrilla, categoriagrilla);
            Titular_Detalle objTitudeta = list2.First(delegate (Titular_Detalle objt) { return (objt.cod_cliente.Equals(clientegrilla) && (objt.cod_titula.Equals(codigogrilla) && (objt.categoria.Equals(categoriagrilla)))); });


            List<TituList> list3 = new TitularBL().Busqueda(77, clientegrilla, codigogrilla, categoriagrilla);
            TituList objTitulist = list3.First(delegate (TituList objtl) { return (objtl.cod_cliente.Equals(clientegrilla)) && (objtl.cod_titula.Equals(codigogrilla) && (objtl.categoria.Equals(categoriagrilla))); });

            try
            {
                if (objTitu != null || objTitudeta != null)
                {
                    ValidarBotonFallecido(clientegrilla, codigogrilla, categoriagrilla, objTitu.estado_titular);
                    //---CUANDO ES TITULAR---
                    if (categoriagrilla == "00")
                    {
                        divClasificacion.Attributes.Add("style", "display:initial;");
                        lblAfiliado.Text = "EDITAR TITULAR";
                        ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                        ddlCategoria.DataValueField = "valor";
                        ddlCategoria.DataTextField = "descrip";
                        ddlCategoria.DataBind();
                    }
                    else
                    {
                        divClasificacion.Attributes.Add("style", "display:none;");
                        lblAfiliado.Text = "EDITAR DEPENDIENTE";
                    }

                    btnGuardarModificar.Visible = true;
                    btnGuardarRegistrar.Visible = false;
                    divchkSusalud.Visible = false;
                    btnBajaModal.Visible = true;
                    txtNumeroPoli.Attributes.Add("readonly", "readonly");
                    txtNombreEmpresa.Attributes.Add("readonly", "readonly");
                    txtCodigoTitu.ReadOnly = true;



                    ddlCategoria.Attributes.Add("readonly", "readonly");
                    chkConcubina.Enabled = false;
                    ddlCategoria.CssClass = "form-control input-sm disabled-button";

                    if (clientegrilla == "98")
                    {
                        ddlPlan.Attributes.Add("readonly", "readonly");
                        ddlPlan.CssClass = "form-control input-sm disabled-button";
                    }
                    else
                    {
                        ddlPlan.Attributes.Remove("readonly");
                        ddlPlan.CssClass = "form-control input-sm";

                    }

                    cod_paciente.Visible = false;
                    id_paciente.Visible = false;
                    pad.Visible = false;
                    //---CUANDO ESTA ACTIVO O DESACTIVADO---


                    string est = "";
                    DataTable dtest = dat.mysql("call sp_fill_2(19,'" + objTitu.cod_cliente + "','" + objTitu.cod_titula + "','" + objTitu.categoria + "')");
                    if (dtest.Rows.Count > 0)
                    {
                        est = dtest.Rows[0][0].ToString();
                    }
                    if (objTitu.estado_titular == 1)
                    {
                        btnActivarAfiliado.Attributes.Add("disabled", "disabled");
                        btnGuardarModificar.Attributes.Remove("disabled");
                        btnBajaModal.Attributes.Remove("disabled");
                        lblEstado.Text = "||COD: " + objTitu.cod_cliente.ToString() + " - " + objTitu.cod_titula.ToString() + " - " + objTitu.categoria.ToString()
                                  + " || DNI: " + objTitu.dni.ToString()
                                  + " ||  " + objTitu.afiliado.ToString()
                                  + " || EDAD: " + objTitudeta.edad.ToString()
                                  + "<br />"
                                  + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; || FECHA ALTA: " + objTitu.fch_alta.ToString()
                                  + " || FECHA BAJA: <label class='text-danger'> " + objTitu.fch_baja.ToString() + "</label>"
                                  + " || FECHA CARENCIA: " + objTitu.fch_caren.ToString()
                                  + " || ESTADO: " + est + "";
                        btnActivarAfiliado.Visible = false;
                    }
                    else
                    {
                        btnBajaModal.Attributes.Add("disabled", "disabled");
                        //btnActivarAfiliado.Attributes.Remove("disabled");
                        btnGuardarModificar.Attributes.Add("disabled", "disabled");
                        lblEstado.Text = "||COD: " + objTitu.cod_cliente.ToString() + " - " + objTitu.cod_titula.ToString() + " - " + objTitu.categoria.ToString()
                                        + " || DNI: " + objTitu.dni.ToString()
                                        + " ||  " + objTitu.afiliado.ToString()
                                        + " || EDAD: " + objTitudeta.edad.ToString()
                                        + "<br />"
                                        + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; || FECHA ALTA: " + objTitu.fch_alta.ToString()
                                        + " || FECHA BAJA: <label class='text-danger'> " + objTitu.fch_baja.ToString() + "</label>"
                                        + " || FECHA CARENCIA: " + objTitu.fch_caren.ToString()
                                        + " || ESTADO: " + est + "";
                    }


                    //---ROL DE USUARIO ES 100 O 50---

                    if (usu.ROL == "50")
                    {
                        btnGuardarModificar.Attributes.Add("disabled", "disabled");
                        btnDependienteNuevo.Attributes.Add("disabled", "disabled");
                        btnBajaModal.Attributes.Add("disabled", "disabled");
                    }

                    if (objTitu.estado_titular == 0)
                    {
                        //----VALIDACION ACTIVAR AFILIADO

                        DataTable dtActivaAfiliado = dat.mysql("call sp_fill (81,'" + Session["USUARIO"] + "','','')");

                        if (dtActivaAfiliado.Rows.Count > 0)
                        {
                            if (dtActivaAfiliado.Rows[0]["ACTIVA_AFI"].ToString() == "1")
                            {
                                if (objTitudeta.fallecido == "1")
                                {
                                    btnActivarAfiliado.Attributes.Add("disabled", "disabled");
                                    btnActivarAfiliado.Visible = false;
                                }
                                else
                                {
                                    btnActivarAfiliado.Attributes.Remove("disabled");
                                    btnActivarAfiliado.Visible = true;
                                }

                            }
                            else
                            {
                                btnActivarAfiliado.Attributes.Add("disabled", "disabled");
                                btnActivarAfiliado.Visible = false;
                            }
                        }
                    }

                    //----VALIDACION GENERAR ORDENES

                    DataTable dtGeneraOrdenes = dat.mysql("call sp_fill (80,'" + Session["USUARIO"] + "','','')");

                    if (dtGeneraOrdenes.Rows.Count > 0)
                    {
                        if (dtGeneraOrdenes.Rows[0]["GENERAORDEN"].ToString() == "1")
                        {
                            lnkImpreOrden.Attributes.Remove("disabled");
                        }
                        else
                        {
                            lnkImpreOrden.Attributes.Add("disabled", "disabled");
                        }
                    }

                    string SSQLX = "call sp_fill (72,'" + clientegrilla + "','" + codigogrilla + "','" + categoriagrilla + "')";
                    DataTable dt = dat.mysql(SSQLX);

                    if (dt.Rows.Count > 0)
                    {
                        Image1.ImageUrl = dt.Rows[0][0].ToString();
                    }

                    if (clientegrilla == "15")
                    {
                        campo2.Visible = true;
                    }
                    if ((clientegrilla == "57"))
                    {
                        if ((objTitudeta.segunda_capa == "N") && (objTitudeta.basico == "N") && (objTitudeta.onco == "N") && (objTitu.estado_titular == 0) && (objTitu.fch_baja == ""))
                        {
                            btnGuardarModificar.Attributes.Remove("disabled");
                        }
                        divContratante.Visible = true;
                        ocultos2.Visible = true;
                        segundacapa.Visible = true;
                        basico.Visible = true;
                        onco.Visible = true;
                        documentoSIMA.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;

                        if (objTitudeta.contratante == "1")
                        {
                            chkContratante.Checked = true;
                        }
                        else
                        {
                            chkContratante.Checked = false;
                        }
                    }
                    else
                    {
                        divContratante.Visible = false;
                        chkContratante.Checked = false;
                    }

                    if (clientegrilla == "37")
                    {
                        lnkCartasGarantia.Visible = true;
                    }
                    else
                    {
                        lnkCartasGarantia.Visible = false;
                    }


                    if ((clientegrilla == "90") || (clientegrilla == "96") || (clientegrilla == "98") || (clientegrilla == "95"))
                    {
                        if (categoriagrilla == "00")
                        {
                            cod_paciente.Visible = false;
                            id_paciente.Visible = false;
                        }
                        else if (Convert.ToInt32(categoriagrilla) >= 04 && Convert.ToInt32(categoriagrilla) <= 20)
                        {
                            cod_paciente.Visible = true;
                            id_paciente.Visible = true;
                        }
                        else if (Convert.ToInt32(categoriagrilla) == 01)
                        {
                            cod_paciente.Visible = true;
                            concubina.Visible = true;
                        }
                        else
                        {
                            cod_paciente.Visible = true;
                        }
                        dpto.Visible = true;
                        rol.Visible = true;
                        ocultos2.Visible = true;
                        pad.Visible = true;
                        programa.Visible = true;


                    }

                    txtNumeroPoli.Text = Convert.ToString(objTitu.cod_cliente);
                    CombosCliente(clientegrilla, objTitu.plan.ToString());
                    cartas123.Visible = false;
                    record123.Visible = false;
                    informes123.Visible = false;
                    NuevoAviso.Visible = false;
                    BuscaAviso.Visible = true;
                    avisos(clientegrilla, objTitu.cod_titula, Convert.ToString(objTitu.categoria));
                    movimientos(clientegrilla, objTitu.cod_titula.ToString(), Convert.ToString(objTitu.categoria), ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(), usu.ID.ToString());

                    txtNombreEmpresa.Text = hfNombreEmpresa.Value.ToString().Substring(3);
                    txtCodigoTitu.Text = objTitu.cod_titula;

                    categoriaHidden.Value = categoriagrilla;
                    ddlCategoria.SelectedValue = comparacion;
                    ddlCentro.SelectedValue = Convert.ToString(objTitu.centro_costo);
                    ddlPlan.SelectedValue = Convert.ToString(objTitu.plan);
                    hfPlan.Value = Convert.ToString(objTitu.plan);
                    ddlSexo.SelectedValue = Convert.ToString(objTitu.sexo);
                    ddlTipoDocumento.SelectedValue = Convert.ToString(objTitu.tipo_doc);
                    txtNacimiento.Text = Convert.ToString(objTitu.fch_naci);
                    txtAlta.Text = Convert.ToString(objTitu.fch_alta);
                    txtBaja.Text = Convert.ToString(objTitu.fch_baja);
                    txtDNI.Text = Convert.ToString(objTitu.dni);


                    if (Page.IsPostBack)
                    {
                        txtContraseña.Attributes.Add("Value", Convert.ToString(objTitu.pass));
                    }

                    //AQUI VIENE EL DETALLE DEL AFILIADO
                    txtEdad.Text = Convert.ToString(objTitulist.edad);

                    ddlDepartamento.SelectedValue = Convert.ToString(objTitudeta.depa_id);
                    ddlDepartamento_SelectedIndexChanged(sender, e);
                    ddlProvincia.SelectedValue = Convert.ToString(objTitudeta.prov_id);
                    ddlProvincia_SelectedIndexChanged(sender, e);
                    ddlDistrito.SelectedValue = Convert.ToString(objTitudeta.dist_id);
                    txtDireccion.Text = Convert.ToString(objTitudeta.direccion);
                    txtObservar.Text = Convert.ToString(objTitudeta.email);
                    txtCorreo1.Text = Convert.ToString(objTitudeta.correo1);
                    txtCorreo2.Text = Convert.ToString(objTitudeta.correo2);
                    txtTelefono1.Text = Convert.ToString(objTitudeta.t_fijo);
                    txtTelefono2.Text = Convert.ToString(objTitudeta.t_movil);
                    ddlEstadoCivil.SelectedValue = Convert.ToString(objTitudeta.estado_civil);
                    txtEdad.Text = Convert.ToString(objTitudeta.edad);
                    txtPeso.Text = Convert.ToString(objTitudeta.peso);
                    txtEstatura.Text = Convert.ToString(objTitudeta.estatura);
                    ddlDiscapacit.SelectedValue = Convert.ToString(objTitudeta.discapacitado);
                    ddlAlcohol.SelectedValue = Convert.ToString(objTitudeta.consume_alcohol);
                    ddlDrogas.SelectedValue = Convert.ToString(objTitudeta.consume_tabaco);
                    txtSangre.Text = Convert.ToString(objTitudeta.grupo_sanguineo);
                    txtCarencia.Text = Convert.ToString(objTitu.fch_caren);
                    ddlPad.SelectedValue = Convert.ToString(objTitudeta.pad);
                    txtDpto.Text = Convert.ToString(objTitudeta.dpto);
                    txtRol.Text = Convert.ToString(objTitudeta.rol);
                    txtCodPaciente.Text = Convert.ToString(objTitudeta.cod_paciente);
                    ddlClasificacion.SelectedValue = Convert.ToString(objTitudeta.clasificacion);
                    if (txtCodPaciente.Text == "1")
                    {
                        chkConcubina.Checked = false;
                    }
                    if (txtCodPaciente.Text == "20")
                    {
                        chkConcubina.Checked = true;
                    }
                    txtIdPaciente.Text = Convert.ToString(objTitudeta.id_paciente);
                    ddlPespecial.SelectedValue = Convert.ToString(objTitudeta.prog_especial);
                    txtCampo2.Text = Convert.ToString(objTitu.campo2);
                    txtCorreo1.Text = Convert.ToString(objTitudeta.correo1);
                    txtCorreo2.Text = Convert.ToString(objTitudeta.correo2);

                    //AQUI DEBERIA IR CODIGO Y ID PACIENTE

                    ddlBasico.SelectedValue = Convert.ToString(objTitudeta.basico);
                    ddlOnco.SelectedValue = Convert.ToString(objTitudeta.onco);
                    ddlCapa.SelectedValue = Convert.ToString(objTitudeta.segunda_capa);
                    txtDocumento.Text = Convert.ToString(objTitudeta.docum);
                    txtNombres.Text = Convert.ToString(objTitudeta.afi_nombre);
                    txtApellidop.Text = Convert.ToString(objTitudeta.afi_apepat);
                    txtApellidom.Text = Convert.ToString(objTitudeta.afi_apemat);

                    NOMBRECOMPLETO = objTitudeta.afi_apepat + " " + objTitudeta.afi_apemat + ", " + objTitudeta.afi_nombre;
                    //CODIGOTITULAR = objTitu.cod_titula;
                    //cartas(clientegrilla, NOMBRECOMPLETO);

                    string SSQLX1 = "CALL SP_UpdateTitu(6,'" + objTitu.cod_cliente + "','" + objTitu.cod_titula + "','" + objTitu.categoria + "','','','','','','','','','','','','')";
                    DataTable dta = dat.mysql(SSQLX1);

                    if (dta.Rows.Count > 0)
                    {
                        lnkActualizacionTitu2.Visible = true;
                    }
                    else
                    {
                        lnkActualizacionTitu2.Visible = false;
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#NUEVOAFILIADO').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
                else
                {
                    lblErrorReg.Text = "El Afiliado seleccionado no tiene datos para mostrar";
                }

            }
            catch (Exception ex)
            {
                lblErrorReg.Text = "Ocurrió un error de carga de datos." + ex.Message;
                return;
                //throw;
            }
        }

        #region FTP

        private string DeleteFile(string fileName, string cliente)
        {
            if (fileName != "")
            {
                if ((System.IO.File.Exists("~/fotos/" + cliente + "/" + fileName)))
                {
                    System.IO.File.Delete("~/fotos/" + cliente + "/" + fileName);
                }

                return "Correcto";

                //string ftp = "ftp://10.100.100.6";
                //string ftpFolder = "/r0/public_html/solben_net/production/www/solben/foto/" + cliente + "/";
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                //request.Method = WebRequestMethods.Ftp.DeleteFile;
                //request.Credentials = new NetworkCredential("root", "wp@z0123qwe");

                //using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                //{
                //    return response.StatusDescription;
                //}
            }
            else
            {
                return "0";
            }
        }

        void ftp(HttpPostedFile file, string ruta_path, string archivo_nombre, string cliente)
        {
            string ftp = "ftp://10.100.100.6";
            string ftpFolder = "/r0/public_html/solben_net/production/www/solben/foto/" + cliente + "/";

            try
            {
                string ftpUserID = "root";
                string ftpPassword = "wp@z0123qwe";
                FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + archivo_nombre);
                ftpReq.UseBinary = true;
                ftpReq.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpReq.Method = WebRequestMethods.Ftp.UploadFile;


                byte[] b = File.ReadAllBytes(Server.MapPath("~/FOTOS/" + archivo_nombre).ToString());

                ftpReq.ContentLength = b.Length;
                using (Stream s = ftpReq.GetRequestStream())
                {
                    s.Write(b, 0, b.Length);
                }

                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();

                if (ftpResp != null)
                {
                    //lblalerta.Text = ftpResp.StatusDescription;
                    lblalerta.Text = "Archivo Subido";
                }


                //##DESCOMENTAR##2##VECES##################################

                //////string script = "";
                //////string us = "wpazo";
                //////string pass = "190181";

                //////FTP Server URL.
                ////string ftp = "ftp://10.100.100.6/";

                //////FTP Folder name. Leave blank if you want to upload to root folder.
                //////string ftpFolder = "Uploads/";
                ////string ftpFolder = "r0/public_html/solben_net/production/www/solben/foto/"+cliente+"/" ;

                ////byte[] fileBytes = null;

                //////Read the FileName and convert it to Byte array.
                ////string fileName = Path.GetFileName(archivo_nombre);
                ////using (StreamReader fileStream = new StreamReader(file.InputStream))
                ////{
                ////    fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                ////    fileStream.Close();
                ////}

                ////try
                ////{
                ////    //Create FTP Request.
                ////    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                ////    request.Method = WebRequestMethods.Ftp.UploadFile;

                ////    //Enter FTP Server credentials.
                ////    request.Credentials = new NetworkCredential("root", "wp@z0123qwe");
                ////    request.ContentLength = fileBytes.Length;
                ////    request.UsePassive = true;
                ////    request.UseBinary = true;
                ////    request.ServicePoint.ConnectionLimit = fileBytes.Length;
                ////    request.EnableSsl = false;

                ////    using (Stream requestStream = request.GetRequestStream())
                ////    {
                ////        requestStream.Write(fileBytes, 0, fileBytes.Length);
                ////        requestStream.Close();
                ////    }

                ////    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                ////    //lblFotoError.Text = "Archivo" + fileName + "subido.";
                ////    response.Close();
            }
            catch (WebException ex)
            {
                lblError.Text = ex.Message.ToString();
                //throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }

        }

        void sftp(string ruta_path, string archivo_nombre, string cliente)
        {
            string _ftpURL = "10.100.100.6"; //Host URL or address of the SFTP server
            string _UserName = "wpazo";      //User Name of the SFTP server
            string _Password = "wp@z0123qwe";   //Password of the SFTP server
            int _Port = 2222;                  //Port No of the SFTP server (if any)
            string _ftpDirectory = "/r0/public_html/solben_net/production/www/solben/foto/" + cliente; //The directory in SFTP server where the files will be uploaded
            string LocalDirectory = ruta_path;   //Local directory from where the files will be uploaded  Server.MapPath("~/FOTOS/" + archivo_nombre).ToString()
            string FileName = archivo_nombre;    //File name, which one will be uploaded

            //IPHostEntry ip = Dns.GetHostByAddress(_ftpURL);
            Sftp oSftp = new Sftp(_ftpURL.ToString(), _UserName, _Password);
            oSftp.Connect(_Port);
            oSftp.Put(LocalDirectory + FileName, _ftpDirectory);
            //+ "/" + FileName
            oSftp.Close();

            //const int port = 22;
            //const string host = "domainna.me";
            //const string username = "chucknorris";
            //const string password = "norrischuck";
            //const string workingdirectory = "/highway/hell";
            //const string uploadfile = @"c:yourfilegoeshere.txt";

            //Console.WriteLine("Creating client and connecting");
            //using (var client = new SftpClient(host, port, username, password))
            //{
            //    client.Connect();
            //    Console.WriteLine("Connected to {0}", host);

            //    client.ChangeDirectory(workingdirectory);
            //    Console.WriteLine("Changed directory to {0}", workingdirectory);

            //    var listDirectory = client.ListDirectory(workingdirectory);
            //    Console.WriteLine("Listing directory:");
            //    foreach (var fi in listDirectory)
            //    {
            //        Console.WriteLine(" - " + fi.Name);
            //    }

            //    using (var fileStream = new FileStream(uploadfile, FileMode.Open))
            //    {
            //        Console.WriteLine("Uploading {0} ({1:N0} bytes)",
            //                            uploadfile, fileStream.Length);
            //        client.BufferSize = 4 * 1024; // bypass Payload error large files
            //        client.UploadFile(fileStream, Path.GetFileName(uploadfile));
            //    }
            //}
        }

        #endregion

        #region RowCommands

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                borrarmensajes();

                if (e.CommandName == "Editar")
                {
                    hfNombreEmpresa.Value = "";
                    RecordConsumoTab.Visible = true;
                    AvisosTab.Visible = true;
                    CartasTab.Visible = true;
                    btnNuevoAviso2.Visible = true;
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    string cliente = Convert.ToString(this.GridView2.DataKeys[currentRowIndex]["cod_cliente"]);
                    string cod_titula = Convert.ToString(this.GridView2.DataKeys[currentRowIndex]["cod_titula"]);
                    string clienteNombre = Convert.ToString(this.GridView2.DataKeys[currentRowIndex]["cod_cliente2"]);
                    hfNombreEmpresa.Value = clienteNombre;
                    grupofamiliar(cod_titula, cliente);

                    List<Titular> lista_Titu = new TitularBL().ListarTitularesGrupo(53, cliente, cod_titula, "00");
                    Titular titu = lista_Titu.First(delegate (Titular obj) { return (obj.cod_cliente.Equals(cliente) && (obj.cod_titula.Equals(cod_titula) && (obj.categoria.Equals("00")))); });
                    usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
                    if ((titu.categoria == "00") && (titu.estado_titular == 0))
                    {
                        btnDependienteNuevo.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        if (usu.ROL == "50")
                        {
                            btnDependienteNuevo.Attributes.Add("disabled", "disabled");
                        }
                        else
                        {
                            btnDependienteNuevo.Attributes.Remove("disabled");
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#GRUPOFAMILIAR').modal();");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }

            }
            catch (Exception ex)
            {

                lblErrorReg.Text = "ERROR, No se pudo cargar uno o mas datos del afiliado.";
            }


        }

        protected void gvGrupoFamiliar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string clientegrilla = Convert.ToString(this.gvGrupoFamiliar.DataKeys[currentRowIndex]["cod_cliente"]);
                string codigogrilla = Convert.ToString(this.gvGrupoFamiliar.DataKeys[currentRowIndex]["cod_titula"]);
                string comparacion = Convert.ToString(this.gvGrupoFamiliar.DataKeys[currentRowIndex]["categoria"]);
                string categoriagrilla = Convert.ToString(this.gvGrupoFamiliar.DataKeys[currentRowIndex]["categoria"]);

                iPassword.Visible = true;
                hfClienteGrilla.Value = clientegrilla;

                CargarAfiliado(Session["USUARIO"].ToString(), clientegrilla, codigogrilla, comparacion, categoriagrilla, sender, e);
                ddlPlan_SelectedIndexChanged(sender, e);
            }
        }

        protected void gvAvisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                int op = Convert.ToInt32(this.gvAvisos.DataKeys[currentRowIndex]["op"]);
                string id = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["Cod Aviso"]);
                string desde = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["Desde"]);
                string hasta = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["Hasta"]);
                string aviso = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["Aviso"]);
                string idClasif = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["idClasif"]);
                string estado = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["estado"]);
                string limite = Convert.ToString(this.gvAvisos.DataKeys[currentRowIndex]["limite"]);

                if (op == 2 && estado == "1")
                {
                    idAviso.Value = "";
                    //btnNuevoAviso.Visible = false;
                    lblAvisoDesc.Visible = true;
                    btnNuevoAviso2.Visible = false;
                    txtdesde.Text = desde;
                    txthasta.Text = hasta;
                    txtAvisoDescrip.Text = aviso;
                    Ddl_ClasifAviso.SelectedValue = idClasif;
                    NuevoAviso.Visible = true;
                    BuscaAviso.Visible = false;
                    ListaAvisos.Visible = false;
                    //txtDescripcion.Visible = true;
                    //btnGrabando.Visible = false;
                    //btnEditar.Visible = true;
                    //txtDescripcion.Text = (string)this.gvAvisos.DataKeys[currentRowIndex]["Aviso"];
                    idAviso.Value = id;
                    btnGuardarAviso.Visible = false;
                    lnkGuardarCambios.Visible = true;
                    Ddl_ClasifAviso.CssClass = "form-control input-sm disabled-button";
                    Ddl_ClasifAviso.Attributes.Add("ReadOnly", "ReadOnly");
                    //txtdesde.CssClass = "form-control input-sm disabled-button";
                    //txtdesde.ReadOnly = true;
                    //txthasta.CssClass = "form-control input-sm disabled-button";
                    //txthasta.ReadOnly = true;
                    //chkSinLimite.CssClass = "input-sm disabled-button";
                    //chkSinLimite.Attributes.Add("ReadOnly", "ReadOnly");
                    if (limite == "1")
                    {
                        chkSinLimite.Checked = true;
                    }
                    else
                    {
                        chkSinLimite.Checked = false;
                    }
                }
                else if (estado == "0")
                {
                    lblAvisos.Text = "No editable";
                    return;
                }
                else
                {
                    lblAvisos.Text = "No editable";
                    return;
                }
            }

            if (e.CommandName == "Eliminar")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                int op = Convert.ToInt32(this.gvAvisos.DataKeys[currentRowIndex]["op"]);
                int id = Convert.ToInt32(this.gvAvisos.DataKeys[currentRowIndex]["Cod Aviso"]);
                int estado = Convert.ToInt32(this.gvAvisos.DataKeys[currentRowIndex]["estado"]);

                if (estado == 0)
                {
                    lblAvisos.Text = "No puede ser desactivado";
                    return;
                }


                switch (op)
                {
                    case 1:
                        {
                            string SSQL = "call sp_avisos (4,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + id + "','','')";
                            DataTable dt = dat.mysql(SSQL);
                            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, ddlCategoria.SelectedValue);
                            break;
                        }
                    case 2:
                        {
                            string SSQL1 = "call sp_avisos (5,'" + id + "','0','','','','')";
                            DataTable dt1 = dat.mysql(SSQL1);
                            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                            break;
                        }
                    default:
                        {
                            lblAvisos.Text = "No puede ser desactivado";
                            return;
                        }
                }

            }
        }

        #endregion

        #region Paginacion
        protected void gvAvisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAvisos.PageIndex = e.NewPageIndex;
            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);



        }

        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            gvMovimientos.PageIndex = e.NewPageIndex;
            string SSQL = "CALL SP_REPORTES_IAFA (75,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ddlMes.SelectedValue.ToString() + "','" + ddlAnio.SelectedValue.ToString() + "','" + usu.ID.ToString() + "','')";
            DataTable dt = dat.mysql(SSQL);
            gvMovimientos.DataSource = dt;
            gvMovimientos.DataBind();
        }

        protected void gvDatosDetalle3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatosDetalle3.PageIndex = e.NewPageIndex;
            cargaTablaDetalle3(txtNumeroPoli.Text, ViewState["ANIORC"].ToString(), ViewState["COD_MESRC"].ToString(), txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text);
        }

        protected void gvInformesMedicosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInformesMedicosDetalle.PageIndex = e.NewPageIndex;
            cargarIMMes(txtNumeroPoli.Text, ViewState["ANIO1"].ToString(), ViewState["COD_MES1"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void gvImpresionCartasDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImpresionCartasDetalle.PageIndex = e.NewPageIndex;
            cargarICMes(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), ViewState["COD_MES"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            //ddlTablas_SelectedIndexChanged(sender, e);
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue), Session["USUARIO"].ToString());
        }

        #endregion

        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((txtNumeroPoli.Text == "90"))
            {
                onco.Visible = true;
            }

            if ((txtNumeroPoli.Text == "57"))
            {
                if ((ddlPlan.SelectedValue == "11") || (ddlPlan.SelectedValue == "12"))
                {
                    segundacapa.Visible = false;
                    onco.Visible = false;
                }
                else if ((ddlPlan.SelectedValue == "10"))
                {
                    segundacapa.Visible = false;
                    onco.Visible = true;
                }
                else
                {
                    segundacapa.Visible = true;
                    onco.Visible = true;
                }
            }

            DataTable dt1 = dat.mysql("CALL SP_SUSALUD_REGAFI(14,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text
                                       + "','" + categoriaHidden.Value + "','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','');");
            DataTable dt2 = dat.mysql("CALL SP_SUSALUD_REGAFI(151,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text
                                                     + "','" + categoriaHidden.Value + "','" + ddlPlan.SelectedValue + "','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','');");
            if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0)
            {
                if (dt1.Rows[0][0].ToString() == "1" && dt2.Rows[0][0].ToString() == "1")
                {
                    if (txtNumeroPoli.Text == "15" || txtNumeroPoli.Text == "90")
                    {
                        if (categoriaHidden.Value == "00")
                        {
                            divAvisoCambioPlan.Attributes.Add("style", "display:block;");
                            planHidden.Value = "1";
                        }
                        else
                        {
                            divAvisoCambioPlan.Attributes.Add("style", "display:none;");
                            planHidden.Value = "0";
                        }
                    }
                    else
                    {
                        divAvisoCambioPlan.Attributes.Add("style", "display:none;");
                        planHidden.Value = "0";
                    }
                }
                else
                {
                    divAvisoCambioPlan.Attributes.Add("style", "display:none;");
                    planHidden.Value = "0";
                }
            }
            else
            {
                divAvisoCambioPlan.Attributes.Add("style", "display:none;");
                planHidden.Value = "0";
            }

        }

        protected void lnkBuscarTitular_Click(object sender, EventArgs e)
        {
            if (txtCodigoTitu.Text == "")
            {
                lblCodigoTitularNoHay.Text = "El código titular esta vacío.";
                return;
            }

            if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96"))
            {
                try
                {
                    string xSQL = "call sp_fill (75,'" + txtCodigoTitu.Text + "','" + ddlCategoria.SelectedValue.ToString() + "',0)";
                    DataTable dt2 = dat.mysql(xSQL);

                    if (dt2.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt2.Rows[0]["categoria"].ToString().Substring(dt2.Rows[0]["categoria"].ToString().Length - 1, 1)) >= 4)
                        {
                            ddlCategoria.SelectedValue = "04";
                        }
                        else
                        {
                            ddlCategoria.SelectedValue = dt2.Rows[0]["categoria"].ToString();
                        }
                        ddlPlan.SelectedValue = dt2.Rows[0]["plan"].ToString();
                        ddlCentro.SelectedValue = dt2.Rows[0]["cent_costo"].ToString();
                        ddlTipoDocumento.SelectedValue = dt2.Rows[0]["tipo_doc"].ToString();
                        txtDNI.Text = dt2.Rows[0]["dni"].ToString();
                        ddlPad.SelectedValue = dt2.Rows[0]["pad"].ToString();
                        txtNombres.Text = dt2.Rows[0]["afi_nombre"].ToString();
                        txtApellidop.Text = dt2.Rows[0]["afi_apepat"].ToString();
                        txtApellidom.Text = dt2.Rows[0]["afi_apemat"].ToString();
                        txtNacimiento.Text = dt2.Rows[0]["fch_naci"].ToString();
                        ddlSexo.SelectedValue = dt2.Rows[0]["sexo"].ToString();
                        ddlEstadoCivil.SelectedValue = dt2.Rows[0]["estado_civil"].ToString();
                        txtObservar.Text = dt2.Rows[0]["email"].ToString();
                        txtRol.Text = dt2.Rows[0]["rol"].ToString();
                        txtDpto.Text = dt2.Rows[0]["dpto"].ToString();
                        ddlDepartamento.SelectedValue = dt2.Rows[0]["depa_id"].ToString();
                        ddlProvincia.SelectedValue = dt2.Rows[0]["prov_id"].ToString();
                        ddlDistrito.SelectedValue = dt2.Rows[0]["dist_id"].ToString();
                        txtDireccion.Text = dt2.Rows[0]["direccion"].ToString();
                        txtTelefono1.Text = dt2.Rows[0]["t_fijo"].ToString();
                        txtTelefono2.Text = dt2.Rows[0]["t_movil"].ToString();
                        txtAlta.Text = dt2.Rows[0]["fch_alta"].ToString();
                        txtCarencia.Text = dt2.Rows[0]["fch_caren"].ToString();
                        txtBaja.Text = dt2.Rows[0]["fch_baja"].ToString();
                        ddlDiscapacit.SelectedValue = dt2.Rows[0]["discapacitado"].ToString();
                        txtContraseña.Text = dt2.Rows[0]["pass"].ToString();
                        txtEdad.Text = dt2.Rows[0]["edad"].ToString();
                        txtEstatura.Text = dt2.Rows[0]["estatura"].ToString();
                        txtSangre.Text = dt2.Rows[0]["grupo_sanguineo"].ToString();
                        ddlAlcohol.SelectedValue = dt2.Rows[0]["consume_alcohol"].ToString();
                        ddlDrogas.SelectedValue = dt2.Rows[0]["consume_tabaco"].ToString();
                        ddlPespecial.SelectedValue = dt2.Rows[0]["prog_especial"].ToString();
                        Image1.ImageUrl = dt2.Rows[0]["foto"].ToString();
                        lblCodigoTitularNoHay.Text = "Datos Cargados";
                    }
                    else
                    {

                        txtNombres.Text = "";
                        txtApellidop.Text = "";
                        txtApellidom.Text = "";
                        txtNacimiento.Text = "";
                        lblCodigoTitularNoHay.Text = "No se encontraron datos";
                        return;

                    }

                }
                catch (Exception ex)
                {
                    lblCodigoTitularNoHay.Text = ex.Message.ToString();
                    return;
                }
            }
            else
            {
                lblCodigoTitularNoHay.Text = "Función disponible, sólo para clientes 90,98,96";
            }



        }

        protected void lnkActualizacionTitu2_Click(object sender, EventArgs e)
        {
            string AfiId = txtNumeroPoli.Text + txtCodigoTitu.Text + categoriaHidden.Value;

            lblMsg.Text = "";

            cargaDatos(AfiId);

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#ACTTITU').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalScript", sb.ToString(), false);
        }

        public void cargaDatos(string VAL)
        {

            string SSQLX2 = null;
            SSQLX2 = "CALL SP_UpdateTitu(2,'" + VAL + "','','','','','','','','','','','','','','')";
            DataTable dt = dat.mysql(SSQLX2);

            if (dt.Rows.Count != 0)
            {
                //txtTipoDocu.Text = dt.Rows[0][9].ToString();
                ddlTipoDocu.SelectedValue = dt.Rows[0][7].ToString();
                txtNroDocu.Text = dt.Rows[0][8].ToString();
                txtEMail.Text = dt.Rows[0][15].ToString();
                txtTelFijo.Text = dt.Rows[0][16].ToString();
                txtTelMovil.Text = dt.Rows[0][17].ToString();
            }

            string SSQLX3 = null;
            SSQLX3 = "CALL SP_UpdateTitu(3,'" + VAL + "','','','','','','','','','','','','','','')";
            DataTable dt3 = dat.mysql(SSQLX3);

            if (dt3.Rows.Count != 0)
            {
                //txtTipoDocu2.Text = dt.Rows[0][6].ToString();
                ddlTipoDocu2.SelectedValue = dt3.Rows[0][4].ToString();
                txtNroDocu2.Text = dt3.Rows[0][5].ToString();
                txtEMail2.Text = dt3.Rows[0][7].ToString();
                txtTelFijo2.Text = dt3.Rows[0][8].ToString();
                txtTelMovil2.Text = dt3.Rows[0][9].ToString();
            }

            CheckTipoDocu2.Checked = false;
            CheckNroDocu2.Checked = false;
            CheckEMail2.Checked = false;
            CheckTelFijo2.Checked = false;
            CheckTelMovil2.Checked = false;
        }

        protected void LnkActualizar_Modal_Click(object sender, EventArgs e)
        {
            //cargaDatos(lblIdAfiliado.Text.ToString());

            string UpdateTipoDocu = null;
            string UpdateNroDocu = null;
            string UpdateEMail = null;
            string UpdateTelFijo = null;
            string UpdateTelMovil = null;
            //ddlTipoDocu.SelectedValue

            if (CheckTipoDocu2.Checked == true)
            {
                UpdateTipoDocu = ddlTipoDocu2.SelectedValue.ToString();
            }
            else
            {
                UpdateTipoDocu = ddlTipoDocu.SelectedValue.ToString();
            }

            if (CheckNroDocu2.Checked == true)
            {
                UpdateNroDocu = txtNroDocu2.Text.ToString();
            }
            else
            {
                UpdateNroDocu = txtNroDocu.Text.ToString();
            }

            if (CheckEMail2.Checked == true)
            {
                UpdateEMail = txtEMail2.Text.ToString();
            }
            else
            {
                UpdateEMail = txtEMail.Text.ToString();
            }

            if (CheckTelFijo2.Checked == true)
            {
                UpdateTelFijo = txtTelFijo2.Text.ToString();
            }
            else
            {
                UpdateTelFijo = txtTelFijo.Text.ToString();
            }

            if (CheckTelMovil2.Checked == true)
            {
                UpdateTelMovil = txtTelMovil2.Text.ToString();
            }
            else
            {
                UpdateTelMovil = txtTelMovil.Text.ToString();
            }

            string id = txtNumeroPoli.Text + txtCodigoTitu.Text + categoriaHidden.Value;

            try
            {
                string SSQLX = null;
                SSQLX = "CALL SP_UpdateTitu(5,'" + id + "','" + UpdateTipoDocu + "','" + UpdateNroDocu + "','" + UpdateEMail + "','" + UpdateTelFijo + "','" + UpdateTelMovil + "','" + Session["USUARIO"] + "','','','','','','','','')";
                DataTable dt = dat.mysql(SSQLX);
                lblMsg.Text = "Datos Actualizados correctamente";
                cargaDatos(id);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
                throw;
            }



            //string SSQLX2 = null;
            //SSQLX2 = "CALL SP_UpdateTitu(6,'" + lblIdAfiliado.Text.ToString() + "','" + ddlTipoDocu2.SelectedValue.ToString() + "','" + txtNroDocu2.Text.ToString() + "','" + txtEMail2.Text.ToString() + "','" + txtTelFijo2.Text.ToString() + "','" + txtTelMovil2.Text.ToString() + "','','','','','','','','','')";
            //dt = conex.mysql(SSQLX2);

            cargaDatos(lblIdAfiliado.Text.ToString());

        }

        protected void LnkCerrar_Modal_Click(object sender, EventArgs e)
        {
            //txtNumeroPoli.Text +  + ddlCategoria.SelectedValue.ToString();
            //CargarAfiliado(Session["USUARIO"].ToString(), txtNumeroPoli.Text, txtCodigoTitu.Text, ddlCategoria.SelectedValue, categoriaHidden.Value, sender, e);
            grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);

            ddlCategoria.DataSource = new ComboBL().ListaCombos(48);
            ddlCategoria.DataValueField = "valor";
            ddlCategoria.DataTextField = "descrip";
            ddlCategoria.DataBind();

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#GRUPOFAMILIAR').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkReporteAveria_Click(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            string password = Convert.ToString(dt.Rows[0][6].ToString());

            Response.Redirect("http://app.laprotectora.com.pe/mproy/wfLogin.aspx?usu=" + usuario + "&pass=" + password + "&origen=2");
        }

        protected void btnNuevoAviso2_Click(object sender, EventArgs e)
        {
            ListaAvisos.Visible = false;
            NuevoAviso.Visible = true;
            btnNuevoAviso2.Visible = false;
            BuscaAviso.Visible = false;
            Ddl_ClasifAviso.CssClass = "form-control input-sm";
            Ddl_ClasifAviso.Attributes.Remove("ReadOnly");
            txtdesde.CssClass = "form-control input-sm";
            txtdesde.ReadOnly = false;
            txthasta.CssClass = "form-control input-sm";
            txthasta.ReadOnly = false;
            chkSinLimite.CssClass = "input-sm";
            chkSinLimite.Attributes.Remove("ReadOnly");
            btnGuardarAviso.Visible = true;
            lnkGuardarCambios.Visible = false;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void btnGuardarAviso_Click(object sender, EventArgs e)
        {
            if ((txtdesde.Text == "") || (Ddl_ClasifAviso.SelectedIndex == 0))
            {
                lblAvisoNuevo.Text = "Ingrese los campos obligatorios (*)";
                return;
            }

            int limite;
            if (chkSinLimite.Checked == true)
            {
                limite = 1;
            }
            else
            {
                limite = 0;
            }

            string SSQLX = null;
            SSQLX = "CALL sp_avisos2(7, '" + txtNumeroPoli.Text + "','" +
                txtCodigoTitu.Text + "','" +
                categoriaHidden.Value + "','" +
                txtAvisoDescrip.Text.ToString() + "','" +
                Session["USUARIO"] + "','" +
                Ddl_ClasifAviso.SelectedValue.ToString() + "','" +
                txtdesde.Text + "','" +
                txthasta.Text + "','" +
                limite + "')";
            DataTable dt = dat.mysql(SSQLX);

            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

            ListaAvisos.Visible = true;
            NuevoAviso.Visible = false;
            BuscaAviso.Visible = true;

            LimpiarCampos();
        }

        protected void btnCancelAviso_Click(object sender, EventArgs e)
        {
            ListaAvisos.Visible = true;
            NuevoAviso.Visible = false;
            btnNuevoAviso2.Visible = true;
            BuscaAviso.Visible = true;
            txtdesde.Text = "";
            txthasta.Text = "";
            Ddl_ClasifAviso.SelectedIndex = 0;
            txtAvisoDescrip.Text = "";
            lblAvisos.Text = "";
        }

        void LimpiarCampos()
        {
            txtAvisoDescrip.Text = "";
            Ddl_ClasifAviso.SelectedIndex = 0;
            txtdesde.Text = "";
            txthasta.Text = "";
            chkSinLimite.Checked = false;
            btnNuevoAviso2.Visible = true;
            lblAvisos.Text = "";
            lblAvisoNuevo.Text = "";

        }

        protected void lnkGuardarCambios_Click(object sender, EventArgs e)
        {

            if ((txtdesde.Text == "") || (Ddl_ClasifAviso.SelectedIndex == 0))
            {
                lblAvisoNuevo.Text = "Ingrese los campos obligatorios (*)";
                return;
            }

            int limite;
            if (chkSinLimite.Checked == true)
            {
                limite = 1;
            }
            else
            {
                limite = 0;
            }

            string SSQL1 = "call sp_avisos (3,'" + idAviso.Value + "','" + txtAvisoDescrip.Text + "','" + Session["USUARIO"].ToString() + "','" + txtdesde.Text + "','" + txthasta.Text + "','" + limite + "')";
            DataTable dt1 = dat.mysql(SSQL1);

            avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

            ListaAvisos.Visible = true;
            NuevoAviso.Visible = false;
            BuscaAviso.Visible = true;

            LimpiarCampos();
        }

        protected void Ddl_ClasifAviso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Ddl_ClasifAviso.SelectedValue == "1")
            {
                txtAvisoDescrip.Text = "AFILIADO LLEGO A SU LIMITE DE COBERTURA. BRINDAR ATENCIÓN POR 2DA CAPA A TRAVÉS DE MAPFRE";

            }
            else
            {
                txtAvisoDescrip.Text = "";
            }
        }

        protected void chkSinLimite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSinLimite.Checked == true)
            {
                txthasta.Text = "";
                txthasta.ReadOnly = true;
            }
            else
            {
                txthasta.ReadOnly = false;
            }

        }

        protected void lnkCartasGarantia_Click(object sender, EventArgs e)
        {
            txtNroSiniestro.Text = "";
            txtPoliza.Text = "";
            txtPlaca.Text = "";
            txtComisaria.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtMonto.Text = "";
            txtPersonaLlamaNombre.Text = "";
            txtPersonaLlamaTelefono.Text = "";
            lblVerificacion22.Text = "";
            lblverificacion.Text = "";
            txtCartaGarantia.Text = "";
            txtNombreConductor.Text = "";
            txtFechaSiniestro.Text = DateTime.Today.ToString("dd/MM/yyyy");


            if ((hfNombres.Value == "") && (hfApellidoPaterno.Value == "") && (hfApellidoMaterno.Value == ""))
            {
                string datospersona = "";
                datospersona = "call sp_fill_2(16,'" + gvGrupoFamiliar.Rows[0].Cells[3].Text + "','','')";
                DataTable dt55 = dat.mysql(datospersona);

                hfNombres.Value = dt55.Rows[0]["afi_nombre"].ToString();
                hfApellidoPaterno.Value = dt55.Rows[0]["afi_apepat"].ToString();
                hfApellidoMaterno.Value = dt55.Rows[0]["afi_apemat"].ToString();
                hfDNI.Value = dt55.Rows[0]["dni"].ToString();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#CARTASMODAL').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkRegistrarCarta_Click(object sender, EventArgs e)
        {
            string SSQL1 = "";
            string SSQL2 = "";
            string SSQL3 = "";
            string SSQL4 = "";
            string SSQL5 = "";
            string SSQL6 = "";
            string montotext = "";
            string montodecimal = "";

            if (txtPoliza.Text == "" && txtPlaca.Text == "")
            {
                lblVerificacion22.Text = "INGRESE LOS CAMPOS REQUERIDOS";

            }
            else
            {
                try
                {

                    if (txtMonto.Text.Contains('.'))
                    {
                        montotext = Util.toText(Convert.ToDouble(txtMonto.Text.Split('.')[0]));
                        montodecimal = Util.toText(Convert.ToDouble(txtMonto.Text.Split('.')[1]));
                    }
                    else
                    {
                        montotext = Util.toText(Convert.ToDouble(txtMonto.Text));
                        montodecimal = Util.toText(Convert.ToDouble("00"));
                    }

                    string datospersona = "";
                    datospersona = "call sp_fill_2(15,'" + hfNombres.Value + "','" + hfApellidoPaterno.Value + "','" + hfApellidoMaterno.Value + "')";
                    DataTable dt55 = dat.mysql(datospersona);


                    SSQL1 = "call SP_INSERTAR_TSOAT(1,'" + txtFechaSiniestro.Text + "','" +
                                                              DateTime.Now.TimeOfDay.ToString() + "','SOAT','" +
                                                              txtNroSiniestro.Text + "','" +
                                                              txtPoliza.Text + "','" +
                                                              txtPlaca.Text + "','','','" +
                                                              dt55.Rows[0]["afi_nombre"].ToString() + "','" +
                                                              dt55.Rows[0]["afi_apepat"].ToString() + "','" +
                                                              dt55.Rows[0]["afi_apemat"].ToString() + "','" +
                                                              ddlTipoDocumento.SelectedItem.Text + "','" +
                                                              txtDNI.Text + "','" +
                                                              ddlClinica.SelectedItem.Text + "','" +
                                                              ddlTipoSiniestro.SelectedItem.Text + "','" +
                                                              DateTime.Now.ToShortDateString() + "','" +
                                                              DateTime.Now.TimeOfDay.ToString() + "','" +
                                                              ddlOcupante.SelectedValue + "','" +
                                                              ddlFallecido.SelectedValue + "','" + txtFechaFallecido.Text + "','" +
                                                              ddlUBIGEO.SelectedValue + "','ACEPTADO','" +
                                                              txtComisaria.Text + "','" +
                                                              txtApellidoPaterno.Text + "','" +
                                                              txtApellidoMaterno.Text + "','" +
                                                              txtNombreConductor.Text + "','','" +
                                                              ddlTipoAtencion.SelectedItem.Text + "','" + txtCartaGarantia.Text + "','" +
                                                              txtMonto.Text + "','" +
                                                              txtPersonaLlamaNombre.Text + "','" +
                                                              txtPersonaLlamaTelefono.Text + "','','1','" + montotext + "|" + montodecimal + "','" + ddlDiagnostico.SelectedItem.Text + "')";

                    DataTable dt1 = dat.mysql(SSQL1);

                    //SINIESTRO
                    SSQL2 = "sp_ver_mante 1,'" + DateTime.Today.ToString("MM/dd/yyyy") + "','" +
                                                 DateTime.Today.ToString("MM/dd/yyyy") + "','','28730','77','','110','6159898','0','0',' Placa: " + txtPlaca.Text + " || Asegurado: PROTECTA, PROTECTA','" + ddlTipoSiniestro.SelectedItem.Text + "','0.00','0','5','116','158','0','0','107','','0','0','DDE','1','0','66155','0','1','0','','','" + txtPersonaLlamaTelefono.Text + "','" + txtPersonaLlamaNombre.Text + "','','0','0','001301131128','','','','0','3784','0'";
                    DataTable dt2 = dat.TSegSQL(SSQL2);

                    //SEGUIMIENTO
                    SSQL3 = "EXEC SP_LOG '2','3', 1, 'SINIESTRO REPORTADO ||CARTA GARANTÍA: " + txtCartaGarantia.Text + " PLACA: " + txtPlaca.Text + "| CONDUCTOR:" + txtNombreConductor.Text + "', '" + dt2.Rows[0][0].ToString() + "'";

                    DataTable dt3 = dat.TSegSQL(SSQL3);

                    //SOLBEN_ID
                    SSQL4 = "call SP_Soat_Update_SolbenId(1,'" + txtNroSiniestro.Text + "','" +
                                                              txtPoliza.Text + "','" +
                                                              txtPlaca.Text + "','" +
                                                              txtDNI.Text + "','" +
                                                              txtCartaGarantia.Text + "','" +
                                                              dt55.Rows[0]["afi_nombre"].ToString() + "','" +
                                                              dt55.Rows[0]["afi_apepat"].ToString() + "','" +
                                                              dt55.Rows[0]["afi_apemat"].ToString() + "','" + ddlCobertura.SelectedValue + "')";

                    DataTable dt4 = dat.mysql(SSQL4);

                    //CREACION DE VEHICULO SINIESTRADO
                    SSQL5 = "SP_VER_9 30,'','28730','" + txtPlaca.Text + "','','','','','','','','','1','110','" +
                                                         dt2.Rows[0][0].ToString() + "','" +
                                                         txtNombreConductor.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text + "','','','','',1";
                    DataTable dt5 = dat.TSegSQL(SSQL5);

                    datospersona = "call sp_fill_2(15,'" + hfNombres.Value + "','" + hfApellidoPaterno.Value + "','" + hfApellidoMaterno.Value + "')";
                    dt55 = dat.mysql(datospersona);

                    lblCorrecto.Text = "CARTA DE GARANTÍA REGISTRADA";

                    hfNombres.Value = "";
                    hfApellidoPaterno.Value = "";
                    hfApellidoMaterno.Value = "";

                    grupofamiliar(dt55.Rows[0]["cod_titula"].ToString(), dt55.Rows[0]["cod_cliente"].ToString());
                    hfCodigoCliente.Value = dt55.Rows[0]["cod_cliente"].ToString();
                    hfCodigoTitular.Value = dt55.Rows[0]["cod_titula"].ToString();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#CARTASMODAL').modal('hide');");
                    sb.Append("$('#GRUPOFAMILIAR').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                    string RUTA = "http://www.solben.net/loginTitus.php?u=" + Session["USUARIO_USU"] + "&p=" + Session["PASS"] + "&p2=2&t=3&n=" + dt55.Rows[0]["solben_id"].ToString();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + RUTA + "');", true);
                }
                catch (Exception EX)
                {
                    lblVerificacion22.Text = EX.Message.ToString();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#CARTASMODAL').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                }
            }
        }

        protected void ddlFallecido_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#CARTASMODAL').modal('hide');");
            sb.Append("$('#NUEVOAFILIADO').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkTramaSoat_Click(object sender, EventArgs e)
        {
            /*Response.Redirect("TramaSoat.aspx");*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('TramaSoat.aspx');", true);
        }

        protected void lnkHistorial_Click(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_fill_2(18,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "')");
            gvHistorial.DataSource = dt;
            gvHistorial.DataBind();
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#HISTORIAL').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkCerrarHistorial_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#NUEVOAFILIADO').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void chkConcubina_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConcubina.Checked)
            {
                txtCodPaciente.Text = "20";
            }
            else
            {
                txtCodPaciente.Text = "1";
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = GridView2.DataKeys[e.Row.RowIndex].Values["clasif"].ToString();

                if (KeyID == "2")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#8ace8a");
                }

            }
        }

        protected void gvGrupoFamiliar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvGrupoFamiliar.DataKeys[e.Row.RowIndex].Values["clasif"].ToString();

                if (KeyID == "2")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#8ace8a");
                }

            }
        }

    }
}