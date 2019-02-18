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

        protected void Page_Load(object sender, EventArgs e)
        {
            orror.Visible = false;
            Label2.Text = "";
            if (!Page.IsPostBack)
            {
                usu = new UsuarioBL().ObtieneUsuario(52, Convert.ToInt32(Session["USUARIO"].ToString()));

                if (usu.AVERIA == "1")
                {
                    lnkReporteAveria.Visible = true;
                }
                else
                {
                    lnkReporteAveria.Visible = false;
                }

                Cargarcombos(usu.ID.ToString());
                ddlEmpresa_SelectedIndexChanged(sender, e);

                hfContador.Value ="";
                hfTipo.Value = "1";
                cldFechas.SelectedDate = DateTime.Now;
                //CARGAR_COMBOS();
                cldFechas_SelectionChanged(sender, e);
                string fulldianame = DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("es"));
                string fullMonthName = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                lblConexion.Text = fulldianame + " " + DateTime.Now.Day + " de " + fullMonthName + " del " + DateTime.Now.Year;

                ddlEmpresa.Visible = false;
            }
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


            string xSQL = "sp_ver 1";
            DataTable dtab = dat.TSegSQL(xSQL);
            ddlUniNeg.DataSource = dtab;
            ddlUniNeg.DataTextField = "descrip";
            ddlUniNeg.DataValueField = "codigo";
            ddlUniNeg.DataBind();

        }

        void CARGAR_CONTADOR(string fecha) 
        {
            xSQL = "CALL SP_GESTION_VOIP(1,304,'" + fecha + "','','','');";
            dtab = dat.mysql(xSQL);
            gvContador.DataSource = dtab;
            gvContador.DataBind();
        }

        void CARGAR_DATA(string VAL, string FECHA,string empresa,string cliente,string unidadNegocio)
        {
            //GESTION
            if (hfTipo.Value == "1")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'" + VAL + "','" + FECHA + "','','" + txtBusqueda.Text + "','" + empresa + "','"+ cliente +"','"+ unidadNegocio +"','','','','','','','','');";
                dtabexport = dat.mysql(xSQL);  
                gvGestion.DataSource = dtabexport;
                gvGestion.DataBind();
                lblCantidadGestion.Text = "Se han encontraron " + dtabexport.Rows.Count + " " + "registros";
            }
            if (hfTipo.Value == "2")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'" + VAL + "','','" + FECHA + "','" + txtBusqueda.Text + "','" + empresa + "','"+ cliente +"','"+ unidadNegocio +"','','','','','','','','');";
                dtabexport = dat.mysql(xSQL);
                gvGestion.DataSource = dtabexport;
                gvGestion.DataBind();
                lblCantidadGestion.Text = "Se han encontraron " + dtabexport.Rows.Count + " " + "registros";
            }
            if (hfTipo.Value == "3")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'','','','" + txtBusqueda.Text + "','" + empresa + "','"+ cliente +"','"+ unidadNegocio +"','','','','','','','','');";
                dtabexport = dat.mysql(xSQL);
                gvGestion.DataSource = dtabexport;
                gvGestion.DataBind();
                lblCantidadGestion.Text = "Se han encontraron " + dtabexport.Rows.Count + " " + "registros";
            }
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

        protected void cldFechas_SelectionChanged(object sender, EventArgs e)
        {
            hfTipo.Value = "1";
            CARGAR_CONTADOR(cldFechas.SelectedDate.ToString("yyyy-MM-dd"));
            CARGAR_DATA("", cldFechas.SelectedDate.ToString("yyyy-MM-dd"),ddlEmpresa.SelectedValue,ddlTablas.SelectedValue,ddlUniNeg.SelectedValue);

            string fulldianame =  Convert.ToDateTime(cldFechas.SelectedDate).ToString("dddd", CultureInfo.CreateSpecificCulture("es"));
            string fullMonthName = Convert.ToDateTime(cldFechas.SelectedDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            lblConexion.Text = fulldianame + " " + Convert.ToDateTime(cldFechas.SelectedDate).Day + " de " + fullMonthName + " del " + Convert.ToDateTime(cldFechas.SelectedDate).Year;
        }

        protected void gvContador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CANTIDAD")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string id = gvContador.DataKeys[currentRowIndex].Values["ID"].ToString();
                hfContador.Value = id;
                hfTipo.Value = "2";
                CARGAR_DATA(id,cldFechas.SelectedDate.ToString("yyyy-MM-dd"),ddlEmpresa.SelectedValue,ddlTablas.SelectedValue,ddlUniNeg.SelectedValue);
                CARGAR_CONTADOR(cldFechas.SelectedDate.ToString("yyyy-MM-dd"));
               
            }

            if (e.CommandName == "CANTIDADSE")
            {
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                string id = gvContador.DataKeys[currentRowIndex].Values["ID"].ToString();
                hfContador.Value = id;
                hfTipo.Value = "1";
                CARGAR_DATA(id, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue);
                CARGAR_CONTADOR(cldFechas.SelectedDate.ToString("yyyy-MM-dd"));
            }
        }

        protected void gvContador_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string KeyID = gvContador.DataKeys[e.Row.RowIndex].Values["ID"].ToString();
                int col = gvContador.Columns.Count - 1;

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
            hfTipo.Value = "3";
            if (txtBusqueda.Text.Length >= 3)
            {
                CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue);
            }
        }

        protected void gvGestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
                string currentCommand = e.CommandName;
                int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());

                if (dt.Rows[0][1].ToString() == "11")
                {
                    string estado = gvGestion.DataKeys[currentRowIndex].Values["IDESTADO"].ToString();
                    string USUREG = gvGestion.DataKeys[currentRowIndex].Values["ID_USU"].ToString();
                    //if ((estado == "439") && (Session["USUARIO"].ToString() == USUREG)) { /*PASA*/ } else { orror.Visible = true; Label2.Text = "No cuenta con permisos para editar este registro."; return; }
                }

                string cod = gvGestion.DataKeys[currentRowIndex].Values["CODIGO"].ToString();
                string aseg = gvGestion.DataKeys[currentRowIndex].Values["TIPO_ASEG"].ToString();
                string idAseg = gvGestion.DataKeys[currentRowIndex].Values["ID_ASEG"].ToString();
                string descripcionAseg = gvGestion.DataKeys[currentRowIndex].Values["DESCRIP_ASEG"].ToString();
                Response.Redirect("NuevaGestion.aspx?id=" + cod + "&as=" + aseg + "&ida=" + idAseg + "&daseg=" + descripcionAseg + "");
                //Response.Redirect("NuevaGestion.aspx?id=" + cod);
                
            }

            if (e.CommandName == "Eliminar")
            {
                xSQL = "CALL SP_GESTION_VOIP(11,'" + e.CommandArgument.ToString() + "','" + Session["USUARIO"].ToString() + "','" + e.CommandArgument.ToString() + "','REGISTRO DE LLAMADA ELIMINADA','');";
                dtab = dat.mysql(xSQL);
                cldFechas_SelectionChanged(sender, e);
                
            }

            if (e.CommandName == "Respuesta")
            {
                string currentCommand = e.CommandName;
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
            cldFechas_SelectionChanged(sender, e);
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

                //DataTable dt = dat.mysql("call sp_ver_tablas(20,'" + usu + "','" + pass + "','','','','','','','','','','','')");

                colorCode = System.Drawing.ColorTranslator.FromHtml("#9BF0E9");

                wst.Cells["A2:G2"].Merge = true;
                wst.Cells["A2:G2"].Style.Font.Bold = true;
                wst.Cells["A2:G2"].Value = "ADMINISTRACIÓN DE CENTRAL DE ASISTENCIA"; 
                wst.Cells["A2:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wst.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(colorCode);

                wst.Cells["A3:G3"].Merge = true;
                wst.Cells["A3:G3"].Style.Font.Bold = true;
                wst.Cells["A3:G3"].Value = Session["USU_NOM"] + " || " + DateTime.Now.ToString();



                //ExcelWorksheet wst = pck.Workbook.Worksheets.Add("REPORTE");

                //wst.SelectedRange["A1:Z100"].Style.Font.Name = "Arial";
                //wst.SelectedRange["A1:Z100"].Style.Font.Size = 10;                

                //wst.Cells["A1"].Value = "ADMINISTRACIÓN DE CENTRAL DE ASISTENCIA";

                ///////wst.Cells["A3:G3"].Value = dt.Rows[0][1].ToString().ToUpper() + " || " + DateTime.Now.ToString();

                //wst.SelectedRange["A4:P4"].Style.Border.BorderAround(ExcelBorderStyle.Thin );
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
            //GESTION
            if (hfTipo.Value == "1")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'','" + cldFechas.SelectedDate.ToString("yyyy-MM-dd") + "','','" + txtBusqueda.Text + "','"+ ddlEmpresa.SelectedValue +"','"+ ddlTablas.SelectedValue +"','"+ ddlUniNeg.SelectedValue +"','','','','','','','','');";
                dtabexport = dat.mysql(xSQL);
            }
            if (hfTipo.Value == "2")
            {
                xSQL = "CALL SP_GESTION_VOIP_2(1,'','','" + cldFechas.SelectedDate.ToString("yyyy-MM-dd") + "','" + txtBusqueda.Text + "','"+ ddlEmpresa.SelectedValue +"','"+ ddlTablas.SelectedValue +"','"+ ddlUniNeg.SelectedValue +"','','','','','','','','');";
                dtabexport = dat.mysql(xSQL);
            }

            ExcelDownload(dtabexport);
        }

        protected void lnkHospitalizados_Click(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            string password = Convert.ToString(dt.Rows[0][6].ToString());

            Response.Redirect("http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=clin_hosp.php");
            

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "key","window.open('Hospitalizados.aspx');", true);
            //return;
        }

        protected void lnkReportesEstadisticos_Click(object sender, EventArgs e)
        {
            //DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            //string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            //string password = Convert.ToString(dt.Rows[0][6].ToString());

            //Response.Redirect("http://190.102.136.157/Reportes/Reporte_Llamadas_2.aspx?mes=2&ano=2016&var1=&var2=&U=");

            Response.Redirect("ReportePrincipal.aspx");

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('ReportesEstadisticos.aspx');", true);
            //return;
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
            hfTipo.Value = "3";
            if (txtBusqueda.Text.Length >= 3)
            {
                CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue);
            }
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = dat.mysql("call sp_fill_2(3,'" + ddlEmpresa.SelectedValue + "','','')");
            ddlTablas.Visible = Convert.ToBoolean(dt.Rows[0]["ddlTablas"]);
            ddlUniNeg.Visible = Convert.ToBoolean(dt.Rows[0]["ddlUniNeg"]);
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue);
        }

        protected void ddlUniNeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            CARGAR_DATA(hfContador.Value, cldFechas.SelectedDate.ToString("yyyy-MM-dd"), ddlEmpresa.SelectedValue, ddlTablas.SelectedValue, ddlUniNeg.SelectedValue);
        }

        protected void lnkReportes_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dat.mysql("call sp_fill_2(4,'" + Session["USUARIO"] + "','','')");
                if (dt.Rows.Count > 0)
                {
                    Response.Write("<script>window.open('" + dt.Rows[0]["LINK"].ToString() + "','_blank');</script>");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "window.open('" + dt.Rows[0]["LINK"].ToString() + "');", false);

                    //Response.Redirect(dt.Rows[0]["LINK"].ToString());
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>window.alert('"+ ex.Message.ToString() +"');</script>");
            }          
        }
    }
}