using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Coppel.CalculoSueldo.Models
{
    public class MovimientoMesModel
    {
        public int MovimientoMesId { get; set; }
        public int NumeroEmpleado { get; set; }
        public string Nombre { get; set; }
        public int RolId { get; set; }
        public int Mes { get; set; }
        public int CantidadEntregas { get; set; }
        public decimal SueldoMesualTotal { get; set; }
        public decimal ValesDespensa { get; set; }
    }
}
