using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VeterinariaHuellitas.Ventanas;
using Microsoft.Data.SqlClient;
using VeterinariaHuellitas.Datos;

namespace VeterinariaHuellitas.Ventanas
{
    public partial class Inicio : Window
    {
        private readonly Conexion conexion = new Conexion();

        public Inicio()
        {
            InitializeComponent();

            CargarEstadisticas();
        }

        // CARGAR RESUMEN GENERAL DESDE SQL SERVER
        private void CargarEstadisticas()
        {
            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        SELECT
                            (SELECT COUNT(*) FROM [Dueños]) AS TotalDuenos,
                            (SELECT COUNT(*) FROM Mascotas) AS TotalMascotas,
                            (SELECT COUNT(*) FROM [Consultas Médicas]) AS TotalConsultas";

                    using (SqlCommand comando =
                           new SqlCommand(consulta, con))
                    {
                        using (SqlDataReader lector =
                               comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                txtTotalDuenos.Text =
                                    lector["TotalDuenos"].ToString();

                                txtTotalMascotas.Text =
                                    lector["TotalMascotas"].ToString();

                                txtTotalConsultas.Text =
                                    lector["TotalConsultas"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el resumen general: " + ex.Message,
                    "Veterinaria Huellitas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // INICIO
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            CargarEstadisticas();
        }

        // REGISTRO DE DUEÑOS
        private void btnDueños_Click(object sender, RoutedEventArgs e)
        {
            Dueños ventana = new Dueños();
            ventana.Show();
            this.Close();
        }

        // GESTIÓN DE MASCOTAS
        private void btnMascotas_Click(object sender, RoutedEventArgs e)
        {
            Mascotas ventana = new Mascotas();
            ventana.Show();
            this.Close();
        }

        // HISTORIAL CLÍNICO
        private void btnHistorial_Click(object sender, RoutedEventArgs e)
        {
            HistorialClinico ventana = new HistorialClinico();
            ventana.Show();
            this.Close();
        }

        // CERRAR SESIÓN
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Login ventana = new Login();
            ventana.Show();
            this.Close();
        }
    }
}