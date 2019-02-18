using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;


namespace SFW.Web
{
    public class Datos
    {
        public DataTable mysql(string cadena)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection cn = new MySqlConnection(cadenaConexion);

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadena, cn);
                da.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Dispose();
            }

           
        }

        public DataTable TSegSQL(string cadena)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["seguros"].ConnectionString;
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cadena, cn);
                da.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                da.SelectCommand.CommandTimeout = 1200;
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Dispose();
            }


        }

        public DataTable SQL(string cadena)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            SqlConnection cn = new SqlConnection(cadenaConexion);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cadena, cn);
                da.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Dispose();
            }


        }


        public DataSet MySQL_DS(string cadena)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection cn = new MySqlConnection(cadenaConexion);

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadena, cn);
                da.SelectCommand.CommandType = CommandType.Text;
                DataSet dt = new DataSet();
                da.SelectCommand.CommandTimeout = 1200;
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cn.Dispose();
            }


        }
    }
}