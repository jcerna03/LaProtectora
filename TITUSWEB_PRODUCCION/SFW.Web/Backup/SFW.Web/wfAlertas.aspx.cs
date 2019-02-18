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
    public partial class wfAlertas : System.Web.UI.Page
    {
        Datos dat = new Datos();
        DataTable dt = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        void MEF(string operacion)
        {
            //string xSQL = "CALL SP_alertHijos('1','','1','');";

            //DataTable dt = dat.mysql(xSQL);

            //gvDatosDetalle1.DataSource = dt;
            //gvDatosDetalle1.DataBind();

            string SSQLX = "call sp_fill (22,0,0,0)";
            dt = dat.mysql(SSQLX);
            gvAlertasMEF23.DataSource = dt;
            gvAlertasMEF23.DataBind();

            SSQLX = "";

            SSQLX = "call sp_fill (23,0,0,0)";
            dt = dat.mysql(SSQLX);
            gvAlertasMEF70.DataSource = dt;
            gvAlertasMEF70.DataBind();


        }
    }
}