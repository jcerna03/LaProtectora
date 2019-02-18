using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using OfficeOpenXml;
using System.Text;
using System.IO;


namespace SFW.Web
{
    public partial class TramaSoat : System.Web.UI.Page
    {

        Datos dat = new Datos();

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }


        void ENVIAR_TRAMA(string val )
        {

            try
            {

                // op,nro_sini,nro_poli,nro_placa
                DataTable dt1 = dat.mysql("CALL SP_TRAMAS_SOAT('" + val + "','" + txtsini.Text + "','" + txtcerti.Text + "','" + txtplaca.Text + "','" + txtdni.Text + "');");

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(ms);
                    
                string Texto = "";

                if (val == "1") Texto = "Apertura";
                if (val == "2") Texto = "Reserva";
                if (val == "3") Texto = "Liquidación";

                using (var package = new ExcelPackage())
                {
                    var ws1 = package.Workbook.Worksheets.Add(Texto);
                    ws1.Cells["A1"].LoadFromDataTable(dt1, true);
                    ms = new MemoryStream(package.GetAsByteArray());
                }

                ms.Position = 0;
                
                
                //ws1.Protection.IsProtected = false;
                //Byte[] bin = pck.GetAsByteArray();
                //File.WriteAllBytes(MapPath("~/Adjuntos/TramaSoat_" + Texto + ".xlsx"), bin);


                //pck.SaveAs(new FileInfo(MapPath("~/Adjuntos/TramaSoat_" + Texto + ".xlsx")));                
                //wb.SaveAs(MyMemoryStream);
                //ms.WriteTo(Response.OutputStream);
                //Response.Flush();
                //Response.End();
                //Response.Redirect("FrmLogeo.aspx");
                    
                                
                string usu = "alertas@laprotectora.com.pe";
                string pass = "@Lert@s2017";
                string body = "";

                MailMessage message = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                message.From = new MailAddress(usu);
                message.Subject = "Trama "+  Texto + " Siniestro:  " + DateTime.Now.ToShortDateString() + " | " + txtsini.Text + " |Poliza: " + txtcerti.Text + " |Placa: " + txtplaca.Text;
                message.Body = "Estimados Señores de Protecta; se adjunta trama de " + Texto + " de siniestro";

                DataTable dtcoreos = dat.mysql("CALL sp_correos_trama();");

                for (int i = 0; i < dtcoreos.Rows.Count;i++ )
                {
                    if(dtcoreos.Rows[i][0].ToString() == "1")
                    {
                        message.To.Add(dtcoreos.Rows[i][1].ToString());
                    }
                    else if (dtcoreos.Rows[i][0].ToString() == "2")
                    {
                        message.CC.Add(dtcoreos.Rows[i][1].ToString());
                    }
                    else if (dtcoreos.Rows[i][0].ToString() == "3")
                    {
                        message.Bcc.Add(dtcoreos.Rows[i][1].ToString());
                    }
                }

                //message.To.Add("siniestrossoat@laprotectora.com.pe");
                //message.CC.Add("");
                //message.Bcc.Add("");
                message.Body = body;

                //Attachment attachFile1 = new Attachment(MapPath("~/Adjuntos/TramaSoat_" + Texto + ".xlsx"));
                Attachment attachFile1 = new Attachment(ms, "TramaSoat_" + Texto + ".xlsx", "application/vnd.ms-excel");

                message.Attachments.Add(attachFile1);               
                message.IsBodyHtml = false;
                
                System.Net.Mail.SmtpClient smpt = new System.Net.Mail.SmtpClient();

                smpt.Credentials = new System.Net.NetworkCredential(usu, pass);
                smpt.Host = "smtp.gmail.com";
                smpt.Port = 587;
                //Puerto del SMTP de Gmail
                smpt.EnableSsl = true;

                smpt.Send(message);

                orror.Visible = true;
                lblErrorReg.Text = "Trama enviada con éxito";


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            orror.Visible = false;
            cargarFiltro();

        }

        void cargarFiltro()
        {
            DataTable dt = dat.mysql("CALL SP_TRAMAS_SOAT(0,'" + txtsini.Text + "','" + txtcerti.Text + "','" + txtplaca.Text + "','" + txtdni.Text + "');");

            if (dt.Rows.Count > 0)
            {
                gvData.DataSource = dt;
                gvData.DataBind();
            }
        }

        protected void btnTramaApertura_Click(object sender, EventArgs e)
        {
            orror.Visible = false;
            if (txtsini.Text != "" && txtcerti.Text != "" && txtplaca.Text != "")
            {

                ENVIAR_TRAMA("1");
            }
            else 
            {
                orror.Visible = true;
                lblErrorReg.Text = "Es necesario ingresar el Nº Siniestro | Nº Certificado | Nº Placa";
            }
        }
        protected void btnTramaReserva_Click(object sender, EventArgs e)
        {
            orror.Visible = false;
            if (txtsini.Text != "" && txtcerti.Text != "" && txtplaca.Text != "")
            {

                ENVIAR_TRAMA("2");
            }
            else 
            {
                orror.Visible = true;
                lblErrorReg.Text = "Es necesario ingresar el Nº Siniestro | Nº Certificado | Nº Placa";
            }

        }
        protected void btnTramaLiqui_Click(object sender, EventArgs e)
        {
            orror.Visible = false;
            if (txtsini.Text != "" && txtcerti.Text != "" && txtplaca.Text != "" && txtdni.Text != "")
            {

                ENVIAR_TRAMA("3");
            }
            else 
            {
                orror.Visible = true;
                lblErrorReg.Text = "Es necesario ingresar el Nº Siniestro | Nº Certificado | Nº Placa | Nº Dni";
            }
        }

        protected void lnkbotonCerrar_Click(object sender, EventArgs e)
        {
            orror.Visible = false;
            lblErrorReg.Text = "";
            
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            cargarFiltro();
        }
    }
}