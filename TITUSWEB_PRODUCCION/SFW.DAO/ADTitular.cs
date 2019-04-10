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
    public class ADTitular
    {
        public string InsertarTitular(Titular titu, Titular_Detalle titu_deta, Usuario usu, int operacion)
        {

            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_ver_titu", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", titu.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu.cod_titula);
                        command.Parameters.AddWithValue("var03", titu.categoria);
                        command.Parameters.AddWithValue("var04", titu.centro_costo);
                        command.Parameters.AddWithValue("var05", titu.plan);
                        command.Parameters.AddWithValue("var06", titu.afiliado);
                        command.Parameters.AddWithValue("var07", titu.parentesco);
                        command.Parameters.AddWithValue("var08", titu.sexo);
                        command.Parameters.AddWithValue("var09", titu.fch_naci);
                        command.Parameters.AddWithValue("var10", titu.fch_alta);
                        command.Parameters.AddWithValue("var11", titu.fch_baja);
                        command.Parameters.AddWithValue("var12", titu.fch_proc);
                        command.Parameters.AddWithValue("var13", titu.pass);
                        command.Parameters.AddWithValue("var14", titu.email);
                        command.Parameters.AddWithValue("var15", titu.tipo_doc);
                        command.Parameters.AddWithValue("var16", titu.dni);
                        command.Parameters.AddWithValue("var17", titu.madre);
                        command.Parameters.AddWithValue("var18", titu.actividad);
                        command.Parameters.AddWithValue("var19", titu.ubicacion);
                        command.Parameters.AddWithValue("var20", titu.estado_titular);
                        command.Parameters.AddWithValue("var21", titu.capitados);
                        command.Parameters.AddWithValue("var22", titu.financia);
                        command.Parameters.AddWithValue("var23", titu.oncologico);
                        command.Parameters.AddWithValue("var24", titu.dx_onco);
                        command.Parameters.AddWithValue("var25", titu.campo1);
                        command.Parameters.AddWithValue("var26", titu.campo2);
                        command.Parameters.AddWithValue("var27", titu.campo3);
                        command.Parameters.AddWithValue("var43", titu.fch_caren);
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
                        //command.Parameters.AddWithValue("var43", titu_deta.fch_fincarencia);
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
                        command.Parameters.AddWithValue("var57", usu.ID);
                        command.Parameters.AddWithValue("var58", titu_deta.foto);
                        command.Parameters.AddWithValue("var59", titu_deta.correo1);//update 260517
                        command.Parameters.AddWithValue("var60", titu_deta.correo2);//update 260517
                        command.Parameters.AddWithValue("var61", titu_deta.clasificacion);
                        command.Parameters.AddWithValue("var62", titu_deta.contratante);


                        string valida = (String)command.ExecuteScalar();

                        if (valida == "1")
                        {
                            LOGMOVIMIENTOS(titu, titu_deta, usu, "", "80", null, null);
                            return valida;
                        }
                        else
                        {
                            LOGMOVIMIENTOS(titu, titu_deta, usu, "", "80", null, null);
                            return valida;
                        }
                            

                        
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                //Console.WriteLine("Error" + ex.Number + "ha ocurrido" + ex.Message);
                LOGMOVIMIENTOS(titu, titu_deta, usu, ex.Message,"80",null,null);
                conexion.Close();
                conexion.Close();
                command.Dispose();

                return "Error Registro";

                throw;
            }

            finally
            {
                conexion.Close();
                conexion.Dispose();
                command.Dispose();
            }

        }

        public Int32 ActualizarTitular(Titular titu, Titular_Detalle titu_deta, Usuario usu, int operacion,Titular old, Titular_Detalle oldDeta)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_mod", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var03", titu.cod_cliente);
                        command.Parameters.AddWithValue("var01", titu.cod_titula);
                        command.Parameters.AddWithValue("var02", titu.categoria);
                        command.Parameters.AddWithValue("var04", titu.centro_costo);
                        command.Parameters.AddWithValue("var05", titu.plan);
                        command.Parameters.AddWithValue("var06", titu.afiliado);
                        command.Parameters.AddWithValue("var07", titu.parentesco);
                        command.Parameters.AddWithValue("var08", titu.sexo);
                        command.Parameters.AddWithValue("var09", titu.fch_naci);
                        command.Parameters.AddWithValue("var10", titu.fch_alta);
                        command.Parameters.AddWithValue("var11", titu.fch_baja);
                        command.Parameters.AddWithValue("var12", titu.email);
                        command.Parameters.AddWithValue("var13", titu.dni);

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

                        command.Parameters.AddWithValue("var29", titu.fch_caren);
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
                        command.Parameters.AddWithValue("var43", titu.pass);
                        command.Parameters.AddWithValue("var44", titu.campo2);
                        command.Parameters.AddWithValue("var45", titu_deta.foto);
                        command.Parameters.AddWithValue("var46", titu_deta.correo1);//update 260517
                        command.Parameters.AddWithValue("var47", titu_deta.correo2);//update 260517
                        command.Parameters.AddWithValue("var48", usu.ID.ToString());//update 210617
                        command.Parameters.AddWithValue("var49", "0");
                        command.Parameters.AddWithValue("var50", "0");
                        command.Parameters.AddWithValue("var51", titu_deta.clasificacion);
                        command.Parameters.AddWithValue("var52", titu_deta.contratante);


                        command.ExecuteNonQuery();

                        LOGMOVIMIENTOS(titu, titu_deta, usu, "","81",old,oldDeta);

                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                LOGMOVIMIENTOS(titu, titu_deta, usu, ex.Message, "81", null, null);
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
        public Int32 RegistrarFechaRenovacionBaja(Titular titu)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;
            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_fecha_bajas", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("op", 1);
                        command.Parameters.AddWithValue("var01", titu.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu.cod_titula);
                        command.Parameters.AddWithValue("var03", titu.categoria);
                        command.Parameters.AddWithValue("var04", "");
                        command.Parameters.AddWithValue("var05", "");
                        command.Parameters.AddWithValue("var06", titu.fch_baja);
                        command.Parameters.AddWithValue("var07", "");
                        command.Parameters.AddWithValue("var08", "");
                        command.Parameters.AddWithValue("var09", "");
                        command.Parameters.AddWithValue("var10", "");

                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
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

        public Int32 RegistrarFechaRenovacionAlta(Titular titu,Titular_Detalle titudeta)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;
            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_fecha_bajas", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("op", 2);
                        command.Parameters.AddWithValue("var01", titu.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu.cod_titula);
                        command.Parameters.AddWithValue("var03", titudeta.categoriaSusalud);
                        command.Parameters.AddWithValue("var04", DateTime.Now.ToString("dd/MM/yyyy"));
                        command.Parameters.AddWithValue("var05", titu.fch_alta);
                        command.Parameters.AddWithValue("var06", titu.fch_baja);
                        command.Parameters.AddWithValue("var07", titu.dni);
                        command.Parameters.AddWithValue("var08", titu.plan);
                        command.Parameters.AddWithValue("var09", titudeta.contrato);
                        command.Parameters.AddWithValue("var10", titudeta.cod_afiliado);


                        command.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
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

        public Collection<Titular> ListarTitulares(int operacion, string cod_cliente)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Titular> collection;
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
                    command.Parameters.AddWithValue("var02", "0");
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaTodoTitular(reader);

                    }
                }

                return collection;
            }
        }

        public Collection<Titular> ListarTitularesGrupo(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<Titular> collection;
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

                        collection = this.ListaTodoTitular(reader);

                    }
                }

                return collection;
            }
        }

        public Collection<TituList> ListarGrupoFamiliar(int operacion, string cod_cliente, string cod_titula)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<TituList> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cod_titula);
                    command.Parameters.AddWithValue("var02", cod_cliente);
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaBusqueda(reader);

                    }
                }

                return collection;
            }
        }

        public Collection<TituList> BusquedaCli(int operacion, string criterioBusqueda, string cod_cliente)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<TituList> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", criterioBusqueda);
                    command.Parameters.AddWithValue("var02", cod_cliente);
                    command.Parameters.AddWithValue("var03", "0");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaBusqueda(reader);

                    }
                }

                return collection;
            }
        }

        public Collection<TituList> Busqueda(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            Collection<TituList> collection;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cod_titula);
                    command.Parameters.AddWithValue("var02", cod_cliente);
                    command.Parameters.AddWithValue("var03", categoria);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        collection = this.ListaBusqueda(reader);

                    }
                }

                return collection;
            }
        }

        public Titular TraeTitular(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            using (conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                Titular titu = null;
                using (command = new MySqlCommand("sp_fill", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("op", operacion);
                    command.Parameters.AddWithValue("var01", cod_titula);
                    command.Parameters.AddWithValue("var02", categoria);
                    command.Parameters.AddWithValue("var03", cod_cliente);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        titu = this.ListaUnTitular(reader);
                    }
                }

                return titu;
            }

        }

        public Int32 BAJACOMPLETA(Titular titu, Titular_Detalle titu_deta, Usuario usu, string fecha, string operacion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_mod", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", titu.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu.cod_titula);
                        command.Parameters.AddWithValue("var03", fecha);
                        command.Parameters.AddWithValue("var04", titu.categoria);
                        command.Parameters.AddWithValue("var05", "0");
                        command.Parameters.AddWithValue("var06", "0");
                        command.Parameters.AddWithValue("var07", "0");
                        command.Parameters.AddWithValue("var08", "0");
                        command.Parameters.AddWithValue("var09", "0");
                        command.Parameters.AddWithValue("var10", "0");
                        command.Parameters.AddWithValue("var11", "0");
                        command.Parameters.AddWithValue("var12", "0");
                        command.Parameters.AddWithValue("var13", "0");

                        command.Parameters.AddWithValue("var14", "0");
                        command.Parameters.AddWithValue("var15", "0");
                        command.Parameters.AddWithValue("var16", "0");
                        command.Parameters.AddWithValue("var17", "0");
                        command.Parameters.AddWithValue("var18", "0");
                        command.Parameters.AddWithValue("var19", "0");
                        command.Parameters.AddWithValue("var20", "0");
                        command.Parameters.AddWithValue("var21", "0");
                        command.Parameters.AddWithValue("var22", "0");
                        command.Parameters.AddWithValue("var23", "0");
                        command.Parameters.AddWithValue("var24", "0");
                        command.Parameters.AddWithValue("var25", "0");
                        command.Parameters.AddWithValue("var26", "0");
                        command.Parameters.AddWithValue("var27", "0");
                        command.Parameters.AddWithValue("var28", "0");

                        command.Parameters.AddWithValue("var29", "0");
                        command.Parameters.AddWithValue("var30", "0");
                        command.Parameters.AddWithValue("var31", "0");
                        command.Parameters.AddWithValue("var32", "0");
                        command.Parameters.AddWithValue("var33", "0");
                        command.Parameters.AddWithValue("var34", "0");
                        command.Parameters.AddWithValue("var35", "0");
                        command.Parameters.AddWithValue("var36", "0");
                        command.Parameters.AddWithValue("var37", "0");
                        command.Parameters.AddWithValue("var38", "0");
                        command.Parameters.AddWithValue("var39", "0");
                        command.Parameters.AddWithValue("var40", "0");
                        command.Parameters.AddWithValue("var41", "0");
                        command.Parameters.AddWithValue("var42", "0");
                        command.Parameters.AddWithValue("var43", "0");
                        command.Parameters.AddWithValue("var44", "0");
                        command.Parameters.AddWithValue("var45", "0");
                        command.Parameters.AddWithValue("var46", "0");
                        command.Parameters.AddWithValue("var47", "0");
                        command.Parameters.AddWithValue("var48", "0");
                        command.Parameters.AddWithValue("var49", "0");
                        command.Parameters.AddWithValue("var50", "0");
                        command.Parameters.AddWithValue("var51", "0");
                        command.Parameters.AddWithValue("var52", "0");

                        command.ExecuteNonQuery();

                        LOGMOVIMIENTOS(titu, titu_deta, usu, "","82",null,null);

                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                LOGMOVIMIENTOS(titu, titu_deta, usu, ex.Message,"80",null,null);
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

        public Int32 ACTIVAR(Titular titu, Titular_Detalle titu_deta, Usuario usu, string operacion)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
            MySqlConnection conexion = null;
            MySqlCommand command = null;

            try
            {
                using (conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (command = new MySqlCommand("sp_mod", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("op", operacion);
                        command.Parameters.AddWithValue("var01", titu.cod_cliente);
                        command.Parameters.AddWithValue("var02", titu.cod_titula);
                        command.Parameters.AddWithValue("var03", titu.categoria);
                        command.Parameters.AddWithValue("var04", titu.fch_alta);
                        command.Parameters.AddWithValue("var05", titu.fch_baja);
                        command.Parameters.AddWithValue("var06", "0");
                        command.Parameters.AddWithValue("var07", "0");
                        command.Parameters.AddWithValue("var08", "0");
                        command.Parameters.AddWithValue("var09", "0");
                        command.Parameters.AddWithValue("var10", "0");
                        command.Parameters.AddWithValue("var11", "0");
                        command.Parameters.AddWithValue("var12", "0");
                        command.Parameters.AddWithValue("var13", "0");

                        command.Parameters.AddWithValue("var14", "0");
                        command.Parameters.AddWithValue("var15", "0");
                        command.Parameters.AddWithValue("var16", "0");
                        command.Parameters.AddWithValue("var17", "0");
                        command.Parameters.AddWithValue("var18", "0");
                        command.Parameters.AddWithValue("var19", "0");
                        command.Parameters.AddWithValue("var20", "0");
                        command.Parameters.AddWithValue("var21", "0");
                        command.Parameters.AddWithValue("var22", "0");
                        command.Parameters.AddWithValue("var23", "0");
                        command.Parameters.AddWithValue("var24", "0");
                        command.Parameters.AddWithValue("var25", "0");
                        command.Parameters.AddWithValue("var26", "0");
                        command.Parameters.AddWithValue("var27", "0");
                        command.Parameters.AddWithValue("var28", "0");

                        command.Parameters.AddWithValue("var29", "0");
                        command.Parameters.AddWithValue("var30", "0");
                        command.Parameters.AddWithValue("var31", "0");
                        command.Parameters.AddWithValue("var32", "0");
                        command.Parameters.AddWithValue("var33", "0");
                        command.Parameters.AddWithValue("var34", "0");
                        command.Parameters.AddWithValue("var35", "0");
                        command.Parameters.AddWithValue("var36", "0");
                        command.Parameters.AddWithValue("var37", "0");
                        command.Parameters.AddWithValue("var38", "0");
                        command.Parameters.AddWithValue("var39", "0");
                        command.Parameters.AddWithValue("var40", "0");
                        command.Parameters.AddWithValue("var41", "0");
                        command.Parameters.AddWithValue("var42", "0");
                        command.Parameters.AddWithValue("var43", "0");
                        command.Parameters.AddWithValue("var44", "0");
                        command.Parameters.AddWithValue("var45", "0");
                        command.Parameters.AddWithValue("var46", "0");
                        command.Parameters.AddWithValue("var47", "0");
                        command.Parameters.AddWithValue("var48", "0");
                        command.Parameters.AddWithValue("var49", "0");
                        command.Parameters.AddWithValue("var50", "0");
                        command.Parameters.AddWithValue("var51", "0");
                        command.Parameters.AddWithValue("var52", "0");


                        command.ExecuteNonQuery();

                        LOGMOVIMIENTOS(titu, titu_deta, usu, "", "376", null, null);

                        return 1;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                LOGMOVIMIENTOS(titu, titu_deta, usu, ex.Message, "81", null, null);
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

        //Metodos de Mapeo entre entidades

        private Collection<Titular> ListaTodoTitular(IDataReader reader)
        {
            Collection<Titular> result = new Collection<Titular>();
            while (reader.Read())
            {
                Titular titu = new Titular();
                if (!Convert.IsDBNull(reader["cod_cliente"]))
                {
                    titu.cod_cliente = Convert.ToString(reader["cod_cliente"]);
                    titu.cod_titula = Convert.ToString(reader["cod_titula"]);
                    titu.categoria = Convert.ToString(reader["categoria"]);
                    titu.centro_costo = Convert.ToString(reader["cent_costo"]);
                    titu.plan = Convert.ToInt32(reader["plan"]);
                    titu.afiliado = Convert.ToString(reader["afiliado"]);
                    titu.parentesco = Convert.ToString(reader["parentesco"]);
                    titu.sexo = Convert.ToString(reader["sexo"]);
                    titu.fch_naci = Convert.ToString(reader["fch_naci"]);
                    titu.fch_alta = Convert.ToString(reader["fch_alta"]);
                    titu.fch_baja = Convert.ToString(reader["fch_baja"]);
                    titu.fch_proc = Convert.ToString(reader["fch_proc"]);
                    titu.fch_caren = Convert.ToString(reader["fch_caren"]);
                    titu.pass = Convert.ToString(reader["pass"]);
                    titu.email = Convert.ToString(reader["email"]);
                    titu.tipo_doc = Convert.ToInt32(reader["tipo_doc"]);
                    titu.dni = Convert.ToString(reader["dni"]);
                    titu.madre = Convert.ToString(reader["madre"]);
                    titu.actividad = Convert.ToString(reader["actividad"]);
                    titu.ubicacion = Convert.ToString(reader["ubicacion"]);
                    titu.estado_titular = Convert.ToInt32(reader["estado_titular"]);
                    titu.capitados = Convert.ToString(reader["capitados"]);
                    titu.financia = Convert.ToString(reader["financia"]);
                    titu.oncologico = Convert.ToString(reader["oncologico"]);
                    titu.dx_onco = Convert.ToString(reader["dx_onco"]);
                    titu.campo1 = Convert.ToString(reader["campo1"]);
                    titu.campo2 = Convert.ToString(reader["campo2"]);
                    titu.campo3 = Convert.ToString(reader["campo3"]);

                }
                result.Add(titu);
            }
            return result;
        }

        private Collection<TituList> ListaBusqueda(IDataReader reader)
        {
            Collection<TituList> result = new Collection<TituList>();
            while (reader.Read())
            {
                TituList titu = new TituList();
                if (!Convert.IsDBNull(reader["cod_cliente"]))
                {
                    titu.cod_cliente = Convert.ToString(reader["cod_cliente"]);
                    titu.cod_cliente2 = Convert.ToString(reader["cod_cliente2"]);
                    titu.cod_titula = Convert.ToString(reader["cod_titula"]);
                    titu.dni = Convert.ToString(reader["dni"]);
                    titu.categoria = Convert.ToString(reader["categoria"]);
                    titu.centro_costo = Convert.ToString(reader["cent_costo"]);
                    titu.plan = Convert.ToInt32(reader["plan"]);
                    titu.afiliado = Convert.ToString(reader["afiliado"]);
                    titu.parentesco = Convert.ToString(reader["categoria1"]);
                    titu.sexo = Convert.ToString(reader["sexo"]);
                    titu.edad = Convert.ToString(reader["edad"]);
                    titu.fch_naci = Convert.ToString(reader["fch_naci"]);
                    titu.fch_alta = Convert.ToString(reader["fch_alta"]);
                    titu.fch_baja = Convert.ToString(reader["fch_baja"]);
                    titu.fch_caren = Convert.ToString(reader["fch_caren"]);
                    titu.Estado = Convert.ToString(reader["Estado"]);
                    titu.Color = Convert.ToString(reader["Color"]);
                    titu.Reporte = Convert.ToString(reader["Reporte_ln"]);
                    titu.clasif = Convert.ToString(reader["clasif"]);

                }
                result.Add(titu);
            }
            return result;
        }

        private Titular ListaUnTitular(IDataReader reader)
        {
            Titular titu = new Titular();
            while (reader.Read())
            {
                if (!Convert.IsDBNull(reader["cod_cliente"]))
                {
                    titu.cod_cliente = Convert.ToString(reader["cod_cliente"]);
                    titu.cod_titula = Convert.ToString(reader["cod_titula"]);
                    titu.categoria = Convert.ToString(reader["categoria"]);
                    titu.centro_costo = Convert.ToString(reader["cent_costo"]);
                    titu.plan = Convert.ToInt32(reader["plan"]);
                    titu.afiliado = Convert.ToString(reader["afiliado"]);
                    titu.parentesco = Convert.ToString(reader["categoria1"]);
                    titu.sexo = Convert.ToString(reader["sexo"]);
                    titu.fch_naci = Convert.ToString(reader["fch_naci"]);
                    titu.fch_alta = Convert.ToString(reader["fch_alta"]);
                    titu.fch_baja = Convert.ToString(reader["fch_baja"]);
                    titu.fch_proc = Convert.ToString(reader["fch_proc"]);
                    titu.fch_caren = Convert.ToString(reader["fch_caren"]);
                    titu.pass = Convert.ToString(reader["pass"]);
                    titu.email = Convert.ToString(reader["email"]);
                    titu.tipo_doc = Convert.ToInt32(reader["tipo_doc"]);
                    titu.dni = Convert.ToString(reader["dni"]);
                    titu.madre = Convert.ToString(reader["madre"]);
                    titu.actividad = Convert.ToString(reader["actividad"]);
                    titu.ubicacion = Convert.ToString(reader["ubicacion"]);
                    titu.estado_titular = Convert.ToInt32(reader["estado_titular"]);
                    titu.capitados = Convert.ToString(reader["capitados"]);
                    titu.financia = Convert.ToString(reader["financia"]);
                    titu.oncologico = Convert.ToString(reader["oncologico"]);
                    titu.dx_onco = Convert.ToString(reader["dx_onco"]);
                    titu.campo1 = Convert.ToString(reader["campo1"]);
                    titu.campo2 = Convert.ToString(reader["campo2"]);
                    titu.campo3 = Convert.ToString(reader["campo3"]);
                }
            }
            return titu;
        }

        public void LOGMOVIMIENTOS(Titular titu, Titular_Detalle titudeta, Usuario usu, string error,string accion,Titular old,Titular_Detalle oldDeta)
        {
            Log lg = new Log();

            lg.ID_USU = usu.ID; lg.ID_PROVINCIA = usu.SEDE;          

            if (error == "")
            {
                switch (accion)
                {
                    case "80":
                        lg.DESCRIPCION = "CAMPOS INGRESADOS:/NOMBRE:" + titudeta.afi_apepat + " " + titudeta.afi_apemat + "," + titudeta.afi_nombre + "/DNI:" + titu.dni + "/NACIMIENTO:" + titu.fch_naci +
                        "/DIRECCION:" + titudeta.direccion + "PARENTESCO:" + titu.parentesco + "ESTADO CIVIL:" + titudeta.estado_civil;

                        lg.MOVIMIENTO = "REGISTRO DE NUEVO AFILIADO " + Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                        lg.TIP_LOG = accion;


                        break;

                    case "81":

                        if ((old != null) && (oldDeta != null))
                        {

                            //if (old.plan.ToString() == "1" && titu.plan.ToString() == "2" && (titu.cod_cliente == "90" || titu.cod_cliente == "98" || titu.cod_cliente == "96" || titu.cod_cliente == "95"))
                            if ((!(oldDeta.onco == "C" || oldDeta.onco == "P")) && (old.plan.ToString() == "1" && titu.plan.ToString() == "2") && (titu.cod_cliente == "90" || titu.cod_cliente == "98" || titu.cod_cliente == "96" || titu.cod_cliente == "95"))
                            
                            
                            {
                                Log lg1 = new Log();
                                lg1.ID_USU = usu.ID; lg1.ID_PROVINCIA = usu.SEDE;  
                                lg1.DESCRIPCION = "PLAN " + old.plan.ToString() + " a PLAN " + titu.plan.ToString();
                                lg1.MOVIMIENTO = "CAMBIO DE PLAN " + Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) +"/"+ Convert.ToString(DateTime.Now.ToShortTimeString());
                                lg1.TIP_LOG = "84";
                                lg1.VALOR01 = old.plan + "-" + titu.plan + "-" + DateTime.Now.AddMonths(3).ToString("dd/MM/yyyy");
                                lg1.FECREG = DateTime.Today.ToString("yyyy-MM-dd");
                                lg1.DNI_AFI = titu.dni;
                                lg1.COD_AFI = Convert.ToString(titu.cod_titula);
                                lg1.CAT_AFI = Convert.ToString(titu.categoria);
                                lg1.COD_CLI = Convert.ToString(titu.cod_cliente);

                                int logcorrecto2 = new ADLOG().InsertarLog(lg1, 10);
                            }


                            lg.DESCRIPCION = "ANTERIOR: " + old.cod_cliente.ToString() + ";" + old.cod_titula.ToString() + ";" + old.categoria.ToString() + ";" + old.dni.ToString() + ";" + old.centro_costo.ToString() + ";" +
                                                            old.estado_titular.ToString() + ";" + old.fch_naci.ToString() + old.fch_alta.ToString() + ";" + old.fch_baja.ToString() + ";" + old.fch_caren.ToString() + ";" +
                                                            old.parentesco.ToString() + old.plan.ToString() +
                                            " NUEVO: " + titu.cod_cliente.ToString() + ";" + titu.cod_titula.ToString() + ";" + titu.categoria.ToString() + ";" + titu.dni.ToString() + ";" + titu.centro_costo.ToString() + ";" +
                                                            titu.estado_titular.ToString() + ";" + titu.fch_naci.ToString() + titu.fch_alta.ToString() + ";" + titu.fch_baja.ToString() + ";" + titu.fch_caren.ToString() + ";" +
                                                            titu.parentesco.ToString() + titu.plan.ToString();
                            lg.MOVIMIENTO = "ACTUALIZACION DE AFILIADO " + Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                            lg.TIP_LOG = accion;
                            lg.VALOR01 = DateTime.Now.ToString("dd/MM/yyyy");
                        }

                        break;

                    case "82":

                        lg.DESCRIPCION = "DAR DE BAJA";

                        lg.MOVIMIENTO = Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                        lg.TIP_LOG = accion;

                        break;

                    case "83":

                        lg.DESCRIPCION = "ERROR DEL SISTEMA";

                        lg.MOVIMIENTO = Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                        lg.TIP_LOG = accion;

                        break;

                    case "84":

                        lg.DESCRIPCION = "CAMBIO DE PLAN";

                        lg.MOVIMIENTO = Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                        lg.TIP_LOG = accion;

                        break;
                    case "376":

                        lg.DESCRIPCION = "ACTIVACION DE AFILIADO";

                        lg.MOVIMIENTO = Convert.ToString(DateTime.Today.ToString("yyyy-MM-dd")) + "/" + Convert.ToString(DateTime.Now.ToShortTimeString());
                        lg.TIP_LOG = accion;

                        break;
                }
            }
            else
            {
                lg.DESCRIPCION = error;
            }

            lg.FECREG = DateTime.Today.ToString("yyyy-MM-dd");
            lg.DNI_AFI = titu.dni;
            lg.COD_AFI = Convert.ToString(titu.cod_titula);
            lg.CAT_AFI = Convert.ToString(titu.categoria);
            lg.COD_CLI = Convert.ToString(titu.cod_cliente);

            int logcorrecto = new ADLOG().InsertarLog(lg, 10);

        }

    }

    }