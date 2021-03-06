﻿using System;
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

namespace SFW.Web
{
    public partial class wfMantenimiento : System.Web.UI.Page
    {
        int resultado;
        int contador;
        string idUsuario;
        Usuario usu = new Usuario();
        Datos dat = new Datos();
        public string NOMBRECOMPLETO = "";
        string rGuardar = HttpContext.Current.Server.MapPath("~/SUBIDOS2/");
        DataTable dt2 = new DataTable();
        DataTable dtab1, dtab2, dtab3 = new DataTable();

        void alertas()
        {
            string alrtas = "CALL SP_alertHijos('1','','1','','','" + DateTime.Today.ToString("yyyy-MM-dd") + "','');";
            DataTable dttt = dat.mysql(alrtas);
            lblTotal.Text = dttt.Rows.Count.ToString();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            txtBaja_CalendarExtender.StartDate = DateTime.Now.AddDays(-5);

            if (!Page.IsPostBack)
            {                
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("Sesion.aspx?usu=&pass=");
                }

                alertas();

                lblTotal.Text = "";
                ddlMes.SelectedValue = DateTime.Now.Month.ToString();
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                TextBox1.Text = Convert.ToString(DateTime.Today);
                Cargarcombos(Session["USUARIO"].ToString());
                ddlDepartamento_SelectedIndexChanged(sender, e);

                usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
                
                lblUsuario.Text = usu.NOMBRE;

                if (usu.AVERIA == "1")
                {
                    lnkReporteAveria.Visible = true;
                }
                else 
                {
                    lnkReporteAveria.Visible = false;
                }                
                
                Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue) , Session["USUARIO"].ToString());
                
                if (ddlTablas.SelectedValue == "15")
                {
                    btnImportar.Visible = true;
                }
                else
                {
                    btnImportar.Visible = false;
                }

                if (ddlTablas.SelectedValue == "37")
                {
                    lnkTramaSoat.Visible = true;
                }
                else
                {
                    lnkTramaSoat.Visible = false;
                }

                                                
                string validacionVoIP = "CALL validaciones_sp('1','" + usu.PERFIL + "','" + usu.ID + "','0');";
                DataTable dttt = dat.mysql(validacionVoIP);
                if (dttt.Rows.Count > 0)
                {
                    btnVOIP.Visible = Convert.ToBoolean(dttt.Rows[0]["Voip"]);                    
                }

                string validacionROL = "CALL validaciones_sp('2','" + usu.ROL + "','0','0');";
                DataTable td1 = dat.mysql(validacionROL);

                if (td1.Rows.Count > 0)
                {
                    btnReportes.Visible = Convert.ToBoolean(td1.Rows[0]["btnReportes"]);
                    btnNuevo.Visible = Convert.ToBoolean(td1.Rows[0]["btnNuevo"]);
                    btnMovimientos.Visible = Convert.ToBoolean(td1.Rows[0]["btnMovimientos"]);
                    lblRol.Text = td1.Rows[0]["lblRol"].ToString();
                }

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

            //

            //DataSet ds =  dat.MySQL_DS("sp_ver_combos()");

            //ddlTipoAtencion.DataSource = ds.Tables[0];
            //ddlTipoAtencion.DataValueField = "VALOR";
            //ddlTipoAtencion.DataTextField = "DESCRIP";
            //ddlTipoAtencion.DataBind();

            //ddlDiagnostico.DataSource = ds.Tables[1];
            //ddlDiagnostico.DataValueField = "VALOR";
            //ddlDiagnostico.DataTextField = "DESCRIP";
            //ddlDiagnostico.DataBind();

            //ddlClinica.DataSource = ds.Tables[2];
            //ddlClinica.DataValueField = "VALOR";
            //ddlClinica.DataTextField = "DESCRIP";
            //ddlClinica.DataBind();

            //ddlUBIGEO.DataSource = ds.Tables[3];
            //ddlUBIGEO.DataValueField = "VALOR";
            //ddlUBIGEO.DataTextField = "DESCRIP";
            //ddlUBIGEO.DataBind();

            //ddlTipoSiniestro.DataSource = ds.Tables[4];
            //ddlTipoSiniestro.DataValueField = "VALOR";
            //ddlTipoSiniestro.DataTextField = "DESCRIP";
            //ddlTipoSiniestro.DataBind();


            //ddlCobertura.DataSource = ds.Tables[5];
            //ddlCobertura.DataValueField = "VALOR";
            //ddlCobertura.DataTextField = "DESCRIP";
            //ddlCobertura.DataBind();

            //ddlDepartamento.DataSource = ds.Tables[6];
            //ddlDepartamento.DataTextField = "descrip";
            //ddlDepartamento.DataValueField = "valor";
            //ddlDepartamento.DataBind();
            
        }

        protected void limpiar()
        {
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
            //txtCodigoTitu.Text = "";
            txtContraseña.Text = "";
            txtSangre.Text = "";
            txtEdad.Text = "";
            txtPeso.Text = "";
            txtEstatura.Text = "";
            lblverificacion.Text = "";
            lblverificacion1.Text = "";
        }

        protected void borrarmensajes()
        {
            lblalerta.Text = "";
            lblCorrecto.Text = "";
            lblError.Text = "";
            lblErrorReg.Text = "";
            duplicado.Visible = false;
            lbldupli.Text = "";
        }

        protected void CombosCliente(string cliente,string plan)
        {
            //===========================================================================

            ddlPlan.DataSource = dat.mysql("call sp_fill(43,'" + cliente + "','" + plan + "','')");
            ddlPlan.DataValueField = "valor";
            ddlPlan.DataTextField = "descrip";
            ddlPlan.DataBind();

            //=======================================================================

            ddlCentro.DataSource = dat.mysql("call sp_fill(50,'" + ddlTablas.SelectedValue + "',0,0)");
            ddlCentro.DataValueField = "valor";
            ddlCentro.DataTextField = "descrip";
            ddlCentro.DataBind();
           
            Ddl_ClasifAviso.DataSource = dat.mysql("CALL sp_avisos2(6, '" + cliente + "','','','','','','','','')");
            Ddl_ClasifAviso.DataValueField = "VALOR";
            Ddl_ClasifAviso.DataTextField = "VALOR01";
            Ddl_ClasifAviso.DataBind();
        }
 
        //protected void Filtro(string busqueda, string cliente)
        //{
        //    DataTable dt = dat.mysql("call sp_fill(7,'" + busqueda + "','" + cliente + "',0)");
        //    GridView2.DataSource = dt;
        //    GridView2.DataBind();

        //    if (GridView2.Rows.Count != 0)
        //    {
        //        lblContador.Text = "Registros Mostrados: " + dt.Rows.Count.ToString();
        //    }
        //    else
        //    {
        //        lblContador.Text = "Registros Mostrados: 0";
        //    } 
        //}

        protected void Filtro(string busqueda,  string cliente, string usuario)
        {
            DataTable dt = dat.mysql("call sp_fill(7,'" + busqueda + "','" + usuario  + "','" +  cliente + "')");
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
            if (ddlTablas.SelectedValue == "15")
            {
                btnImportar.Visible = true;
            }
            else
            {
                btnImportar.Visible = false;
            }

            if (ddlTablas.SelectedValue == "37")
            {
                lnkTramaSoat.Visible = true;
            }
            else
            {
                lnkTramaSoat.Visible = false;
            }

            CombosCliente(ddlTablas.SelectedValue,"0");
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue) , Session["USUARIO"].ToString());
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue) , Session["USUARIO"].ToString());
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            

            Image1.ImageUrl = "~/image/photo.png";
            lblCodigoTitularCarac.Text = "";
            lblCodigoTitularCorrecto.Text = "";
            lblCodigoTitularNoHay.Text = "";

            if (ddlTablas.SelectedValue == "00")
            {
                lblErrorReg.Text = "ADVERTENCIA ,Seleccione un cliente para poder crear un nuevo registro.";
                return;
            }
            else
            {
                borrarmensajes();
                limpiar();

                //combosNuevoEditar(); myg pending

                RecordConsumoTab.Visible = false;
                AvisosTab.Visible = false;
                CartasTab.Visible = false;
                txtNumeroPoli.Text = Convert.ToString(ddlTablas.SelectedValue);
                txtNombreEmpresa.Text = ddlTablas.SelectedItem.Text.ToString().Substring(3);
                //txtNumeroPoli.ReadOnly = true;
                txtCodigoTitu.Text = "";
                txtCodigoTitu.ReadOnly = false;
                btnGuardarModificar.Visible = false;
                btnBajaModal.Visible = false;
                ddlCategoria.SelectedIndex = 0;
                ddlCategoria.Attributes.Add("readonly", "readonly");
                ddlCategoria.CssClass = "form-control input-sm disabled-button";
                lblAfiliado.Text = "NUEVO TITULAR";
                btnGuardarRegistrar.Visible = true;
                txtAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddlDepartamento.SelectedValue = "15";
                ddlDepartamento_SelectedIndexChanged(sender, e);
                lnkBD.Visible = true;
                //txtDNI.Width = new Unit("70%");

                //=======================================================================

                ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                ddlCategoria.DataValueField = "valor";
                ddlCategoria.DataTextField = "descrip";
                ddlCategoria.DataBind();

                CombosCliente(ddlTablas.SelectedValue,"0");

                id_paciente.Visible = false;
                cod_paciente.Visible = false;
                lnkActualizacionTitu2.Visible = false;

                switch (txtNumeroPoli.Text)
                {
                    case "90":
                    //case "95":
                    case "96":
                    case "98":

                    //lnkBD.Visible = false;
                    //lnkTraer.Visible = true;

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
                        pad.Visible = false;
                        campo2.Visible = false;
                        txtCodigoTitu.ReadOnly = false;
                        lnkBuscarTitular.Enabled = true;
                        break;
                    case "15":
                        //carencia.Visible = true;
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
            Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue) , idUsuario);
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
            int cuentadni = 0;
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            //Titular titu = new Titular();
            //List<Titular> list = new TitularBL().ListarTitulares(53, Convert.ToString(ddlTablas.SelectedValue));

            string validacionDNI = "CALL sp_fill('82','" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + txtDNI.Text + "');";

            DataTable dtficha = dat.mysql(validacionDNI);

            if (dtficha.Rows.Count > 0)
            {
                cuentadni = Convert.ToInt32(dtficha.Rows[0][0]);
            }
           
            //titu = list.Find(delegate(Titular obj) { return (titu.cod_cliente.Equals(ddlTablas.SelectedValue) && (titu.cod_titula.Equals(txtCodigoTitu.Text))); });
            if (cuentadni > 0)
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
            else
            {

                if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96") || (txtNumeroPoli.Text == "95"))
                {
                    if (ddlCategoria.SelectedValue == "00")
                    {
                        if (txtDNI.Text == "" || txtCodigoTitu.Text == "" || txtApellidom.Text == ""
                                                                      || txtApellidop.Text == ""
                                                                      || txtNombres.Text == ""
                                                                      || txtNacimiento.Text == "")
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
                    if (ddlCategoria.SelectedValue != "00" && ddlCategoria.SelectedValue != "04")
                    {
                        if (txtDNI.Text == "" || txtCodigoTitu.Text == "" || txtApellidom.Text == ""
                              || txtApellidop.Text == ""
                              || txtNombres.Text == ""
                              || txtNacimiento.Text == ""
                              || txtCodPaciente.Text == "")
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
                        if (txtDNI.Text == "" || txtCodigoTitu.Text == "" || txtApellidom.Text == ""
                              || txtApellidop.Text == ""
                              || txtNombres.Text == ""
                              || txtNacimiento.Text == ""
                              || txtCodPaciente.Text == ""
                              || txtIdPaciente.Text == "")
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

                if (txtNumeroPoli.Text == "37")
                {
                    txtDNI.Text = "0000";
                    txtCodigoTitu.Text = "0000";
                }

                if (txtDNI.Text == "" || txtCodigoTitu.Text == "" || txtApellidom.Text == "" || txtApellidop.Text == "" || txtNombres.Text == "" || txtNacimiento.Text == "")
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

                    if (txtNumeroPoli.Text == "37")
                    {
                        txtDNI.Text = "";
                        txtCodigoTitu.Text = "";
                    }

                    Titular obj = new Titular();
                    Titular_Detalle obj_det = new Titular_Detalle();

                    obj.cod_cliente = txtNumeroPoli.Text; obj.cod_titula = txtCodigoTitu.Text; obj.categoria = ddlCategoria.SelectedValue; obj.centro_costo = ddlCentro.SelectedValue;
                    obj.plan = Convert.ToInt32(ddlPlan.SelectedValue); obj.afiliado = txtApellidop.Text + " " + txtApellidom.Text + "," + txtNombres.Text; obj.parentesco = ddlCategoria.SelectedItem.Text; obj.sexo = ddlSexo.SelectedValue;
                    obj.fch_naci = txtNacimiento.Text; obj.fch_alta = txtAlta.Text; obj.fch_baja = txtBaja.Text; obj.fch_proc = Convert.ToString(DateTime.Today.ToShortDateString()); obj.pass = txtDNI.Text; obj.email = txtObservar.Text;
                    obj.tipo_doc = Convert.ToInt32(ddlTipoDocumento.SelectedValue); obj.dni = txtDNI.Text; obj.madre = "0"; obj.actividad = "0"; obj.ubicacion = "0"; obj.estado_titular = 1; obj.capitados = "0";
                    obj.financia = "0"; obj.oncologico = ddlOnco.SelectedValue; obj.dx_onco = "0"; obj.campo1 = "0"; obj.campo2 = txtCampo2.Text; obj.campo3 = "0"; obj.fch_caren = txtCarencia.Text;

                    obj_det.cod_cliente = txtNumeroPoli.Text; obj_det.cod_titula = txtCodigoTitu.Text; obj_det.categoria = ddlCategoria.SelectedValue;
                    obj_det.depa_id = ddlDepartamento.SelectedValue; obj_det.prov_id = ddlProvincia.SelectedValue; obj_det.dist_id = ddlDistrito.SelectedValue;
                    obj_det.direccion = txtDireccion.Text; obj_det.email = txtObservar.Text; obj_det.t_fijo = txtTelefono1.Text; obj_det.t_movil = txtTelefono2.Text; obj_det.estado_civil = Convert.ToInt32(ddlEstadoCivil.SelectedValue);
                    obj_det.edad = txtEdad.Text; obj_det.peso = txtPeso.Text; obj_det.estatura = txtEstatura.Text; obj_det.discapacitado = ddlDiscapacit.SelectedValue; obj_det.consume_alcohol = ddlAlcohol.SelectedValue;
                    obj_det.consume_tabaco = ddlDrogas.SelectedValue; obj_det.grupo_sanguineo = txtSangre.Text; obj_det.fch_fincarencia = txtCarencia.Text; obj_det.pad = ddlPad.SelectedValue; obj_det.dpto = txtDpto.Text;
                    obj_det.rol = txtRol.Text; obj_det.prog_especial = ddlPespecial.SelectedValue; obj_det.cod_paciente = txtCodPaciente.Text; obj_det.id_paciente = txtIdPaciente.Text; obj_det.basico = ddlBasico.SelectedValue; obj_det.onco = ddlOnco.SelectedValue;
                    obj_det.segunda_capa = ddlCapa.SelectedValue; obj_det.docum = txtDocumento.Text; obj_det.afi_nombre = txtNombres.Text; obj_det.afi_apepat = txtApellidop.Text; obj_det.afi_apemat = txtApellidom.Text; 
                    //update260517
                    obj_det.correo1 = txtCorreo1.Text; obj_det.correo2 = txtCorreo2.Text;

                    string fotoregistro = "";
                    string categoriaReal = "";

                    if (Page.IsPostBack)
                    {
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
                                fotoregistro = "90" + "-" + txtCodigoTitu.Text + "-" + categoriaReal + extensionFoto;
                                subeFotos("90", txtCodigoTitu.Text, categoriaReal);
                            }
                            else
                            {
                                fotoregistro = txtNumeroPoli.Text + "-" + txtCodigoTitu.Text + "-" + categoriaReal + extensionFoto;
                                subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaReal);
                            }                                                        
                        }
                        else
                        {
                            fotoregistro = "";                            
                        }
                    }

                    obj_det.foto = fotoregistro;

                    hfNombres.Value = txtNombres.Text;
                    hfApellidoPaterno.Value = txtApellidop.Text;
                    hfApellidoMaterno.Value = txtApellidom.Text;

                    string resultado = new TitularBL().InsertarTitular(obj, obj_det, usu, 1);
                    //subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, ddlCategoria.SelectedValue);

                    if (ddlCategoria.SelectedValue == "01")
                    {
                        if ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "98") || (txtNumeroPoli.Text == "96"))
                        {
                            string xSQL = "call sp_fill(74,'90','" + txtCodigoTitu.Text + "',0);";
                            DataTable dt = dat.mysql(xSQL);
                            if (dt.Rows[0]["foto"].ToString() != "0")
                            {
                                //string elimino = DeleteFile(dt.Rows[0]["foto"].ToString(), "90");
                                string archivo = "90-" + txtCodigoTitu.Text + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4);
                                string elimino = DeleteFile("90-" + txtCodigoTitu.Text + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4), "90");
                                //90-000000-00.jpg
                            }                            
                        }
                        else
                        {
                            string xSQL = "call sp_fill(74,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "',0);";
                            DataTable dt = dat.mysql(xSQL);
                            if (dt.Rows[0]["foto"].ToString() != "0")
                            {
                                //string elimino = DeleteFile(dt.Rows[0]["foto"].ToString(), txtNumeroPoli.Text);
                                string archivo = txtNumeroPoli.Text + "-" + txtCodigoTitu.Text + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4);
                                string elimino = DeleteFile(txtNumeroPoli.Text + "-" + txtCodigoTitu.Text + "-01" + dt.Rows[0]["foto"].ToString().Substring(dt.Rows[0]["foto"].ToString().Length - 4, 4), txtNumeroPoli.Text);                                
                            }
                        } 
                    }

                    if (resultado == "1")
                    {
                        lblverificacion.Text = "Titular registrado satisfactoriamente";
                        limpiar();
                        Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                        //Cargarcombos(txtNumeroPoli.Text);

                        if (txtNumeroPoli.Text == "37")
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
                        Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#NUEVOAFILIADO').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    }

                    //Cargarcombos(txtNumeroPoli.Text);
                }
            }

        }

        //protected void listardependientes(string cod_titula, string cliente)
        //{

        //    GridView1EdicionTitular.DataSource = new TitularBL().ListarGrupoFamiliar(777, cliente, cod_titula);
        //    GridView1EdicionTitular.DataBind();
        //    lblContadorEdicionTitular.Text = "El titular seleccionado cuenta con " + Convert.ToString(GridView1EdicionTitular.Rows.Count) + " dependientes.";

        //}

        protected void grupofamiliar(string cod_titula, string cliente)
        {
            gvGrupoFamiliar.DataSource = new TitularBL().ListarGrupoFamiliar(777, cliente, cod_titula);
            gvGrupoFamiliar.DataBind();
        }

        protected void btnGuardarModificar_Click(object sender, EventArgs e)
        {
            extensionFoto = "";
            borrarmensajes();

            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            List<Titular> OLD_LIST = new TitularBL().ListarTitularesGrupo(53, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular OLD_objTitu = OLD_LIST.First(delegate(Titular objTI) { return (objTI.cod_cliente.Equals(txtNumeroPoli.Text) && (objTI.cod_titula.Equals(txtCodigoTitu.Text) && (objTI.categoria.Equals(categoriaHidden.Value)))); });

            List<Titular_Detalle> OLD_LIST2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular_Detalle OLD_objTitudeta = OLD_LIST2.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });


            Titular obj = new Titular();
            Titular_Detalle obj_det = new Titular_Detalle();

            obj.cod_cliente = Convert.ToString(txtNumeroPoli.Text); obj.cod_titula = txtCodigoTitu.Text;
            obj.categoria = categoriaHidden.Value; obj.centro_costo = ddlCentro.SelectedValue;
            obj.plan = Convert.ToInt32(ddlPlan.SelectedValue); obj.afiliado = txtApellidop.Text + " " + txtApellidom.Text + "," + txtNombres.Text;
            obj.parentesco = ddlCategoria.SelectedItem.Text; obj.sexo = ddlSexo.SelectedValue;
            obj.fch_naci = txtNacimiento.Text; obj.fch_alta = txtAlta.Text; obj.fch_baja = txtBaja.Text;
            obj.fch_proc = Convert.ToString(DateTime.Today); obj.pass = txtContraseña.Text; obj.email = txtObservar.Text;
            obj.tipo_doc = Convert.ToInt32(ddlTipoDocumento.SelectedValue); obj.dni = txtDNI.Text; obj.madre = "0"; obj.actividad = "0"; obj.ubicacion = "0";
            obj.estado_titular = 1; obj.capitados = "0";
            obj.financia = "0"; obj.oncologico = ddlOnco.SelectedValue; obj.dx_onco = "0"; obj.campo1 = "0"; obj.campo2 = txtCampo2.Text; obj.campo3 = "0"; obj.fch_caren = txtCarencia.Text;

            obj_det.cod_cliente = Convert.ToString(txtNumeroPoli.Text); obj_det.cod_titula = txtCodigoTitu.Text; obj_det.categoria = categoriaHidden.Value;
            obj_det.depa_id = ddlDepartamento.SelectedValue; obj_det.prov_id = ddlProvincia.SelectedValue; obj_det.dist_id = ddlDistrito.SelectedValue;
            obj_det.direccion = txtDireccion.Text; obj_det.email = txtObservar.Text; obj_det.t_fijo = txtTelefono1.Text;
            obj_det.t_movil = txtTelefono2.Text; obj_det.estado_civil = Convert.ToInt32(ddlEstadoCivil.SelectedValue);
            obj_det.edad = txtEdad.Text; obj_det.peso = txtPeso.Text; obj_det.estatura = txtEstatura.Text; obj_det.discapacitado = ddlDiscapacit.SelectedValue;
            obj_det.consume_alcohol = ddlAlcohol.SelectedValue; obj_det.consume_tabaco = ddlDrogas.SelectedValue; obj_det.grupo_sanguineo = txtSangre.Text;
            obj_det.fch_fincarencia = txtCarencia.Text; obj_det.pad = ddlPad.SelectedValue; obj_det.dpto = txtDpto.Text; obj_det.rol = txtRol.Text;
            obj_det.prog_especial = ddlPespecial.SelectedValue; obj_det.cod_paciente = txtCodPaciente.Text; obj_det.id_paciente = txtIdPaciente.Text; obj_det.basico = ddlBasico.SelectedValue;
            obj_det.onco = ddlOnco.SelectedValue; obj_det.segunda_capa = ddlCapa.SelectedValue; obj_det.docum = txtDocumento.Text; obj_det.afi_nombre = txtNombres.Text;
            obj_det.afi_apepat = txtApellidop.Text; obj_det.afi_apemat = txtApellidom.Text; 
            //update 260517
            obj_det.correo1 = txtCorreo1.Text; obj_det.correo2 = txtCorreo2.Text;

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
                            subeFotos("90", txtCodigoTitu.Text, categoriaHidden.Value);
                        }
                        else
                        {
                            fotoActualizacion = txtNumeroPoli.Text + "-" + txtCodigoTitu.Text + "-" + categoriaHidden.Value + extensionFoto;
                            subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
                        }
                    }
                    else
                    {
                        fotoActualizacion = OLD_objTitudeta.foto;
                        extensionFoto = fotoActualizacion.Substring(fotoActualizacion.Length - 4, 4);
                    }                                       
                }
                else
                {
                    if (OLD_objTitudeta.foto != "")
                    {
                        fotoActualizacion = OLD_objTitudeta.foto;
                    }
                    else
                    {
                        fotoActualizacion = "";
                    }                    
                }
            }

            obj_det.foto = fotoActualizacion;


            int resultado = new TitularBL().ActualizarTitular(obj, obj_det, usu, 1, OLD_objTitu, OLD_objTitudeta);

            if (resultado == 1)
            {

                if (OLD_objTitu.plan == 1 && obj.plan == 2 && (obj.cod_cliente == "90" || obj.cod_cliente == "98" || obj.cod_cliente == "96" || obj.cod_cliente == "95"))
                {
                    string xSQL = "call sp_avisos (2,'" + obj.cod_cliente.ToString() + "','" + obj.cod_titula.ToString() + "','" + categoriaHidden.Value + "','(CAMBIO DE PLAN 1 A  PLAN 2) EL AFILIADO CAMBIARÁ DE PLAN EL DÍA  " + DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy") + " ','" + usu.ID.ToString() + "','')";
                    DataTable dt = dat.mysql(xSQL);
                }

                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                lblCorrecto.Text = "Afiliado modificado satisfactoriamente";
                limpiar();
                gAnio.Visible = false;
                gMeses.Visible = false;
                lblFinal.Text = "";
                lblEvoDet.Visible = false;
                btnCerrar_Click(sender, e);
            }
            else
            {
                lblError.Text = "Error de Modificación, refresque la página y vuelva a intentarlo, si el error persiste comunicarse con el Administrador del Sistema.";
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }

        }

        protected void btnDependienteNuevo_Click(object sender, EventArgs e)
        {
            borrarmensajes();
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
            ddlCategoria.CssClass = "form-control input-sm";
            btnBajaModal.Visible = false;
            btnActivarAfiliado.Visible = false;
            btnGuardarModificar.Visible = false;
            btnGuardarRegistrar.Visible = true;
            txtAlta.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlDepartamento.SelectedValue = "15";
            ddlDepartamento_SelectedIndexChanged(sender, e);
            lnkBD.Visible = true;
            id_paciente.Visible = false;
            cod_paciente.Visible = false;
            lnkActualizacionTitu2.Visible = false;

            CombosCliente(gvGrupoFamiliar.Rows[0].Cells[1].Text,"0");

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
                    campo2.Visible = false;
                    List<Titular> list = new TitularBL().ListarTitularesGrupo(53, Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text), gvGrupoFamiliar.Rows[0].Cells[3].Text, "00");
                    Titular objTitu = list.First(delegate(Titular obj) { 
                                return (obj.cod_cliente.Equals(Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text)) 
                                        && (obj.cod_titula.Equals(gvGrupoFamiliar.Rows[0].Cells[3].Text) 
                                        && (obj.categoria.Equals("00")))); });


                    List<Titular_Detalle> list2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text), gvGrupoFamiliar.Rows[0].Cells[3].Text, "00");
                    Titular_Detalle objTitudeta = list2.First(delegate(Titular_Detalle objt){
                                return (objt.cod_cliente.Equals(Convert.ToString(gvGrupoFamiliar.Rows[0].Cells[1].Text))
                                        && (objt.cod_titula.Equals(gvGrupoFamiliar.Rows[0].Cells[3].Text) 
                                        && (objt.categoria.Equals("00")))); });

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
                    campo2.Visible = false;
                    break;
                case "26":
                    id_paciente.Visible = false;
                    cod_paciente.Visible = false;
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
            if (lblAfiliado.Text.Equals("NUEVO TITULAR"))
            {
                ////Filtro(txtBusqueda.Text, Convert.ToString(txtNumeroPoli.Text));
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text, idUsuario);

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
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            List<Titular> listbaja = new TitularBL().ListarTitularesGrupo(53, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular objTitu_baja = listbaja.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(txtNumeroPoli.Text) && (obj.cod_titula.Equals(txtCodigoTitu.Text) && (obj.categoria.Equals(categoriaHidden.Value)))); });

            List<Titular_Detalle> listbaja2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular_Detalle objTitudeta_baja = listbaja2.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });

            if ((categoriaHidden.Value == "00") && ((txtNumeroPoli.Text == "90") || (txtNumeroPoli.Text == "95") || (txtNumeroPoli.Text == "96") || (txtNumeroPoli.Text == "98")))
            {
                switch (txtNumeroPoli.Text)
                {
                    case "90":
                    case "95":
                    case "96":
                    case "98":

                        foreach (ListItem listItem in CheckBoxList1.Items)
                        {
                            if (listItem.Selected)
                            {

                                List<Titular> listbajapetro = new TitularBL().ListarTitularesGrupo(53, CheckBoxList1.SelectedValue, txtCodigoTitu.Text, categoriaHidden.Value);
                                Titular objTitu_bajapetro = listbajapetro.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(CheckBoxList1.SelectedValue) && (obj.cod_titula.Equals(txtCodigoTitu.Text) && (obj.categoria.Equals(categoriaHidden.Value)))); });

                                List<Titular_Detalle> listbaja2petro = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, CheckBoxList1.SelectedValue, txtCodigoTitu.Text, categoriaHidden.Value);
                                Titular_Detalle objTitudeta_bajapetro = listbaja2petro.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(CheckBoxList1.SelectedValue) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });

                                resultado = new TitularBL().BAJACOMPLETA(objTitu_bajapetro, objTitudeta_bajapetro, usu, TextBox1.Text, "3");
                                contador = contador + 1;
                            }

                        }
                        break;
                }
            }
            else
            {
                if (TextBox1.Text == "")
                {
                    resultado = 0;
                    contador = -1;
                }
                else
                {
                    resultado = new TitularBL().BAJACOMPLETA(objTitu_baja, objTitudeta_baja, usu, TextBox1.Text, "3");
                    contador = -1;
                }
            }


            if (resultado == 1 || contador >= 1)
            {
                lblCorrecto.Text = "Afiliado dado de Baja";
                lblError.Text = "";
                limpiar();
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }
            if (resultado == 0)
            {
                lblError.Text = "Ocurrió un error, no se dió de baja al afiliado, verifique si ingreso una fecha";
                lblCorrecto.Text = "";
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }

            if (contador  == 0)
            {
                lblError.Text = "No seleccionó ningún cliente o no colocó una fecha, inténtelo nuevamente seleccionando uno o más clientes.";
                lblCorrecto.Text = "";
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }
        }

        protected void btnBajaModal_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";

            CheckBoxList1.Visible = true;

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
                        CheckBoxList1.DataSource = dt;
                        CheckBoxList1.DataTextField = "cliente";
                        CheckBoxList1.DataValueField = "codigo";
                        CheckBoxList1.DataBind();

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
                //SSQLX = "call sp_avisos (1,'" + cliente + "','" + titula + "','" + categoria + "',0,0,0)";
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
            //txtDescripcion.Visible = true;
            //btnGrabando.Visible = true;
            //btnCancelarAviso.Visible = true;
            //btnNuevoAviso.Visible = false;
        }      

        protected void btnReportes_Click(object sender, EventArgs e)
        {

            string SSQL = "call sp_fill(79,'" + Convert.ToInt32(Session["USUARIO"].ToString()) + "','78','0')";
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


            //usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            //string usuario = Convert.ToString(usu.USER);
            //string pass = Convert.ToString(usu.PASS);
            ////Response.Redirect("http://190.102.136.157/AppGer/inicio.aspx?usu=" + usuario + "&pass=" + pass + "");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://190.102.136.157/AppGer/inicio.aspx?usu=" + usuario + "&pass=" + pass + "&op=78');", true);
        }

        protected void btnActivarAfiliado_Click(object sender, EventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

            List<Titular> listbaja = new TitularBL().ListarTitularesGrupo(53, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular objTitu_baja = listbaja.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(txtNumeroPoli.Text) && (obj.cod_titula.Equals(txtCodigoTitu.Text) && (obj.categoria.Equals(categoriaHidden.Value)))); });


            List<Titular_Detalle> listbaja2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
            Titular_Detalle objTitudeta_baja = listbaja2.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(txtNumeroPoli.Text) && (objt.cod_titula.Equals(txtCodigoTitu.Text) && (objt.categoria.Equals(categoriaHidden.Value)))); });

            int resultado = new TitularBL().ACTIVAR(objTitu_baja, objTitudeta_baja, usu, TextBox1.Text, "5");


            if (resultado == 1)
            {
                lblCorrecto.Text = "Afiliado activado";
                limpiar();
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }
            else
            {
                lblError.Text = "Error de activación";
                Filtro(txtBusqueda.Text, txtNumeroPoli.Text , idUsuario);
                grupofamiliar(txtCodigoTitu.Text, txtNumeroPoli.Text);
                btnCerrar_Click(sender, e);
            }
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            //Session.Abandon();
            Session.Clear();
            Response.Redirect("Sesion.aspx?usu=&pass=");
        }

        protected void lnkAlertas_Click(object sender, EventArgs e)
        {
            //usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            //Response.Redirect("http://app.laprotectora.com.pe/AppGer/inicio.aspx?usu=" + usu.USER.ToString() + "&pass=" + usu.PASS.ToString() + "&op=78");
            string SSQL = "call sp_fill(79,'" + Convert.ToInt32(Session["USUARIO"].ToString()) + "','78','0')";
            DataTable dt = dat.mysql(SSQL);

            if (dt.Rows.Count > 0)
            {
                //--Response.Redirect(dt.Rows[0]["Ruta"].ToString());
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
            movimientos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value, ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(),usu.ID.ToString());
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
                    dt2 =  Util. GenerarDataTable(rGuardar + flpImp.FileName);
                }

                //'Dim xLoadTAb As String = "LOAD DATA LOCAL INFILE '" & Session("xdFILE") & "' " &
                //'                                    "INTO TABLE solben_bd.T_FINANCIA " &
                //'                                    "FIELDS TERMINATED BY ',' " &
                //'                                    "LINES TERMINATED BY '\n'; "

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
            //cartas(txtNumeroPoli.Text, txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text);
        }

        protected void FullPostBack(object sender, EventArgs e)
        {

            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string solben_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["solben_id"].ToString();
            string atencion_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["atencion_id"].ToString();
            string cod_cliente = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["cod_cliente"].ToString();
            string js = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/loginTitus.php?u=" + Session["USUARIO_USU"] + "&p=" + Session["PASS"] + "&p2=2&t=" + atencion_id + "&n=" + solben_id + "');", true);

            //switch (atencion_id)
            //{
            //    case "1":
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/impreso_m.php?nro=" + solben_id + "');", true);
            //        //js = "window.open('http://www.solben.net/solben/impreso_m.php?nro=" + solben_id + "', '_blank');";
            //        //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        break;
            //    case "2":
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/impreso_d.php?nro=" + solben_id + "');", true);
            //        //js = "window.open('http://www.solben.net/solben/impreso_d.php?nro='" + solben_id + "'', '_blank');";
            //        //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        break;
            //    case "4":
            //    case "6":
            //        if ((cod_cliente == "90") || (cod_cliente == "96") || (cod_cliente == "98"))
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro=" + solben_id + "');", true);
            //            //js = "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro='" + solben_id + "'', '_blank');";
            //            //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion.php?nro=" + solben_id + "');", true);
            //            //js = "window.open('http://www.solben.net/solben/garantias/orden_atencion.php?nro='" + solben_id + "'', '_blank');";
            //            //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        }
            //        break;
            //    case "5":
            //    case "3":
            //        if ((cod_cliente == "90") || (cod_cliente == "96") || (cod_cliente == "98") || (cod_cliente == "95"))
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro=" + solben_id + "');", true);
            //            //js = "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro='" + solben_id + "'', '_blank');";
            //            //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/carta_garantia.php?nro=" + solben_id + "');", true);
            //            //js = "window.open('http://www.solben.net/solben/garantias/carta_garantia.php?nro='" + solben_id + "'', '_blank');";
            //            //ClientScript.RegisterStartupScript(this.GetType(), "Open Signature.aspx", js, true);
            //        }
            //        break;
            //    default:
            //        break;
            //}

            ////<a href='listado_ampliaciones.php?nro=".$r2['solben_id']."' target='_blank'>
            ////string fruitName = ((sender as LinkButton).NamingContainer as GridViewRow).Cells[0].Text;
            ////string message = "alert('Full PostBack: You clicked " + fruitName + "');";
            ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", message, true);
        }

        //protected void LinkAlertas_Load(object sender, EventArgs e)
        //{
        //    LinkAlertas.NavigateUrl = "http://190.102.136.157/AppGer/inicio.aspx?usu=" + usu.USER + "&pass=" + usu.PASS + "&op=78";
        //}

        void subeFotos(string cli,string titu,string cate) 
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
            //if (txtCodigoTitu.Text != "")
            //{
            //    subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, ddlCategoria.SelectedValue);

            //    string SSQLX = "call sp_fill (72,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + ddlCategoria.SelectedValue + "')";
            //    DataTable dt = dat.mysql(SSQLX);

            //    if (dt.Rows.Count > 0)
            //    {
            //        Image1.ImageUrl = dt.Rows[0][0].ToString();
            //    }

            //}
            //else
            //{
            //    lblError.Text = "Debe ingresar primero un Codigo de Titular.";
            //}


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
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('FichaPersonal.aspx?cc=" + txtNumeroPoli.Text + "&ct=" + txtCodigoTitu.Text + "&c=" + categoriaHidden.Value + "');", true);
            return;
            //Response.Redirect("FichaPersonal.aspx?cc=" + txtNumeroPoli.Text + "&ct=" + txtCodigoTitu.Text + "&c=" + comparacion + "");
        }

        //protected void btnGrabando_Click(object sender, EventArgs e)
        //{
        //    try 
        //        {
        //            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
        //        string xSQL = "call sp_avisos (2,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + txtDescripcion.Text + "','" + usu.ID + "','')";
        //        DataTable dt = dat.mysql(xSQL);

        //        lblAvisoDesc.Visible = false;
        //        txtDescripcion.Visible = false;
        //        btnGrabando.Visible = false;
        //        btnCancelarAviso.Visible = false;
        //        btnNuevoAviso.Visible = true;

        //        avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

        //        lblAvisos.Text = "Aviso Guardado";

        //        }
        //        catch (Exception ex)
        //        {
		
        //           lblAvisos.Text = ex.Message.ToString();
        //        }
        //}

        //protected void btnEditar_Click(object sender, EventArgs e)
        //{
        //    string SSQL1 = "call sp_avisos (3,'" + idAviso.Value + "','"+ txtDescripcion.Text +"','','','','')";
        //    DataTable dt1 = dat.mysql(SSQL1);

        //    avisos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);

        //}

        protected void lnkBajaTitus_Click(object sender, EventArgs e)
        {
            if (ddlTablas.SelectedValue.ToString() == "57")
            {
                //exportar57();
                exportardbf();

            }
            else
            {
                exportardbf();

            }                      
        }

        void exportar57()
        {
            string xSQL = "CALL SP_ESTADO_TITUS('57','');";
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

                wst.Cells["A4:K4"].Style.Font.Bold = true;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#DCDCDC");
                wst.Cells["A4:K4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A4:K4"].Style.Fill.BackgroundColor.SetColor(colorCode);

                wst.Cells["A4"].LoadFromDataTable(dtabexport, true);

                wst.Cells["A4"].Value = "COD_CLIENTE";
                wst.Cells["B4"].Value = "COD_TITULA";
                wst.Cells["C4"].Value = "CATEGORIA";
                wst.Cells["D4"].Value = "AFILIADO";
                wst.Cells["E4"].Value = "DNI";
                wst.Cells["F4"].Value = "FECHA_NACIMIENTO";
                wst.Cells["G4"].Value = "FECHA_ALTA";
                wst.Cells["H4"].Value = "FECHA_BAJA";
                wst.Cells["I4"].Value = "BASICO";
                wst.Cells["J4"].Value = "ONCO";
                wst.Cells["K4"].Value = "ESTADO_TITULAR";


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
            string xSQL = "call sp_fill (60,'"+ Convert.ToInt32(ddlCategoria.SelectedValue) +"',0,0)";
            DataTable dt = dat.mysql(xSQL);
            txtCodPaciente.Text = dt.Rows[0][0].ToString();

            if (ddlCategoria.SelectedValue == "04")
            {
                id_paciente.Visible = true;
                txtIdPaciente.ReadOnly = false;

            }
            else
            {
                id_paciente.Visible = false;
                txtIdPaciente.Text = "";
                txtIdPaciente.ReadOnly = true;
       
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

        void descargaRecord(DataTable dt1,DataTable dt2,DataTable dt3,string titulo,string asegurado)
        { 
            
        string serie = DateTime.Now.Day.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Year.ToString();
        Response.Clear();
        Response.Charset = "";
        Response.ContentEncoding = Encoding.UTF8;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Color colorCode;

        using(var pck = new  OfficeOpenXml.ExcelPackage())
	    {
		    ExcelWorksheet wst  = pck.Workbook.Worksheets.Add("REPORTE");

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
                lblNoHayRecord.Text = "Ocurrió un error al cargar record de consumo "+ex.Message.ToString();
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
        void loadIC(string cliente, string titula,string categoria)
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
            string SSQL = "CALL SP_REPORTES_IAFA (70,'" + cliente + "','"+ titula +"','"+ categoria +"','"+ anio +"','"+ ddlTipoOrdenAtencion.SelectedValue.ToString() +"','','')";
            DataTable dt = dat.mysql(SSQL);
            gvImpresionCartasMes.DataSource = dt;
            gvImpresionCartasMes.DataBind();

            ChartMesesIC();
        }

        void cargarICMes(string cliente, string anio, string mes,string titula, string categoria)
        {
            icdetalle.Visible = true;
            string SSQL = "CALL SP_REPORTES_IAFA (71,'" + cliente + "','"+ titula +"','"+ categoria +"','"+ anio +"','" + mes + "','"+ ddlTipoOrdenAtencion.SelectedValue.ToString() +"','')";
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
                cargarICAnio(txtNumeroPoli.Text, ViewState["ANIO"].ToString(),txtCodigoTitu.Text,categoriaHidden.Value);
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
            cargarICMes(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), ViewState["COD_MES"].ToString(), txtCodigoTitu.Text,categoriaHidden.Value);
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
            catch (Exception ex )
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
        
        void movimientos(string cliente, string titula,string categoria,string mes,string anio,string usu)
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
            //txtDescripcion.Visible = false;
            //btnGrabando.Visible = false;
            //btnCancelarAviso.Visible = false;
            //btnNuevoAviso.Visible = true;
        }

        protected void lnkImpreOrden_Click(object sender, EventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/loginIII.php?u="+ usu.USER.ToString() +"&p="+ usu.PASS.ToString() +"&d=3&p2="+ txtNumeroPoli.Text + txtCodigoTitu.Text + categoriaHidden.Value +"');", true);
            
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

                //'Dim xLoadTAb As String = "LOAD DATA LOCAL INFILE '" & Session("xdFILE") & "' " &
                //'                                    "INTO TABLE solben_bd.T_FINANCIA " &
                //'                                    "FIELDS TERMINATED BY ',' " &
                //'                                    "LINES TERMINATED BY '\n'; "

                foreach (DataRow row in dt2.Rows)
                {
                    try
                    {
                        string xLoadTAb = "INSERT INTO PRIM_CARG_II(iafas,cod_titula,fecha,periodo,tipoPC,tipoIP,monto,consumo,pago,nuevoSaldo,fecha_reg,usuario)" +
                                          "VALUES ('" + row["valor1"].ToString() + "','" + row["valor2"] + "','" + row["valor3"] + "','" + row["valor4"] + "','" + row["valor5"] + "','','" + row["valor6"] + "','" + row["valor7"] + "','" + row["valor8"] + "','" + row["valor9"] + "','" + DateTime.Now + "','" + row["valor10"] + "')";
                        dat.mysql(xLoadTAb);


                        //VALUES('".trim($datos[0])."','".trim($datos[1])."','".trim($datos[2])."','".trim($datos[3])."','".trim($datos[4])."','','".$datos[5]."','".$datos[6]."','".$datos[7]."','".$datos[8]."',now(),'".$usuario['id']."')
                        //INSERT INTO ACTUALIZACIONES(valor1,valor2,valor3,valor4,valor5,valor6,valor7,valor8,valor9,valor10,valor11,valor12) 
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
            Titular objTitu = list.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(clientegrilla) && (obj.cod_titula.Equals(codigogrilla) && (obj.categoria.Equals(categoriagrilla)))); });


            List<Titular_Detalle> list2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, clientegrilla, codigogrilla, categoriagrilla);
            Titular_Detalle objTitudeta = list2.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(clientegrilla) && (objt.cod_titula.Equals(codigogrilla) && (objt.categoria.Equals(categoriagrilla)))); });


            List<TituList> list3 = new TitularBL().Busqueda(77, clientegrilla, codigogrilla, categoriagrilla);
            TituList objTitulist = list3.First(delegate(TituList objtl) { return (objtl.cod_cliente.Equals(clientegrilla)) && (objtl.cod_titula.Equals(codigogrilla) && (objtl.categoria.Equals(categoriagrilla))); });

            try
            {
                if (objTitu != null || objTitudeta != null)
                {
                    //---CUANDO ES TITULAR---
                    if (categoriagrilla == "00")
                    {
                        //listardependientes(codigogrilla, clientegrilla);
                        lblAfiliado.Text = "EDITAR TITULAR";
                        ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                        ddlCategoria.DataValueField = "valor";
                        ddlCategoria.DataTextField = "descrip";
                        ddlCategoria.DataBind();
                    }
                    else
                    {
                        lblAfiliado.Text = "EDITAR DEPENDIENTE";
                    }

                    btnGuardarModificar.Visible = true;
                    btnGuardarRegistrar.Visible = false;
                    btnBajaModal.Visible = true;
                    txtNumeroPoli.Attributes.Add("readonly", "readonly");
                    txtNombreEmpresa.Attributes.Add("readonly", "readonly");
                    txtCodigoTitu.ReadOnly = true;
                    ddlCategoria.Attributes.Add("readonly", "readonly");
                    ddlCategoria.CssClass = "form-control input-sm disabled-button";
                    cod_paciente.Visible = false;
                    id_paciente.Visible = false;
                    pad.Visible = false;
                    //---CUANDO ESTA ACTIVO O DESACTIVADO---

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
                                  + " || ESTADO: ACTIVO";
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
                                        + " || ESTADO: BAJA";
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
                                btnActivarAfiliado.Attributes.Remove("disabled");
                                btnActivarAfiliado.Visible = true;
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

                    //string path_ruta = "http://www.solben.net/solben/foto/" + clientegrilla + "/" + clientegrilla + "-" + codigogrilla + "-" + categoriagrilla + ".jpg";

                    //if (RemoteFileExists(path_ruta) == true)
                    //{
                    //    Image1.ImageUrl = "http://www.solben.net/solben/foto/" + clientegrilla + "/" + clientegrilla + "-" + codigogrilla + "-" + categoriagrilla + ".jpg";
                    //}
                    //else
                    //{
                    //    Image1.ImageUrl = "~/image/photo.png";
                    //}

                    if (clientegrilla == "15")
                    {
                        campo2.Visible = true;
                    }

                    //if ((clientegrilla == "55") || (clientegrilla == "56"))
                    if ((clientegrilla == "57"))
                    {
                        if ((objTitudeta.segunda_capa == "N") && (objTitudeta.basico == "N") && (objTitudeta.onco == "N") && (objTitu.estado_titular == 0) && (objTitu.fch_baja == ""))
                        {
                            btnGuardarModificar.Attributes.Remove("disabled");
                        }

                        ocultos2.Visible = true;
                        segundacapa.Visible = true;
                        basico.Visible = true;
                        onco.Visible = true;
                        documentoSIMA.Visible = false;
                        id_paciente.Visible = false;
                        cod_paciente.Visible = false;
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
                            //pad.Visible = true;
                        }
                        else if (Convert.ToInt32(categoriagrilla) >= 04 && Convert.ToInt32(categoriagrilla) <= 20)
                        {
                            cod_paciente.Visible = true;
                            id_paciente.Visible = true;
                            //pad.Visible = true;
                        }else{
                            cod_paciente.Visible = true;
                           // id_paciente.Visible = true;
                            //pad.Visible = true;
                        }
                        dpto.Visible = true;
                        rol.Visible = true;
                        ocultos2.Visible = true;
                        pad.Visible = true;
                        programa.Visible = true;

                        //ddlCategoria_SelectedIndexChanged(sender, e);

                    }

                    CombosCliente(clientegrilla, objTitu.plan.ToString());
                    cartas123.Visible = false;
                    record123.Visible = false;
                    informes123.Visible = false;
                    NuevoAviso.Visible = false;
                    BuscaAviso.Visible = true;
                    //loadDetail(clientegrilla, objTitudeta.afi_apepat + " " + objTitudeta.afi_apemat + ", " + objTitudeta.afi_nombre);
                    //loadIC(clientegrilla, objTitu.cod_titula, Convert.ToString(objTitu.categoria));
                    //loadIM(clientegrilla, objTitu.cod_titula, Convert.ToString(objTitu.categoria));
                    avisos(clientegrilla, objTitu.cod_titula, Convert.ToString(objTitu.categoria));
                    movimientos(clientegrilla, objTitu.cod_titula.ToString(), Convert.ToString(objTitu.categoria), ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(), usu.ID.ToString());

                    txtNumeroPoli.Text = Convert.ToString(objTitu.cod_cliente);
                    txtNombreEmpresa.Text = hfNombreEmpresa.Value.ToString().Substring(3);
                    txtCodigoTitu.Text = objTitu.cod_titula;
                    //ddlCategoria.SelectedValue = Convert.ToString(objTitu.categoria);
                    categoriaHidden.Value = categoriagrilla;
                    ddlCategoria.SelectedValue = comparacion;
                    ddlCentro.SelectedValue = Convert.ToString(objTitu.centro_costo);
                    ddlPlan.SelectedValue = Convert.ToString(objTitu.plan);
                    ddlSexo.SelectedValue = Convert.ToString(objTitu.sexo);
                    txtNacimiento.Text = Convert.ToString(objTitu.fch_naci);
                    txtAlta.Text = Convert.ToString(objTitu.fch_alta);
                    txtBaja.Text = Convert.ToString(objTitu.fch_baja);
                    txtDNI.Text = Convert.ToString(objTitu.dni);
                    if (Page.IsPostBack)
                    {
                        //txtContraseña.Text = Convert.ToString(objTitu.pass);
                        txtContraseña.Attributes.Add("Value", Convert.ToString(objTitu.pass));
                        //txtContraseña.Attributes.Add("Text", Convert.ToString(objTitu.pass));

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

                    string SSQLX1 = "CALL SP_UpdateTitu(6,'" + objTitu.cod_cliente + "','" + objTitu.cod_titula + "','"+ objTitu.categoria +"','','','','','','','','','','','','')";
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

        private string DeleteFile(string fileName,string cliente)
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
                    //combosNuevoEditar(); myg pending

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
                    Titular titu = lista_Titu.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(cliente) && (obj.cod_titula.Equals(cod_titula) && (obj.categoria.Equals("00")))); });

                    if ((titu.categoria == "00") && (titu.estado_titular == 0))
                    {
                        btnDependienteNuevo.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        btnDependienteNuevo.Attributes.Remove("disabled");
                    }
                    
                    usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
                    if (usu.ROL == "50")
                    {
                        btnDependienteNuevo.Attributes.Add("disabled", "disabled");
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#GRUPOFAMILIAR').modal('show');");
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
                    Ddl_ClasifAviso.Attributes.Add("ReadOnly","ReadOnly");
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
                Filtro(txtBusqueda.Text, Convert.ToString(ddlTablas.SelectedValue) , Session["USUARIO"].ToString());
            }

        #endregion

            protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
            {
                //if ((txtNumeroPoli.Text == "55") || (txtNumeroPoli.Text == "56"))
                if ((txtNumeroPoli.Text == "57") )
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
                    SSQLX = "CALL SP_UpdateTitu(5,'" + id + "','" + UpdateTipoDocu + "','" + UpdateNroDocu + "','" + UpdateEMail + "','" + UpdateTelFijo + "','" + UpdateTelMovil + "','"+ Session["USUARIO"] +"','','','','','','','','')";
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
                SSQLX = "CALL sp_avisos2(7, '" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + txtAvisoDescrip.Text.ToString() + "','" + Session["USUARIO"] + "','" + Ddl_ClasifAviso.SelectedValue.ToString() + "','" + txtdesde.Text + "','" + txthasta.Text + "','"+ limite +"')";
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

                string SSQL1 = "call sp_avisos (3,'" + idAviso.Value + "','" + txtAvisoDescrip.Text + "','"+ Session["USUARIO"].ToString() +"','"+ txtdesde.Text +"','"+ txthasta.Text +"','"+ limite +"')";
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
                    datospersona = "call sp_fill_2(16,'" + gvGrupoFamiliar.Rows[0].Cells[3].Text +"','','')";
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
                                                                  dt55.Rows[0]["afi_nombre"].ToString() +"','" +
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
                                                                  txtPersonaLlamaTelefono.Text + "','','1','"+ montotext +"|"+ montodecimal +"','"+ ddlDiagnostico.SelectedItem.Text +"')";

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
                                                                  dt55.Rows[0]["afi_apemat"].ToString() + "','"+ ddlCobertura.SelectedValue +"')";

                        DataTable dt4 = dat.mysql(SSQL4);

                        //CREACION DE VEHICULO SINIESTRADO
                        //SSQL5 = "SP_VER_9 30,'','28730','" + txtPlaca.Text + "','clase','marca','modelo','añofab','motor','chasis','asientos','pasajeros','1','110','" + 
                        //                                     dt2.Rows[0][0].ToString() + "','" + 
                        //                                     txtNombreConductor.Text +' '+ txtApellidoPaterno.Text +' '+ txtApellidoMaterno.Text + "','TALLER','PRESUPUESTO','INICIO TALLER','FIN TALLER',1";
                        SSQL5 = "SP_VER_9 30,'','28730','" + txtPlaca.Text + "','','','','','','','','','1','110','" +
                                                             dt2.Rows[0][0].ToString() + "','" +
                                                             txtNombreConductor.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text + "','','','','',1";
                        DataTable dt5 = dat.TSegSQL(SSQL5);
                        
                        //ASIGNACION DE VEHICULO CON SINIESTRO
                        //SSQL6 = "sp_mante 16,'" + dt2.Rows[0][0].ToString() + "','28730','" +
                        //                        txtNombreConductor.Text + ' ' + txtApellidoPaterno.Text + ' ' + txtApellidoMaterno.Text + "','TALLER ASIGNADO','MONTO PRESUPUESTO','FECINGTALLER','FECHASALIDATALLER','FECHAINSPECCION','MONTORESPCIVIL','MONTODAÑO','LUGAR SINIESTRO','DESCRIPSINIES','1','" + dt5.Rows[0][0].ToString() + "',RC,PERTOT,DANOCU,'CONTACTOTALLER',TODORIESGO,'MONTOTODORIESGO'";
                        //no se usa porque sale error de foranea
                        //SSQL6 = "sp_mante 16,'" + dt2.Rows[0][0].ToString() + "','28730','" +
                        //                       txtNombreConductor.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text + "','','','','','','','','','','1','" + dt5.Rows[0][2].ToString() + "',0,0,0,'',1,'0'";

                        //DataTable dt6 = dat.TSegSQL(SSQL6);

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

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('"+ RUTA +"');", true);
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

    }
}