using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using SFW.BL;
using SFW.BE;

namespace SFW.Web
{
    public partial class GestionVoip : System.Web.UI.Page
    {
        Datos dat = new Datos();
        string xSQL;
        DataTable dtab;
        DataTable dtabexport;
        Usuario usu = new Usuario();
        DataTable datos = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USUARIO"] == null || Session["afiliado"] == null || Session["DATOS"] == null)
            {
                Response.Redirect("Sesion.aspx?usu=&pass=");
            }
            datos = ((DataTable)Session["DATOS"]);
            usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));
            if (!Page.IsPostBack)
            {
                cldFechas.SelectedDate = DateTime.Now;
                string fulldianame = DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("es"));
                string fullMonthName = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                lblConexion.Text = fulldianame + " " + DateTime.Now.Day + " de " + fullMonthName + " del " + DateTime.Now.Year;

                validarBotones();
                Cargarcombos(usu.ID.ToString());
                hfTipo.Value = "1";
                hfContador.Value = "";
                CARGAR_CONTADOR(DateTime.Now.ToString("yyyy-MM-dd"));
                CARGAR_DATA("", DateTime.Now.ToString("yyyy-MM-dd"), "", "", "", "");
            }
        }

        public void validarBotones()
        {
            if (datos.Rows[0]["clientes"].ToString() == "62")
            {
                hfUsuario.Value = datos.Rows[0]["usuario_id"].ToString();
                lblTitulo.Text = "Gestión de afiliados – La Protectora Salud";
                divIso.Attributes.Add("style", "display:none;");
                filtrosAuna.Attributes.Add("style", "display:initial;");
                lnkHospitalizados.Visible = false;
                LnkRegresar.Visible = false;
                lnkReporteAveria.Visible = false;
                lnkReportes.Visible = false;
                if (datos.Rows[0]["interno"].ToString() == "7")
                {
                    lnkRegistrar.Visible = false;
                }
            
            }
            else
            {
                hfUsuario.Value = "";
                lblTitulo.Text = "Adminitración de Central de Asistencia";
                divIso.Attributes.Add("style", "display:initial;");
                filtrosAuna.Attributes.Add("style", "display:none;");
                if (usu.AVERIA == "1")
                {
                    lnkReporteAveria.Visible = true;
                }
                else
                {
                    lnkReporteAveria.Visible = false;
                }
            }
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_fill_2(3,'" + ddlEmpresa.SelectedValue + "','','')");
            //77 1 T / 0 U
            //88 0 T / 1 U
            //*  0 T / 0 U
            ddlTablas.Visible = Convert.ToBoolean(dt.Rows[0]["ddlTablas"]);
            ddlUniNeg.Visible = Convert.ToBoolean(dt.Rows[0]["ddlUniNeg"]);
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }
        protected void cldFechas_SelectionChanged(object sender, EventArgs e)
        {
            hfTipo.Value = "1";
            hfContador.Value = "";
            txtBusqueda.Text = "";
            CARGAR_CONTADOR(cldFechas.SelectedDate.ToString("yyyy-MM-dd"));
            CARGAR_DATA("", cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
            string fulldianame = Convert.ToDateTime(cldFechas.SelectedDate).ToString("dddd", CultureInfo.CreateSpecificCulture("es"));
            string fullMonthName = Convert.ToDateTime(cldFechas.SelectedDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            lblConexion.Text = fulldianame + " " + Convert.ToDateTime(cldFechas.SelectedDate).Day + " de " + fullMonthName + " del " + Convert.ToDateTime(cldFechas.SelectedDate).Year;
        }
        protected void cldFechas_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            hfTipo.Value = "2";
            hfContador.Value = "";
            txtBusqueda.Text = "";
            CARGAR_CONTADOR(cldFechas.VisibleDate.ToString("yyyy-MM-dd"));
            CARGAR_DATA("", cldFechas.VisibleDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
            string fullMonthName = Convert.ToDateTime(cldFechas.VisibleDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            lblConexion.Text =  fullMonthName + " del " + Convert.ToDateTime(cldFechas.VisibleDate).Year;
     
        }
        protected void ddlUniNeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }

        protected void Cargarcombos(string usu)
        {
            ddlTablas.DataSource = dat.mysql("call sp_fill(84,'" + usu + "',0,0)");
            ddlTablas.DataTextField = "descrip";
            ddlTablas.DataValueField = "valor";
            ddlTablas.DataBind();

            ddlEmpresa.DataSource = dat.mysql("call sp_fill_2(2,0,0,0)");
            ddlEmpresa.DataTextField = "descrip";
            ddlEmpresa.DataValueField = "valor";
            ddlEmpresa.DataBind();

            ddlEstado.DataSource = dat.mysql("CALL SP_GESTION_VOIP(121,'304','','','','');");
            ddlEstado.DataTextField = "descrip";
            ddlEstado.DataValueField = "ID";
            ddlEstado.DataBind();

            ddlRegistra.DataSource = dat.mysql("CALL SP_GESTION_VOIP(122,'','','','','');");
            ddlRegistra.DataTextField = "descrip";
            ddlRegistra.DataValueField = "ID";
            ddlRegistra.DataBind();


            ddlDeriva.DataSource = dat.mysql("CALL SP_GESTION_VOIP(122,'','','','','');");
            ddlDeriva.DataTextField = "descrip";
            ddlDeriva.DataValueField = "ID";
            ddlDeriva.DataBind();

            string xSQL = "sp_ver 1";
            DataTable dtab = dat.TSegSQL(xSQL);
            ddlUniNeg.DataSource = dtab;
            ddlUniNeg.DataTextField = "descrip";
            ddlUniNeg.DataValueField = "codigo";
            ddlUniNeg.DataBind();

        }

        void CARGAR_CONTADOR(string fecha)
        {
            xSQL = "CALL SP_GESTION_VOIP(0,304,'" + fecha + "','','','" + hfUsuario.Value + "');";
            dtab = dat.mysql(xSQL);
            gvContador.DataSource = dtab;
            gvContador.DataBind();
        }

        void CARGAR_DATA(string estado, string fecha, string empresa, string cliente, string unidadNegocio, string iso)
        {
            //GESTION
            xSQL = "CALL SP_GESTION_VOIP_2(1,'" + estado + "','" + fecha + "','','" + txtBusqueda.Text + "','" + empresa + "','" + cliente + "','" + unidadNegocio + "','" + iso + "','" + ddlEstado.SelectedValue + "','" + ddlRegistra.SelectedValue + "','" + ddlDeriva.SelectedValue + "','','','','" + hfUsuario.Value + "');";
            if (hfTipo.Value == "2")//Acumulado del mes
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'" + estado + "','','" + fecha + "','" + txtBusqueda.Text + "','" + empresa + "','" + cliente + "','" + unidadNegocio + "','" + iso + "','" + ddlEstado.SelectedValue + "','" + ddlRegistra.SelectedValue + "','" + ddlDeriva.SelectedValue + "','','','','" + hfUsuario.Value + "');";
            }
            dtabexport = dat.mysql(xSQL);
            gvGestion.DataSource = dtabexport;
            gvGestion.DataBind();
            lblCantidadGestion.Text = "Se han encontraron " + dtabexport.Rows.Count + " " + "registros";
        }

        protected void lnkRegistrar_Click(object sender, EventArgs e)
        {
            if (Session["USUARIO"] == null)
            {
                Response.Redirect("Sesion.aspx");
            }
            else
            {
                Response.Redirect("NuevaGestion.aspx?id=0&as=&ida=&daseg=");
            }
        }
        protected void LnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfMantenimiento.aspx");
        }
        protected void gvContador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            hfTipo.Value = "1";
            if (e.CommandName == "CANTIDAD")
            {
                hfTipo.Value = "2";
            }
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            hfContador.Value = gvContador.DataKeys[currentRowIndex].Values["ID"].ToString();
            txtBusqueda.Text = "";
            CARGAR_CONTADOR(cldFechas.SelectedDate.ToString("yyyy-MM-dd"));
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);

        }
        protected void gvContador_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string KeyID = gvContador.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                if (KeyID == hfContador.Value)
                {
                    if (hfTipo.Value == "2")
                    {
                        e.Row.Cells[3].BackColor = System.Drawing.Color.LightBlue;
                        return;
                    }
                    else
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightBlue;
                        return;
                    }
                }
            }
        }
        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            hfContador.Value = "";
            hfTipo.Value = "2";
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }

        protected void gvGestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string cod = gvGestion.DataKeys[currentRowIndex].Values["CODIGO"].ToString();
                Response.Redirect("NuevaGestion.aspx?id=" + cod + "");
            }
            if (e.CommandName == "Eliminar")
            {
                xSQL = "CALL SP_GESTION_VOIP(11,'" + e.CommandArgument.ToString() + "','" + Session["USUARIO"].ToString() + "','" + e.CommandArgument.ToString() + "','REGISTRO DE LLAMADA ELIMINADA','');";
                dtab = dat.mysql(xSQL);
                CARGAR_CONTADOR(DateTime.Now.ToString("yyyy-MM-dd"));
                CARGAR_DATA("", DateTime.Now.ToString("yyyy-MM-dd"), "", "", "", "");
            }
            if (e.CommandName == "Respuesta")
            {
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string RPTA = gvGestion.DataKeys[currentRowIndex].Values["RESPUESTA"].ToString();
                txtRespuesta.Text = RPTA;
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("$('#RESPUESTA').modal('show');");
                sb.Append("</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
            }
        }

        protected void gvGestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGestion.PageIndex = e.NewPageIndex;
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }

        void ExcelDownload(DataTable dt1)
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
                wst.Cells["A1"].Value = "LA PROTECTORA";
                wst.Cells["A1"].Style.Font.Bold = true;
                wst.Cells["A1:G1"].Merge = true;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");
                wst.Cells["A2:G2"].Merge = true;
                wst.Cells["A2:G2"].Style.Font.Bold = true;
                wst.Cells["A2:G2"].Value = "ADMINISTRACIÓN DE CENTRAL DE ASISTENCIA";
                wst.Cells["A2:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(colorCode);
                wst.Cells["A3:G3"].Merge = true;
                wst.Cells["A3:G3"].Style.Font.Bold = true;
                wst.Cells["A3:G3"].Value = Session["USU_NOM"] + " || " + DateTime.Now.ToString();
                wst.Cells["A4:U4"].Style.Font.Bold = true;
                colorCode = System.Drawing.ColorTranslator.FromHtml("#DCDCDC");
                wst.Cells["A4:U4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A4:U4"].Style.Fill.BackgroundColor.SetColor(colorCode);

                wst.Cells["M:M"].Style.Numberformat.Format = "dd/MM/yyyy";
                wst.Cells["N:N"].Style.Numberformat.Format = "dd/MM/yyyy";

                dt1.Columns.Remove("IDESTADO");
                dt1.Columns.Remove("ID_USU");
                dt1.Columns.Remove("DESCRIP_EMI");

                wst.Cells["A4"].LoadFromDataTable(dt1, true);

                wst.Cells["A4"].Value = "CODIGO";
                wst.Cells["B4"].Value = "TIPO LLAMADA";
                wst.Cells["C4"].Value = "TIPO EMISOR";
                wst.Cells["D4"].Value = "DESCRIPCION EMISOR";
                wst.Cells["E4"].Value = "TIPO ASEGURADO";
                wst.Cells["F4"].Value = "DESCRIPCION ASEGURADO";
                //NOMBRE_CONT,TELEFONO_CONT,CORREO_CONT
                wst.Cells["G4"].Value = "NOMBRE CONTACTO";
                wst.Cells["H4"].Value = "TELEFONO CONTACTO";
                wst.Cells["I4"].Value = "CORREO CONTACTO";
                wst.Cells["J4"].Value = "GESTION";
                wst.Cells["K4"].Value = "SUBGESTION";
                wst.Cells["L4"].Value = "USUARIO";
                wst.Cells["M4"].Value = "FECHA REGISTRO";
                wst.Cells["N4"].Value = "FECHA RESUELTO";
                wst.Cells["O4"].Value = "RESPUESTA";
                wst.Cells["P4"].Value = "OBSERVACION";
                wst.Cells["Q4"].Value = "ESTADO";
                wst.Cells["R4"].Value = "ID ASEGURADO";
                wst.Cells["S4"].Value = "ID CLIENTE";
                wst.Cells["T4"].Value = "POLIZA";
                wst.Cells["U4"].Value = "UNIDAD NEGOCIO";
                for (int row2 = 0; row2 <= dt1.Rows.Count; row2++)
                {
                    for (int column = 1; column <= dt1.Columns.Count; column++)
                    {
                        wst.Cells[row2 + 4, column].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(0, 0, 0));
                    }
                }
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
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VOIP" + serie + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
            }
        }

        protected void lnkDescargarExcel_Click(object sender, EventArgs e)
        {
            string cliente = "";
            string usuario = "";
            if (datos.Rows[0]["clientes"].ToString() == "62")
            {
                usuario = datos.Rows[0]["usuario_id"].ToString();
                cliente = "62";
            }
            //GESTION
            xSQL = "CALL SP_GESTION_VOIP_2(1,'" + hfContador.Value + "','" + cldFechas.SelectedDate.ToString("yyyy-MM-dd") + "','','" + txtBusqueda.Text + "','" + ddlEmpresa.SelectedValue + "','" + cliente + "','" + ddlUniNeg.SelectedValue + "','','','','','','','','" + usuario + "');";
            if (hfTipo.Value == "2")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'" + hfContador.Value + "','','" + cldFechas.SelectedDate.ToString("yyyy-MM-dd") + "','" + txtBusqueda.Text + "','" + ddlEmpresa.SelectedValue + "','" + cliente + "','" + ddlUniNeg.SelectedValue + "','','','','','','','','" + usuario + "');";
            }
            dtabexport = dat.mysql(xSQL);
            ExcelDownload(dtabexport);
        }

        protected void lnkHospitalizados_Click(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            string password = Convert.ToString(dt.Rows[0][6].ToString());

            Response.Redirect("http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=clin_hosp.php");

        }

        protected void lnkReportesEstadisticos_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportePrincipal.aspx");
        }

        protected void lnkReporteAveria_Click(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            string password = Convert.ToString(dt.Rows[0][6].ToString());
            Response.Redirect("http://app.laprotectora.com.pe/mproy/wfLogin.aspx?usu=" + usuario + "&pass=" + password + "&origen=2");
        }

        protected void ddlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }
        protected void lnkReportes_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dat.mysql("call sp_fill_2(4,'" + Session["USUARIO"] + "','','')");
                if (dt.Rows.Count > 0)
                {
                    Response.Write("<script>window.open('" + dt.Rows[0]["LINK"].ToString() + "','_blank');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>window.alert('" + ex.Message.ToString() + "');</script>");
            }
        }
        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {

            Session.Clear();
            Response.Redirect("Sesion.aspx?usu=&pass=");
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }

        protected void gvGestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfIdUsuarioRegistra = (HiddenField)e.Row.FindControl("hfUsuarioRegistra");
                HiddenField hfIdUsuarioDeriva = (HiddenField)e.Row.FindControl("hfUsuarioDeriva");

                LinkButton lnkEditar = (LinkButton)e.Row.FindControl("lnkEditarGestion");
                if (datos.Rows[0]["clientes"].ToString() == "62" && datos.Rows[0]["interno"].ToString() != "6" && datos.Rows[0]["interno"].ToString() != "4")
                {
                    if (datos.Rows[0]["usuario_id"].ToString() == hfIdUsuarioRegistra.Value || datos.Rows[0]["usuario_id"].ToString() == hfIdUsuarioDeriva.Value)
                    {
                        lnkEditar.Visible = true;
                    }
                    else
                    {
                        lnkEditar.Visible = false;
                    }

                }
            }

        }

        protected void ddlRegistra_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA("", cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }

        protected void ddlDeriva_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA("", cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue, ddlIso.SelectedValue);
        }


    }
}