using System;
using System.Collections.Generic;
using System.Text;

namespace VeterinariaHuellitas.Modelos
{
    public class Mascota
    {
        public int CodigoMascota { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raza { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public int CodigoDueño { get; set; }
    }
}