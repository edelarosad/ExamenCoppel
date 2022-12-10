using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.Models
{
    public class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string UsuarioClave { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
    }
}
