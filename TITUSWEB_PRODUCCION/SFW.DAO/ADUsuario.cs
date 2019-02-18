using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.BE;
using System.Configuration;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace SFW.DAO
{
    public class ADUsuario
    {
        public Collection<Usuario> ListarUsuarios(int operacion)
        {

            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
             Collection<Usuario> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_ver_tablas",conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", "0");
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");
                    command.Parameters.AddWithValue("var04", "0");
                    command.Parameters.AddWithValue("var05", "0");
                    command.Parameters.AddWithValue("var06", "0");
                    command.Parameters.AddWithValue("var07", "0");
                    command.Parameters.AddWithValue("var08", "0");
                    command.Parameters.AddWithValue("var09", "0");
                    command.Parameters.AddWithValue("var10", "0");
                    command.Parameters.AddWithValue("var11", "0");
                    command.Parameters.AddWithValue("var12", "0");
                    command.Parameters.AddWithValue("var13", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaTodosUsuarios(reader);

                    }
                }

                return collection;
            }
        }

        public Usuario ObtieneUsuario(int operacion,int id)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                Usuario usu = null;
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", id);
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        usu = this.ListaUnUsuario(reader);
                    }
                }

                return usu;
            }
        }

        private Collection<Usuario> ListaTodosUsuarios(IDataReader reader)
        {
            Collection<Usuario> result = new Collection<Usuario>();
            while (reader.Read())
            {
                Usuario usu = new Usuario();
                if (!Convert.IsDBNull(reader["ID"]))
                {
                    usu.ID = Convert.ToInt32(reader["ID"]);
                    usu.USER = Convert.ToString(reader["USER"]);
                    usu.PASS = Convert.ToString(reader["PASS"]);
                    usu.SEDE = Convert.ToString(reader["SEDE"]);
                    usu.PERMISOS = Convert.ToString(reader["PERMISOS"]);
                    usu.NOMBRE = Convert.ToString(reader["NOMBRE"]);
                    usu.ROL = Convert.ToString(reader["ROL"]);
                    usu.PERFIL = Convert.ToString(reader["PERFIL"]);
                    usu.AVERIA = Convert.ToString(reader["AVERIA"]);
                }
                result.Add(usu);
            }
            return result;            
        }

        private Usuario ListaUnUsuario(IDataReader reader)
        {
            Usuario usu = new Usuario();
            while (reader.Read())
            {
                if (!Convert.IsDBNull(reader["ID"]))
                {
                    usu.ID = Convert.ToInt32(reader["ID"]);
                    usu.USER = Convert.ToString(reader["USER"]);
                    usu.PASS = Convert.ToString(reader["PASS"]);
                    usu.SEDE = Convert.ToString(reader["SEDE"]);
                    usu.PERMISOS = Convert.ToString(reader["PERMISOS"]);
                    usu.NOMBRE = Convert.ToString(reader["NOMBRE"]);
                    usu.ROL = Convert.ToString(reader["ROL"]);
                    usu.PERFIL = Convert.ToString(reader["PERFIL"]);
                    usu.AVERIA = Convert.ToString(reader["AVERIA"]);
                }
            }
            return usu;
        }
 


    }
}
