﻿using IBM.WMQ;
using pe.gob.susalud.jr.transaccion.susalud.bean;
using pe.gob.susalud.jr.transaccion.susalud.service.imp;
using SFW.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace SFW.Web
{
    public class ReporteSUSALUD
    {
        Datos dat = new Datos();
        RegafiUpdate271ServiceImpl impl = new RegafiUpdate271ServiceImpl();
        RegafiUpdate997ServiceImpl impl2 = new RegafiUpdate997ServiceImpl();
        String x12n = "";
        MQQueueManager mqQMgr;             //* MQQueueManager instance
        string qmgrName;
        MQQueue mqQueue;                      //* MQQueue instance
        string queueName;                     //* Name of queue to use
        MQMessage mqMsg;                       //* MQMessage instance
        MQPutMessageOptions mqPutMsgOpts;     //* MQPutMessageOptions instance   
        MQGetMessageOptions mqGetMsgOpts;      //* MQGetMessageOptions instance
        string strCorrelativo;
        public string EnvioSUSALUD(string op, Titular titu, Titular_Detalle titu_Deta)
        {
            DataTable dtv = dat.mysql("CALL sp_fill_3('6','" + titu.cod_cliente + "','','','','','','','','','');");
            if (dtv.Rows[0][0].ToString() == "0")
            {
                return "";
            }

            DataTable dtParametros = dat.mysql("CALL sp_fill_3('7','" + titu.cod_cliente + "','','','','','','','','','');");
            string var_Hostname = dtParametros.Rows[0]["valor"].ToString();
            string var_Puerto = dtParametros.Rows[0]["val01"].ToString();
            string var_Name = dtParametros.Rows[0]["val02"].ToString();
            string var_Canal = dtParametros.Rows[0]["descrip"].ToString();
            string var_NameIN = dtParametros.Rows[0]["val04"].ToString();
            string var_NameOUT = dtParametros.Rows[0]["val05"].ToString();
            //CONEXION DE LA COLA
            string ConexionColaResult = ConexionIBM(var_Hostname, var_Puerto, var_Name, var_Canal);
            if (ConexionColaResult.Substring(0, 1) == "0")
            {
                return "ERROR: " + ConexionColaResult + " MQ CONEXION";
            }
            //GENERACION DE X12
            string x12Generado = GenerateX12XmlFromBean(op, titu, titu_Deta);
            if (x12Generado.Contains("Validator"))
            {
                string errorPrincipal = getBetween(x12Generado, "<< Validator Código >>[", "]");
                string error1;
                string error2;

                error1 = errorPrincipal.Substring(0, 3);

                if (errorPrincipal.Length == 5)
                {
                    error2 = errorPrincipal.Substring(errorPrincipal.Length - 2, 2);
                }
                else
                {
                    error2 = errorPrincipal.Substring(errorPrincipal.Length - 1, 1);
                }

                string call = "call sp_fill(76,'" + error1 + "','" + error2 + "','');";
                DataTable dtab = dat.mysql(call);

                return "ERROR: " + dtab.Rows[0][0].ToString() + " CREACION X12";
            }

            //PUT MESSAGE MQ
            string putMensajeResult = "";
            //System.Threading.Thread.Sleep(3000);
            //GET MESSAGE MQ
            contador = 0;
            string getMnesajeResult = "";
            while ((getMnesajeResult == "0ERROR: create of MQQueueManager ended with {0}MQRC_NO_MSG_AVAILABLEMQ GET MENSAJE" || getMnesajeResult == ""))
            {
                putMensajeResult = PutMensaje(codificador(x12Generado), var_NameIN);
                if (putMensajeResult.Substring(0, 1) == "0")
                {
                    return "ERROR: " + putMensajeResult + " MQ PUT MENSAJE";
                }
                getMnesajeResult = GetMensaje(var_NameOUT);
                contador++;
            }
            if (putMensajeResult.Substring(0, 1) == "0")
            {
                return "ERROR: " + putMensajeResult + " MQ PUT MENSAJE";
            }
            if (getMnesajeResult.Substring(0, 1) == "0")
            {
                return "ERROR: " + getMnesajeResult + " MQ GET MENSAJE " + contador;
            }
            else
            {
                if (getMnesajeResult.Contains("*AK5*0000") || getMnesajeResult.Contains("*AK5*0002"))
                {
                    string stroe = "CALL SP_SUSALUD('2','" +
                    titu.cod_cliente + "','" + //var01
                    titu.cod_titula + "','" + //var02
                    titu_Deta.categoriaSusalud + "','" + //var03
                    op + "','" + //var04
                    titu_Deta.afi_apepat + "','" + //var05
                    titu_Deta.afi_nombre + "','" + //var06
                    titu_Deta.afi_apemat + "','" + //var07
                    titu.tipo_doc + "','" + //var08
                    titu.dni + "','" + //var09
                    titu_Deta.estado_afiliado + "','" + //var10
                    titu.tipo_doc + "','" + //var11
                    titu.dni + "','" + //var12
                    titu.fch_naci + "','" + //var13
                    titu.sexo + "','" + //var14
                    titu_Deta.fallecido + "','" + //var15
                    titu_Deta.cod_afiliado + "','" + //var16
                    titu_Deta.contrato + "','" + //var17
                    titu_Deta.estado_afiliacion + "','" + //var18
                    titu_Deta.causalBaja + "','" + //var19
                    titu.plan + "','" + //var20
                    titu.fch_alta + "','" + //var21
                    titu.fch_baja + "','" + //var22
                    "PER"+ "','" + //var23
                    strCorrelativo + "','" +//var24
                    "','','','','','');";
                    DataTable dtab1 = dat.mysql(stroe);

                    return getMnesajeResult;
                }
                else
                {
                    string errorprincipal = getBetween(getMnesajeResult, "*   ~AK4*", "*");
                    string errorsecundario = getBetween(getMnesajeResult, "*   ~AK4*" + errorprincipal + "*", "  *PER*");

                    if (errorprincipal == "200 " && errorsecundario == "29" && contador > 1)
                    {

                        string stroe = "CALL SP_SUSALUD('2','" +
                            titu.cod_cliente + "','" + //var01
                            titu.cod_titula + "','" + //var02
                            titu_Deta.categoriaSusalud + "','" + //var03
                            op + "','" + //var04
                            titu_Deta.afi_apepat + "','" + //var05
                            titu_Deta.afi_nombre + "','" + //var06
                            titu_Deta.afi_apemat + "','" + //var07
                            titu.tipo_doc + "','" + //var08
                            titu.dni + "','" + //var09
                            titu_Deta.estado_afiliado + "','" + //var10
                            titu.tipo_doc + "','" + //var11
                            titu.dni + "','" + //var12
                            titu.fch_naci + "','" + //var13
                            titu.sexo + "','" + //var14
                            titu_Deta.fallecido + "','" + //var15
                            titu_Deta.cod_afiliado + "','" + //var16
                            titu_Deta.contrato + "','" + //var17
                            titu_Deta.estado_afiliacion + "','" + //var18
                            titu_Deta.causalBaja + "','" + //var19
                            titu.plan + "','" + //var20
                            titu.fch_alta + "','" + //var21
                            titu.fch_baja + "','" + //var22
                            "PER" + "','" + //var23
                            strCorrelativo + "','" +//var24
                            "','','','','','');";
                        DataTable dtab1 = dat.mysql(stroe);

                        return getMnesajeResult;


                    }
                    else
                    {

                        string call = "call sp_fill(76,'" + errorprincipal.Trim() + "','" + errorsecundario.Trim() + "','');";
                        DataTable dtab = dat.mysql(call);
                        string call2 = "CALL sp_fill_3('9','" + strCorrelativo + "','" + "ERROR: " + dtab.Rows[0][0].ToString() + " RESPUESTA SUSALUD" + "','','','','','','','','');";
                        DataTable dtab2 = dat.mysql(call2);
                        return "ERROR: " + dtab.Rows[0][0].ToString() + " RESPUESTA SUSALUD";
                    }

                }
            }
            return x12Generado;
        }
        public string GenerateX12XmlFromBean(string operacion, Titular titu, Titular_Detalle titu_deta)
        {
            string nombre1 = "";
            string paterno = "";
            string materno = "";

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytespaterno = utf8.GetBytes(titu_deta.afi_apepat);
            byte[] utfBytesnombre = utf8.GetBytes(titu_deta.afi_nombre);
            byte[] utfBytesmaterno = utf8.GetBytes(titu_deta.afi_apemat);

            byte[] isoBytespaterno = Encoding.Convert(utf8, iso, utfBytespaterno);
            byte[] isoBytesnombre = Encoding.Convert(utf8, iso, utfBytesnombre);
            byte[] isoBytesmaterno = Encoding.Convert(utf8, iso, utfBytesmaterno);

            nombre1 = iso.GetString(isoBytesnombre);
            paterno = iso.GetString(isoBytespaterno);
            materno = iso.GetString(isoBytesmaterno);


            string x12resultado = "";

            string call = "CALL SP_SUSALUD('" + "1" + "','" +
                titu.cod_cliente + "','" + //var01
                titu.cod_titula + "','" + //var02
                titu_deta.categoriaSusalud + "','" + //var03
                operacion + "','" + //var04
                paterno + "','" + //var05
                nombre1 + "','" + //var06
                materno + "','" + //var07
                titu.tipo_doc + "','" + //var08
                titu.dni + "','" + //var09
                titu_deta.estado_afiliado + "','" + //var10
                titu.tipo_doc + "','" + //var11
                titu.dni + "','" + //var12
                titu.fch_naci + "','" + //var13
                titu.sexo + "','" + //var14
                titu_deta.fch_falleci + "','" + //var15
                titu_deta.cod_afiliado + "','" + //var16
                titu_deta.contrato + "','" + //var17
                titu_deta.estado_afiliacion + "','" + //var18
                titu_deta.causalBaja + "','" + //var19
                titu.plan + "','" + //var20
                titu.fch_alta + "','" + //var21
                titu.fch_baja + "','" + //var22
                "','','','','','','','');";
            DataTable dtab = dat.mysql(call);
            string x12_1x1 = "";
            string x12_1x1_xml = "";

            int fila = 0;
            foreach (DataRow row in dtab.Rows)
            {
                In271RegafiUpdate entidad = new In271RegafiUpdate();
                java.util.ArrayList listaAfiliados = new java.util.ArrayList();
                java.util.ArrayList listaAfiliaciones = new java.util.ArrayList();
                In271RegafiUpdateAfiliado afiliado = new In271RegafiUpdateAfiliado();
                In271RegafiUpdateAfiliacion afiliacion = new In271RegafiUpdateAfiliacion();

                //ENTIDAD GENERAL            
                entidad.setNoTransaccion(dtab.Rows[0]["01_transaccion"].ToString());
                entidad.setIdRemitente(dtab.Rows[0]["02_ID_remi_Emis"].ToString());
                entidad.setIdReceptor(dtab.Rows[0]["03_ID_Receptor"].ToString());
                entidad.setFeTransaccion(dtab.Rows[0]["04_Fecha"].ToString());
                entidad.setHoTransaccion(dtab.Rows[0]["05_Hora"].ToString());
                strCorrelativo = dtab.Rows[0]["Correlativo"].ToString();
                entidad.setIdCorrelativo(dtab.Rows[0]["06_Correlativo"].ToString());
                entidad.setIdTransaccion(dtab.Rows[0]["07_Iden_uni_Transac"].ToString());
                entidad.setTiFinalidad(dtab.Rows[0]["08_Finalidad"].ToString());
                entidad.setCaRemitente(dtab.Rows[0]["09_Calif_Remi_Emi"].ToString());
                entidad.setTiOperacion(dtab.Rows[0]["10_Tip_Opera"].ToString());
                //AFILIADOS
                afiliado.setApPaternoAfiliado(row["11_ApePat"].ToString());
                afiliado.setNoAfiliado1(row["12_Nom_1"].ToString());
                afiliado.setNoAfiliado2(row["13_Nom_2"].ToString());
                afiliado.setApMaternoAfiliado(row["14_ApeMat"].ToString());
                afiliado.setTiDocumentoAfil(row["15_Cod_Tip_Doc"].ToString());
                afiliado.setNuDocumentoAfil(row["16_Num_Doc"].ToString());
                afiliado.setEstadoAfiliado(row["17_Est_Afi"].ToString());
                afiliado.setTiDocumentoAct(row["18_Cod_Act_Tip_Doc_Afi"].ToString());
                afiliado.setNuDocumentoAct(row["19_Num_Act_Doc_Afi"].ToString());
                afiliado.setApCasadaAfiliado(row["20_Ape_casada"].ToString());
                afiliado.setCoNacionalidad(row["21_Nacionalidad"].ToString());
                afiliado.setUbigeo(row["22_Ubigeo"].ToString());
                afiliado.setFeNacimiento(row["23_Fch_Nac"].ToString());
                afiliado.setGenero(row["24_Genero"].ToString());
                afiliado.setCoPaisDoc(row["25_Cod_Pais_Emi"].ToString());
                afiliado.setFefallecimiento(row["26_Fch_Falleci"].ToString());
                afiliado.setCoPaisEmisorDocAct(row["27_Cod_Act_Pais_Emi"].ToString());
                afiliado.setFeActPersonaxIafas(row["28_Fch_Act_Dat"].ToString());
                afiliado.setIdAfiliadoNombre(row["29_Ident_Nom2"].ToString());
                afiliado.setTiDocTutor(row["30_Cod_Tip_Doc_Apod"].ToString());
                afiliado.setCoPaisEmisorDocTutor(row["34_Cod_Pais_Doc_Apod"].ToString());//PENDIENTE
                afiliado.setNuDocTutor(row["31_Num_Doc_Apod"].ToString());
                afiliado.setTiVinculoTutor(row["32_Vinculo"].ToString());
                afiliado.setFeValidacionReniec(row["33_Fch_Vali_Reniec"].ToString());
                listaAfiliados.add(0, afiliado);
                entidad.setIn271RegafiUpdateAfiliado(listaAfiliados);
                //AFILIACION
                afiliacion.setTiDocTitular(row["35_Tip_Doc_Titu"].ToString());
                afiliacion.setNuDocTitular(row["36_Num_Doc_Titu"].ToString());
                afiliacion.setFeNacimientoTitular(row["37_Fch_Nac_Titu"].ToString());
                afiliacion.setCoPaisEmisorDocTitular(row["38_Cod_Pais_Doc_Titu"].ToString());
                afiliacion.setTiContratante(row["39_Calif_contrat"].ToString());
                afiliacion.setApPaternoContratante(row["40_ApePat_Contrat"].ToString());
                afiliacion.setNoContratante1(row["41_Nom_1_Contrat"].ToString());
                afiliacion.setNoContratante2(row["42_Nom_2_Contrat"].ToString());
                afiliacion.setNoContratante3(row["43_Nom_3_Contrat"].ToString());
                afiliacion.setNoContratante4(row["44_Nom_4_Contrat"].ToString());
                afiliacion.setApMaternoContratante(row["45_Apemat_Contrat"].ToString());
                afiliacion.setTiDocContratante(row["46_Tip_Doc_Contrat"].ToString());
                afiliacion.setNuDocContratante(row["47_Num_Doc_Contrat"].ToString());
                afiliacion.setApCasadaContratante(row["48_ApeCasada_Contrat"].ToString());
                afiliacion.setFeNacimientoContratante(row["49_Fch_Nac_Contrat"].ToString());
                afiliacion.setCoPaisEmisorDocContratante(row["50_Cod_Pais_Doc_Contrat"].ToString());
                afiliacion.setCoAfiliacion(row["51_Cod_Inter_Afi"].ToString());
                afiliacion.setCoContrato(row["52_Cod_Contrato"].ToString());
                afiliacion.setCoUnicoMultisectorial(row["53_Cod_Uni_Multisec"].ToString());
                afiliacion.setTiregimen(row["54_Tip_Reg_Finan"].ToString());
                afiliacion.setEsAfiliacion(row["55_Est_Afiliacion"].ToString());
                afiliacion.setCoCausalBaja(row["56_Causal_Baja"].ToString());
                afiliacion.setTiPlanSalud(row["57_Tip_PlanSalud"].ToString());
                afiliacion.setNoProductoSalud(row["58_Nom_Inter_Prod"].ToString());
                afiliacion.setCoProducto(row["59_Cod_Inter_Prod"].ToString());
                afiliacion.setParentesco(row["60_Tip_Parent_Afi"].ToString());
                afiliacion.setCoRenipress(row["61_Cod_Ascrip_iafas_RENIPRESS"].ToString());
                afiliacion.setPkAfiliado(row["62_PK_Afi"].ToString());
                afiliacion.setFeActEstado(row["63_Fch_Act_Est_Afi"].ToString());
                afiliacion.setFeIniAfiliacion(row["64_Fch_Ini_Afi"].ToString());
                afiliacion.setFeFinAfiliacion(row["65_Fch_Fin_Afi"].ToString());
                afiliacion.setFeIniCobertura(row["66_Fch_Ini_Laten"].ToString());
                afiliacion.setFeFinCobertura(row["67_Fch_Fin_Laten"].ToString());
                afiliacion.setFeActOperacion(row["68_Fch_Act"].ToString());
                afiliacion.setTiActOperacion(row["69_Hora_Act"].ToString());
                afiliacion.setCoTiCobertura(row["70_Cod_Per_Cober"].ToString());
                afiliacion.setIdAfiliacionNombre(row["71_Ident_Nom_Contrat"].ToString());
                listaAfiliaciones.add(0, afiliacion);
                entidad.setIn271ResSctrDetalle(listaAfiliaciones);

                string x12 = impl.beanToX12N(entidad);

                if (x12.Contains("Validator"))
                {
                    x12resultado = x12;
                    break;
                }
                else
                {
                    x12n = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
                    x12n += Environment.NewLine;
                    x12n += @"<sus:Online271RegafiUpdateRequest xmlns:sus=""http://www.susalud.gob.pe/Afiliacion/Online271RegafiUpdateRequest.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.susalud.gob.pe/Afiliacion/Online271RegafiUpdateRequest.xsd ../MsgSetProjOnline271RegafiUpdateRequest/importFiles/Online271RegafiUpdateRequest.xsd "">";
                    x12n += Environment.NewLine;
                    x12n += "<txNombre>271_REGAFI_UPDATE</txNombre>";
                    x12n += Environment.NewLine;
                    x12n += "<txPeticion>";
                    x12n += x12;
                    x12n += "</txPeticion>";
                    x12n += Environment.NewLine;
                    x12n += "</sus:Online271RegafiUpdateRequest>";

                    StringBuilder sb = new StringBuilder();
                    string output = x12n;
                    sb.Append(output);
                    sb.Append("\r\n");

                    x12_1x1_xml += sb.ToString();
                    x12resultado = x12_1x1_xml;
                    x12_1x1 += x12;
                    fila++;
                }
            }
            return x12resultado;
        }
        public string ConexionIBM(string hostname, string puerto, string name, string canal)
        {
            string colaResultado = "";
            MQEnvironment.Hostname = hostname;
            MQEnvironment.Port = Convert.ToInt32(puerto);
            qmgrName = name;
            MQEnvironment.Channel = Convert.ToString(canal);
            try
            {
                if (!MQEnvironment.properties.Contains(MQC.TRANSPORT_PROPERTY))
                {
                    MQEnvironment.properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_CLIENT);
                }

                mqQMgr = new MQQueueManager(qmgrName);

                if (mqQMgr.IsConnected)
                {
                    colaResultado = "1Conecto";
                }
            }
            catch (Exception ex)
            {
                colaResultado = "0" + LeerException(ex);
            }

            return colaResultado;
        }

        public string LeerException(Exception ex)
        {
            string msg = ex.Message;
            if (ex.InnerException != null)
            {
                msg = msg + ":::--- Inner EX.---:::" + LeerException(ex.InnerException);
            }
            return msg;
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
        //POR VERIFICAR SERVER.MAPPATH
        public string PutMensaje(string XmlX12N, string NameIn)
        {
            string MessageResult = "";
            MemoryStream strm = new MemoryStream();
            XSD.Online271RegafiUpdateRequest objXm = new XSD.Online271RegafiUpdateRequest();

            objXm.txNombre = "271_REGAFI_UPDATE";
            objXm.txPeticion = XmlX12N;
            XmlSerializer xa = new XmlSerializer(objXm.GetType());
            string appPath = HttpContext.Current.Server.MapPath("~/xml/");
            string dbPath = "\\xml\\";
            string fullpath = appPath + dbPath;
            FileStream file = new FileStream(appPath + Guid.NewGuid().ToString() + ".xml", FileMode.Create, FileAccess.ReadWrite);
            TextWriter tw = new StreamWriter(strm, UTF8Encoding.UTF8);
            xa.Serialize(strm, objXm);
            //escribiendo al disco
            xa.Serialize(file, objXm);
            var pos = strm.Position;
            strm.Position = 0;
            StreamReader reader = new StreamReader(strm, true);
            var str = reader.ReadToEnd();
            strm.Position = pos;
            tw.Flush();
            tw.Close();

            try
            {
                //OPEN QUEUE
                queueName = NameIn;
                //mqQueue = mqQMgr.AccessQueue(queueName, MQC.MQOO_OUTPUT Or MQC.MQOO_INPUT_SHARED Or MQC.MQOO_INQUIRE)
                mqQueue = mqQMgr.AccessQueue(queueName, MQC.MQOO_OUTPUT); //el parámetro  openoptions debe tener los mismos permisos de la cola(queueName), en este caso solo tiene permiso de PUT
                //PREPAR EL MENSAJE A ENVIAR
                mqMsg = new MQMessage();
                //If xml_ds <> String.Empty Then
                mqMsg.CharacterSet = 819;
                mqMsg.Encoding = 273;
                mqMsg.Format = "CMQC.MQFMT_STRING";
                mqMsg.WriteBytes(str.ToString());
                //mqMsg.Format = MQC.MQFMT_STRING
                mqPutMsgOpts = new MQPutMessageOptions();
                //DEJANDO EL MENSAJE
                mqQueue.Put(mqMsg, mqPutMsgOpts);
                MessageResult = "1Exito - MQ PUT MENSAJE";
                //borrando texto
                XmlX12N = "";
            }
            catch (Exception ex)
            {
                MessageResult = "0Error: " + ex.Message + "MQ PUT MENSAJE";
            }
            return MessageResult;
        }
        int contador;
        public string GetMensaje(string NameOut)
        {
            string getMensajeResult = "";
            try
            {

                Byte[] msgbytes;
                MQQueue mqQueueGET;

                //OPEN QUEUE
                string queueNameget = NameOut;
                //se cambió el parámetro openoptions de acuerdo  a los permisos de las colas
                mqQueueGET = mqQMgr.AccessQueue(queueNameget, MQC.MQOO_INPUT_SHARED | MQC.MQOO_BROWSE);
                mqGetMsgOpts = new MQGetMessageOptions();
                mqQueueGET.Get(mqMsg, mqGetMsgOpts);
                msgbytes = mqMsg.ReadBytes(mqMsg.MessageLength);
                string getStr = System.Text.Encoding.UTF8.GetString(msgbytes);
                string idMensaje = System.BitConverter.ToString(mqMsg.MessageId);
                getMensajeResult = "1ID: " + idMensaje + "MENSAJE: " + getStr;
                string call = "CALL sp_fill_3('8','" + strCorrelativo + "','" + getMensajeResult + "','','','','','','','','');";
                DataTable dtab = dat.mysql(call);
            }
            catch (Exception ex)
            {
                getMensajeResult = "0ERROR: create of MQQueueManager ended with {0}" + ex.Message.ToString() + "MQ GET MENSAJE";
            }
            return getMensajeResult;
        }
        public string codificador(string cadena)
        {

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(cadena);

            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);



            return iso.GetString(isoBytes);

        }

    }
}