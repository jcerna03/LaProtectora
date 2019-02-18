using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.DAO;
using SFW.BE;

namespace SFW.BL
{
    public class ComboBL
    {
        private ADCombo cbo = new ADCombo();

        public List<Combo> ListaCombos(int operacion)
        {
            return new List<Combo>(cbo.ListaCombos(operacion));
        }

        public List<Combo> ListaCombos(int operacion, string cliente)
        {
            return new List<Combo>(cbo.ListaCombos(operacion,cliente));
        }
    }
}
