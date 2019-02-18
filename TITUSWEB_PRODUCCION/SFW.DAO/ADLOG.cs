using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SFW.BE;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace SFW.DAO
{
    public class ADLOG
    {
        public Int32 InsertarLog(Log logreg,int operacion)
        {

            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_ver_tablas", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", logreg.ID_USU);
                        command.Parameters.AddWithValue("var02", logreg.ID_PROVINCIA);
                        command.Parameters.AddWithValue("var03", logreg.DESCRIPCION);
                        command.Parameters.AddWithValue("var04", logreg.MOVIMIENTO);
                        command.Parameters.AddWithValue("var05", logreg.FECREG);
                        command.Parameters.AddWithValue("var06", logreg.DNI_AFI);
                        command.Parameters.AddWithValue("var07", logreg.COD_AFI);
                        command.Parameters.AddWithValue("var08", logreg.CAT_AFI);
                        command.Parameters.AddWithValue("var09", logreg.COD_CLI);
                        command.Parameters.AddWithValue("var10", logreg.TIP_LOG);
                        command.Parameters.AddWithValue("var11", logreg.VALOR01);
                        command.Parameters.AddWithValue("var12", "0");
                        command.Parameters.AddWithValue("var13", "0");
                        command.ExecuteNonQuery();

                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Error" + ex.Number + "ha ocurrido" + ex.Message);
                conexion.Close();
                conexion.Close();
                command.Dispose();

                return 0;

                throw;
            }

            finally
            {
                conexion.Close();
                conexion.Dispose();
                command.Dispose();
            }

        }
    }
}
