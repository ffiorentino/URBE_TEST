using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models.DTO
{
    public class ReportDTO
    {
        public string nombre { get; set; }
        public string capacidad { get; set; }
        public string disponibilidad { get; set; }
        public string ocupacion { get; set; }
        public string tieneProyector { get; set; }
        public string utilizaProyector { get; set; }
        public string tienePizarra { get; set; }
        public string utilizaPizarra { get; set; }
        public string tieneInternet { get; set; }
        public string utilizaInternet { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
    }
}
