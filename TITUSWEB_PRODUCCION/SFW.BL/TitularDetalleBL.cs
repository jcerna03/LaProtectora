using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.DAO;
using SFW.BE;

namespace SFW.BL
{
    public class TitularDetalleBL
    {
        private ADTitular_Detalle titulardeta = new ADTitular_Detalle();

        //public int InsertarTitular(Titular_Detalle titudeta, int ope)
        //{
        //    return titulardeta.InsertarTitularDetalle(titudeta, ope);
        //}

        //public int ActualizarTitular(Titular_Detalle titudeta, int ope)
        //{
        //    return titulardeta.ActualizarTitularDetalle(titudeta, ope);
        //}

        public List<Titular_Detalle> ListarDetallesdeTitulares(int operacion, string cod_cliente)
        {
            return new List<Titular_Detalle>(titulardeta.ListarDetallesdeTitulares(operacion, cod_cliente));
        }

        public List<Titular_Detalle> ListarDetallesGrupoFamiliar(int operacion, string cod_cliente,string cod_titula,string categoria)
        {
            return new List<Titular_Detalle>(titulardeta.ListarDetallesGrupoFamiliar(operacion, cod_cliente, cod_titula,categoria));
        }

    }
}
