using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.BE;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace SFW.DAO
{
    public class ADCombo
    {
        public Collection<Combo> ListaCombos(int operacion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Combo> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", "0");
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaTodoLosDatosDeCombo(reader);

                    }
                }

                return collection;
            }
        }

        public Collection<Combo> ListaCombos(int operacion,string cliente)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Combo> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cliente);
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaTodoLosDatosDeCombo(reader);

                    }
                }

                return collection;
            }
        }

        private Collection<Combo> ListaTodoLosDatosDeCombo(IDataReader reader)
        {
            Collection<Combo> result = new Collection<Combo>();
            while (reader.Read())
            {
                Combo cbo = new Combo();
                if (!Convert.IsDBNull(reader["valor"]))
                {
                    cbo.valor = Convert.ToString(reader["valor"]);
                    cbo.descrip = Convert.ToString(reader["descrip"]);
                }
                result.Add(cbo);
            }
            return result;
        }
    }
}
