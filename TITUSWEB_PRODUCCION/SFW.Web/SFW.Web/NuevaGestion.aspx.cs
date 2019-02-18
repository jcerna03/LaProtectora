﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using SFW.BL;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SFW.BE;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Services;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Configuration;

namespace SFW.Web
{
    public partial class NuevaGestion : System.Web.UI.Page
    {

        Datos dat = new Datos();
        string xSQL;
        DataTable dtab;
        Usuario usu = new Usuario();
        DataTable dtab1, dtab2, dtab3 = new DataTable();
        List<string> adjuntosCliente = new List<string>();
        public string NOMBRECOMPLETO = "";
        string idLlamada;
        string idDatos, idDocum;
        string idGestion, idSubGestion, idEstado;
        DataTable datos = new DataTable();
        public void validarBotones()
        {
            if (datos.Rows[0]["clientes"].ToString() == "62")
            {
                hfCliente.Value = datos.Rows[0]["clientes"].ToString();
                hfPerfil.Value = datos.Rows[0]["interno"].ToString();
                divRespuestaGeneral.Attributes.Add("style", "display:none;");
                divGuardar2.Attributes.Add("style", "display:none;");
                divGuardar3.Attributes.Add("style", "display:none;");

            }
            else
            {
                hfCliente.Value = "";
                hfPerfil.Value = "";
                divRespuestaGeneral.Attributes.Add("style", "display:initial;");
                divGuardar2.Attributes.Add("style", "display:initial;");
                divGuardar3.Attributes.Add("style", "display:initial;");

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["DATOS"] == null || Session["USUARIO"] == null)
            {
                Response.Redirect("Sesion.aspx?usu=&pass=");
            }
            datos = ((DataTable)Session["DATOS"]);

            validarBotones();
            if (!Page.IsPostBack)
            {
                Cargarcombos();
                if (Request.QueryString["id"] != null)
                {
                    hfIdVoip.Value = Request.QueryString["id"].ToString();
                    DataTable dtfill = dat.mysql("call sp_fill_2(0,'" + hfIdVoip.Value + "','','')");
                    if (dtfill.Rows.Count > 0)
                    {
                        hfidEmpresa.Value = dtfill.Rows[0]["IDEMPRESA"].ToString();
                        hfTipoAsegurado.Value = dtfill.Rows[0]["TIPO_ASEG"].ToString();
                        hfIdAsegurado.Value = dtfill.Rows[0]["ID_ASEG"].ToString();
                        hfDescripcionAsegurado.Value = dtfill.Rows[0]["DESCRIP_ASEG"].ToString();
                        if (hfIdAsegurado.Value != "")
                        {
                            if (hfIdAsegurado.Value.Length == 10)
                            {
                                hfCod_Cliente.Value = hfIdAsegurado.Value.Substring(0, 2);
                                hfCod_Titula.Value = hfIdAsegurado.Value.Substring(2, 6);
                                hfCategoria.Value = hfIdAsegurado.Value.Substring(8, 2);
                                hfComparacion.Value = hfIdAsegurado.Value.Substring(8, 2);
                                EDITAR(hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value, hfCategoria.Value, hfidEmpresa.Value);
                            }
                            else
                            {
                                hfCod_Cliente.Value = hfIdAsegurado.Value;
                                EDITAR(hfIdAsegurado.Value, "", "", "", hfidEmpresa.Value);
                            }
                        }

                        if (hfIdVoip.Value != "0")
                        {
                            CARGAR_REGISTRO_DATOS(hfIdVoip.Value);
                            CARGAR_RESUELTOS(hfIdVoip.Value);
                        }
                        if (hfTipoAsegurado.Value == "OTROS")
                        {
                            GENERALOTROS();
                        }
                    }

                    CARGAR_DATA();

                }
            }
        }
        void EDITAR(string Cod_cliente, string Cod_titula, string Categoria, string Comparacion, string Empresa)
        {

            if (Cod_cliente.Length + Cod_titula.Length + Categoria.Length == 10)
            {
                hfCod_Cliente.Value = Cod_cliente;
                hfCod_Titula.Value = Cod_titula;
                hfCategoria.Value = Categoria;
                hfComparacion.Value = Comparacion;
                DataTable dt = dat.mysql("call sp_fill(15,'" + hfCod_Titula.Value + "','" + hfCategoria.Value + "','" + hfCod_Cliente.Value + "')");
                if (dt.Rows.Count > 0)
                {
                    hfEstado_Titular.Value = dt.Rows[0]["estado_titular"].ToString();
                    avisosNuevaGestion(hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                    observacionesSusalud(hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                    AseguradoPrincipal(hfCod_Titula.Value, hfCategoria.Value, hfCod_Cliente.Value);
                }
            }
            else
            {
                hfCod_Cliente.Value = Cod_cliente;
                PolizasClientes(hfCod_Cliente.Value);
                DocumentosClientes(hfCod_Cliente.Value);
                ClientePrincipal(hfCod_Cliente.Value);
                lnkEditarAsegurado.Visible = false;
                lnkEditarAsegura2.Visible = false;
            }

            GENERALASEGURADO(Convert.ToInt32(Empresa));
            DOCUMENTOS(hfCod_Cliente.Value, Convert.ToInt32(Empresa));
        }

        void CARGAR_DATA()
        {
            ORIGEN();
            ORIGENMODAL();
            EMISOR();
            EMISORMODAL();
            GESTION(ddlTablas.SelectedValue);
            ASEGURADO();
            ESTADO();
        }

        void LIMPIAR()
        {
            lblOrigen.Text = "";
            hfOrigen.Value = "";
            hfIdEmisor.Value = "";
            hfEmisor.Value = "";
            lblEmisor.Text = "";
            divlblEmisor.Attributes.Add("style", "display:none;");
            divlblAsegurado.Attributes.Add("style", "display:none;");
            hfIdAsegurado.Value = "";
            hfTipoAsegurado.Value = "";
            hfidDatos.Value = "";
            hfidDocum.Value = "";
            hfidAvisos.Value = "";
            hfGestion.Value = "";
            hfSubGestion.Value = "";
            txtRespuesta.Text = "";
            txtOtros.Text = "";
            hfEstado.Value = "";
            txtRespuestaGeneral.Text = "";
            txtContactoNombre.Text = "";
            txtContactoTelefono.Text = "";
            txtContactoCorreo.Text = "";
            txtFamiliar.Text = "";
            txtTelefonoContactoFamiliar.Text = "";
            txtTelefonoContactoOtros.Text = "";
            txtCorreoContactoFamiliar.Text = "";
            txtCorreoContactoOtros.Text = "";
            lblAsegurado.Text = "";
            lnkEditarAsegurado.Visible = false;
            lnkEditarAsegura2.Visible = false;
            gvDatosAfiliado.DataSource = null;
            gvDatosAfiliado.DataBind();
            gvClientePrincipal.DataSource = null;
            gvClientePrincipal.DataBind();
            gvDocumentos.DataSource = null;
            gvDocumentos.DataBind();
            gvAvisos.DataSource = null;
            gvAvisos.DataBind();
            gvGeneral.DataSource = null;
            gvGeneral.DataBind();
            gvSubGestion.DataSource = null;
            gvSubGestion.DataBind();
            lblNohayAvisos1.Text = "";
            lblDocumentos.Text = "";
            lblNoHayDNI.Text = "";
            lblFechaNacimiento.Text = "";
            lblnoHayObs.Text = "";
            gvGestionResueltos.DataSource = null;
            gvGestionResueltos.DataBind();
            divlblRegistro.Attributes.Add("style", "display:none;");
            Session["currentRESUELTOS"] = null;
            Session["RESUELTOS"] = null;
            lblPolizaReg.Text = "";
            botonesSiniestros.Visible = false;
        }

        void CARGAR_REGISTRO_DATOS(string id)
        {
            dtab = dat.mysql("CALL SP_GESTION_VOIP(7," + id + ",'','','','');");

            hfOrigen.Value = dtab.Rows[0]["TIPO_LLAMADA"].ToString();
            hfOrigenModal.Value = dtab.Rows[0]["TIPO_LLAMADA"].ToString();
            hfOrigenDescripModal.Value = dtab.Rows[0]["LLAMADADESCRIP"].ToString();

            if (hfOrigenDescripModal.Value == "LIBRO DE RECLAMACIONES")
            {
                hfLibro.Value = "SI";
            }
            else
            {
                hfLibro.Value = "NO";
            }

            hfEmisor.Value = dtab.Rows[0]["TIPO_EMI"].ToString();
            hfEmisorModal.Value = dtab.Rows[0]["TIPO_EMI"].ToString();
            hfEmisorDescripModal.Value = dtab.Rows[0]["EMISORDESCRIP"].ToString();

            txtContactoNombre.Text = dtab.Rows[0]["NOMBRE_CONT"].ToString();
            txtContactoNombreModal.Text = dtab.Rows[0]["NOMBRE_CONT"].ToString();

            txtContactoTelefono.Text = dtab.Rows[0]["TELEFONO_CONT"].ToString();
            txtContactoCorreo.Text = dtab.Rows[0]["CORREO_CONT"].ToString();
            txtContactoDni.Text = dtab.Rows[0]["DNI_EMI"].ToString();

            divlblEmisor.Attributes.Add("style", "display:inline;");
            hfIdEmisor.Value = dtab.Rows[0]["ID_EMI"].ToString();
            lblEmisor.Text = dtab.Rows[0]["DESCRIP_EMI"].ToString();

            divlblAsegurado.Attributes.Add("style", "display:inline;");
            lblAsegurado.Text = dtab.Rows[0]["DESCRIP_ASEG"].ToString();

            hfGestion.Value = dtab.Rows[0]["TIPO_GESTION"].ToString();
            hfSubGestion.Value = dtab.Rows[0]["TIPO_SUBGESTION"].ToString();
            lblsubgestion.Text = dtab.Rows[0]["SUBGESTIONDESCRIP"].ToString();

            txtRespuestaGeneral.Text = dtab.Rows[0]["RESPUESTA"].ToString();
            hfEstado.Value = dtab.Rows[0]["ESTADO"].ToString();

            if (hfEmisor.Value == "407")
            {
                txtOtros.Text = dtab.Rows[0]["NOMBRE_CONT"].ToString();
                if (dtab.Rows[0]["CLIENTE"].ToString() != "99")
                {
                    ddlOtrosCliente.SelectedValue = dtab.Rows[0]["CLIENTE"].ToString();
                }
                txtTelefonoContactoOtros.Text = dtab.Rows[0]["TELEFONO_CONT"].ToString();
                txtCorreoContactoOtros.Text = dtab.Rows[0]["CORREO_CONT"].ToString();
            }
            if (hfEmisor.Value == "406")
            {
                txtFamiliar.Text = dtab.Rows[0]["NOMBRE_CONT"].ToString();
                txtTelefonoContactoFamiliar.Text = dtab.Rows[0]["TELEFONO_CONT"].ToString();
                txtCorreoContactoFamiliar.Text = dtab.Rows[0]["CORREO_CONT"].ToString();
            }
            ///////////////////////////////////////////////////////////////////
            if (hfEstado.Value == "438" && dtab.Rows[0]["USUARIODERIVA"].ToString() != "")
            {
                divUsuDeriva.Attributes.Add("style", "display:initial;");
                lblUsuaDeriva.Text = dtab.Rows[0]["USUARIODERIVA"].ToString();
            }
            else
            {
                divUsuDeriva.Attributes.Add("style", "display:none;");
            }
            ///////////////////////////////////////////////////////////////////
            if (hfCliente.Value == "62")
            {
                divBuscarReclamo.Attributes.Add("style", "display:initial");
            }
            else
            {
                divBuscarReclamo.Attributes.Add("style", "display:none");
            }

            SUBGESTION(hfGestion.Value);
        }

        void ORIGEN()
        {
            xSQL = "CALL SP_GESTION_VOIP(1,300,'" + hfCliente.Value + "','','" + hfPerfil.Value + "','');";
            dtab = dat.mysql(xSQL);
            gvOrigen.DataSource = dtab;
            gvOrigen.DataBind();
            gvOrigenModal.DataSource = dtab;
            gvOrigenModal.DataBind();
        }

        void ORIGENMODAL()
        {
            xSQL = "CALL SP_GESTION_VOIP(1,300,'" + hfCliente.Value + "','','" + hfPerfil.Value + "','');";
            dtab = dat.mysql(xSQL);
            gvOrigenModal.DataSource = dtab;
            gvOrigenModal.DataBind();
        }

        void EMISOR()
        {
            xSQL = "CALL SP_GESTION_VOIP(1,301,'" + hfCliente.Value + "','','','');";
            dtab = dat.mysql(xSQL);
            gvEmisor.DataSource = dtab;
            gvEmisor.DataBind();
        }

        void EMISORMODAL()
        {
            xSQL = "CALL SP_GESTION_VOIP(1,301,'" + hfCliente.Value + "','','','');";
            dtab = dat.mysql(xSQL);
            gvEmisorModal.DataSource = dtab;
            gvEmisorModal.DataBind();
        }

        void GESTION(string empresa)
        {

            xSQL = "CALL SP_GESTION_VOIP(1,302,'" + hfCliente.Value + "','" + empresa + "','','');";
            dtab = dat.mysql(xSQL);
            gvGestion.DataSource = dtab;
            gvGestion.DataBind();
        }

        void ASEGURADO()
        {
            xSQL = "CALL SP_GESTION_VOIP(20,309,'','','','');";
            dtab = dat.mysql(xSQL);
            gvAsegurado.DataSource = dtab;
            gvAsegurado.DataBind();
        }

        void ESTADO()
        {
            //ESTADO
            xSQL = "CALL SP_GESTION_VOIP(12,'304','','','" + hfPerfil.Value + "','');";
            dtab = dat.mysql(xSQL);
            gvEstado.DataSource = dtab;
            gvEstado.DataBind();
        }

        void GENERALASEGURADO(int aseguradoCliente)
        {
            //GENERALES
            xSQL = "CALL SP_GESTION_VOIP(23,308,'" + hfEstado_Titular.Value + "','" + aseguradoCliente + "','','" + hfCliente.Value + "');";
            dtab = dat.mysql(xSQL);
            gvGeneral.DataSource = dtab;
            gvGeneral.DataBind();
        }

        void GENERALOTROS()
        {
            //DOC GENERALES
            xSQL = "CALL SP_GESTION_VOIP(23,308,'','','','" + hfCliente.Value + "');";
            dtab = dat.mysql(xSQL);

            for (int i = dtab.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtab.Rows[i];
                if (dr["ID"].ToString() != "551")
                    dr.Delete();
            }

            gvGeneral.DataSource = dtab;
            gvGeneral.DataBind();
        }

        void DOCUMENTOS(string cliente, int aseguradoCliente)
        {

            xSQL = "CALL SP_GESTION_VOIP(22,305,'" + cliente + "','" + aseguradoCliente + "','','');";
            dtab = dat.mysql(xSQL);

            if (dtab.Rows.Count == 0)
            {
                gvDocumentos.DataSource = null;
                gvDocumentos.DataBind();
                lblDocumentos.Text = "No existen documentos registrados";
            }
            else
            {
                gvDocumentos.DataSource = dtab;
                gvDocumentos.DataBind();
            }
        }

        void SUBGESTION(string VAL)
        {
            xSQL = "CALL SP_GESTION_VOIP(21,303," + VAL + ",'" + hfLibro.Value + "','" + hfPerfil.Value + "','');";
            dtab = dat.mysql(xSQL);
            gvSubGestion.DataSource = dtab;
            gvSubGestion.DataBind();
        }

        void CARGARDATAMODAL()
        {
            ORIGENMODAL();
            EMISORMODAL();
        }

        void OCULTAR_MSJ()
        {

            lblalerta.Text = "";
            correcto.Visible = false;
            lblErrorReg.Text = "";
            orror.Visible = false;
        }

        void NUEVO_REGISTRO()
        {

            if (hfOrigen.Value == "" || hfIdEmisor.Value == "" || hfIdAsegurado.Value == "" || hfGestion.Value == "" || hfSubGestion.Value == "" || hfCod_Cliente.Value == "")
            {
                lblErrorReg.Text = "Debe ingresar los campos obligatorios (*)";
                orror.Visible = true;
                return;
            }

            try
            {
                string idAsegurado;
                string nombrecontacto = "";
                string telefonocontacto = "";
                string correocontacto = "";

                try
                {
                    DataTable dtRegRes = (DataTable)Session["currentRESUELTOS"];
                    if (dtRegRes == null)
                    {
                        dtRegRes = (DataTable)Session["RESUELTOS"];
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                nombrecontacto = txtContactoNombre.Text;
                telefonocontacto = txtContactoTelefono.Text;
                correocontacto = txtContactoCorreo.Text;

                if (hfIdAsegurado.Value == "9999999999")
                {
                    idAsegurado = "9999999999";
                }
                else
                {
                    idAsegurado = hfIdAsegurado.Value + hfCod_Titula.Value + hfCategoria.Value;
                }

                if (hfSiniestroReg.Value != "")
                {
                    txtRespuestaGeneral.Text += " ||ID SINIESTRO:  " + hfSiniestroReg.Value;
                }

                string respuestaGeneral = "";

                if (txtRespuestaGeneral.Text.Trim() != "")
                {
                    respuestaGeneral = txtRespuestaGeneral.Text;
                }

                if (idAsegurado != "")
                {
                    string xSQL = "CALL SP_MANTE_GESTION(1,'" + hfOrigen.Value + "','" + hfEmisor.Value + "','" + hfIdEmisor.Value + "','" + lblEmisor.Text + "'," +
                    "'" + hfTipoAsegurado.Value + "','" + idAsegurado + "','" + lblAsegurado.Text + "','" + hfGestion.Value + "'," +
                    "'" + hfSubGestion.Value + "','" + respuestaGeneral + "','','" + hfEstado.Value + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["USUARIO"] + "','" + nombrecontacto + "','" + telefonocontacto + "','" +
                    correocontacto + "','" + hfidEmpresa.Value + "','" + hfidUnidad_Negocio.Value + "','" + hfidPoliza.Value + "','" + '-' + "','" + hfNroPoliza.Value + "','" + hfUnidad_Negocio.Value + "','','" + hfDniEmisor.Value + "','" + hfDniAsegurado.Value + "','" + hfIdUsuarioDeriva.Value + "','','','')";
                    DataTable dt = dat.mysql(xSQL);
                    string idvoid = dt.Rows[0][0].ToString();
                    if (idvoid != "0")
                    {
                        try
                        {
                            DataTable dtRegRes = (DataTable)Session["currentRESUELTOS"];
                            if (dtRegRes == null)
                            {
                                dtRegRes = (DataTable)Session["RESUELTOS"];
                            }

                            if (dtRegRes != null)
                            {
                                foreach (DataRow item in dtRegRes.Rows)
                                {
                                    string QueryResuelto = "call SP_MANTE_GESTION(3,'" + item["ORIGEN"].ToString() + "','" + item["IDSALIENTE"].ToString() + "','" + item["DESCRIP_CONTACTO"].ToString() + "','" + item["RESPUESTA"].ToString() + "','" +
                                                                        item["OBS"].ToString() + "','" + item["USU_REG"].ToString() + "','" + idvoid + "','" + item["NRO"].ToString() + "','" + item["ARCHIVO"].ToString() + "','','','','','','','','','','','','','','','','','','','','','');";
                                    dat.mysql(QueryResuelto);

                                }
                            }

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        lblalerta.Text = "REGISTRO GUARDADO.";
                        correcto.Visible = true;
                        LIMPIAR();
                        CARGAR_DATA();
                        Response.Redirect("GestionVoip.aspx");
                    }
                    else
                    {
                        lblErrorReg.Text = "Error : No se registró la gestión";
                        orror.Visible = true;
                    }
                }
                else
                {
                    lblErrorReg.Text = "Los datos de asegurado, estan incompletos, vuelva a seleccionarlo, si el error persiste comunicarse con el Administrador del sistema.";
                    orror.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {

                lblErrorReg.Text = "Error : " + ex.Message.ToString();
                orror.Visible = true;
            }
        }

        void ACTUALIZA_REGISTRO(string IDVAL)
        {

            try
            {
                string idAsegurado;
                string nombrecontacto = "";
                string telefonocontacto = "";
                string correocontacto = "";

                if (hfIdAsegurado.Value == "9999999999")
                {
                    idAsegurado = "9999999999";
                }
                else
                {
                    idAsegurado = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                }

                if (hfDatosAdicionales.Value == "1")
                {
                    nombrecontacto = txtFamiliar.Text;
                    telefonocontacto = txtTelefonoContactoFamiliar.Text;
                    correocontacto = txtCorreoContactoFamiliar.Text;
                }
                else if (hfDatosAdicionales.Value == "2")
                {
                    nombrecontacto = txtOtros.Text;
                    telefonocontacto = txtTelefonoContactoOtros.Text;
                    correocontacto = txtCorreoContactoOtros.Text;
                }
                else
                {
                    nombrecontacto = txtContactoNombre.Text;
                    telefonocontacto = txtContactoTelefono.Text;
                    correocontacto = txtContactoCorreo.Text;
                }

                if (idAsegurado != "")
                {
                    string xSQL = "CALL SP_MANTE_GESTION(2,'" + IDVAL + "','" + hfOrigen.Value + "','" + hfEmisor.Value + "','" + hfIdEmisor.Value + "','" + lblEmisor.Text + "'," +
                                    "'" + hfTipoAsegurado.Value + "','" + idAsegurado + "','" + lblAsegurado.Text + "','" + hfGestion.Value + "'," +
                                    "'" + hfSubGestion.Value + "','" + txtRespuestaGeneral.Text + "','','" + hfEstado.Value + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["USUARIO"] + "','" + nombrecontacto + "','" + telefonocontacto + "','" + correocontacto + "','" +
                                    hfidEmpresa.Value + "','" +
                                    hfidUnidad_Negocio.Value + "','" + hfidPoliza.Value + "','','" + hfNroPoliza.Value + "','" + hfUnidad_Negocio.Value + "','" + hfDniEmisor.Value + "','" + hfDniAsegurado.Value + "','" + hfIdUsuarioDeriva.Value + "','','','')";
                    dat.mysql(xSQL);
                    lblalerta.Text = "REGISTRO ACTUALIZADO.";
                    correcto.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#mdlConfirmacion').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                }
                else
                {
                    lblErrorReg.Text = "Los datos de asegurado, estan incompletos o mal cargados, vuelva a seleccionarlo, si el error persiste comunicarse con el Administrador del sistema.";
                    orror.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = "Error : " + ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void lnkGuardar_Click(object sender, EventArgs e)
        {
            OCULTAR_MSJ();

            if (Session["USUARIO"] == null)
            {
                Response.Redirect("Sesion.aspx");
            }
            else
            {
                if (hfIdVoip.Value == "0")
                {
                    NUEVO_REGISTRO();
                    if (hfServicioSiniestro.Value != "" && hfpoli.Value != "")
                    {
                        GUARDAR_SINIESTRO_SERVICIO(hfServicioSiniestro.Value, hfpoli.Value);
                    }
                }
                else
                {
                    ACTUALIZA_REGISTRO(hfIdVoip.Value);
                }
            }
        }

        void cargar_proveedor()
        {
            //PROVEEDOR
            xSQL = "CALL SP_GESTION_VOIP(4,'" + txtBusquedaPro.Text + "','','','','');";
            dtab = dat.mysql(xSQL);
            gvProveedor.DataSource = dtab;
            gvProveedor.DataBind();
        }

        void cargar_ejecutivo()
        {
            //EJECUTIVO
            if (hfDerivadoEjecutivo.Value == "1")
            {
                xSQL = "CALL SP_GESTION_VOIP(5,'" + txtEjecutivo01.Text + "','" + Session["USUARIO"].ToString() + "','" + hfLibro.Value + "','" + hfOrdenSubGestion.Value + "','" + hfCliente.Value + "');";
            }
            else
            {
                xSQL = "CALL SP_GESTION_VOIP(5,'" + txtEjecutivo01.Text + "','','','','" + hfCliente.Value + "');";
            }
            dtab = dat.mysql(xSQL);
            gvEjecutivo.DataSource = dtab;
            gvEjecutivo.DataBind();
        }

        void cargar_afiliado()
        {
            xSQL = "CALL sp_fill(83,'" + txtBusquedaAfiliado.Text + "','" + Session["USUARIO"].ToString() + "','" + ddlTablas.SelectedValue + "')";
            dtab = dat.mysql(xSQL);
            if (hfCliente.Value != "62")
            {
                string xs1 = "sp_ver_14 19,'" + txtBusquedaAfiliado.Text + "'";
                DataTable dtsql = dat.TSegSQL(xs1);
                dtab.Merge(dtsql);
            }
            gvAfiliado.DataSource = dtab;
            gvAfiliado.DataBind();
        }



        protected void gvAfiliado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECCIONAR")
            {
                try
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                    string idAfiliado = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["cod_cliente"]);
                    string nombreAfiliado = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["afiliado"]);
                    string cod_cliente = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["cod_cliente"]);
                    string cod_titula = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["cod_titula"]);
                    string comparacion = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["categoria"]);
                    string categoria = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["categoria"]);
                    string dni = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["dni"]);
                    string fecnac = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["fch_naci"]);
                    
                    hfidEmpresa.Value = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["idempresa"]);
                    hfidUnidad_Negocio.Value = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["idUnidad_Negocio"]);
                    hfUnidad_Negocio.Value = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["UNIDAD_NEGOCIO"]);
                    hfEstado_Titular.Value = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["estado_titular"]);

                    if (dni == "")
                    {
                        lblNoHayDNI.Text = "No hay un dni registrado, favor de ingresarlo.";
                    }
                    if (fecnac == "")
                    {
                        lblFechaNacimiento.Text = "No hay fecha de nacimiento registrada, favor de ingresarlo.";
                    }

                    txtContactoNombre.Text = nombreAfiliado;
                    txtContactoNombreModal.Text = nombreAfiliado;

                    divlblAsegurado.Attributes.Add("style", "display:inline;");
                    lblAsegurado.Text = nombreAfiliado;
                    hfIdAsegurado.Value = idAfiliado;
                    hfCod_Cliente.Value = cod_cliente;

                    DOCUMENTOS(hfCod_Cliente.Value, Convert.ToInt32(hfidEmpresa.Value));
                    GENERALASEGURADO(Convert.ToInt32(hfidEmpresa.Value));

                    #region LP
                    if ((idAfiliado == cod_titula) && (hfidEmpresa.Value == "88"))//LA PROTECTORA
                    {
                        hfCod_Cliente.Value = idAfiliado;
                        hfCod_Titula.Value = "";
                        hfCategoria.Value = "";
                        hfComparacion.Value = "";
                        hfNombreCliente.Value = nombreAfiliado;

                        PolizasClientes(idAfiliado);
                        DocumentosClientes(idAfiliado);
                        ClientePrincipal(idAfiliado);

                        gvPolizasCliente.Columns[1].Visible = true;
                        lnkSinAsignar.Visible = true;
                        lnkEditarAsegurado.Visible = false;
                        lnkEditarAsegura2.Visible = false;

                        if (hfTipoModalAfiliado.Value == "emisor")
                        {
                            divlblEmisor.Attributes.Add("style", "display:inline;");
                            lblEmisor.Text = nombreAfiliado;
                            hfIdEmisor.Value = idAfiliado;
                            hfDniEmisor.Value = dni;

                            string xSQL = "sp_ver_14 20,'" + idAfiliado + "'";
                            DataTable dtab = dat.TSegSQL(xSQL);

                            if (dtab.Rows.Count > 0)
                            {
                                txtContactoDni.Text = dtab.Rows[0]["dni"].ToString();
                                txtContactoTelefono.Text = dtab.Rows[0]["Telefono"].ToString();
                                txtContactoCorreo.Text = dtab.Rows[0]["E_mail"].ToString();
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#Afiliado').modal('hide');");
                        sb.Append("$('#PolizasCliente').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                    #endregion
                    else
                    {
                        hfCod_Cliente.Value = cod_cliente;
                        hfCod_Titula.Value = cod_titula;
                        hfCategoria.Value = categoria;
                        hfDniAsegurado.Value = dni;
                        hfComparacion.Value = comparacion;

                        if (hfTipoModalAfiliado.Value == "emisor")
                        {
                            divlblEmisor.Attributes.Add("style", "display:inline;");
                            lblEmisor.Text = nombreAfiliado;
                            hfIdEmisor.Value = idAfiliado;
                            hfDniEmisor.Value = dni;

                            string xSQL = "CALL sp_fill(15,'" + cod_titula + "','" + categoria + "','" + cod_cliente + "')";
                            DataTable dtab = dat.mysql(xSQL);

                            if (dtab.Rows.Count > 0)
                            {
                                txtContactoTelefono.Text = dtab.Rows[0]["t_fijo"].ToString();
                                txtContactoCorreo.Text = dtab.Rows[0]["email"].ToString();
                                txtContactoDni.Text = dtab.Rows[0]["dni"].ToString();
                            }
                        }

                        avisosNuevaGestion(hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                        observacionesSusalud(hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                        AseguradoPrincipal(hfCod_Titula.Value, hfCategoria.Value, hfCod_Cliente.Value);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#Afiliado').modal('hide');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                    orror.Visible = true;
                }
            }

            if (e.CommandName == "VER")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string cod_cliente = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["cod_cliente"]);
                string cod_titula = Convert.ToString(this.gvAfiliado.DataKeys[currentRowIndex]["cod_titula"]);

                if (cod_cliente == cod_titula)
                {
                    PolizasClientes(cod_cliente);
                    gvPolizasCliente.Columns[1].Visible = false;
                    lnkSinAsignar.Visible = false;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#PolizasCliente').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
                else
                {
                    grupofamiliar(cod_titula, cod_cliente);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#GRUPOFAMILIAR').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
            }
        }

        protected void grupofamiliar(string cod_titula, string cliente)
        {
            string xSQL = "CALL sp_fill_2(1,'" + cod_titula + "','" + cliente + "','')";
            dtab = dat.mysql(xSQL);
            gvGrupoFamiliar.DataSource = dtab;
            gvGrupoFamiliar.DataBind();
        }

        //##############################################################################################################

        protected void gvOrigen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {
                if (e.CommandName == "Seleccionar")
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    idLlamada = Convert.ToString(this.gvOrigen.DataKeys[currentRowIndex]["ID"]);
                    hfOrigen.Value = idLlamada;
                    hfOrigenModal.Value = idLlamada;
                    hfOrigenDescripModal.Value = gvOrigen.DataKeys[currentRowIndex]["DESCRIP"].ToString();
                    if (gvOrigen.DataKeys[currentRowIndex]["ORDEN"].ToString() == "15")
                    {
                        hfLibro.Value = "SI";
                    }
                    else
                    {
                        hfLibro.Value = "NO";
                    }
                    ORIGEN();
                }
            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvEmisor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            if (e.CommandName == "BUSCAR")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                hfEmisor.Value = Convert.ToString(this.gvEmisor.DataKeys[currentRowIndex]["ID"]);
                hfEmisorModal.Value = hfEmisor.Value;
                hfEmisorDescripModal.Value = this.gvEmisor.DataKeys[currentRowIndex]["DESCRIP"].ToString();
                hfOrdenEmisor.Value = this.gvEmisor.DataKeys[currentRowIndex]["ORDEN"].ToString();
                EMISOR();
                divddlOtrosCliente.Attributes.Add("style","display:none;");
                ddlOtrosCliente.Visible = false;
                hfTipoModalAfiliado.Value = "emisor";

                if (hfOrdenEmisor.Value == "1" || hfOrdenEmisor.Value == "6")
                {
                    cargar_afiliado();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Afiliado').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                }
                else if (hfOrdenEmisor.Value == "2" || hfOrdenEmisor.Value == "10")
                {
                    limpiarIpress();
                    cargar_proveedor();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Proveedor').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
                else if (hfOrdenEmisor.Value == "3" || hfOrdenEmisor.Value == "9")
                {
                    limpiarEjecutivo();
                    cargar_ejecutivo();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Ejecutivo').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                }
                else if (hfOrdenEmisor.Value == "4" || hfOrdenEmisor.Value == "7")
                {
                    limpiarFamiliar();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Familiar').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
                else if (hfOrdenEmisor.Value == "5" || hfOrdenEmisor.Value == "8" || hfOrdenEmisor.Value == "11")
                {
                    limpiarOtros();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Otros').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
            }
        }

        public void limpiarOtros()
        {
            frmdatosAdicionalesAsegurado.Visible = false;
            frmdatosAdicionales.Visible = true;
        }

        public void limpiarFamiliar()
        {
            txtFamiliar.Text = "";
            txtTelefonoContactoFamiliar.Text = "";
            txtCorreoContactoFamiliar.Text = "";
            txtDniContactoFamiliar.Text = "";

        }

        public void limpiarEjecutivo()
        {
            hfDerivadoEjecutivo.Value = "0";
            txtEjecutivo.Text = "";
        }

        public void limpiarIpress()
        {
            txtContactoProveedor.Text = "";
            lblContactoProveedor.Text = "";
            txtBusquedaPro.Text = "";
            txtBusquedaPro.Focus();
            lblAsegurado.Text = "";
        }

        protected void gvAsegurado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            if (e.CommandName == "BUSCAR")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string Asegurado = Convert.ToString(this.gvAsegurado.DataKeys[currentRowIndex]["ID"]);
                hfAsegurado.Value = Asegurado;
                hfTipoAsegurado.Value = Convert.ToString(this.gvAsegurado.DataKeys[currentRowIndex]["DESCRIP"]);
                hfTipoModalAfiliado.Value = "asegurado";
                divddlOtrosCliente.Attributes.Add("style", "display:inline-block;");
                ddlOtrosCliente.Visible = true;
                ASEGURADO();
                if (hfTipoAsegurado.Value == "ASEGURADO")
                {
                    cargar_afiliado();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Afiliado').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
                else if (hfTipoAsegurado.Value == "OTROS")
                {
                    limpiarOtros();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type='text/javascript'>");
                    sb.Append("$('#Otros').modal('show');");
                    sb.Append("</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                }
            }
        }

        protected void gvGeneral_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {
                if (e.CommandName == "Seleccionar")
                {

                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    idDatos = Convert.ToString(this.gvGeneral.DataKeys[currentRowIndex]["ID"]);
                    hfidDatos.Value = idDatos;
                    GENERALASEGURADO(Convert.ToInt32(hfidEmpresa.Value));

                    DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");

                    //RECORD DE CONSUMO
                    if (idDatos == "466")
                    {
                        if (dt != null)
                        {
                            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
                            string password = Convert.ToString(dt.Rows[0][6].ToString());
                            string datpac = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key",
                             "window.open('http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=recordconsumo.php&p1=paciente&v1=" + datpac + "');", true);
                            return;
                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error al cargar el récord de consumo";
                            return;
                        }

                    }

                    //IMPRESION CARTAS
                    if (idDatos == "467")
                    {
                        if (dt != null)
                        {
                            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
                            string password = Convert.ToString(dt.Rows[0][6].ToString());
                            string datpac = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key",
                             "window.open('http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=busqueda_ordenesAtencion.php&p1=datpac&v1=" + datpac + "');", true);
                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error en la carga de Órdenes de Atención";
                        }
                    }

                    //SOLBEN
                    if (idDatos == "551")
                    {
                        if (dt != null)
                        {
                            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
                            string password = Convert.ToString(dt.Rows[0][6].ToString());
                            string datpac = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key",
                             "window.open('http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=principal.php');", true);
                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error en la carga de Órdenes de Atención";
                        }
                    }

                    //GENERACION DE CARTAS
                    if (idDatos == "468")
                    {
                        if (dt != null)
                        {
                            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
                            string password = Convert.ToString(dt.Rows[0][6].ToString());
                            string datpac = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key",

                                "window.open('http://www.solben.net/loginIII.php?u=" + usuario + "&p=" + password + "&d=3&p2=" + hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value + "');", true);

                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error en la carga de Órdenes de Atención";
                        }
                    }

                    //ESTADO DE SOLICITUD
                    if (idDatos == "469")
                    {
                        if (dt != null)
                        {
                            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
                            string password = Convert.ToString(dt.Rows[0][6].ToString());
                            string datpac = hfCod_Cliente.Value + hfCod_Titula.Value + hfCategoria.Value;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key",
                             "window.open('http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=garantias/gen_cargan.php');", true);
                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error en la carga de Órdenes de Atención";
                        }
                    }

                    //MAS DATOS
                    if (idDatos == "552")
                    {

                        usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
                        NOMBRECOMPLETO = "";
                        lblNohayCarta.Text = "";
                        lblNoHayInformes.Text = "";
                        lblNoHayRecord.Text = "";
                        if (Convert.ToInt32(hfComparacion.Value) > 4 && Convert.ToInt32(hfComparacion.Value) <= 26)
                        {
                            hfComparacion.Value = "04";
                        }

                        List<Titular> list = new TitularBL().ListarTitularesGrupo(53, hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                        Titular objTitu = list.First(delegate(Titular obj) { return (obj.cod_cliente.Equals(hfCod_Cliente.Value) && (obj.cod_titula.Equals(hfCod_Titula.Value) && (obj.categoria.Equals(hfCategoria.Value)))); });


                        List<Titular_Detalle> list2 = new TitularDetalleBL().ListarDetallesGrupoFamiliar(54, hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                        Titular_Detalle objTitudeta = list2.First(delegate(Titular_Detalle objt) { return (objt.cod_cliente.Equals(hfCod_Cliente.Value) && (objt.cod_titula.Equals(hfCod_Titula.Value) && (objt.categoria.Equals(hfCategoria.Value)))); });


                        List<TituList> list3 = new TitularBL().Busqueda(77, hfCod_Cliente.Value, hfCod_Titula.Value, hfCategoria.Value);
                        TituList objTitulist = list3.First(delegate(TituList objtl) { return (objtl.cod_cliente.Equals(hfCod_Cliente.Value)) && (objtl.cod_titula.Equals(hfCod_Titula.Value) && (objtl.categoria.Equals(hfCategoria.Value))); });

                        try
                        {
                            if (objTitu != null || objTitudeta != null)
                            {
                                //---CUANDO ES TITULAR---
                                if (hfCategoria.Value == "00")
                                {
                                    //listardependientes(codigogrilla, clientegrilla);
                                    lblAfiliado12.Text = "EDITAR TITULAR";
                                    ddlCategoria.DataSource = new ComboBL().ListaCombos(47);
                                    ddlCategoria.DataValueField = "valor";
                                    ddlCategoria.DataTextField = "descrip";
                                    ddlCategoria.DataBind();
                                }
                                else
                                {
                                    lblAfiliado12.Text = "EDITAR DEPENDIENTE";
                                }

                                txtNumeroPoli.Attributes.Add("readonly", "readonly");
                                txtCodigoTitu.ReadOnly = true;
                                ddlCategoria.Attributes.Add("readonly", "readonly");
                                cod_paciente.Visible = false;
                                id_paciente.Visible = false;
                                pad.Visible = false;
                                //---CUANDO ESTA ACTIVO O DESACTIVADO---

                                if (objTitu.estado_titular == 1)
                                {
                                    lblEstado12.Text = "||COD: " + objTitu.cod_cliente.ToString() + " - " + objTitu.cod_titula.ToString() + " - " + objTitu.categoria.ToString()
                                              + " || DNI: " + objTitu.dni.ToString()
                                              + " ||  " + objTitu.afiliado.ToString()
                                              + " || EDAD: " + objTitudeta.edad.ToString()
                                              + "<br />"
                                              + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; || FECHA ALTA: " + objTitu.fch_alta.ToString()
                                              + " || FECHA BAJA: <label class='text-danger'> " + objTitu.fch_baja.ToString() + "</label>"
                                              + " || FECHA CARENCIA: " + objTitu.fch_caren.ToString()
                                              + " || ESTADO: ACTIVO";
                                }
                                else
                                {
                                    lblEstado12.Text = "||COD: " + objTitu.cod_cliente.ToString() + " - " + objTitu.cod_titula.ToString() + " - " + objTitu.categoria.ToString()
                                                    + " || DNI: " + objTitu.dni.ToString()
                                                    + " ||  " + objTitu.afiliado.ToString()
                                                    + " || EDAD: " + objTitudeta.edad.ToString()
                                                    + "<br />"
                                                    + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; || FECHA ALTA: " + objTitu.fch_alta.ToString()
                                                    + " || FECHA BAJA: <label class='text-danger'> " + objTitu.fch_baja.ToString() + "</label>"
                                                    + " || FECHA CARENCIA: " + objTitu.fch_caren.ToString()
                                                    + " || ESTADO: BAJA";
                                }

                                string path_ruta = "http://www.solben.net/solben/foto/" + hfCod_Cliente.Value + "/" + hfCod_Cliente.Value + "-" + hfCod_Titula.Value + "-" + hfCategoria.Value + ".jpg";

                                if (RemoteFileExists(path_ruta) == true)
                                {
                                    Image1.ImageUrl = "http://www.solben.net/solben/foto/" + hfCod_Cliente.Value + "/" + hfCod_Cliente.Value + "-" + hfCod_Titula.Value + "-" + hfCategoria.Value + ".jpg";
                                }
                                else
                                {
                                    Image1.ImageUrl = "~/image/photo.png";
                                }

                                if (hfCod_Cliente.Value == "15")
                                {
                                    campo2.Visible = true;
                                }

                                if ((hfCod_Cliente.Value == "57"))
                                {
                                    if ((objTitudeta.segunda_capa == "N") && (objTitudeta.basico == "N") && (objTitudeta.onco == "N") && (objTitu.estado_titular == 0) && (objTitu.fch_baja == ""))
                                    {

                                    }

                                    ocultos2.Visible = true;
                                    segundacapa.Visible = true;
                                    basico.Visible = true;
                                    onco.Visible = true;
                                    documentoSIMA.Visible = false;
                                    id_paciente.Visible = false;
                                    cod_paciente.Visible = false;
                                }

                                if ((hfCod_Cliente.Value == "90") || (hfCod_Cliente.Value == "96") || (hfCod_Cliente.Value == "98") || (hfCod_Cliente.Value == "95"))
                                {
                                    if (hfCategoria.Value == "00")
                                    {
                                        cod_paciente.Visible = false;
                                        id_paciente.Visible = false;
                                        pad.Visible = true;
                                    }
                                    else
                                    {
                                        cod_paciente.Visible = true;
                                        id_paciente.Visible = true;
                                        pad.Visible = true;
                                    }
                                    dpto.Visible = true;
                                    rol.Visible = true;
                                    ocultos2.Visible = true;
                                    pad.Visible = true;
                                    programa.Visible = true;
                                }

                                CombosCliente(hfCod_Cliente.Value);
                                cartas123.Visible = false;
                                record123.Visible = false;
                                informes123.Visible = false;
                                avisos(hfCod_Cliente.Value, objTitu.cod_titula, Convert.ToString(objTitu.categoria));
                                movimientos(hfCod_Cliente.Value, objTitu.cod_titula.ToString(), Convert.ToString(objTitu.categoria), ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(), usu.ID.ToString());
                                txtNumeroPoli.Text = Convert.ToString(objTitu.cod_cliente);
                                txtCodigoTitu.Text = objTitu.cod_titula;
                                categoriaHidden.Value = hfCategoria.Value;
                                ddlCategoria.SelectedValue = hfComparacion.Value;
                                ddlCentro.SelectedValue = Convert.ToString(objTitu.centro_costo);
                                ddlPlan.SelectedValue = Convert.ToString(objTitu.plan);
                                ddlSexo.SelectedValue = Convert.ToString(objTitu.sexo);
                                txtNacimiento.Text = Convert.ToString(objTitu.fch_naci);
                                txtAlta.Text = Convert.ToString(objTitu.fch_alta);
                                txtBaja.Text = Convert.ToString(objTitu.fch_baja);
                                txtDNI.Text = Convert.ToString(objTitu.dni);

                                //AQUI VIENE EL DETALLE DEL AFILIADO
                                txtEdad.Text = Convert.ToString(objTitulist.edad);

                                ddlDepartamento.SelectedValue = Convert.ToString(objTitudeta.depa_id);
                                ddlDepartamento_SelectedIndexChanged(sender, e);
                                ddlProvincia.SelectedValue = Convert.ToString(objTitudeta.prov_id);
                                ddlProvincia_SelectedIndexChanged(sender, e);
                                ddlDistrito.SelectedValue = Convert.ToString(objTitudeta.dist_id);
                                txtDireccion.Text = Convert.ToString(objTitudeta.direccion);
                                txtObservar.Text = Convert.ToString(objTitudeta.email);
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

                    //CLIENTES
                    if (idDatos == "3769")
                    {
                        if (dt != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script type='text/javascript'>");
                            sb.Append("$('#PolizasCliente').modal('show');");
                            sb.Append("</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                        }
                        else
                        {
                            lblalerta.Text = "Hubo un error en la carga de Pólizas";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {
                if (e.CommandName == "Seleccionar")
                {
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    idDocum = Convert.ToString(this.gvDocumentos.DataKeys[currentRowIndex]["ID"]);
                    string ruta = Convert.ToString(this.gvDocumentos.DataKeys[currentRowIndex]["RUTA"]);

                    if (idDocum == "3770")
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#DocumentosCliente').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                    else
                    {
                        hfidDocum.Value = idDocum;

                        //RUTA FTP
                        xSQL = "CALL SP_GESTION_VOIP(8,'','','','','');";
                        dtab = dat.mysql(xSQL);

                        documentoFrame.Attributes.Add("src", "PDF.aspx?PDF=" + dtab.Rows[0][3].ToString() + ruta);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("$('#DOCUMENTO').modal('show');");
                        sb.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

                        DOCUMENTOS(hfCod_Cliente.Value, Convert.ToInt32(hfidEmpresa.Value));
                    }
                }
            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvGestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {

                if (e.CommandName == "Seleccionar")
                {

                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    lblsubgestion.Text = "";
                    idGestion = Convert.ToString(this.gvGestion.DataKeys[currentRowIndex]["ID"]);

                    hfGestion.Value = idGestion;
                    hfSubGestion.Value = "";
                    if (hfCliente.Value == "62")
                    {
                        divBuscarReclamo.Attributes.Add("style", "display:initial");
                    }
                    else
                    {
                        divBuscarReclamo.Attributes.Add("style", "display:none");
                    }
                    SUBGESTION(idGestion);

                    GESTION(ddlTablas.SelectedValue);

                    if (idGestion == "414")
                    {

                    }

                }


            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvSubGestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {
                if (e.CommandName == "Seleccionar")
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                    idSubGestion = Convert.ToString(this.gvSubGestion.DataKeys[currentRowIndex]["ID"]);
                    hfSubGestion.Value = idSubGestion;
                    lblsubgestion.Text = Convert.ToString(this.gvSubGestion.DataKeys[currentRowIndex]["DESCRIP"]);
                    hfOrdenSubGestion.Value = Convert.ToString(this.gvSubGestion.DataKeys[currentRowIndex]["ORDEN"]);
                    SUBGESTION(hfGestion.Value);
                }
            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvEstado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {

                if (e.CommandName == "Seleccionar")
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                    idEstado = Convert.ToString(this.gvEstado.DataKeys[currentRowIndex]["ID"]);
                    hfEstado.Value = idEstado;
                    if (this.gvEstado.DataKeys[currentRowIndex]["DESCRIP"].ToString() == "DERIVADO")
                    {

                        if (hfLibro.Value == "" || hfOrdenSubGestion.Value == "")
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script type='text/javascript'>");
                            sb.Append("alert('" + "Seleccionar ORIGEN y SUBGESTIÓN" + "');");
                            sb.Append("</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                            return;
                        }
                        txtAsunto.Text = hfCod_Cliente.Value + '-' + hfCod_Titula.Value + '-' + hfCategoria.Value + ' ' + lblAsegurado.Text;

                        int contador = gvGestionResueltos.Rows.Count - 1;
                        if (gvGestionResueltos.Rows.Count > 0)
                        {
                            txtBody.Text = gvGestionResueltos.Rows[contador].Cells[10].Text;
                        }

                        StringBuilder sb_ = new StringBuilder();
                        sb_.Append("<script type='text/javascript'>");
                        sb_.Append("$('#CORREO').modal('show');");
                        sb_.Append("</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb_.ToString(), false);
                    }

                    ESTADO();
                }


            }
            catch (Exception ex)
            {

                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        //##############################################################################################################

        protected void gvOrigen_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvOrigen.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvOrigen.Columns.Count - 1;

                if (KeyID == hfOrigen.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                }

            }


        }

        protected void gvEmisor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvEmisor.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                if (KeyID == hfEmisor.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.LightBlue;
                }

            }
        }

        protected void gvAsegurado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvAsegurado.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvAsegurado.Columns.Count - 1;

                if (KeyID == hfAsegurado.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.LightBlue;
                }

            }
        }

        protected void gvGeneral_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvGeneral.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvGeneral.Columns.Count - 1;

                if (KeyID == hfidDatos.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;

                }

            }
        }

        protected void gvDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvDocumentos.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvDocumentos.Columns.Count - 1;

                if (KeyID == hfidDocum.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;

                }

            }
        }

        protected void gvGestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvGestion.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvGestion.Columns.Count - 1;

                if (KeyID == hfGestion.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;

                }

            }
        }

        protected void gvSubGestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvSubGestion.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvSubGestion.Columns.Count - 1;

                if (KeyID == hfSubGestion.Value)
                {
                    hfOrdenSubGestion.Value = gvSubGestion.DataKeys[e.Row.RowIndex].Values["ORDEN"].ToString();
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;

                }

            }
        }

        protected void gvEstado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvEstado.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvEstado.Columns.Count - 1;

                if (KeyID == hfEstado.Value)
                {
                    //e.Row.Cells[0].BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;

                }

            }
        }

        //##############################################################################################################

        protected void lnkRegistrarFami_Click(object sender, EventArgs e)
        {
            divlblEmisor.Attributes.Add("style", "display:inline;");
            hfIdEmisor.Value = "9999999999";
            lblEmisor.Text = txtFamiliar.Text;
            txtContactoNombre.Text = txtFamiliar.Text;
            txtContactoNombreModal.Text = txtFamiliar.Text;
            hfDniEmisor.Value = txtDniContactoFamiliar.Text;

            txtContactoTelefono.Text = txtTelefonoContactoFamiliar.Text;
            txtContactoCorreo.Text = txtCorreoContactoFamiliar.Text;
            txtContactoDni.Text = txtDniContactoFamiliar.Text;
            hfDatosAdicionales.Value = "1";
        }

        protected void lnkRegistrarOtros_Click(object sender, EventArgs e)
        {
            if (hfTipoModalAfiliado.Value == "emisor")
            {
                divlblEmisor.Attributes.Add("style", "display:inline;");
                hfIdEmisor.Value = "9999999999";
                lblEmisor.Text = txtOtros.Text.Replace("\r\n", " ");
                hfDniEmisor.Value = txtDniContactoOtros.Text;
                txtContactoNombre.Text = txtOtros.Text;
                txtContactoNombreModal.Text = txtOtros.Text;

                txtContactoTelefono.Text = txtTelefonoContactoOtros.Text;
                txtContactoCorreo.Text = txtCorreoContactoOtros.Text;
                txtContactoDni.Text = txtDniContactoOtros.Text;
                hfDatosAdicionales.Value = "2";
            }
            else if (hfTipoModalAfiliado.Value == "asegurado")
            {
                divlblAsegurado.Attributes.Add("style", "display:inline;");
                hfIdAsegurado.Value = "9999999999";
                if (ddlOtrosCliente.SelectedValue != "00")
                {
                   hfIdAsegurado.Value = ddlOtrosCliente.SelectedValue.ToString()+"99999999"; 
                }
                
                hfCod_Cliente.Value = ddlOtrosCliente.SelectedValue;
                lblAsegurado.Text = txtOtros.Text.Replace("\r\n", " ");
                hfDniAsegurado.Value = txtDniContactoOtros.Text;
            }
        }

        protected void ddlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_afiliado();
        }

        protected void LnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionVoip.aspx");
        }

        void AseguradoPrincipal(string codtitula, string categoria, string cliente)
        {
            DataTable dt = dat.mysql("call sp_fill(15,'" + codtitula + "','" + categoria + "','" + cliente + "')");
            gvDatosAfiliado.DataSource = dt;
            gvDatosAfiliado.DataBind();
        }

        protected void gvProveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;this.style.backgroundColor='lightblue';");
                    //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ceedfc'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvProveedor, "Select$" + e.Row.RowIndex);
                    //e.Row.Attributes.Add("onclick", "location.href='Detalle.aspx?PER=" & e.Row.Cells(0).Text & "'");
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtContactoProveedor.Text == "" || txtDniProveedor.Text == "")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("alert('" + "Ingresar Contacto y Dni" + "');");
                sb.Append("$('#Proveedor').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
            else
            {
                string idClini = gvProveedor.Rows[gvProveedor.SelectedIndex].Cells[1].Text.ToString();
                string nomClini = gvProveedor.Rows[gvProveedor.SelectedIndex].Cells[2].Text.ToString();
                divlblEmisor.Attributes.Add("style", "display:inline;");
                lblEmisor.Text = nomClini + "  ||  " + txtContactoProveedor.Text.ToString().ToUpper();
                hfIdEmisor.Value = idClini;
                hfDniEmisor.Value = txtDniProveedor.Text;
                txtContactoNombre.Text = txtContactoProveedor.Text.ToString().ToUpper();
                txtContactoNombreModal.Text = txtContactoProveedor.Text.ToString().ToUpper();
                txtContactoDni.Text = txtDniProveedor.Text;

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#Proveedor').modal('hide');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

            }

        }

        protected void gvEjecutivo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvEjecutivo, "Select$" + e.Row.RowIndex);
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvEjecutivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdEjecutivo = gvEjecutivo.Rows[gvEjecutivo.SelectedIndex].Cells[1].Text.ToString();
            string NombreEjecutivo = gvEjecutivo.Rows[gvEjecutivo.SelectedIndex].Cells[2].Text.ToString();

            string xSQL = "CALL SP_GESTION_VOIP(9,'" + IdEjecutivo + "',0,0,0,0)";
            DataTable dt = dat.mysql(xSQL);
            if (dt.Rows.Count > 0)
            {
                txtEmailCliente.Text = dt.Rows[0][0].ToString();
            }

            if (hfDerivadoEjecutivo.Value == "1")
            {
                hfIdUsuarioDeriva.Value = IdEjecutivo;
                txtEjecutivo01.Text = NombreEjecutivo;
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#Ejecutivo').modal('hide');");
                sb.Append("$('#CORREO').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
            else
            {
                divlblEmisor.Attributes.Add("style", "display:inline;");
                hfIdEmisor.Value = IdEjecutivo;
                hfNombreEmisor.Value = NombreEjecutivo;
                lblEmisor.Text = NombreEjecutivo;
            }

        }

        protected void lnkBuscarAfiliado_Click(object sender, EventArgs e)
        {
            cargar_afiliado();
        }

        protected void btnBusquedaPro_Click(object sender, EventArgs e)
        {
            cargar_proveedor();
        }

        protected void lnkEjecutivo_Click(object sender, EventArgs e)
        {
            cargar_ejecutivo();
        }

        /*############################################################################################################################*/
        /*############################################################################################################################*/
        /*CARGAR LOS DATOS COMPLETOS DEL ASEGURADO SELECCIONADO (MODAL NUEVO AFILIADO DE WFMANTENIMIENTO)*/
        /*############################################################################################################################*/
        /*############################################################################################################################*/

        protected void lnkFoto_Click(object sender, EventArgs e)
        {
            if (txtCodigoTitu.Text != "")
            {
                subeFotos(txtNumeroPoli.Text, txtCodigoTitu.Text, ddlCategoria.SelectedValue);
            }
            else
            {
                lblError.Text = "Debe ingresar primero un Codigo de Titular.";
            }
        }

        void subeFotos(string cli, string titu, string cate)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    string ext = Path.GetExtension(file2.Value).ToString();
                    string pat = Server.MapPath("~/FOTOS/" + cli + "-" + titu + "-" + cate + ext).ToString();
                    string pat2 = Server.MapPath("~/FOTOS/");
                    HttpPostedFile psf = file2.PostedFile;
                    psf.SaveAs(pat);
                    ftp(psf, pat2, cli + "-" + titu + "-" + cate + ext, cli);
                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                }

            }

        }

        void ftp(HttpPostedFile file, string ruta_path, string archivo_nombre, string cliente)
        {
            //FTP Server URL.
            string ftp = "ftp://10.100.100.6/";

            //FTP Folder name. Leave blank if you want to upload to root folder.
            //string ftpFolder = "Uploads/";
            string ftpFolder = "r0/public_html/solben_net/production/www/solben/foto/" + cliente + "/";

            byte[] fileBytes = null;

            //Read the FileName and convert it to Byte array.
            string fileName = Path.GetFileName(archivo_nombre);
            using (StreamReader fileStream = new StreamReader(file.InputStream))
            {
                fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                fileStream.Close();
            }

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential("root", "wp@z0123qwe");
                request.ContentLength = fileBytes.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                request.ServicePoint.ConnectionLimit = fileBytes.Length;
                request.EnableSsl = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
            }
            catch (WebException ex)
            {
                lblError.Text = ex.Message.ToString();
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
            }
            else
            {
                id_paciente.Visible = false;
            }
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

                if (dt2.Rows.Count >= 1)
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

                    if (dt.Rows.Count >= 1)
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

        protected void lnkTraer_Click(object sender, EventArgs e)
        {
            //NO HACE NADA
        }

        protected void txtIdPaciente_TextChanged(object sender, EventArgs e)
        {
            if (txtIdPaciente.Text.Length < 2)
            {
                txtIdPaciente.Text = "0" + txtIdPaciente.Text;
            }
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

        protected void lnkCagarCartas_Click(object sender, EventArgs e)
        {
            loadIC(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
        }

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

        protected void lnkExportarIC_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (69,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','','')";
                dtab1 = dat.mysql(xSQL);
                string SSQL = "CALL SP_REPORTES_IAFA (70,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO"] + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','')";
                dtab2 = dat.mysql(SSQL);
                string YSQL = "CALL SP_REPORTES_IAFA (71,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO"] + "','" + ViewState["COD_MES"] + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','')";
                dtab3 = dat.mysql(YSQL);
            }
            catch (Exception ex)
            {
                lblNohayCarta.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3);
        }

        void descargaRecord(DataTable dt1, DataTable dt2, DataTable dt3)
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
                wst.Cells["A1:E1"].Value = "REPORTE";
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["F1:J1"].Merge = true;
                wst.Cells["F1:J1"].Style.Font.Bold = true;
                wst.Cells["F1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wst.Cells["F1:J1"].Value = "SEDE";
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["F1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["F1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["A4"].LoadFromDataTable(dt1, true);
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Merge = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Font.Bold = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2).ToString() + ""].Value = "CANTIDAD";
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1).ToString() + ""].LoadFromDataTable(dt2, true);
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Merge = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Font.Bold = true;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ":J" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2).ToString() + ""].Value = "CANTIDAD";
                wst.Cells["A" + (dt1.Rows.Count + 4 + 1 + 2 + 2 + 1 + dt2.Rows.Count + 2 + 2 + 1).ToString() + ""].LoadFromDataTable(dt3, true);

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=RECORD" + serie + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
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

        void cargarICAnio(string cliente, string anio, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (70,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','','')";
            DataTable dt = dat.mysql(SSQL);
            gvImpresionCartasMes.DataSource = dt;
            gvImpresionCartasMes.DataBind();

            ChartMesesIC();
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

        protected void PostgvICMes(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["COD_MES"] = gvImpresionCartasMes.DataKeys[row.RowIndex].Values["COD_MES"].ToString();
            ViewState["MES"] = gvImpresionCartasMes.DataKeys[row.RowIndex].Values["MES"].ToString();
            cargarICMes(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), ViewState["COD_MES"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
            lblDetalleIC.Text = "CARTAS " + ViewState["MES"].ToString() + " DEL AÑO " + ViewState["ANIO"].ToString();
        }

        void cargarICMes(string cliente, string anio, string mes, string titula, string categoria)
        {
            icdetalle.Visible = true;
            string SSQL = "CALL SP_REPORTES_IAFA (71,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + mes + "','" + ddlTipoOrdenAtencion.SelectedValue.ToString() + "','')";
            DataTable dt = dat.mysql(SSQL);
            gvImpresionCartasDetalle.DataSource = dt;
            gvImpresionCartasDetalle.DataBind();
        }

        protected void FullPostBack(object sender, EventArgs e)
        {

            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string solben_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["solben_id"].ToString();
            string atencion_id = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["atencion_id"].ToString();
            string cod_cliente = gvImpresionCartasDetalle.DataKeys[row.RowIndex].Values["cod_cliente"].ToString();

            switch (atencion_id)
            {
                case "1":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/impreso_m.php?nro=" + solben_id + "');", true);
                    break;
                case "2":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/impreso_d.php?nro=" + solben_id + "');", true);
                    break;
                case "4":
                case "6":
                    if ((cod_cliente == "90") || (cod_cliente == "96") || (cod_cliente == "98"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro=" + solben_id + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion.php?nro=" + solben_id + "');", true);
                    }
                    break;
                case "5":
                case "3":
                    if ((cod_cliente == "90") || (cod_cliente == "96") || (cod_cliente == "98") || (cod_cliente == "95"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/orden_atencion_p.php?nro=" + solben_id + "');", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/garantias/carta_garantia.php?nro=" + solben_id + "');", true);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void gvImpresionCartasDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImpresionCartasDetalle.PageIndex = e.NewPageIndex;
            cargarICMes(txtNumeroPoli.Text, ViewState["ANIO"].ToString(), ViewState["COD_MES"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void lnkCargarRecord_Click(object sender, EventArgs e)
        {
            loadDetail(txtNumeroPoli.Text, txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text);
        }

        //RECORD DE CONSUMO//
        //###########################################################
        void loadDetail(string cliente, string nombre)
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

                string xSQL = "CALL SP_REPORTES_IAFA (4,'" + cliente + "','','','','" + nombre + "','','')";
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

        protected void lnkExportarRecord_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (4,'" + txtNumeroPoli.Text + "','','','','" + txtApellidop.Text + ' ' + txtApellidom.Text + ", " + txtNombres.Text + "','','')";
                dtab1 = dat.mysql(xSQL);

                NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;

                string SSQL = "CALL SP_REPORTES_IAFA (6,'" + txtNumeroPoli.Text + "','','','','" + ViewState["ANIORC"].ToString() + "','" + NOMBRECOMPLETO + "','')";
                dtab2 = dat.mysql(SSQL);

                NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;

                string YSQL = "CALL SP_REPORTES_IAFA (13,'" + txtNumeroPoli.Text + "','','','','" + ViewState["ANIORC"].ToString() + "','" + ViewState["COD_MESRC"].ToString() + "','" + NOMBRECOMPLETO + "')";
                dtab3 = dat.mysql(YSQL);

            }
            catch (Exception ex)
            {
                lblNoHayRecord.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3);
        }

        protected void PartialPostgv1(object sender, EventArgs e)
        {
            RCanual.Visible = true;
            RCmensual.Visible = true;
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            ViewState["ANIORC"] = gvDatosDetalle1.DataKeys[row.RowIndex].Values["ANIO"].ToString();
            NOMBRECOMPLETO = txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text;
            cargaTablaDetalle2(txtNumeroPoli.Text, ViewState["ANIORC"].ToString(), NOMBRECOMPLETO);
        }

        void cargaTablaDetalle2(string cliente, string anio, string nombre)
        {
            lblDetaEvoIC.Visible = true;
            lblGraficaIC2.Visible = true;
            string SSQL = "CALL SP_REPORTES_IAFA (6,'" + cliente + "','','','','" + anio + "','" + nombre + "','')";
            DataTable dt = dat.mysql(SSQL);

            int i = 1;
            while (i <= dt.Rows.Count - 1)
            {
                dt.Rows[i][6] = Convert.ToString(Math.Round(Convert.ToDecimal(dt.Rows[i][4]) * 100 / Convert.ToDecimal(dt.Rows[0][4]), 2)) + "%";
                i = i + 1;
            }

            dt.Rows[0][5] = "";
            dt.Rows[0][6] = "100%";

            dt.DefaultView.Sort = "COD_MES ASC";
            gvDatosDetalle2.DataSource = dt;
            gvDatosDetalle2.DataBind();

            cargaMeses();
        }

        void cargaMeses()
        {
            try
            {
                string VAL = "";
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

        protected void PartialPostgv3(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            string NRO_INTER = gvDatosDetalle3.DataKeys[row.RowIndex].Values["NRO_INTER"].ToString();
            string NRO_PLANI = gvDatosDetalle3.DataKeys[row.RowIndex].Values["NRO_PLANI"].ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('http://www.solben.net/solben/detalle_liquidacion_report.php?nroin=" + NRO_INTER + "&p=" + NRO_PLANI + "');", true);
        }

        protected void gvDatosDetalle3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatosDetalle3.PageIndex = e.NewPageIndex;
            cargaTablaDetalle3(txtNumeroPoli.Text, ViewState["ANIORC"].ToString(), ViewState["COD_MESRC"].ToString(), txtApellidop.Text + " " + txtApellidom.Text + ", " + txtNombres.Text);
        }

        void avisos(string cliente, string titula, string categoria)
        {
            lblNohayAvisos.Text = "";
            DataTable dt = new DataTable();
            try
            {
                avisos123.Visible = true;
                string SSQLX = null;
                SSQLX = "call sp_avisos (1,'" + cliente + "','" + titula + "','" + categoria + "',0,0,0)";
                dt = dat.mysql(SSQLX);

                if (dt.Rows.Count == 0)
                {
                    avisos123.Visible = false;
                    lblNohayAvisos.Text = "No existen avisos registrados";
                }
                else
                {
                    gv_Avisos.DataSource = dt;
                    gv_Avisos.DataBind();
                }

            }
            catch (Exception ex)
            {
                avisos123.Visible = false;
                lblNohayAvisos.Text = "Ocurrió un error al cargar avisos " + ex.Message.ToString();
            }
        }

        protected void gv_Avisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Avisos.PageIndex = e.NewPageIndex;
            string SSQLX = "call sp_avisos (1,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "',0,0,0)";
            DataTable dt = dat.mysql(SSQLX);
            gv_Avisos.DataSource = dt;
            gv_Avisos.DataBind();
        }

        protected void lnkCargarInformes_Click(object sender, EventArgs e)
        {
            loadIM(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value);
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

        protected void lnkExportarInformesMedicos_Click(object sender, EventArgs e)
        {
            try
            {
                string xSQL = "CALL SP_REPORTES_IAFA (72,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','','','','')";
                dtab1 = dat.mysql(xSQL);
                string SSQL = "CALL SP_REPORTES_IAFA (73,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO1"].ToString() + "','','','')";
                dtab2 = dat.mysql(SSQL);
                string YSSQL = "CALL SP_REPORTES_IAFA (74,'" + txtNumeroPoli.Text + "','" + txtCodigoTitu.Text + "','" + categoriaHidden.Value + "','" + ViewState["ANIO1"].ToString() + "','" + ViewState["COD_MES1"].ToString() + "','','')";
                dtab3 = dat.mysql(YSSQL);

            }
            catch (Exception ex)
            {
                lblNoHayRecord.Text = ex.Message.ToString();
            }

            descargaRecord(dtab1, dtab2, dtab3);
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

        void cargarIMAnio(string cliente, string anio, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (73,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','','','')";
            DataTable dt = dat.mysql(SSQL);
            gvInformesMedicosMes.DataSource = dt;
            gvInformesMedicosMes.DataBind();

            ChartMesesIM();
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

        void cargarIMMes(string cliente, string anio, string mes, string titula, string categoria)
        {
            string SSQL = "CALL SP_REPORTES_IAFA (74,'" + cliente + "','" + titula + "','" + categoria + "','" + anio + "','" + mes + "','','')";
            DataTable dt = dat.mysql(SSQL);
            gvInformesMedicosDetalle.DataSource = dt;
            gvInformesMedicosDetalle.DataBind();
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

        protected void gvInformesMedicosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInformesMedicosDetalle.PageIndex = e.NewPageIndex;
            cargarIMMes(txtNumeroPoli.Text, ViewState["ANIO1"].ToString(), ViewState["COD_MES1"].ToString(), txtCodigoTitu.Text, categoriaHidden.Value);
        }

        protected void btnBuscarCG_Click(object sender, EventArgs e)
        {
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            movimientos(txtNumeroPoli.Text, txtCodigoTitu.Text, categoriaHidden.Value, ddlMes.SelectedValue.ToString(), ddlAnio.SelectedValue.ToString(), usu.ID.ToString());
        }

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
                }
            }
            catch (Exception ex)
            {
                movimientos123.Visible = false;
                lblNoHayMovimientos.Text = "Ocurrió un error al cargar los movimientos " + ex.Message.ToString();
            }


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

        protected void lnkFichaPersonal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('Ficha2.aspx?cc=" + txtNumeroPoli.Text + "&ct=" + txtCodigoTitu.Text + "&c=" + categoriaHidden.Value + "');", true);
            return;
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
                    result = false;
                }
            }
            return result;
        }

        protected void CombosCliente(string cliente)
        {

            //===========================================================================

            ddlPlan.DataSource = dat.mysql("call sp_fill(43,'" + cliente + "',0,0)");
            ddlPlan.DataValueField = "valor";
            ddlPlan.DataTextField = "descrip";
            ddlPlan.DataBind();

            //=======================================================================

            ddlCentro.DataSource = dat.mysql("call sp_fill(50,'" + cliente + "',0,0)");
            ddlCentro.DataValueField = "valor";
            ddlCentro.DataTextField = "descrip";
            ddlCentro.DataBind();
        }

        protected void Cargarcombos()
        {
            ddlTablas.DataSource = dat.mysql("call sp_fill(84,'" + Session["USUARIO"].ToString() + "',0,0)");
            ddlTablas.DataTextField = "descrip";
            ddlTablas.DataValueField = "valor";
            ddlTablas.DataBind();

            ddlOtrosCliente.DataSource = dat.mysql("call sp_fill(84,'" + Session["USUARIO"].ToString() + "',0,0)");
            ddlOtrosCliente.DataTextField = "descrip";
            ddlOtrosCliente.DataValueField = "valor";
            ddlOtrosCliente.DataBind();



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

            ddlParentescoAfi.DataSource = dat.mysql("CALL sp_fill(47,'','','');");
            ddlParentescoAfi.DataValueField = "valor";
            ddlParentescoAfi.DataTextField = "descrip";
            ddlParentescoAfi.DataBind();

            ddlSexoAfi.DataSource = dat.mysql("CALL sp_fill(49,'','','')");
            ddlSexoAfi.DataValueField = "valor";
            ddlSexoAfi.DataTextField = "descrip";
            ddlSexoAfi.DataBind();

            ddlEstadoPoliza.DataSource = dat.TSegSQL("sp_ver_14 24");
            ddlEstadoPoliza.DataValueField = "CODIGO";
            ddlEstadoPoliza.DataTextField = "DESCRIP";
            ddlEstadoPoliza.DataBind();

            DataTable tab = dat.TSegSQL("sp_ver 168");
            ddlEjecutivoSiniestro.DataSource = tab;
            ddlEjecutivoSiniestro.DataValueField = "codigo";
            ddlEjecutivoSiniestro.DataTextField = "descrip";
            ddlEjecutivoSiniestro.DataBind();

            xSQL = "sp_ver 12,4";
            dtab = dat.TSegSQL(xSQL);
            ddlTipoStro.DataSource = dtab;
            ddlTipoStro.DataTextField = "descrip";
            ddlTipoStro.DataValueField = "codigo";
            ddlTipoStro.DataBind();

            xSQL = "SP_VER_10 12";
            dtab = dat.TSegSQL(xSQL);
            ddlTipoStroServicio.DataSource = dtab;
            ddlTipoStroServicio.DataTextField = "descrip";
            ddlTipoStroServicio.DataValueField = "codigo";
            ddlTipoStroServicio.DataBind();

        }

        void avisosNuevaGestion(string cliente, string titula, string categoria)
        {
            DataTable dt = new DataTable();
            try
            {
                string SSQLX = "call sp_avisos (1,'" + cliente + "','" + titula + "','" + categoria + "',0,0,0)";
                dt = dat.mysql(SSQLX);
                if (dt.Rows.Count > 0)
                {
                    divAvisos.Visible = true;
                    gvAvisos.DataSource = dt;
                    gvAvisos.DataBind();
                }
                else
                {
                    divAvisos.Visible = false;
                }
            }
            catch (Exception ex)
            {
                divAvisos.Visible = false;
                lblNohayAvisos1.Text = "Ocurrió un error al cargar avisos " + ex.Message.ToString();
            }
        }

        void observacionesSusalud(string cliente, string codtitula, string categoria)
        {
            DataTable dt2 = new DataTable();
            try
            {

                string xSQL = "CALL T_TRAMA_SUSALUD(13,'" + cliente + "','" + codtitula + "','" + categoria + "','','','','','','');";
                dt2 = dat.mysql(xSQL);
                if (dt2.Rows.Count > 0)
                {
                    divObs.Visible = true;
                    gvOBSusalud.DataSource = dt2;
                    gvOBSusalud.DataBind();
                }
                else
                {
                    divObs.Visible = false;
                }

            }
            catch (Exception ex)
            {
                divObs.Visible = false;
                lblnoHayObs.Text = "Ocurrió un error al cargar avisos " + ex.Message.ToString();
            }
        }

        protected void lnkBuscarejecutivo_Click(object sender, EventArgs e)
        {
            hfDerivadoEjecutivo.Value = "1";
            txtEjecutivo01.Text = "";

            cargar_ejecutivo();
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#Ejecutivo').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void lnkEnviarCorreo_Click(object sender, EventArgs e)
        {
            divUsuDeriva.Attributes.Add("style", "display:initial;");
            lblUsuaDeriva.Text = txtEjecutivo01.Text;

            if (chkCorreo.Checked)
            {
                if (Page.IsPostBack)
                {
                    HttpFileCollection uploadFiles = Request.Files;
                    int i;
                    for (i = 0; i < uploadFiles.Count - 1; i++)
                    {
                        HttpPostedFile postedFile = uploadFiles[i];
                        System.IO.Stream instream = postedFile.InputStream;
                        byte[] fileData = new byte[postedFile.ContentLength];
                        instream.Read(fileData, 0, postedFile.ContentLength);

                        adjuntosCliente.Add(Server.MapPath("Adjuntos/" + i + postedFile.FileName));
                        postedFile.SaveAs(Server.MapPath("Adjuntos/" + i + postedFile.FileName));
                    }

                    bool correo = Util.EnvioCorreo("smtp.gmail.com", "info@laprotectorasalud.com.pe", "lpsalud.2015", txtEmailCliente.Text, txtAsunto.Text, txtBody.Text, adjuntosCliente);
                    txtRespuesta.Text = txtBody.Text;

                    if (correo == true)
                    {
                        Response.Write("<script>window.alert('Correo enviado con éxito');</script>");
                    }
                    else
                    {
                        divUsuDeriva.Attributes.Add("style", "display:none;");
                        lblUsuaDeriva.Text = "";
                        Response.Write("<script>window.alert('Ocurrió un error al enviar correo');</script>");
                    }
                }
            }
            else
            {
                Response.Write("<script>window.alert('Derivado correctamente');</script>");
            }

        }

        protected void btnGuardarModificar_Click(object sender, EventArgs e)
        {
            string xSQL = "";

            try
            {
                xSQL = "call T_TRAMA_SUSALUD(4,'" + txtCodigoClienteAfi.Text + "','" + Session["USUARIO"] + "','" + Session["USUARIO"] + "','" + txtDocumentoAfi.Text + "','" +
                                                    txtCodigoTitularAfi.Text + "','" + hfCategoria.Value + "','','','');";
                dat.mysql(xSQL);

                xSQL = "call T_TRAMA_SUSALUD(5,'','" + txtCodigoClienteAfi.Text + txtCodigoTitularAfi.Text + hfCategoria.Value + "','" + txtApellidoPaternoAfi.Text + "','" + txtApellidoMaternoAfi.Text + "','" + txtNombresAfi.Text + "','" +
                                                   txtFechaNacimientoAfi.Text + "','" + txtDocumentoAfi.Text + "','" + ddlSexoAfi.SelectedItem.Text.Substring(0, 1) + "','" +
                                                   txtApellidoPaternoAfi.Text + " " + txtApellidoMaternoAfi.Text + ", " + txtNombresAfi.Text + "');";
                dat.mysql(xSQL);

                AseguradoPrincipal(txtCodigoTitularAfi.Text, hfCategoria.Value, txtCodigoClienteAfi.Text);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void lnkEditarAsegurado_Click(object sender, EventArgs e)
        {

            DataTable dt = dat.mysql("call sp_fill(15,'" + hfCod_Titula.Value + "','" + hfCategoria.Value + "','" + hfCod_Cliente.Value + "')");
            txtCodigoClienteAfi.Text = dt.Rows[0][1].ToString();
            txtCodigoTitularAfi.Text = dt.Rows[0][2].ToString();

            if (Convert.ToInt32(dt.Rows[0][43]) > 4 && Convert.ToInt32(dt.Rows[0][43]) <= 26)
            {
                ddlParentescoAfi.SelectedValue = "04";
            }

            txtDocumentoAfi.Text = dt.Rows[0][11].ToString();
            txtNombresAfi.Text = dt.Rows[0][38].ToString();
            txtApellidoPaternoAfi.Text = dt.Rows[0][39].ToString();
            txtApellidoMaternoAfi.Text = dt.Rows[0][40].ToString();
            txtFechaNacimientoAfi.Text = dt.Rows[0][8].ToString();
            ddlSexoAfi.SelectedValue = dt.Rows[0][7].ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#AFILIADORESUMEN').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        [WebMethod]
        public static void RemoveFile(string fileName, string key)
        {
            List<HttpPostedFile> files = (List<HttpPostedFile>)HttpContext.Current.Cache[key];
            files.RemoveAll(f => f.FileName.ToLower().EndsWith(fileName.ToLower()));
        }

        protected void lnkCerrarGrupoFamiliar_Click(object sender, EventArgs e)
        {
            if (hfComodinCerrar.Value == "1")
            {
                hfComodinCerrar.Value = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#GRUPOFAMILIAR').modal('hide');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
            else
            {
                hfComodinCerrar.Value = "";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#GRUPOFAMILIAR').modal('hide');");
                sb.Append("$('#Afiliado').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }


        }

        protected void gvDatosAfiliado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VER")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string cod_cliente = Convert.ToString(this.gvDatosAfiliado.DataKeys[currentRowIndex]["cod_cliente2"]);
                string cod_titula = Convert.ToString(this.gvDatosAfiliado.DataKeys[currentRowIndex]["cod_titula"]);
                hfComodinCerrar.Value = "1";
                grupofamiliar(cod_titula, cod_cliente);
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#GRUPOFAMILIAR').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
        }

        protected void gvGrupoFamiliar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvGrupoFamiliar, "Select$" + e.Row.RowIndex);
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvGrupoFamiliar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cliente = gvGrupoFamiliar.DataKeys[gvGrupoFamiliar.SelectedIndex].Values["cod_cliente"].ToString();
            string codtitula = gvGrupoFamiliar.DataKeys[gvGrupoFamiliar.SelectedIndex].Values["cod_titula"].ToString();
            string categoria = gvGrupoFamiliar.DataKeys[gvGrupoFamiliar.SelectedIndex].Values["categoria"].ToString();
            string nomAfi = gvGrupoFamiliar.DataKeys[gvGrupoFamiliar.SelectedIndex].Values["afiliado"].ToString();
            string IDEMP = gvGrupoFamiliar.DataKeys[gvGrupoFamiliar.SelectedIndex].Values["IDEMPRESA"].ToString();

            lblAsegurado.Text = nomAfi;
            hfCod_Cliente.Value = cliente;
            hfIdAsegurado.Value = cliente;
            hfCod_Titula.Value = codtitula;
            hfCategoria.Value = categoria;
            hfidEmpresa.Value = IDEMP;


            divlblAsegurado.Attributes.Add("style", "display:inline;");
            lblAsegurado.Text = nomAfi;


            AseguradoPrincipal(codtitula, categoria, cliente);
            EDITAR(cliente, codtitula, categoria, categoria, IDEMP);

            DOCUMENTOS(cliente, Convert.ToInt32(hfidEmpresa.Value));
            GENERALASEGURADO(Convert.ToInt32(hfidEmpresa.Value));
        }

        protected void lnkAgregar_Click(object sender, EventArgs e)
        {
            CARGARDATAMODAL();
            txtRespuesta.Text = "";
            TxtObs.Text = "";
            lblErrorModal.Text = "";
            lnkGuardarResuelto.Visible = true;
            lnkModificarResuelto.Visible = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#RESUELTOS').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        /*DECLARACION DATATABLE CON COLUMNAS QUE ACEPTAN NULL*/

        DataTable dtResuelto = new DataTable
        {
            Columns = { 
                       new DataColumn("NRO", typeof(string)),
                       new DataColumn("ORIGEN", typeof(string)){ AllowDBNull = true },
                       new DataColumn("DESCRIPORIGEN", typeof(string)){ AllowDBNull = true },
                       new DataColumn("IDSALIENTE", typeof(string)){ AllowDBNull = true },
                       new DataColumn("DESCRIPSALIENTE", typeof(string)){ AllowDBNull = true },
                       new DataColumn("DESCRIP_CONTACTO", typeof(string)){ AllowDBNull = true },
                       new DataColumn("RESPUESTA", typeof(string)) { AllowDBNull = true },
                       new DataColumn("OBS", typeof(string)) { AllowDBNull = true },
                       new DataColumn("USU_REG", typeof(string)){ AllowDBNull = true },
                       new DataColumn("FEC_REG", typeof(string)){ AllowDBNull = true },
                       new DataColumn("ARCHIVO", typeof(string)){ AllowDBNull = true },
            }
        };
        DataRow dr = null;
        int rowindice = 0;

        public string Upload()
        {
            string Ruta = "";
            try
            {
                if (fuArchivoResuelto.FileName.Length != 0 || fuArchivoResuelto.PostedFile.ToString() != "")
                {

                    Boolean fileOK = false;
                    String newPath = HttpContext.Current.Server.MapPath("~/");

                    if (fuArchivoResuelto.HasFile)
                    {
                        try
                        {
                            if (!(Directory.Exists(newPath)))
                            {
                                Directory.CreateDirectory(newPath);
                            }
                            if (Directory.Exists(newPath))
                            {
                                string vl_strfilename = fuArchivoResuelto.FileName;
                                Ruta = "Archivo\\" + vl_strfilename;
                                fuArchivoResuelto.PostedFile.SaveAs(newPath + Ruta);

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }
            catch (Exception error)
            {

            }
            return Ruta.Replace("\\", "/");
        }

        protected void lnkGuardarResuelto_Click(object sender, EventArgs e)
        {

            if (hfOrigenModal.Value == "" || hfOrigenDescripModal.Value == "" || hfEmisorModal.Value == "" || hfEmisorDescripModal.Value == "" ||
                txtContactoNombreModal.Text == "" || txtRespuesta.Text == "")
            {
                lblErrorModal.Text = "Debe ingresar los campos obligatorios";
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#RESUELTOS').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
                return;
            }
            else
            {
                if (hfIdVoip.Value == "" || hfIdVoip.Value == "0")
                {
                    lblErrorModal.Text = "";
                    if (Session["RESUELTOS"] == null)
                    {
                        dr = dtResuelto.NewRow();
                        dr["NRO"] = 0;
                        dr["ORIGEN"] = int.Parse(hfOrigenModal.Value);
                        dr["DESCRIPORIGEN"] = hfOrigenDescripModal.Value;
                        dr["IDSALIENTE"] = int.Parse(hfEmisorModal.Value);
                        dr["DESCRIPSALIENTE"] = hfEmisorDescripModal.Value;
                        dr["DESCRIP_CONTACTO"] = txtContactoNombreModal.Text;
                        dr["RESPUESTA"] = txtRespuesta.Text;
                        dr["OBS"] = TxtObs.Text;
                        dr["USU_REG"] = Session["USUARIO"];
                        dr["FEC_REG"] = DateTime.Today.ToShortDateString();
                        dr["ARCHIVO"] = Upload();
                        dtResuelto.Rows.Add(dr);
                        Session["RESUELTOS"] = dtResuelto;
                        if (txtRespuestaGeneral.Text != "")
                        {
                            txtRespuestaGeneral.Text += System.Environment.NewLine + DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                        }
                        else
                        {
                            txtRespuestaGeneral.Text += DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                        }
                        if (dtResuelto.Rows.Count == 0)
                        {
                            divlblRegistro.Attributes.Add("style", "display:none;");
                        }
                        else
                        {
                            divlblRegistro.Attributes.Add("style", "display:initial;");
                        }
                        gvGestionResueltos.DataSource = dtResuelto;
                        gvGestionResueltos.DataBind();
                    }
                    else
                    {
                        DataTable dtCurrentTable = (DataTable)Session["RESUELTOS"];
                        DataRow drCurrentRow = null;
                        if (dtCurrentTable.Rows.Count > 0)
                        {
                            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                            {
                                drCurrentRow = dtCurrentTable.NewRow();
                                drCurrentRow["NRO"] = i;
                                drCurrentRow["ORIGEN"] = hfOrigenModal.Value;
                                drCurrentRow["DESCRIPORIGEN"] = hfOrigenDescripModal.Value;
                                drCurrentRow["IDSALIENTE"] = int.Parse(hfEmisorModal.Value);
                                drCurrentRow["DESCRIPSALIENTE"] = hfEmisorDescripModal.Value;
                                drCurrentRow["DESCRIP_CONTACTO"] = txtContactoNombreModal.Text;
                                drCurrentRow["RESPUESTA"] = txtRespuesta.Text;
                                drCurrentRow["OBS"] = TxtObs.Text;
                                drCurrentRow["USU_REG"] = Session["USUARIO"];
                                drCurrentRow["FEC_REG"] = DateTime.Today.ToShortDateString();
                                drCurrentRow["ARCHIVO"] = Upload();
                                rowindice++;
                            }
                            if (txtRespuestaGeneral.Text != "")
                            {
                                txtRespuestaGeneral.Text += System.Environment.NewLine + DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                            }
                            else
                            {
                                txtRespuestaGeneral.Text += DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                            }
                            dtCurrentTable.Rows.Add(drCurrentRow);
                            Session["currentRESUELTOS"] = dtCurrentTable;
                            if (dtCurrentTable.Rows.Count == 0)
                            {
                                divlblRegistro.Attributes.Add("style", "display:none;");
                            }
                            else
                            {
                                divlblRegistro.Attributes.Add("style", "display:initial;");
                            }
                            gvGestionResueltos.DataSource = dtCurrentTable;
                            gvGestionResueltos.DataBind();
                        }
                    }
                }
                else
                {
                    string ruta = Upload();
                    string QueryResuelto = "call SP_MANTE_GESTION(3,'" + hfOrigenModal.Value + "','" + hfEmisorModal.Value + "','" + txtContactoNombreModal.Text + "','" + txtRespuesta.Text + "','" +
                                  TxtObs.Text + "','" + Session["USUARIO"] + "','" + hfIdVoip.Value + "','','" + ruta + "','','','','','','','','','','','','','','','','','','','','','');";
                    dat.mysql(QueryResuelto);
                    if (txtRespuestaGeneral.Text != "")
                    {
                        txtRespuestaGeneral.Text += System.Environment.NewLine + DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                    }
                    else
                    {
                        txtRespuestaGeneral.Text += DateTime.Today.ToShortDateString() + " - " + txtRespuesta.Text;
                    }
                    CARGAR_RESUELTOS(hfIdVoip.Value);
                }
            }
        }

        void CARGAR_RESUELTOS(string idvoip)
        {
            //LISTAR GESTION RESUELTOS POR VOIP
            string gresul = "CALL SP_GESTION_VOIP(13,'" + idvoip + "','','','','');";
            DataTable dtabgr = dat.mysql(gresul);
            gvGestionResueltos2.DataSource = dtabgr;
            gvGestionResueltos2.DataBind();
            if (dtabgr.Rows.Count == 0)
            {
                divlblRegistro.Attributes.Add("style", "display:none;");
            }

            else
            {
                divlblRegistro.Attributes.Add("style", "display:initial;");
            }

        }

        protected void gvOrigenModal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string KeyID = gvOrigenModal.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvOrigenModal.Columns.Count - 1;
                if (KeyID == hfOrigenModal.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }

        protected void gvOrigenModal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            try
            {
                if (e.CommandName == "Seleccionar")
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    string descrip = gvOrigenModal.DataKeys[currentRowIndex]["DESCRIP"].ToString();
                    idLlamada = Convert.ToString(this.gvOrigenModal.DataKeys[currentRowIndex]["ID"]);
                    hfOrigenModal.Value = idLlamada;
                    hfOrigenDescripModal.Value = descrip;
                    ORIGENMODAL();
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvEmisorModal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string KeyID = gvEmisorModal.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvEmisorModal.Columns.Count - 1;
                if (KeyID == hfEmisorModal.Value)
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }

        protected void gvEmisorModal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OCULTAR_MSJ();
            if (e.CommandName == "Seleccionar")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string TIPEMI = Convert.ToString(this.gvEmisorModal.DataKeys[currentRowIndex]["ID"]);
                string DESCRIP = Convert.ToString(this.gvEmisorModal.DataKeys[currentRowIndex]["DESCRIP"]);

                hfEmisorModal.Value = TIPEMI;
                hfEmisorDescripModal.Value = DESCRIP;
                EMISORMODAL();
                hfTipoModalAfiliado.Value = "EMISOR";
            }
        }

        protected void gvGestionResueltos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt01;

            if (Session["currentRESUELTOS"] == null)
            {
                dt01 = (DataTable)Session["RESUELTOS"];
            }
            else
            {
                dt01 = (DataTable)Session["currentRESUELTOS"];
            }

            hfModificatemp.Value = "";

            if (e.CommandName == "ARCHIVO")
            {
                lnkModificarResuelto.Visible = true;
                lnkGuardarResuelto.Visible = false;

                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string ARCHIVO = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["ARCHIVO"]);


                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#ifArchivo').attr('src', '" + ARCHIVO + "');");
                sb.Append("$('#mdlVisualizador').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }

            if (e.CommandName == "ACTUALIZAR")
            {
                lnkModificarResuelto.Visible = true;
                lnkGuardarResuelto.Visible = false;

                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                int NRO = Convert.ToInt32(this.gvGestionResueltos.DataKeys[currentRowIndex]["NRO"]);
                string ORIGEN = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["ORIGEN"]);
                string DESCRIPORIGEN = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["DESCRIPORIGEN"]);
                string IDSALIENTE = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["IDSALIENTE"]);
                string DESCRIPSALIENTE = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["DESCRIPSALIENTE"]);
                string DESCRIP_CONTACTO = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["DESCRIP_CONTACTO"]);
                string RESPUESTA = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["RESPUESTA"]);
                string OBS = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["OBS"]);
                int USU_REG = Convert.ToInt32(this.gvGestionResueltos.DataKeys[currentRowIndex]["USU_REG"]);
                string Archivo = Convert.ToString(this.gvGestionResueltos.DataKeys[currentRowIndex]["ARCHIVO"]);

                hfModificatemp.Value = Convert.ToString(NRO);
                hfOrigenModal.Value = ORIGEN;
                hfEmisorModal.Value = IDSALIENTE;

                CARGARDATAMODAL();

                txtContactoNombreModal.Text = DESCRIP_CONTACTO;
                txtRespuesta.Text = RESPUESTA;
                TxtObs.Text = OBS;

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#RESUELTOS').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }

            if (e.CommandName == "ELIMINAR")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                int NRO = Convert.ToInt32(this.gvGestionResueltos.DataKeys[currentRowIndex]["NRO"]);

                dt01.Rows[currentRowIndex].Delete();
                Session["currentRESUELTOS"] = dt01;

                if (dt01.Rows.Count == 0)
                {
                    Session["RESUELTOS"] = null;
                    Session["currentRESUELTOS"] = null; divlblRegistro.Attributes.Add("style", "display:none;");
                }

                else
                {
                    divlblRegistro.Attributes.Add("style", "display:initial;");
                }
                gvGestionResueltos.DataSource = dt01;
                gvGestionResueltos.DataBind();
            }

        }

        protected void lnkModificarResuelto_Click(object sender, EventArgs e)
        {
            if (hfIdVoip.Value == "" || hfIdVoip.Value == "0")
            {

                DataTable dt01 = (DataTable)Session["currentRESUELTOS"];

                DataRow[] rows = dt01.Select("NRO =" + hfModificatemp.Value);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        row["ORIGEN"] = hfOrigenModal.Value;
                        row["DESCRIPORIGEN"] = hfOrigenDescripModal.Value;
                        row["IDSALIENTE"] = int.Parse(hfEmisorModal.Value);
                        row["DESCRIPSALIENTE"] = hfEmisorDescripModal.Value;
                        row["DESCRIP_CONTACTO"] = txtContactoNombreModal.Text;
                        row["RESPUESTA"] = txtRespuesta.Text;
                        row["OBS"] = TxtObs.Text;
                        row["USU_REG"] = Session["USUARIO"];
                        row["ARCHIVO"] = Upload();

                        dt01.AcceptChanges();
                        row.SetModified();
                    }
                }

                Session["currentRESUELTOS"] = dt01;
                if (dt01.Rows.Count == 0)
                {
                    divlblRegistro.Attributes.Add("style", "display:none;");
                }
                else
                {
                    divlblRegistro.Attributes.Add("style", "display:initial;");
                }
                gvGestionResueltos.DataSource = dt01;
                gvGestionResueltos.DataBind();

            }
            else
            {
                string archivo = Upload();
                string xSQL = "CALL SP_MANTE_GESTION(4,'" + hfModificatemp.Value + "','" + hfOrigenModal.Value + "','" + hfEmisorModal.Value + "','" + txtContactoNombreModal.Text + "'," +
                                                        "'" + txtRespuesta.Text + "','" + TxtObs.Text + "','" + Session["USUARIO"] + "','" + archivo + "'," +
                                                        "'','','',''," +
                                                        "'','','','','','','','','','','','','','','','','','')";
                DataTable dt = dat.mysql(xSQL);
                CARGAR_RESUELTOS(hfIdVoip.Value);
            }


        }

        protected void gvGestionResueltos2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ARCHIVO")
            {
                lnkModificarResuelto.Visible = true;
                lnkGuardarResuelto.Visible = false;

                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string ARCHIVO = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["ARCHIVO"]);


                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#ifArchivo').attr('src', '" + ARCHIVO + "');");
                sb.Append("$('#mdlVisualizador').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }



            if (e.CommandName == "ACTUALIZAR")
            {
                lnkModificarResuelto.Visible = true;
                lnkGuardarResuelto.Visible = false;

                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                int NRO = Convert.ToInt32(this.gvGestionResueltos2.DataKeys[currentRowIndex]["NRO"]);
                string ORIGEN = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["ORIGEN"]);
                string DESCRIPORIGEN = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["DESCRIPORIGEN"]);
                string IDSALIENTE = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["IDSALIENTE"]);
                string DESCRIPSALIENTE = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["DESCRIPSALIENTE"]);
                string DESCRIP_CONTACTO = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["DESCRIP_CONTACTO"]);
                string RESPUESTA = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["RESPUESTA"]);
                string OBS = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["OBS"]);
                int USU_REG = Convert.ToInt32(this.gvGestionResueltos2.DataKeys[currentRowIndex]["USU_REG_ID"]);

                hfModificatemp.Value = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["ID"]);
                hfOrigenModal.Value = ORIGEN;
                hfEmisorModal.Value = IDSALIENTE;

                CARGARDATAMODAL();

                txtContactoNombreModal.Text = DESCRIP_CONTACTO;
                txtRespuesta.Text = RESPUESTA;
                TxtObs.Text = OBS;

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#RESUELTOS').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }

            if (e.CommandName == "ELIMINAR")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string ID = Convert.ToString(this.gvGestionResueltos2.DataKeys[currentRowIndex]["ID"]);
                string xSQL = "CALL SP_MANTE_GESTION(5,'" + ID + "','','',''," +
                                                    "'','','',''," +
                                                    "'','','',''," +
                                                    "'','','','','','','','','','','','','','','','','','')";
                DataTable dt = dat.mysql(xSQL);
                CARGAR_RESUELTOS(hfIdVoip.Value);
            }
        }

        protected void gvAfiliado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAfiliado.PageIndex = e.NewPageIndex;
            cargar_afiliado();
        }

        void PolizasClientes(string cliente)
        {
            lblPolizasCliente.Text = "";

            //POLIZAS
            string gresul = "sp_ver_polizas 1,'1','" + cliente + "','','" + txtBuscadorPoliza.Text + "','2','" + ddlEstadoPoliza.SelectedValue + "'";
            DataTable dtabgr = dat.TSegSQL(gresul);

            if (dtabgr.Rows.Count > 0)
            {
                if (dtabgr.Rows[0][0].ToString() != "0")
                {
                    gvPolizasCliente.DataSource = dtabgr;
                    gvPolizasCliente.DataBind();
                    lblPolizasCliente.Text = "";
                }
                else
                {
                    gvPolizasCliente.DataSource = null;
                    gvPolizasCliente.DataBind();
                    lblPolizasCliente.Text = dtabgr.Rows[0][1].ToString();
                }
            }
            else
            {
                gvPolizasCliente.DataSource = null;
                gvPolizasCliente.DataBind();
                lblPolizasCliente.Text = "No existen pólizas registrados para este cliente.";
            }
        }

        void DocumentosClientes(string cliente)
        {
            lblDocumentosCliente.Text = "";

            //DOCUMENTOS
            string gresul = "sp_ver_14 22,'" + cliente + "','" + txtDocumentosCliente.Text + "'";
            DataTable dtabgr = dat.TSegSQL(gresul);

            if (dtabgr.Rows.Count > 0)
            {
                gvDocumentosCliente.DataSource = dtabgr;
                gvDocumentosCliente.DataBind();
                lblDocumentosCliente.Text = "";
            }
            else
            {
                gvDocumentosCliente.DataSource = null;
                gvDocumentosCliente.DataBind();
                lblDocumentosCliente.Text = "No existen documentos registrados para este cliente.";
            }
        }

        protected void gvDocumentosCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocumentosCliente.PageIndex = e.NewPageIndex;
            DocumentosClientes(hfCod_Cliente.Value);
        }

        protected void gvDocumentosCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VER")
            {
                try
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                    string ext = Convert.ToString(this.gvDocumentosCliente.DataKeys[currentRowIndex]["ext"]);
                    string ruta = Convert.ToString(this.gvDocumentosCliente.DataKeys[currentRowIndex]["ruta"]);

                    DataTable dt = dat.TSegSQL("sp_manuales 1");

                    string ftp = dt.Rows[0][5].ToString();

                    string RUTA3 = ftp + ruta + ext;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + RUTA3 + "');", true);

                }
                catch (Exception ex)
                {
                    lblErrorReg.Text = ex.Message.ToString();
                    orror.Visible = true;
                }
            }
        }

        protected void gvPolizasCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detalle")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                string id_cli = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["clientecontratante"]);
                string idpoliza = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["idpoliza"]);
                string nropoliza = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["NroPoliza"]);
                string estado = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["Estado"]);

                lblPolizaReg.Text = "";
                DETALLE(idpoliza);
                ASEGURADOS(idpoliza, txtAsegurados.Text);
                VEHICULOSASEGURADOS(idpoliza, txtVehiculos.Text);
                INMUEBLES(idpoliza, txtFiltroInmueble.Text);
                CARGAR_SINIESTROS(id_cli, idpoliza);
                ENDOSOS(idpoliza, txtEndosos.Text);
                DOCUMENTOSFOTOS_RC(idpoliza, txtDocumentosRC.Text);
                SEGUIMIENTO_RC(idpoliza, txtSeguimientoRC.Text);

                foreach (GridViewRow i in gvPolizasCliente.Rows)
                {
                    i.BackColor = Color.White;
                }

                GridViewRow row = gvPolizasCliente.Rows[currentRowIndex];
                row.BackColor = System.Drawing.Color.LightBlue;
            }

            if (e.CommandName == "Seleccionar")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                string id_cli = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["clientecontratante"]);
                string idpoliza = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["idpoliza"]);
                string nropoliza = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["NroPoliza"]);
                string estado = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["Estado"]);

                string aseguradora = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["aseguradora"]);
                string riesgo = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["Riesgo"]);
                string vigencia = Convert.ToString(this.gvPolizasCliente.DataKeys[currentRowIndex]["FECVIG"]);

                txtRespuestaGeneral.Text = "PÓLIZA SELECCIONADA:" + nropoliza + "|| ASEGURADORA: " + aseguradora + "|| RIESGO: " + riesgo + " ||VIGENCIA: " + vigencia;

                hfpoli.Value = idpoliza;
                hfNroPoliza.Value = nropoliza;
                hfidPoliza.Value = hfpoli.Value;

                GESTION(ddlTablas.SelectedValue);

                AseguradoBienAsegurado(hfpoli.Value);

                foreach (GridViewRow i in gvPolizasCliente.Rows)
                {
                    i.BackColor = Color.White;
                }

                GridViewRow row = gvPolizasCliente.Rows[currentRowIndex];
                row.BackColor = System.Drawing.Color.LightBlue;

                lblPolizaReg.Text = "PÓLIZA SELECCIONADA:" + nropoliza + "|| ASEGURADORA: " + aseguradora + "|| RIESGO: " + riesgo + " ||VIGENCIA: " + vigencia;

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#PolizasCliente').modal('hide');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

            }

            if (e.CommandName == "PDF")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                lblPolizaReg.Text = "";
                string RUTA = gvPolizasCliente.DataKeys[currentRowIndex].Values["RUTA"].ToString();

                DataTable dt = dat.TSegSQL("sp_manuales 1");

                string ftp = dt.Rows[0][5].ToString();
                string RUTA2 = ftp + RUTA;

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("window.open('" + RUTA2 + "',1,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=700,left=10,top=10');");
                sb.Append("</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", sb.ToString(), false);
            }

        }

        protected void gvPolizasCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPolizasCliente.PageIndex = e.NewPageIndex;
            PolizasClientes(hfCod_Cliente.Value);
        }

        void ClientePrincipal(string cliente)
        {
            DataTable dt = dat.TSegSQL("sp_ver_14 20,'" + cliente + "'");
            gvClientePrincipal.DataSource = dt;
            gvClientePrincipal.DataBind();

            gvDatosAfiliado.DataSource = null;
            gvDatosAfiliado.DataBind();
        }

        protected void gvClientePrincipal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VER")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                string cod_cliente = Convert.ToString(this.gvClientePrincipal.DataKeys[currentRowIndex]["cod_cliente"]);

                PolizasClientes(cod_cliente);

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#PolizasCliente').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
        }

        void DETALLE(string idpoli)
        {
            string TIPORIESGO = "sp_ver 69,'" + idpoli + "'";
            DataTable dtab1 = dat.TSegSQL(TIPORIESGO);

            if (dtab1.Rows[0][0].ToString() == "1")
            {
                if (dtab1.Rows[0][1].ToString() == "4")
                {
                    aseguradosgrilla.Visible = false;
                    vehiculogrilla.Visible = false;
                    inmueblegrilla.Visible = true;
                }
                else
                {
                    aseguradosgrilla.Visible = false;
                    vehiculogrilla.Visible = false;
                    inmueblegrilla.Visible = false;
                }
            }
            else if (dtab1.Rows[0][0].ToString() == "2")
            {
                aseguradosgrilla.Visible = true;
                vehiculogrilla.Visible = false;
                inmueblegrilla.Visible = false;
            }
            else if (dtab1.Rows[0][0].ToString() == "3")
            {
                aseguradosgrilla.Visible = false;
                vehiculogrilla.Visible = true;
                inmueblegrilla.Visible = false;
            }
            else
            {
                aseguradosgrilla.Visible = false;
                vehiculogrilla.Visible = false;
                inmueblegrilla.Visible = false;
            }

            //DataTable data = (DataTable)Session["DATOS"]; Mario dijo perfil 1 porque en este caso se ve todo

            string DETALLE = "SP_DETALLE_CONSULTAS 2," + idpoli + ",'1'";
            DataTable dtab = dat.TSegSQL(DETALLE);
            gvDetallePoliza.DataSource = dtab;
            gvDetallePoliza.DataBind();
            gvDetallePoliza.Columns[0].ItemStyle.CssClass = "plomo";
            gvDetallePoliza.Columns[2].ItemStyle.CssClass = "plomo";
            gvDetallePoliza.Columns[4].ItemStyle.CssClass = "plomo";
            gvDetallePoliza.Columns[6].ItemStyle.CssClass = "plomo";
            detapoli.Visible = true;
        }

        void ASEGURADOS(string id, string busqueda)
        {
            string ASEGURADOS = "sp_ver 55,'" + id + "', '" + busqueda + "'";
            DataTable dtab = dat.TSegSQL(ASEGURADOS);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvAsegurados.DataSource = dtab;
                gvAsegurados.DataBind();
                lblAseguradoPolizas.Text = "";
            }
            else
            {
                gvAsegurados.DataSource = null;
                gvAsegurados.DataBind();
                lblAseguradoPolizas.Text = dtab.Rows[0][1].ToString();
            }

        }

        void VEHICULOSASEGURADOS(string id, string busqueda)
        {
            string VEHICULOS = "sp_ver 64,'" + id + "', '" + busqueda + "'";
            DataTable dtab = dat.TSegSQL(VEHICULOS);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gv_Vehiculos.DataSource = dtab;
                gv_Vehiculos.DataBind();
                lblVehiculosPoliza.Text = "";
            }
            else
            {
                gv_Vehiculos.DataSource = null;
                gv_Vehiculos.DataBind();
                lblVehiculosPoliza.Text = dtab.Rows[0][1].ToString();
            }

        }

        void INMUEBLES(string id, string busqueda)
        {
            string INMUEBLE = "sp_ver_12 9,'" + id + "', '" + busqueda + "'";
            DataTable dtab = dat.TSegSQL(INMUEBLE);

            if (dtab.Rows.Count > 0)
            {
                gvInmuebles.DataSource = dtab;
                gvInmuebles.DataBind();
                lblInmuebleGrilla.Text = "";
            }
            else
            {
                gvInmuebles.DataSource = null;
                gvInmuebles.DataBind();
                lblInmuebleGrilla.Text = "No existen registros";
            }

        }

        public void CARGAR_SINIESTROS(string id_cli, string idpoliza)
        {
            string xSQL = "sp_ver 13,'" + id_cli + "','" + idpoliza + "','','','1'";
            DataTable dtab = dat.TSegSQL(xSQL);
            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvSiniestros.DataSource = dtab;
                gvSiniestros.DataBind();
                lblSiniestroPo.Text = "";
            }
            else
            {
                gvSiniestros.DataSource = null;
                gvSiniestros.DataBind();
                lblSiniestroPo.Text = dtab.Rows[0][1].ToString();
            }
        }

        void ENDOSOS(string id, string busqueda)
        {

            DataTable dt = dat.TSegSQL("sp_ver 48,'" + Session["USUARIO"] + "'");

            string ENDOSOS = "sp_ver 47,'" + id + "','" + busqueda + "','" + dt.Rows[0][3].ToString() + "'";
            DataTable dtab = dat.TSegSQL(ENDOSOS);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gv_Endosos.DataSource = dtab;
                gv_Endosos.DataBind();
                lbl_Endosos.Text = "";

                for (int n = 0; n < gv_Endosos.Columns.Count; n++) { gv_Endosos.Columns[n].ItemStyle.Width = 10; }


                if (dt.Rows[0][3].ToString() == "3")
                {
                    gv_Endosos.Columns[12].Visible = false;
                    gv_Endosos.Columns[13].Visible = false;
                }
            }
            else
            {
                gv_Endosos.DataSource = null;
                gv_Endosos.DataBind();
                lbl_Endosos.Text = dtab.Rows[0][1].ToString();
            }
        }

        void DOCUMENTOSFOTOS_RC(string id, string buscador)
        {
            DataTable dt = dat.TSegSQL("sp_ver 48,'" + Session["USUARIO"] + "'");

            string DOCUMENTOSFOTOS = "sp_ver 29,'2','" + id + "','" + buscador + "','','" + dt.Rows[0][3].ToString() + "'";
            DataTable dtab = dat.TSegSQL(DOCUMENTOSFOTOS);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gv_Documentos.DataSource = dtab;
                gv_Documentos.DataBind();
                lbl_Documentos.Text = "";
            }
            else
            {
                gv_Documentos.DataSource = null;
                gv_Documentos.DataBind();
                lbl_Documentos.Text = dtab.Rows[0][1].ToString();
            }
        }

        void SEGUIMIENTO_RC(string id, string buscador)
        {
            string SEGUIMIENTO = "sp_ver 31,'2','" + id + "','" + buscador + "'";
            DataTable dtab = dat.TSegSQL(SEGUIMIENTO);


            if (dtab.Rows[0][0].ToString() != "0")
            {
                gv_seguimiento.DataSource = dtab;
                gv_seguimiento.DataBind();
                lbl_Seguimiento.Text = "";
            }
            else
            {
                gv_seguimiento.DataSource = null;
                gv_seguimiento.DataBind();
                lbl_Seguimiento.Text = dtab.Rows[0][1].ToString();
            }
        }

        protected void lnkAseg_Click(object sender, EventArgs e)
        {
            ASEGURADOS(hfpoli.Value, txtAsegurados.Text);
        }

        protected void gvAsegurados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAsegurados.PageIndex = e.NewPageIndex;
            ASEGURADOS(hfpoli.Value, txtAsegurados.Text);
        }

        protected void lnkVehAseg_Click(object sender, EventArgs e)
        {
            VEHICULOSASEGURADOS(hfpoli.Value, txtVehiculos.Text);
        }

        protected void gv_Vehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Vehiculos.PageIndex = e.NewPageIndex;
            VEHICULOSASEGURADOS(hfpoli.Value, txtVehiculos.Text);
        }

        protected void lnkBuscarInmueble_Click(object sender, EventArgs e)
        {
            INMUEBLES(hfpoli.Value, txtFiltroInmueble.Text);
        }

        protected void gvInmuebles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void lnkEndosos_Click(object sender, EventArgs e)
        {
            ENDOSOS(hfpoli.Value, txtEndosos.Text);
        }

        protected void gv_Endosos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Endosos.PageIndex = e.NewPageIndex;
            ENDOSOS(hfpoli.Value, txtEndosos.Text);
        }

        protected void gvSiniestros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VER")
            {

                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                string cod_cli = Convert.ToString(this.gvSiniestros.DataKeys[currentRowIndex]["ClienteContratante"]);
                string cod_sini = Convert.ToString(this.gvSiniestros.DataKeys[currentRowIndex]["Siniestro"]);
                string cod_poli = Convert.ToString(this.gvSiniestros.DataKeys[currentRowIndex]["idpoliza"]);

                Response.Redirect("GestionSiniestros.aspx?id=" + cod_cli + "&idpoli=" + cod_poli + "&idsin=" + cod_sini);

            }
        }

        protected void gvSiniestros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSiniestros.PageIndex = e.NewPageIndex;
            CARGAR_SINIESTROS(hfCod_Cliente.Value, hfpoli.Value);
        }

        protected void lnkCoaseguro_Click(object sender, EventArgs e)
        {
            COASEGURO(hfpoli.Value, txtCoaseguro.Text);
        }

        protected void gvCoaseguro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoaseguro.PageIndex = e.NewPageIndex;
            COASEGURO(hfpoli.Value, txtCoaseguro.Text);
        }

        protected void lnkCobertura_Click(object sender, EventArgs e)
        {
            COBERTURA(Request.QueryString["idpoli"].ToString(), txtCobertura.Text);
        }

        protected void gvCobertura_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCobertura.PageIndex = e.NewPageIndex;
            COBERTURA(Request.QueryString["idpoli"].ToString(), txtCobertura.Text);
        }

        protected void lnk_Seguimiento_Click(object sender, EventArgs e)
        {
            SEGUIMIENTO_RC(hfpoli.Value, txtSeguimientoRC.Text);
        }

        protected void gv_seguimiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_seguimiento.PageIndex = e.NewPageIndex;
            SEGUIMIENTO_RC(hfpoli.Value, txtSeguimientoRC.Text);
        }

        protected void lnk_Documentos_Click(object sender, EventArgs e)
        {
            DOCUMENTOSFOTOS_RC(hfpoli.Value, txtDocumentosRC.Text);
        }

        protected void gv_Documentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Documentos.PageIndex = e.NewPageIndex;
            DOCUMENTOSFOTOS_RC(hfpoli.Value, txtDocumentosRC.Text);
        }

        protected void gv_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VER")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                string RUTA = gv_Documentos.DataKeys[currentRowIndex].Values["ruta"].ToString();
                string NOM = gv_Documentos.DataKeys[currentRowIndex].Values["nombre_archivo"].ToString();
                string EXT = gv_Documentos.DataKeys[currentRowIndex].Values["ext"].ToString();
                string tipo = gv_Documentos.DataKeys[currentRowIndex].Values["tipo"].ToString();

                string IDEST = gv_Documentos.DataKeys[currentRowIndex].Values["IDESTADO"].ToString();

                if (IDEST == "5")
                {
                    lblErrorReg.Text = "NO SE PUEDE MONSTRAR ESTE ARCHIVO PORQUE HA SIDO DESACTIVADO.";
                    orror.Visible = true;
                }
                else
                {

                    DataTable dt = dat.TSegSQL("sp_manuales 1");

                    string carpetas = RUTA.Replace(RUTA.Split('/').Last(), "");
                    string ftp = dt.Rows[0]["ftpCred"].ToString(); //urlCompleta
                    string ftpusuario = dt.Rows[0]["Usuario"].ToString(); //Usuario
                    string ftpPass = dt.Rows[0]["passw"].ToString(); //Pass
                    string ftpRuta = dt.Rows[0]["ftpW"].ToString() + carpetas; //Ruta
                    string ftpNombreArchivo = RUTA.Split('/').Last() + EXT; //NombreArchivo
                    string ftperror = dt.Rows[0]["ERROR"].ToString(); //NombreArchivo

                    string RUTA2 = ftp + RUTA + EXT;
                    string RUTAERROR = ftp + ftperror;

                    if (Util.CheckIfFileExistsOnServer(ftpNombreArchivo, ftpRuta, ftpusuario, ftpPass) == true)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("window.open('" + RUTA2 + "',1,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=700,left=10,top=10');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript'>");
                        sb.Append("window.open('" + RUTAERROR + "',1,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=700,left=10,top=10');");
                        sb.Append("</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", sb.ToString(), false);
                    }
                }
            }
        }

        void COASEGURO(string id, string busqueda)
        {
            string iVAL = "sp_ver 52,'" + id + "','" + busqueda + "'";
            DataTable dtab = dat.SQL(iVAL);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvCoaseguro.DataSource = dtab;
                gvCoaseguro.DataBind();

                lbl_Coaseguro.Text = "";
            }
            else
            {
                gvCoaseguro.DataSource = null;
                gvCoaseguro.DataBind();
                lbl_Coaseguro.Text = dtab.Rows[0][1].ToString();
            }

        }

        void COBERTURA(string idpoli, string busqueda)
        {
            string iVAL = "sp_ver 53,'" + idpoli + "','" + busqueda + "' ";
            DataTable dtab = dat.SQL(iVAL);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvCobertura.DataSource = dtab;
                gvCobertura.DataBind();
                lbl_Cobertura.Text = "";
            }
            else
            {
                gvCobertura.DataSource = null;
                gvCobertura.DataBind();
                lbl_Cobertura.Text = dtab.Rows[0][1].ToString();
            }
        }

        protected void lnkBuscaPoliza_Click(object sender, EventArgs e)
        {
            PolizasClientes(hfCod_Cliente.Value);
        }

        protected void lnkDocumentosCliente_Click(object sender, EventArgs e)
        {
            DocumentosClientes(hfCod_Cliente.Value);
        }

        protected void lnkSinAsignar_Click(object sender, EventArgs e)
        {
            txtRespuestaGeneral.Text = "";
            txtRespuestaGeneral.Text = "Sin Asignar, no se asignó ninguna póliza.";

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#PolizasCliente').modal('hide');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);

        }

        protected void lnkNuevoSiniestro_Click(object sender, EventArgs e)
        {
            hfServicioSiniestro.Value = "2";
            lblValidacionSiniestro.Text = "";
            ddlTipoStro.Visible = true;
            ddlTipoStroServicio.Visible = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#Siniestro').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        void AseguradoBienAsegurado(string poliza)
        {

            ddlEjecutivoSiniestro.SelectedIndex = 0;
            txtAsegurado_.Text = "";
            ddlTipoStro.SelectedIndex = 0;
            ddlTipoStroServicio.SelectedIndex = 0;
            txtLugarSiniestro.Text = "";
            txtDescripcion.Text = "";

            //------------------------------------

            botonesSiniestros.Visible = true;
            txtCliente_.Text = hfNombreCliente.Value;

            string TIPORIESGO = "sp_ver 69,'" + poliza + "'";
            DataTable dtab1 = dat.TSegSQL(TIPORIESGO);

            if (dtab1.Rows[0][0].ToString() == "1") //GENERALES TEXTO DESCRIPCION
            {
                DivBienAsegurado_.Visible = false;
                DivAsegurado_.Visible = false;

                chkNohayAsegurado.Visible = false;
                chkNohaybienAsegurado.Visible = false;

                txtDescripcion.ReadOnly = false;
                lnkNuevoSiniestro.Visible = true;
                lnkNuevoSiniestroServicio.Visible = false;
            }
            if (dtab1.Rows[0][0].ToString() == "2") //ASEGURADO TEXTO DESCRIPCION
            {
                hfBienOAsegurado.Value = "1";
                Asegurados_(poliza, txtAsegurado_.Text);
                lnkNuevoSiniestro.Visible = true;
                lnkNuevoSiniestroServicio.Visible = false;
            }
            if (dtab1.Rows[0][0].ToString() == "3") //VEHICULOS TEXTO DESCRIPCION
            {
                hfBienOAsegurado.Value = "0";
                BienAsegurado_(poliza, txtBienAsegurado_.Text);
                lnkNuevoSiniestroServicio.Visible = true;
                lnkNuevoSiniestro.Visible = true;
            }
        }

        void Asegurados_(string poliza, string filtro)
        {
            string ASEGURADOS = "sp_ver_2 9,'" + poliza + "','" + filtro + "'";
            DataTable dtab = dat.TSegSQL(ASEGURADOS);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvAsegurado_.DataSource = dtab;
                gvAsegurado_.DataBind();
                txtDescripcion.ReadOnly = true;
                DivAsegurado_.Visible = true;
            }
            else
            {
                gvAsegurado_.DataSource = null;
                gvAsegurado_.DataBind();
                txtDescripcion.Text = "";
                txtDescripcion.ReadOnly = false;
                DivAsegurado_.Visible = false;
            }

            DivBienAsegurado_.Visible = false;
        }

        void BienAsegurado_(string poliza, string filtro)
        {
            string VEHICULOSMODAL = "sp_ver 64,'" + poliza + "','" + filtro + "'";
            DataTable dtab = dat.TSegSQL(VEHICULOSMODAL);

            if (dtab.Rows[0][0].ToString() != "0")
            {
                gvBienAsegurado_.DataSource = dtab;
                gvBienAsegurado_.DataBind();
                txtDescripcion.ReadOnly = true;
                DivBienAsegurado_.Visible = true;
            }
            else
            {
                gvBienAsegurado_.DataSource = null;
                gvBienAsegurado_.DataBind();
                txtDescripcion.Text = "";
                txtDescripcion.ReadOnly = false;
                DivBienAsegurado_.Visible = false;
            }
            DivAsegurado_.Visible = false;
        }

        protected void gvAsegurado__SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string idAseguradoPoliza = gvAsegurado_.DataKeys[gvAsegurado_.SelectedIndex].Values["idAseguradoPoliza"].ToString();
                CARGA_ASEGURADO(idAseguradoPoliza);
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvAsegurado__RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvAsegurado_, "Select$" + e.Row.RowIndex);
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvBienAsegurado__RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvBienAsegurado_, "Select$" + e.Row.RowIndex);
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        protected void gvBienAsegurado__SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string idAseguradoPoliza = gvBienAsegurado_.DataKeys[gvBienAsegurado_.SelectedIndex].Values["idVehAseg"].ToString();
                CARGA_VEHICULOASEGURADO(idAseguradoPoliza);
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
                orror.Visible = true;
            }
        }

        void CARGA_ASEGURADO(string aseguradopoli)
        {
            string xSQL = "sp_ver 56,'" + aseguradopoli + "',''";
            DataTable asegurado = new DataTable();
            asegurado = dat.TSegSQL(xSQL);

            if (asegurado.Rows.Count > 0)
            {
                /*DATOS CARGADOS PARA EDITAR*/
                hfIdAseguradoPoliza.Value = aseguradopoli;
                txtDescripcion.Text = "Asegurado: " + asegurado.Rows[0][9].ToString() + " " + asegurado.Rows[0][10].ToString();
                //---------------------------------------------------
                hftxtAsegurado.Value = asegurado.Rows[0][5].ToString();
                hftxtDependiente.Value = asegurado.Rows[0][6].ToString();
                hfddlTipoAsegurado.Value = asegurado.Rows[0][7].ToString();
                hfTipoAseg.Value = asegurado.Rows[0][7].ToString();
                hftxtNombreTitular.Value = asegurado.Rows[0][9].ToString();
                hftxtApellidoTitular.Value = asegurado.Rows[0][10].ToString();
                hftxtTitular.Value = asegurado.Rows[0][9].ToString() + " " + asegurado.Rows[0][10].ToString();
                hftxtNombre.Value = asegurado.Rows[0][9].ToString();
                hftxtApellidos.Value = asegurado.Rows[0][10].ToString();
                hftxtAseguradoNombres.Value = asegurado.Rows[0][9].ToString() + " " + asegurado.Rows[0][10].ToString();
                hftxtClienteContratante.Value = asegurado.Rows[0][11].ToString();
            }
            else
            {
                hftxtAsegurado.Value = "";
                hftxtDependiente.Value = "";
                hfddlTipoAsegurado.Value = "";
                hfTipoAseg.Value = "";
                hftxtNombreTitular.Value = "";
                hftxtApellidoTitular.Value = "";
                hftxtTitular.Value = "";
                hftxtNombre.Value = "";
                hftxtApellidos.Value = "";
                hftxtAseguradoNombres.Value = "";
                hftxtClienteContratante.Value = "";
            }
        }

        void CARGA_VEHICULOASEGURADO(string idVehAse)
        {
            //################################################################################### 
            string xSQL = "sp_ver 65,'" + idVehAse + "',''";
            DataTable Vehiculoasegurado = new DataTable();
            Vehiculoasegurado = dat.TSegSQL(xSQL);

            if (Vehiculoasegurado.Rows[0][0].ToString() == "0")
            {
                return;
            }
            else
            {
                /*DATOS CARGADOS PARA EDITAR*/
                hfBienOAsegurado.Value = idVehAse;
                txtDescripcion.Text = "Cliente: " + Vehiculoasegurado.Rows[0][3].ToString() + " || " +
                                "Placa: " + Vehiculoasegurado.Rows[0][5].ToString() + " || " +
                                "Clase: " + Vehiculoasegurado.Rows[0][6].ToString() + " || " +
                                "Marca: " + Vehiculoasegurado.Rows[0][7].ToString() + " || " +
                                "Modelo: " + Vehiculoasegurado.Rows[0][8].ToString() + " || " +
                                "Año Fabricación: " + Vehiculoasegurado.Rows[0][13].ToString();
            }
        }

        protected void lnkGuardarAsegurado__Click(object sender, EventArgs e)
        {
            if (txtLugarSiniestro.Text != "" && ddlEjecutivoSiniestro.SelectedIndex != 0 && txtDescripcion.Text != "")
            {
                hfEjecutivoSini.Value = ddlEjecutivoSiniestro.SelectedValue;
                hfLugarSini.Value = txtLugarSiniestro.Text;
                hfDescripcionSini.Value = txtDescripcion.Text;
                txtRespuestaGeneral.Text += System.Environment.NewLine + System.Environment.NewLine + "AVISO: SE REGISTRÓ NUEVO SINIESTRO.";

                if (hfServicioSiniestro.Value == "1")
                {
                    hfTipoSini.Value = ddlTipoStroServicio.SelectedValue;
                }
                else
                {
                    hfTipoSini.Value = ddlTipoStro.SelectedValue;
                }

                lblValidacionSiniestro.Text = "";

                if (chkNohayAsegurado.Checked == true)
                {
                    txtDescripcion.Text = "NUEVO SINIESTRO (SIN ASIGNAR ASEGURADO)";
                    txtRespuestaGeneral.Text += System.Environment.NewLine + System.Environment.NewLine + "AVISO: SE REGISTRÓ NUEVO SINIESTRO (SIN ASIGNAR)";
                }

                if (chkNohaybienAsegurado.Checked == true)
                {
                    txtDescripcion.Text = "NUEVO SINIESTRO (SIN ASIGNAR VEHICULO ASEGURADO)";
                    txtRespuestaGeneral.Text += System.Environment.NewLine + System.Environment.NewLine + "AVISO: SE REGISTRÓ NUEVO SINIESTRO (SIN ASIGNAR)";
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#Siniestro').modal('hide');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
            else
            {
                lblValidacionSiniestro.Text = "Aviso: Debe llenar los campos obligatorios (*)";
            }
        }

        void GUARDAR_SINIESTRO_SERVICIO(string servicioOSiniestro, string poliza)
        {
            string usuario = "";
            string xSQL = "sp_ver_15 8,'" + poliza + "'";
            DataTable datosPoliza = new DataTable();
            datosPoliza = dat.TSegSQL(xSQL);

            string strCiaSeg = datosPoliza.Rows[0]["ASEGURADORA"].ToString();
            string idCiaSeg = datosPoliza.Rows[0]["CiaSeg"].ToString();
            string NroPoli = datosPoliza.Rows[0]["NroPoliza"].ToString();
            string idRies = datosPoliza.Rows[0]["idriesgo"].ToString();
            string idAjus = "0";
            string idUniNeg = datosPoliza.Rows[0]["idUniNeg"].ToString();
            string idMoneda = datosPoliza.Rows[0]["idMoneda"].ToString();
            string idpoli = datosPoliza.Rows[0]["idPoliza"].ToString();
            hfEjecutivoCuenta.Value = datosPoliza.Rows[0]["idFuncionario"].ToString();
            //---------
            string idEjecu = hfEjecutivoSini.Value;
            string idTipStro = "";
            string idEstado = "";
            string servicio = "";
            if (hfBienOAsegurado.Value == "0") // VEHICULO
            {
                idEstado = "110";
            }
            else
            {
                idEstado = "107";
            }

            if (servicioOSiniestro == "1") //SERVICIO 1
            {
                idTipStro = hfTipoSini.Value;
                servicio = "0";
            }
            else //SINIESTRO 2
            {
                idTipStro = hfTipoSini.Value;
                servicio = "";
            }

            //SINIESTRO SERVICIO ESTADO ATENDIDO 110
            //SINIESTRO NORMAL EN ATENCION 107            
            string web = "0";
            string montoperdida = "0.00";
            string iso = "2";
            //ESTAN VISIBLE FALSE EN PRODUCCION
            string descripsiniest = "";
            string nroajustador = "";
            string obsEstado = "";
            string ciaEjecutivo = "";
            string EmailAseg = "";

            string queryUsu = "call sp_fill_2(7,'" + Session["USUARIO"].ToString() + "',0,0)";
            DataTable dataUsu = new DataTable();
            dataUsu = dat.mysql(queryUsu);

            if (dataUsu.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dataUsu.Rows[0]["usuario_id_evo"].ToString()))
                {
                    usuario = dataUsu.Rows[0]["usuario_id_evo"].ToString();
                }
                else
                {
                    lblErrorReg.Text = "Error : No tiene permisos para registrar un siniestro (Usuario Evo). ";
                    orror.Visible = true;
                    return;
                }
            }
            xSQL = "sp_ver_mante 1,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + strCiaSeg + "','" +
                                        hfCod_Cliente.Value + "','" + idRies + "','" + hfLugarSini.Value + "','" +
                                        idCiaSeg + "','" + NroPoli + "','0','" +
                                        idAjus + "','" + hfDescripcionSini.Value + "','" + descripsiniest + "','" +
                                        montoperdida + "','0','" + idUniNeg + "','" +
                                        idEjecu + "','" + idTipStro + "','0','0','" +
                                        idEstado + "','" + nroajustador + "','0','0','" +
                                        obsEstado + "','" + idMoneda + "','0','" + idpoli + "','" +
                                        iso + "','" + usuario + "','" + web + "','" +
                                        ciaEjecutivo + "','" + EmailAseg + "','" + txtContactoTelefono.Text + "','" + txtContactoNombre.Text + "','" + servicio + "'"; //35 campos
            try
            {
                DataTable tab = dat.TSegSQL(xSQL);
                if (tab.Rows.Count > 0)
                {
                    string idSini = tab.Rows[0][0].ToString();
                    hfSiniestroReg.Value = idSini;
                    if (hfBienOAsegurado.Value == "0")
                    {
                        REGISTRAR_VEHICULO_SINIESTRO(hfBienOAsegurado.Value, idSini);
                    }
                    else
                    {
                        REGISTRAR_SINIESTRO(hfIdAseguradoPoliza.Value, idSini);
                    }
                    ValidaCorreo(idSini);
                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = "Error : No se pudo guardar el Registro. " + ex.Message.ToString();
                orror.Visible = true;
            }
        }

        void REGISTRAR_SINIESTRO(string idasegurado, string idsin)
        {
            string xSQL1 = "";
            try
            {
                xSQL1 = "sp_mante 12,'" + idsin + "','" + hftxtAsegurado.Value + "','" + hftxtDependiente.Value + "','"
                                        + hftxtNombre.Value + "','" + hftxtApellidos.Value + "','" + hftxtClienteContratante.Value + "','0','0','0','"
                                        + hftxtNombreTitular.Value + "','" + hftxtApellidoTitular.Value + "'," + hfTipoAseg.Value + ",'" +
                                        idasegurado + "','','',''";
                DataTable data = dat.TSegSQL(xSQL1);
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
            }
        }

        void REGISTRAR_VEHICULO_SINIESTRO(string idVehiculoPoliza, string idsin)
        {
            string RC = "0";
            string perTot = "0";
            string DanOcu = "0";
            string todoRiesgo = "0";

            if (hfckRespCivil.Value == "1") { RC = "1"; }
            if (hfckPerdidaTotal.Value == "1") { perTot = "1"; }
            if (hfckDañoOcupantes.Value == "1") { DanOcu = "1"; }
            if (hfckTodoRiesgo.Value == "1") { todoRiesgo = "1"; }

            string xSQL1 = "";
            try
            {
                xSQL1 = "sp_mante 16,'" + idsin + "','"
                            + hfCod_Cliente.Value + "','" + hftxtConductor.Value + "','"
                            + hfddlTallerAsignado.Value + "','" + hftxtMontoPresupuesto.Value + "','"
                            + hftxtFechaIngresoTaller.Value + "','" + hftxtFechaSalidaTaller.Value + "','"
                            + hftxtFechaInspeccion.Value + "','" + hftxtMontoRespCivil.Value + "','"
                            + hftxtMontoDañoOcupantes.Value + "','" + hftxtLugarSiniestro2.Value + "','"
                            + hftxtDescripcionSiniestro2.Value + "','" + Session["USUARIO"] + "','"
                            + idVehiculoPoliza + "'," + RC + ","
                            + perTot + "," + DanOcu + ",'"
                            + hftxtContactoTaller.Value + "'," + todoRiesgo + ",'"
                            + hftxtMontoTodoRiesgo.Value + "'";

                DataTable data = dat.TSegSQL(xSQL1);
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = ex.Message.ToString();
            }
        }

        void ValidaCorreo(string idsini)
        {
            string strCorreo = "SP_VER_10 15,'" + hfCod_Cliente.Value + "'";
            DataTable dtCorreoLP = dat.TSegSQL(strCorreo);
            string correo = "";

            if (dtCorreoLP.Rows.Count > 0)
            {
                if (dtCorreoLP.Rows[0]["VAL"].ToString() == "1")
                {
                    correo = dtCorreoLP.Rows[0]["PARA"].ToString();
                }
                else
                {
                    correo = "";
                }

                EnviarCorreo(correo, hfEjecutivoCuenta.Value, idsini);
            }
        }

        void EnviarCorreo(string correoCliente, string ejecutivoCuenta, string idsini)
        {
            try
            {
                string persona = "SP_VER_10 20,'" + ddlEjecutivoSiniestro.SelectedValue + "','" + ejecutivoCuenta + "','" + idsini + "'";
                DataTable dtPersona = dat.TSegSQL(persona);

                //  VALIDACION DE LA FECHA DE OCURRENCIA < FECHA DE HOY - 1 DIA

                if (dtPersona.Rows[0]["VALOR"].ToString() == "1")
                {

                    //  ENVIO DE CORREO

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(dtPersona.Rows[0]["CUENTA_CORREO"].ToString(), dtPersona.Rows[0]["SUBNOMBRE"].ToString(), Encoding.UTF8);
                    mail.Subject = dtPersona.Rows[0]["ASUNTO"].ToString();

                    string Var_ResultHtml = "";
                    string HtmlPDF = "";

                    Var_ResultHtml += "<html>";
                    Var_ResultHtml += "<body>";
                    Var_ResultHtml += "<div style='padding-right: 15px; padding-left: 15px; margin-right: auto; margin-left: auto;'>";

                    Var_ResultHtml += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:large; width: 100%;'>";
                    HtmlPDF += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:small; width: 100%;'>";
                    Var_ResultHtml += "<tr><td style='text-align: left; border-bottom:solid 5px #B22;'>";
                    HtmlPDF += "<tr><td style='text-align: left; border-bottom:solid 5px #B22;'>";
                    Var_ResultHtml += "<img src='cid:imagen' width='370px' height='120px'/>";
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #7f7f7f;'>";
                    HtmlPDF += "<tr><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += "<br />Estimado cliente, hemos tomado nota de su aviso de siniestro el mismo que ha sido<br />";
                    HtmlPDF += "Estimado cliente, hemos tomado nota de su aviso de siniestro el mismo que ha sido<br />";
                    Var_ResultHtml += " reportado a la compañia de seguros " + hfStrinCiaseg.Value + "<br />";
                    HtmlPDF += " reportado a la compañia de seguros " + hfStrinCiaseg.Value + "<br />";
                    Var_ResultHtml += " así como también ha sido registrado en nuestros sistemas.";
                    HtmlPDF += " así como también ha sido registrado en nuestros sistemas.";
                    Var_ResultHtml += "<br /><br />";
                    HtmlPDF += "<br /><br />";
                    Var_ResultHtml += "A continuación, le indicamos los datos del ejecutivo de La Protectora responsable<br />";
                    HtmlPDF += "A continuación, le indicamos los datos del ejecutivo de La Protectora responsable<br />";
                    Var_ResultHtml += " en hacer seguimiento de su caso, apoyarlo y mantenerlo informado del mismo.<br />";
                    HtmlPDF += " en hacer seguimiento de su caso, apoyarlo y mantenerlo informado del mismo.<br />";
                    Var_ResultHtml += "</td></tr></table>";
                    HtmlPDF += "</td></tr></table>";

                    Var_ResultHtml += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:large; text-align:left; width: 100%;'>";
                    HtmlPDF += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:small; text-align:left; width: 100%;'>";
                    Var_ResultHtml += "<tr><td style='color: #000; width: 20%;'>";
                    HtmlPDF += "<tr><td style='color: #000; width: 20%;'>";
                    Var_ResultHtml += "<strong>Ejecutivo</strong>";
                    HtmlPDF += "<strong>Ejecutivo</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f; width: 80%;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f; width: 80%;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["Nombre"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["Nombre"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Telf. Movil</strong>";
                    HtmlPDF += "<strong>Telf. Movil</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["MOVIL"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["MOVIL"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Telf. Fijo</strong>";
                    HtmlPDF += "<strong>Telf. Fijo</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["FIJO"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["FIJO"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Correo</strong>";
                    HtmlPDF += "<strong>Correo</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["EMAIL"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["EMAIL"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Riesgo</strong>";
                    HtmlPDF += "<strong>Riesgo</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["RIESGO"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["RIESGO"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Cobertura</strong>";
                    HtmlPDF += "<strong>Cobertura</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["COBERTURA"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["COBERTURA"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Siniestro</strong>";
                    HtmlPDF += "<strong>Siniestro</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["SINIESTRO"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["SINIESTRO"].ToString();
                    Var_ResultHtml += "</td></tr>";
                    HtmlPDF += "</td></tr>";

                    Var_ResultHtml += "<tr><td style='color: #000;'>";
                    HtmlPDF += "<tr><td style='color: #000;'>";
                    Var_ResultHtml += "<strong>Estado</strong>";
                    HtmlPDF += "<strong>Estado</strong>";
                    Var_ResultHtml += "</td><td style='color: #7f7f7f;'>";
                    HtmlPDF += "</td><td style='color: #7f7f7f;'>";
                    Var_ResultHtml += ": " + dtPersona.Rows[0]["ESTADO"].ToString();
                    HtmlPDF += ": " + dtPersona.Rows[0]["ESTADO"].ToString();
                    Var_ResultHtml += "</td></tr></table>";
                    HtmlPDF += "</td></tr></table>";

                    Var_ResultHtml += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:large; width: 100%;'>";
                    HtmlPDF += "<table border='0' cellpadding='5' cellspacing='5' style='font-family:Calibri; font-size:small; width: 100%;'>";
                    Var_ResultHtml += "<tr><td style='color: #7f7f7f; text-align: left'>";
                    HtmlPDF += "<tr><td style='color: #7f7f7f; text-align: left'>";
                    Var_ResultHtml += dtPersona.Rows[0]["COPYR"].ToString();
                    HtmlPDF += dtPersona.Rows[0]["COPYR"].ToString();
                    Var_ResultHtml += "</td></tr></table>";
                    HtmlPDF += "</td></tr></table>";

                    Var_ResultHtml += "</div></body></html>";

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Var_ResultHtml, Encoding.UTF8, MediaTypeNames.Text.Html);

                    LinkedResource img = new LinkedResource(Server.MapPath("~/images/logoLP.png"), MediaTypeNames.Image.Jpeg);
                    img.ContentId = "imagen";
                    htmlView.LinkedResources.Add(img);

                    mail.AlternateViews.Add(htmlView);

                    if (correoCliente != "") { mail.To.Add(correoCliente); }
                    mail.To.Add(dtPersona.Rows[0]["EJEC_SINI"].ToString());
                    mail.CC.Add(dtPersona.Rows[0]["EJEC_CUEN"].ToString());

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(dtPersona.Rows[0]["CUENTA_CORREO"].ToString(), dtPersona.Rows[0]["CLAVE_CORREO"].ToString());
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    // ENVIO DE MENSAJES DE TEXTO

                    ServiceReferencesEmail.webServiceDemoSoapClient ms = new ServiceReferencesEmail.webServiceDemoSoapClient();
                    if (dtPersona.Rows[0]["NROCLIE"].ToString() != "")
                    {
                        ms.CorreoMasivo("correomasivo", "lp2017", "3", dtPersona.Rows[0]["CUENTA_CORREO"].ToString(), "", "Port:7-" + dtPersona.Rows[0]["NROCLIE"].ToString(), dtPersona.Rows[0]["MSJ"].ToString());
                    }
                    if (dtPersona.Rows[0]["NROEJEC"].ToString() != "")
                    {
                        ms.CorreoMasivo("correomasivo", "lp2017", "3", dtPersona.Rows[0]["CUENTA_CORREO"].ToString(), "", "Port:7-" + dtPersona.Rows[0]["NROEJEC"].ToString(), dtPersona.Rows[0]["MSJ"].ToString());
                    }

                    //  REGISTRO DEL SEGUIMIENTO ENVIO DE CORREO Y SMS

                    string strData = "SP_VER_10 21,'" + idsini + "','1','" + dtPersona.Rows[0]["CUENTA_CORREO"].ToString() + "','" + dtPersona.Rows[0]["NROCLIE"].ToString() + "'";

                    dat.TSegSQL(strData);

                    // CREACION DEL DOCUMENTO PDF

                    iTextSharp.text.Image imagenLP = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/logoLP.png"));
                    imagenLP.BorderWidth = 0;
                    imagenLP.SetAbsolutePosition(50, 660);
                    imagenLP.ScaleAbsoluteWidth(220);
                    imagenLP.ScaleAbsoluteHeight(70);

                    string ArchiPDF = "EmailAlertasNuevoSiniestro.pdf";

                    string descrip = "ARCHIVO PDF - CONFIRMACION DE ENVIO DE CORREO ELECTRONICO";
                    string filename = System.IO.Path.GetFileName(ArchiPDF);
                    string extension = System.IO.Path.GetExtension(ArchiPDF);

                    string fulldianame = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).ToString("dddd", CultureInfo.CreateSpecificCulture("es"));
                    string fullMonthName = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    string valDia = new CultureInfo("en-US").TextInfo.ToTitleCase(fulldianame);
                    string valMes = new CultureInfo("en-US").TextInfo.ToTitleCase(fullMonthName);

                    string fechaImp = " " + valDia + " " + DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).Day.ToString() + " de " + valMes + " del " + DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).Year;
                    fechaImp += ", " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();

                    string sqlx2 = "SP_VER_10 22,'7','" + descrip + "','" + idsini + "','" + Session["USUARIO"] + "','" + extension + "','0'";

                    Document document = new Document(PageSize.A4);
                    string FileName = Guid.NewGuid().ToString();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~/DOCUMENTOS/" + ArchiPDF), FileMode.Create));
                    document.Open();

                    // Insertamos la imagen en el documento
                    document.Add(imagenLP);

                    string Html_String = "";

                    Html_String += "<html><body>";
                    Html_String += "<div style='padding-right: 15px; padding-left: 15px; margin-right: auto; margin-left: auto;'>";

                    Html_String += "<table border='0' cellpadding='2' cellspacing='0' style='font-family:Calibri; font-size:large; width: 100%;'>";
                    Html_String += "<tr><td style='text-align:left; border-bottom:solid 1px #000; border-top:solid 1px #000;'>";
                    Html_String += "<strong>" + dtPersona.Rows[0]["ASUNTO"].ToString() + "</strong>";
                    Html_String += "</td></tr></table>";
                    Html_String += "<table border='0' cellpadding='2' cellspacing='0' style='font-family:Calibri; font-size:small; width: 100%;'>";
                    Html_String += "<tr><td style='text-align:left; width:50%;'>";
                    Html_String += "<strong>" + dtPersona.Rows[0]["SUBNOMBRE"].ToString() + "</strong> | " + dtPersona.Rows[0]["CUENTA_CORREO"].ToString() + "<br/>";

                    if (correoCliente == "")
                    {
                        Html_String += "Para: " + dtPersona.Rows[0]["EJEC_SINI"].ToString() + "<br/>";
                        Html_String += "CC: " + dtPersona.Rows[0]["EJEC_CUEN"].ToString();
                    }
                    else
                    {
                        Html_String += "Para: " + correoCliente + "<br/>";
                        Html_String += "CC: " + dtPersona.Rows[0]["EJEC_CUEN"].ToString() + ", " + dtPersona.Rows[0]["EJEC_CUEN"].ToString();
                    }

                    Html_String += "</td><td style='text-align:right; width:50%; vertical-align:top'>";
                    Html_String += fechaImp;
                    Html_String += "</td></tr></table>";

                    Html_String += "<br /><br /><br /><br /><br /><br /><br />";

                    Html_String += HtmlPDF;

                    Html_String += "</div></body></html>";

                    HTMLWorker worker = new HTMLWorker(document);
                    StringReader sr = new StringReader(Html_String);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

                    document.Close();

                    PdfAction action = new PdfAction(PdfAction.PRINTDIALOG);

                    DataTable dtDocum = new DataTable();
                    //--
                    dtDocum = dat.SQL(sqlx2);

                    string ruta = dtDocum.Rows[0][0].ToString() + extension;
                    //-------------------------------------------------------------------------------------------------------------------------------------------------
                }
                else
                {
                    //  REGISTRO DE SEGUIMIENTO VALIDACION FECHA OCURRENCIA > FECHA DE HOY
                    string strData = "SP_VER_10 21,'" + idsini + "','0'";
                    //--
                    dat.TSegSQL(strData);
                }

            }
            catch (Exception ex)
            {
                lblErrorReg.Text = "Error, debe llenar los campos obligatorios.";
                orror.Visible = true;
            }
        }

        protected void lnkBuscarAsegurado__Click(object sender, EventArgs e)
        {
            Asegurados_(hfpoli.Value, txtAsegurado_.Text);
        }

        protected void lnkNuevoSiniestroServicio_Click(object sender, EventArgs e)
        {
            hfServicioSiniestro.Value = "1";
            lblValidacionSiniestro.Text = "";
            ddlTipoStro.Visible = false;
            ddlTipoStroServicio.Visible = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("$('#Siniestro').modal('show');");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
        }

        protected void gvBienAsegurado__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBienAsegurado_.PageIndex = e.NewPageIndex;
            BienAsegurado_(hfpoli.Value, txtBienAsegurado_.Text);
        }

        protected void gvAsegurado__PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBienAsegurado_.PageIndex = e.NewPageIndex;
            Asegurados_(hfpoli.Value, txtAsegurado_.Text);
        }

        protected void lnkBuscarBienAsegurado__Click(object sender, EventArgs e)
        {
            BienAsegurado_(hfpoli.Value, txtBienAsegurado_.Text);
        }

        protected void chkNohayAsegurado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNohayAsegurado.Checked == true)
            {
                txtDescripcion.Text = "NUEVO SINIESTRO (SIN ASIGNAR ASEGURADO)";
            }
            else
            {
                txtDescripcion.Text = "";
            }

        }

        protected void chkNohaybienAsegurado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNohaybienAsegurado.Checked == true)
            {
                txtDescripcion.Text = "NUEVO SINIESTRO (SIN ASIGNAR VEHICULO ASEGURADO)";
            }
            else
            {
                txtDescripcion.Text = "";
            }

        }

        protected void gvSubGestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSubGestion.PageIndex = e.NewPageIndex;
            SUBGESTION(hfGestion.Value);
        }

        protected void lnkBuscarReclamo_Click(object sender, EventArgs e)
        {
            xSQL = "CALL SP_GESTION_VOIP(21,303," + hfGestion.Value + ",'','','" + txtBuscarReclamo.Text + "');";
            dtab = dat.mysql(xSQL);
            gvSubGestion.DataSource = dtab;
            gvSubGestion.DataBind();
        }

        protected void lnkConfirmar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionVoip.aspx");
        }

        protected void gvGestionResueltos2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvGestionResueltos2.DataKeys[e.Row.RowIndex].Values["ARCHIVO"].ToString();
                LinkButton lb = e.Row.FindControl("lnkArchivo") as LinkButton;
                Label lblresp = e.Row.FindControl("lblresp") as Label;
                Label lblobs = e.Row.FindControl("lblobs") as Label;


                if (lblresp.Text.Length > 70)
                {
                    lblresp.Text = lblresp.Text.Substring(0, 70) + "...";
                }
                if (lblobs.Text.Length > 50)
                {
                    lblobs.Text = lblobs.Text.Substring(0, 50) + "...";
                }

                if (KeyID == "")
                {
                    lb.Visible = false;
                }
                else
                {
                    lb.Visible = true;
                }

            }

        }

        protected void gvGestionResueltos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvGestionResueltos.DataKeys[e.Row.RowIndex].Values["ARCHIVO"].ToString();
                LinkButton lb = e.Row.FindControl("lnkArchivo") as LinkButton;
                Label lblresp = e.Row.FindControl("lblresp") as Label;
                Label lblobs = e.Row.FindControl("lblobs") as Label;


                if (lblresp.Text.Length > 70)
                {
                    lblresp.Text = lblresp.Text.Substring(0, 70) + "...";
                }
                if (lblobs.Text.Length > 50)
                {
                    lblobs.Text = lblobs.Text.Substring(0, 50) + "...";
                }

                if (KeyID == "")
                {
                    lb.Visible = false;
                }
                else
                {
                    lb.Visible = true;
                }

            }
        }

        protected void gvEjecutivo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEjecutivo.PageIndex = e.NewPageIndex;
            cargar_ejecutivo();
        }

        protected void chkCorreo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCorreo.Checked)
            {
                rfvEmailCliente.Enabled = true;
                divCorreo.Attributes.Add("style", "display:initial;");
            }
            else
            {
                rfvEmailCliente.Enabled = false;
                divCorreo.Attributes.Add("style", "display:none;");
            }
        }

        protected void gvProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProveedor.PageIndex = e.NewPageIndex;
            cargar_proveedor();
        }
    }
}