using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.ViewModels
{
    public class EmpleadosViewModel
    {
        public int EmpleadoId { get; set; }
        public int NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public int RolId { get; set; }
        public string RolClave { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

    }
}
