using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.Models
{
    public class EmpleadoModel
    {
        public int EmpleadoId { get; set; }
        public int NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int RolId { get; set; }
        public bool bActivo { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

    }
}
