using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.BE;
using SFW.DAO;

namespace SFW.BL
{
    public class TitularBL
    {
        private ADTitular titular = new ADTitular();

        public string InsertarTitular(Titular titu,Titular_Detalle titu_deta,Usuario usu, int ope)
        {
            if (Convert.ToInt32(titu.categoria) >= 4 && Convert.ToInt32(titu.categoria) <= 21 )
            {
                titu.categoria = "04";
            }
            return titular.InsertarTitular(titu, titu_deta,usu, ope);
        }

        public int ActualizarTitular(Titular titu, Titular_Detalle titu_deta, Usuario usu, int ope,Titular oldTitu,Titular_Detalle oldTituDeta)
        {
            return titular.ActualizarTitular(titu, titu_deta, usu, ope,oldTitu,oldTituDeta);
        }

        public int RegistrarFechaRenovacionAlta(Titular titu, Titular_Detalle titu_deta)
        {
            return titular.RegistrarFechaRenovacionAlta(titu, titu_deta);
        }
        public int RegistrarFechaRenovacionBaja(Titular titu)
        {
            return titular.RegistrarFechaRenovacionBaja(titu);
        }

        public List<Titular> ListarTitulares(int operacion,string cod_cliente)
        {
            return new List<Titular>(titular.ListarTitulares(operacion,cod_cliente));
        }

        public List<Titular> ListarTitularesGrupo(int operacion, string cod_cliente, string cod_titula,string categoria)
        {
            return new List<Titular>(titular.ListarTitularesGrupo(operacion, cod_cliente, cod_titula,categoria));
        }

        public List<TituList> ListarGrupoFamiliar(int operacion, string cod_cliente, string cod_titula)
        {
            return new List<TituList>(titular.ListarGrupoFamiliar(operacion, cod_cliente, cod_titula));
        }

        public List<TituList> BusquedaCli(int operacion, string criteriobusqueda, string cod_cliente)
        {
            return new List<TituList>(titular.BusquedaCli(operacion, criteriobusqueda, cod_cliente));
        }

        public List<TituList> Busqueda(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            return new List<TituList>(titular.Busqueda(operacion,cod_cliente, cod_titula,categoria));
        }

        public Titular TraeTitular(int operacion, string cod_cliente, string cod_titula, string categoria)
        {
            return titular.TraeTitular(operacion, cod_cliente, cod_titula, categoria);
        }

        public int BAJACOMPLETA(Titular titu, Titular_Detalle titu_deta, Usuario usu,string  fecha,string operacion)
        {
            return titular.BAJACOMPLETA(titu,titu_deta,usu,fecha,operacion);
        }

        public Int32 ACTIVAR(Titular titu, Titular_Detalle titu_deta, Usuario usu, string fecha, string operacion)
        {
            return titular.ACTIVAR(titu, titu_deta, usu, fecha, operacion);
        }
    }
}
