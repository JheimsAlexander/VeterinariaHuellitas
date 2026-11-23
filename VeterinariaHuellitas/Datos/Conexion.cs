using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace VeterinariaHuellitas.Datos
{
    public class Conexion
    {
            // CADENA DE CONEXIÓN A SQL SERVER
            private readonly string cadenaConexion =
            @"Server=.\SQLEXPRESS;
              Database=VeterinariaHuellitas;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion =
                new SqlConnection(cadenaConexion);

            conexion.Open();

            return conexion;
        }
    }
}