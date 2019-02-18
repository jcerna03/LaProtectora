using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SFW.BE;
using System.Data;
using System.Data.SqlClient;

namespace SFW.Web
{
    public partial class Sesion : System.Web.UI.Page
    {
        Datos dat = new Datos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["usu"]))
                    {
                        Session["usu"] = Request.QueryString["usu"].ToString();
                        Session["pass"] = Request.QueryString["pass"].ToString();
                        Session["afiliado"] = "0";
                        if (!string.IsNullOrEmpty(Request.QueryString["afi"]))
                        {
                            Session["afiliado"] = Request.QueryString["afi"].ToString();
                        }

                        if ((Session["usu"].ToString() != "") && (Session["pass"].ToString() != ""))
                        {
                            iniciar(Session["usu"].ToString(), Session["pass"].ToString(), Session["afiliado"].ToString());
                        }

                    }
                }
                catch (Exception)
                {

                }


            }
        }

        string perfil_id = "";
        void iniciar(string usu, string pass, string afi)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["usu"]))
                {
                    string xxSQL = "sp_ver_14 17,'" + usu + "','" + pass + "'";

                    DataTable xxDt = dat.TSegSQL(xxSQL);
                    usu = xxDt.Rows[0]["USU"].ToString();
                    pass = xxDt.Rows[0]["PASS"].ToString();
                }

                DataTable dt = dat.mysql("call sp_ver_tablas(20,'" + usu + "','" + pass + "','','','','','','','','','','','')");

                if (dt.Rows.Count != 0)
                {
                    Session["DATOS"] = dt;
                    Session["afiliado"] = afi;
                    Session["USUARIO"] = Convert.ToString(dt.Rows[0][0].ToString());
                    Session["USU_NOM"] = Convert.ToString(dt.Rows[0]["nombre"].ToString());
                    Session["PASS"] = Convert.ToString(dt.Rows[0]["UsuClaveSolben"].ToString());
                    Session["USUARIO_USU"] = Convert.ToString(dt.Rows[0]["Usuario"].ToString());
                    Session.Timeout = 120;
                    perfil_id = dt.Rows[0][5].ToString();

                    if (dt.Rows[0]["clientes"].ToString() == "62")
                    {
                        Response.Redirect("GestionVoip.aspx", false);
                    }
                    else
                    {
                        if (perfil_id == "11")
                        {
                            Response.Redirect("GestionVoip.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("wfMantenimiento.aspx", false);
                        }

                    }

                }
                else
                {
                    lblalerta.Text = "Usuario o Contraseña Incorrectos";
                }
            }
            catch (Exception ex)
            {
                lblalerta.Text = ex.Message.ToString();
            }






        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            iniciar(txtUsuario.Text, txtPassword.Text, "0");
        }
    }
}