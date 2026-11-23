using System;
using System.Collections.Generic;
using System.Text;

namespace VeterinariaHuellitas.Modelos
{
    public class ConsultaMedica
    {
        public int CodigoConsulta { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Sintomas { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Medicamentos { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public int CodigoMascota { get; set; }
    }
}