using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;
using System.Net;
using System.Collections;
using System.Threading;
using Tamir.SharpSsh;
using Tamir.Streams;

namespace SFW.Web
{
    public partial class FichaPersonal : System.Web.UI.Page
    {
        Datos dat = new Datos();

        protected void Page_Load(object sender, EventArgs e)
        {
            string codigocliente = Request.QueryString["cc"];
            string codigoTitular = Request.QueryString["ct"];
            string categoria = Request.QueryString["c"];
            llenaFicha(codigocliente, codigoTitular, categoria);
        }

        void llenaFicha(string dato1, string dato2, string dato3) 
        {

            string path_ruta = "http://www.solben.net/solben/foto/" + dato1 + "/" + dato1 + "-" + dato2 + "-" + dato3 + ".jpg";

            if (RemoteFileExists(path_ruta) == true)
            {
                Image1.ImageUrl = "http://www.solben.net/solben/foto/" + dato1 + "/" + dato1 + "-" + dato2 + "-" + dato3 + ".jpg";
            }

            string ficha = "CALL sp_fill('5577','"+ dato1 +"','"+ dato2 +"','"+ dato3 +"');";
            DataTable dtficha = dat.mysql(ficha);

            
            lblcodigocliente.Text = dtficha.Rows[0][0].ToString();
            lblcodigotitular.Text =  dtficha.Rows[0][1].ToString();
            lblparentesco.Text = dtficha.Rows[0][2].ToString();
            lblplan.Text = dtficha.Rows[0][3].ToString();
            lblcentrodecosto.Text = dtficha.Rows[0][4].ToString();
            //lbltipoDocumento.Text = dtficha.Rows[0][5].ToString();
            lblnumerodedocumento.Text = dtficha.Rows[0][6].ToString();
            lblnombres.Text = dtficha.Rows[0][7].ToString();
            lblapellidopaterno.Text = dtficha.Rows[0][8].ToString();
            lblapellidomaterno.Text = dtficha.Rows[0][9].ToString();

            lblfechanacimiento.Text = dtficha.Rows[0][10].ToString();
            if (dtficha.Rows[0][11].ToString() == "M")
            {
                CheckBox1.Checked = true;
                CheckBox2.Checked = false;
            }
            else
            {
                CheckBox1.Checked = false;
                CheckBox2.Checked = true;
            }
            //lblcorreo.Text = dtficha.Rows[0][12].ToString();
            lblfechadebaja.Text = dtficha.Rows[0][13].ToString();
            lbldepartamento.Text = dtficha.Rows[0][14].ToString();
            lblprovincia.Text = dtficha.Rows[0][15].ToString();
            lbldistrito.Text = dtficha.Rows[0][16].ToString();
            lbldireccion.Text = dtficha.Rows[0][17].ToString();
            lbltelefonofijo.Text = dtficha.Rows[0][18].ToString();
            lbltelefonomovil.Text = dtficha.Rows[0][19].ToString();
            lblfechadealta.Text = dtficha.Rows[0][20].ToString();
            lblfechadecarencia.Text = dtficha.Rows[0][21].ToString();
            //lbledad.Text = dtficha.Rows[0][22].ToString();
            //lblpeso.Text = dtficha.Rows[0][23].ToString();
            //lblestatura.Text = dtficha.Rows[0][24].ToString();
            //lblgruposanguineo.Text = dtficha.Rows[0][25].ToString();
            //lblconsumealcohol.Text = dtficha.Rows[0][26].ToString();
            //lblconsumedrogas.Text = dtficha.Rows[0][27].ToString();
            //lblpersonadiscapacitada.Text = dtficha.Rows[0][28].ToString();
            lblEstado.Text = dtficha.Rows[0][29].ToString();

            if (dato3 == "00")
            {
                lblbeneficiarios.Visible = false;
                gvBeneficiarios.Visible = true;
                string dependientes = "CALL sp_fill('5578','" + dato1 + "','" + dato2 + "','');";
                DataTable dtdependientes = dat.mysql(dependientes);
                gvBeneficiarios.DataSource = dtdependientes;
                gvBeneficiarios.DataBind();
            }
            else
            {
                lblbeneficiarios.Visible = true;
                gvBeneficiarios.Visible = false;
            }

           


            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            sb.Append("window.print();");
            sb.Append("</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", sb.ToString(), false);
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
                    //Any exception will returns false.
                    result = false;
                }
            }
            return result;
        }
    }
}