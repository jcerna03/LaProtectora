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

namespace SFW.Web
{
    public partial class ReportePrincipal : System.Web.UI.Page
    {
        Datos dat = new Datos();
        DataTable dt1, dt2, dt3, dt4, dt5;
        string IDMES;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                combos();
                ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio_SelectedIndexChanged(sender, e);
            }
        }

        void descargaRecord(DataTable dt1, DataTable dt2, DataTable dt3,DataTable dt4,DataTable dt5, string titulo, string asegurado)
        {

            string serie = "REPORTE" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            Response.Clear();
            Response.Charset = "";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Color colorCode;

            using (var pck = new OfficeOpenXml.ExcelPackage())
            {

                string xSQL = "CALL SP_GESTION_VOIP(16,'" + ddlAnio.SelectedValue + "','" + hfIDMES.Value +"','','','');";
                DataTable dt6 = dat.mysql(xSQL);

                if (dt6.Rows.Count != 0)
                {
                    ExcelWorksheet wst = pck.Workbook.Worksheets.Add("DATA");

                    wst.Cells["A1:J1"].Merge = true;
                    wst.Cells["A1:J1"].Style.Font.Bold = true;
                    wst.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst.Cells["A1:J1"].Value = "DATOS";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst.Cells["J:J"].Style.Numberformat.Format = "dd/MM/yyyy";
                    wst.Cells["K:K"].Style.Numberformat.Format = "dd/MM/yyyy";
                    dt6.Columns.Remove("IDESTADO");
                    dt6.Columns.Remove("ID_USU");
                    //dt1.Columns.Remove("X");
                    wst.Cells["A4"].LoadFromDataTable(dt6, true);
                }

                if (dt1.Rows.Count != 0)
                {
                    ExcelWorksheet wst = pck.Workbook.Worksheets.Add("PRINCIPAL");

                    wst.Cells["A1:J1"].Merge = true;
                    wst.Cells["A1:J1"].Style.Font.Bold = true;
                    wst.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst.Cells["A1:J1"].Value = "REPORTE PRINCIPAL";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst.Cells["A4"].LoadFromDataTable(dt1, true);
                }

                if (dt2.Rows.Count != 0)
                {
                    ExcelWorksheet wst2 = pck.Workbook.Worksheets.Add("IAFAS");

                    wst2.Cells["A1:J1"].Merge = true;
                    wst2.Cells["A1:J1"].Style.Font.Bold = true;
                    wst2.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst2.Cells["A1:J1"].Value = "REPORTE IAFAS";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst2.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst2.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst2.Cells["A4"].LoadFromDataTable(dt2, true);
                }

                if (dt3.Rows.Count != 0)
                {
                    ExcelWorksheet wst3 = pck.Workbook.Worksheets.Add("USUARIO");

                    wst3.Cells["A1:J1"].Merge = true;
                    wst3.Cells["A1:J1"].Style.Font.Bold = true;
                    wst3.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst3.Cells["A1:J1"].Value = "REPORTE USUARIOS";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst3.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst3.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst3.Cells["A4"].LoadFromDataTable(dt3, true);
                }

                if (dt4.Rows.Count != 0)
                {
                    ExcelWorksheet wst4 = pck.Workbook.Worksheets.Add("GESTION");

                    wst4.Cells["A1:J1"].Merge = true;
                    wst4.Cells["A1:J1"].Style.Font.Bold = true;
                    wst4.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst4.Cells["A1:J1"].Value = "REPORTE GESTION";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst4.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst4.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst4.Cells["A4"].LoadFromDataTable(dt4, true);
                }

                if (dt5.Rows.Count != 0)
                {
                    ExcelWorksheet wst5 = pck.Workbook.Worksheets.Add("LLAMADA");

                    wst5.Cells["A1:J1"].Merge = true;
                    wst5.Cells["A1:J1"].Style.Font.Bold = true;
                    wst5.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wst5.Cells["A1:J1"].Value = "REPORTE LLAMADA";
                    colorCode = System.Drawing.ColorTranslator.FromHtml("#4EE384");
                    wst5.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wst5.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(colorCode);
                    wst5.Cells["A4"].LoadFromDataTable(dt5, true);
                }

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + serie + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
            }

        }

        protected void lnkDescargarExcel_Click(object sender, EventArgs e)
        {
            string xSQL = "CALL SP_GESTION_VOIP(15,'" + ddlAnio.SelectedValue +"','','','','');";
            dt1 = dat.mysql(xSQL);

            if (hfIDMES.Value != "")
            {
                xSQL = "call SP_GESTION_VOIP(14,'','1','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                dt2 = dat.mysql(xSQL);


                xSQL = "call SP_GESTION_VOIP(14,'','2','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                dt3 = dat.mysql(xSQL);


                xSQL = "call SP_GESTION_VOIP(14,'','3','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                dt4 = dat.mysql(xSQL);


                xSQL = "call SP_GESTION_VOIP(14,'','4','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                dt5 = dat.mysql(xSQL);
            }

            descargaRecord(dt1,dt2,dt3,dt4,dt5,"REPORTE VOIP","");
        }

        void combos()
        {
            ddlAnio.DataSource = dat.mysql("call sp_fill(70,0,0,0);");
            ddlAnio.DataValueField = "descrip";
            ddlAnio.DataTextField = "descrip";
            ddlAnio.DataBind();
        }

        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xSQL = "CALL SP_GESTION_VOIP(15,'"+ ddlAnio.SelectedValue +"','','','','');";
            dt1 = dat.mysql(xSQL);
            gvReportePrincipal.DataSource = dt1;
            gvReportePrincipal.DataBind();

            gvReporte01.DataSource = null;
            gvReporte01.DataBind();
            gvReporte02.DataSource = null;
            gvReporte02.DataBind();
            gvReporte03.DataSource = null;
            gvReporte03.DataBind();
            gvReporte04.DataSource = null;
            gvReporte04.DataBind();
        }

        protected void gvReportePrincipal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "VER")
                {
                    string currentCommand = e.CommandName;
                    int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
                    hfIDMES.Value = Convert.ToString(this.gvReportePrincipal.DataKeys[currentRowIndex]["IDMES"]);

                    string xSQL = "call SP_GESTION_VOIP(14,'','1','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                    dt2 = dat.mysql(xSQL);
                    gvReporte01.DataSource = dt2;
                    gvReporte01.DataBind();

                    xSQL = "call SP_GESTION_VOIP(14,'','2','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                    dt3 = dat.mysql(xSQL);
                    gvReporte02.DataSource = dt3;
                    gvReporte02.DataBind();

                    xSQL = "call SP_GESTION_VOIP(14,'','3','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                    dt4 = dat.mysql(xSQL);
                    gvReporte03.DataSource = dt4;
                    gvReporte03.DataBind();

                    xSQL = "call SP_GESTION_VOIP(14,'','4','" + ddlAnio.SelectedValue + '-' + hfIDMES.Value + "','','');";
                    dt5 = dat.mysql(xSQL);
                    gvReporte04.DataSource = dt5;
                    gvReporte04.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblErrorReg.Text = "ERROR";
            }
        }
    }
}