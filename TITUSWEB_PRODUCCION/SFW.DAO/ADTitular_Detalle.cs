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
    public class ADTitular_Detalle
    {
        public Int32 InsertarTitularDetalle(Titular_Detalle titu_deta, int operacion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {

                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("call sp_ver_titu", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", titu_deta.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu_deta.cod_titula);
                        command.Parameters.AddWithValue("var03", titu_deta.categoria);
                        command.Parameters.AddWithValue("var28", titu_deta.depa_id);
                        command.Parameters.AddWithValue("var29", titu_deta.prov_id);
                        command.Parameters.AddWithValue("var30", titu_deta.dist_id);
                        command.Parameters.AddWithValue("var31", titu_deta.direccion);
                        command.Parameters.AddWithValue("var32", titu_deta.email);
                        command.Parameters.AddWithValue("var33", titu_deta.t_fijo);
                        command.Parameters.AddWithValue("var34", titu_deta.t_movil);
                        command.Parameters.AddWithValue("var35", titu_deta.estado_civil);
                        command.Parameters.AddWithValue("var36", titu_deta.edad);
                        command.Parameters.AddWithValue("var37", titu_deta.peso);
                        command.Parameters.AddWithValue("var38", titu_deta.estatura);
                        command.Parameters.AddWithValue("var39", titu_deta.discapacitado);
                        command.Parameters.AddWithValue("var40", titu_deta.consume_alcohol);
                        command.Parameters.AddWithValue("var41", titu_deta.consume_tabaco);
                        command.Parameters.AddWithValue("var42", titu_deta.grupo_sanguineo);
                        command.Parameters.AddWithValue("var43", titu_deta.fch_fincarencia);
                        command.Parameters.AddWithValue("var44", titu_deta.pad);
                        command.Parameters.AddWithValue("var45", titu_deta.dpto);
                        command.Parameters.AddWithValue("var46", titu_deta.rol);
                        command.Parameters.AddWithValue("var47", titu_deta.prog_especial);
                        command.Parameters.AddWithValue("var48", titu_deta.cod_paciente);
                        command.Parameters.AddWithValue("var49", titu_deta.id_paciente);
                        command.Parameters.AddWithValue("var50", titu_deta.basico);
                        command.Parameters.AddWithValue("var51", titu_deta.onco);
                        command.Parameters.AddWithValue("var52", titu_deta.segunda_capa);
                        command.Parameters.AddWithValue("var53", titu_deta.docum);
                        command.Parameters.AddWithValue("var54", titu_deta.afi_nombre);
                        command.Parameters.AddWithValue("var55", titu_deta.afi_apepat);
                        command.Parameters.AddWithValue("var56", titu_deta.afi_apemat);
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

        public Int32 ActualizarTitularDetalle(Titular_Detalle titu_deta, int operacion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("call sp_mod", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", titu_deta.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu_deta.cod_titula);
                        command.Parameters.AddWithValue("var03", titu_deta.categoria);
                        command.Parameters.AddWithValue("var14", titu_deta.depa_id);
                        command.Parameters.AddWithValue("var15", titu_deta.prov_id);
                        command.Parameters.AddWithValue("var16", titu_deta.dist_id);
                        command.Parameters.AddWithValue("var17", titu_deta.direccion);
                        command.Parameters.AddWithValue("var18", titu_deta.email);
                        command.Parameters.AddWithValue("var19", titu_deta.t_fijo);
                        command.Parameters.AddWithValue("var20", titu_deta.t_movil);
                        command.Parameters.AddWithValue("var21", titu_deta.estado_civil);
                        command.Parameters.AddWithValue("var22", titu_deta.edad);
                        command.Parameters.AddWithValue("var23", titu_deta.peso);
                        command.Parameters.AddWithValue("var24", titu_deta.estatura);
                        command.Parameters.AddWithValue("var25", titu_deta.discapacitado);
                        command.Parameters.AddWithValue("var26", titu_deta.consume_alcohol);
                        command.Parameters.AddWithValue("var27", titu_deta.consume_tabaco);
                        command.Parameters.AddWithValue("var28", titu_deta.grupo_sanguineo);
                        command.Parameters.AddWithValue("var29", titu_deta.fch_fincarencia);
                        command.Parameters.AddWithValue("var30", titu_deta.pad);
                        command.Parameters.AddWithValue("var31", titu_deta.dpto);
                        command.Parameters.AddWithValue("var32", titu_deta.rol);
                        command.Parameters.AddWithValue("var33", titu_deta.prog_especial);
                        command.Parameters.AddWithValue("var34", titu_deta.cod_paciente);
                        command.Parameters.AddWithValue("var35", titu_deta.id_paciente);
                        command.Parameters.AddWithValue("var36", titu_deta.basico);
                        command.Parameters.AddWithValue("var37", titu_deta.onco);
                        command.Parameters.AddWithValue("var38", titu_deta.segunda_capa);
                        command.Parameters.AddWithValue("var39", titu_deta.docum);
                        command.Parameters.AddWithValue("var40", titu_deta.afi_nombre);
                        command.Parameters.AddWithValue("var41", titu_deta.afi_apepat);
                        command.Parameters.AddWithValue("var42", titu_deta.afi_apemat);
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

        public Collection<Titular_Detalle> ListarDetallesGrupoFamiliar(int operacion, string cod_cliente, string cod_titula,string categoria)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Titular_Detalle> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cod_cliente);
                    command.Parameters.AddWithValue("var02", cod_titula);
                    command.Parameters.AddWithValue("var03", categoria);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        collection = this.ListaTodoTitularDetalle(reader);
                    }
                }

                return collection;
            }
        }

        public Collection<Titular_Detalle> ListarDetallesdeTitulares(int operacion, string cod_cliente)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Titular_Detalle> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01",cod_cliente);
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        collection = this.ListaTodoTitularDetalle(reader);
                    }
                }

                return collection;
            }
        }

        public Titular_Detalle TraeTitular(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                Titular_Detalle titudeta = null;
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cod_titula);
                    command.Parameters.AddWithValue("var02", categoria);
                    command.Parameters.AddWithValue("var03", cod_cliente);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        titudeta = this.ListaUnTitularDetalle(reader);
                    }
                }

                return titudeta;
            }

        }

        //Metodos de Mapeo entre entidades

        private Collection<Titular_Detalle> ListaTodoTitularDetalle(IDataReader reader)
        {
            Collection<Titular_Detalle> result = new Collection<Titular_Detalle>();
            while (reader.Read())
            {
                Titular_Detalle titudeta = new Titular_Detalle();
                if (!Convert.IsDBNull(reader["cod_cliente"]))
                {
                    titudeta.cod_cliente = Convert.ToString(reader["cod_cliente"]);
                    titudeta.cod_titula = Convert.ToString(reader["cod_titula"]);
                    titudeta.categoria = Convert.ToString(reader["categoria"]);
                    titudeta.categoriaSusalud = Convert.ToString(reader["categoria"]);

                    titudeta.depa_id = Convert.ToString(reader["depa_id"]);
                    titudeta.prov_id = Convert.ToString(reader["prov_id"]);
                    titudeta.dist_id = Convert.ToString(reader["dist_id"]);
                    titudeta.direccion = Convert.ToString(reader["direccion"]);
                    titudeta.email = Convert.ToString(reader["email"]);
                    titudeta.t_fijo = Convert.ToString(reader["t_fijo"]);
                    titudeta.t_movil = Convert.ToString(reader["t_movil"]);
                    titudeta.estado_civil = Convert.ToInt32(reader["estado_civil"]);
                    titudeta.edad = Convert.ToString(reader["edad"]);
                    titudeta.peso = Convert.ToString(reader["peso"]);
                    titudeta.estatura = Convert.ToString(reader["estatura"]);
                    titudeta.discapacitado = Convert.ToString(reader["discapacitado"]);
                    titudeta.consume_alcohol = Convert.ToString(reader["consume_alcohol"]);
                    titudeta.consume_tabaco = Convert.ToString(reader["consume_tabaco"]);
                    titudeta.grupo_sanguineo = Convert.ToString(reader["grupo_sanguineo"]);
                    titudeta.fch_fincarencia = Convert.ToString(reader["fch_fincarencia"]);
                    titudeta.pad = Convert.ToString(reader["pad"]);
                    titudeta.dpto = Convert.ToString(reader["dpto"]);
                    titudeta.rol = Convert.ToString(reader["rol"]);
                    titudeta.prog_especial = Convert.ToString(reader["prog_especial"]);
                    titudeta.cod_paciente = Convert.ToString(reader["cod_paciente"]);
                    titudeta.id_paciente = Convert.ToString(reader["id_paciente"]);
                    titudeta.basico = Convert.ToString(reader["basico"]);
                    titudeta.onco = Convert.ToString(reader["onco"]);
                    titudeta.segunda_capa = Convert.ToString(reader["segunda_capa"]);
                    titudeta.docum = Convert.ToString(reader["docum"]);
                    titudeta.afi_nombre = Convert.ToString(reader["afi_nombre"]);
                    titudeta.afi_apepat = Convert.ToString(reader["afi_apepat"]);
                    titudeta.afi_apemat = Convert.ToString(reader["afi_apemat"]);
                    titudeta.foto = Convert.ToString(reader["foto"]);
                    titudeta.correo1 = Convert.ToString(reader["correo1"]);
                    titudeta.correo2 = Convert.ToString(reader["correo2"]);
                    titudeta.contrato = Convert.ToString(reader["contrato"]);
                    titudeta.cod_afiliado = Convert.ToString(reader["cod_afiliado"]);
                    titudeta.fallecido = Convert.ToString(reader["fallecido"]);
                    titudeta.clasificacion = Convert.ToString(reader["clasif"]);
                    titudeta.contratante = Convert.ToString(reader["contratante"]);


                }
                result.Add(titudeta);
            }
            return result;
        }

        private Titular_Detalle ListaUnTitularDetalle(IDataReader reader)
        {
            Titular_Detalle titudeta = new Titular_Detalle();
            while (reader.Read())
            {
                if (!Convert.IsDBNull(reader["cod_cliente"]))
                {
                    titudeta.cod_cliente = Convert.ToString(reader["cod_cliente"]);
                    titudeta.cod_titula = Convert.ToString(reader["cod_titula"]);
                    titudeta.categoria = Convert.ToString(reader["categoria"]);
                    titudeta.depa_id = Convert.ToString(reader["depa_id"]);
                    titudeta.prov_id = Convert.ToString(reader["prov_id"]);
                    titudeta.dist_id = Convert.ToString(reader["dist_id"]);
                    titudeta.direccion = Convert.ToString(reader["direccion"]);
                    titudeta.email = Convert.ToString(reader["email"]);
                    titudeta.t_fijo = Convert.ToString(reader["t_fijo"]);
                    titudeta.t_movil = Convert.ToString(reader["t_movil"]);
                    titudeta.estado_civil = Convert.ToInt32(reader["estado_civil"]);
                    titudeta.edad = Convert.ToString(reader["edad"]);
                    titudeta.peso = Convert.ToString(reader["peso"]);
                    titudeta.estatura = Convert.ToString(reader["estatura"]);
                    titudeta.discapacitado = Convert.ToString(reader["discapacitado"]);
                    titudeta.consume_alcohol = Convert.ToString(reader["consume_alcohol"]);
                    titudeta.consume_tabaco = Convert.ToString(reader["consume_tabaco"]);
                    titudeta.grupo_sanguineo = Convert.ToString(reader["grupo_sanguineo"]);
                    titudeta.fch_fincarencia = Convert.ToString(reader["fch_fincarencia"]);
                    titudeta.pad = Convert.ToString(reader["pad"]);
                    titudeta.dpto = Convert.ToString(reader["dpto"]);
                    titudeta.rol = Convert.ToString(reader["rol"]);
                    titudeta.prog_especial = Convert.ToString(reader["prog_especial"]);
                    titudeta.cod_paciente = Convert.ToString(reader["cod_paciente"]);
                    titudeta.id_paciente = Convert.ToString(reader["id_paciente"]);
                    titudeta.basico = Convert.ToString(reader["basico"]);
                    titudeta.onco = Convert.ToString(reader["onco"]);
                    titudeta.segunda_capa = Convert.ToString(reader["segunda_capa"]);
                    titudeta.docum = Convert.ToString(reader["docum"]);
                    titudeta.afi_nombre = Convert.ToString(reader["afi_nombre"]);
                    titudeta.afi_apepat = Convert.ToString(reader["afi_apepat"]);
                    titudeta.afi_apemat = Convert.ToString(reader["afi_apemat"]);
                }
            }
            return titudeta;
        }
    }
}
