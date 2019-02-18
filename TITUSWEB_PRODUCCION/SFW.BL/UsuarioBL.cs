using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFW.DAO;
using SFW.BE;

namespace SFW.BL
{
    public class UsuarioBL
    {
        private ADUsuario usuario = new ADUsuario();

        public List<Usuario> ListarUsuarios(int operacion)
        {
            return new List<Usuario>(usuario.ListarUsuarios(operacion));
        }

        public Usuario ObtieneUsuario(int operacion, int id)
        {
            return  usuario.ObtieneUsuario(operacion,id);
        }


    }
}
