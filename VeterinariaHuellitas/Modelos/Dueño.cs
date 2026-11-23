using System;
using System.Collections.Generic;
using System.Text;

namespace VeterinariaHuellitas.Modelos
{
    public class Dueño
    {
        public int CodigoDueño { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Dirección { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }
}