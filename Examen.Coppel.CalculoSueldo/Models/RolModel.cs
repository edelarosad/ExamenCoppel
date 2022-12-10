using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.Models
{
    public class RolModel
    {
        public int RolId { get; set; }
        public string RolClave { get; set; }
        public string Descripcion { get; set; }
        public bool bActivo { get; set; }
    }
}
