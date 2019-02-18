using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SFW.Web
{
    public partial class PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PDF"] != null)
            {
                pdfiframe.Attributes["src"] = Request.QueryString["PDF"];
                Image1.Visible = false;
            }
            else 
            {
                Image1.Visible = true;
            }
            
        }
    }
}