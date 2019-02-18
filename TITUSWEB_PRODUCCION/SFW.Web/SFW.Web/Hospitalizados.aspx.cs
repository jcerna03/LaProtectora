using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SFW.Web
{
    public partial class Hospitalizados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Datos dat = new Datos();
            DataTable dt = dat.mysql("call sp_ver_tablas(19,'" + Session["USUARIO"] + "','','','','','','','','','','','','')");
            string usuario = Convert.ToString(dt.Rows[0][5].ToString());
            string password = Convert.ToString(dt.Rows[0][6].ToString());

            HospitalizadosFrame.Attributes["src"] = "http://www.solben.net/loginV.php?u=" + usuario + "&p=" + password + "&d=clin_hosp.php";
            Image1.Visible = false;

        }
    }
}