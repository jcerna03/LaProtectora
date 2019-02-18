using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.BE;
using SFW.DAO;

namespace SFW.BL
{
    public class LOGBL
    {
        private ADLOG log = new ADLOG();

        public int InsertLog(Log lg, int ope)
        {
            return log.InsertarLog(lg, ope);
        }
    }
}
