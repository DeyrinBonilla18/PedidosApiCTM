using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPedidos.Models
{
    public class UsuarioInfo
    {
        public int idUsuario { get; set; }
        public string nUsuario { get; set; }
        public int idRol { get; set; }
        public string Contraseña { get; set; }
    }
}