using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFW.BE;
using SFW.BL;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Data;
using System.IO;

namespace SFW.Web
{
    public class Util
    {
        public static Usuario ValidarUsuario(string usu, string pwd)
        {
            List<Usuario> list = new UsuarioBL().ListarUsuarios(16);
            Usuario objUser = list.Find(delegate(Usuario obj) { return (obj.USER.Equals(usu) && (obj.PASS.Equals(pwd))); });
            if (objUser != null)
                return objUser;
            return null;
        }

        public static Boolean EnvioCorreo(string dominio, string desde,string contra, string hacia,string asunto,string cuerpo,List<string> adjuntos)
        {

            //string smtpAddress = "smtp.mail.yahoo.com";
            string smtpAddress = dominio;
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = desde;
            string password = contra;
            string emailTo = hacia;
            string subject = asunto;
            string body = cuerpo;

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;

                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //// Can set to false, if you are sending pure text.
                    //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                    //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));
                    foreach (string item in adjuntos)
                    {
                        mail.Attachments.Add(new System.Net.Mail.Attachment(item));
                    }
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                return false;
            }

            

        }

        public static DataTable unionDataTables(DataTable dt1, DataTable dt2)
        {
            dt1.Merge(dt2);
            return dt1;
        }

        //FUNCIONA CON FTP
        public static Boolean CheckIfFileExistsOnServer(string fileName, string ruta, string usuario, string pass)
        {
            var request = (FtpWebRequest)WebRequest.Create(ruta + fileName);
            request.Credentials = new NetworkCredential(usuario, pass);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
        }

        public static DataTable GenerarDataTable(string ruta)
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


        public static string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
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


    }    
}