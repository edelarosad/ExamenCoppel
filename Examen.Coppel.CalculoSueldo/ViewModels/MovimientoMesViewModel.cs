using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.ViewModels
{
    public class MovimientoMesViewModel
    {
        public int MovimientoMesId { get; set; }
        public int NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Mes { get; set; }
        public int CantidadEntregas { get; set; }
        public decimal SueldoBruto { get; set; }
        public decimal Isr { get; set; }
        public decimal Bono { get; set; }
        public decimal ValesDespensa { get; set; }
        public decimal SueldoTotal { get; set; }
    }
}
