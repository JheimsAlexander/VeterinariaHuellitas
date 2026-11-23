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
using System;
using System.Data;
using System.Globalization;
using VeterinariaHuellitas.Datos;
using Microsoft.Data.SqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.Win32;
using System.IO;


namespace VeterinariaHuellitas.Ventanas
{
    public partial class HistorialClinico : Window
    {
        private readonly Conexion conexion = new Conexion();

        public HistorialClinico()
        {
            InitializeComponent();

            dpFecha.SelectedDate = DateTime.Today;
            QuestPDF.Settings.License = LicenseType.Community;

            CargarMascotas();
            CargarConsultas();
        }

        // CARGAR MASCOTAS REGISTRADAS
        private void CargarMascotas()
        {
            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        SELECT
                            CodigoMascota,
                            Nombre,
                            CONCAT(Nombre, ' - ', Especie) AS NombreCompleto
                        FROM Mascotas
                        ORDER BY Nombre";

                    SqlDataAdapter adaptador =
                        new SqlDataAdapter(consulta, con);

                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    cmbMascota.ItemsSource = tabla.DefaultView;
                    cmbMascota.DisplayMemberPath = "NombreCompleto";
                    cmbMascota.SelectedValuePath = "CodigoMascota";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar mascotas: " + ex.Message);
            }
        }

        // CARGAR CONSULTAS MÉDICAS
        private void CargarConsultas()
        {
            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        SELECT
                            c.CodigoConsulta,
                            c.Fecha,
                            c.CodigoMascota,
                            m.Nombre AS NombreMascota,
                            c.Motivo,
                            c.Sintomas,
                            c.Diagnostico,
                            c.Tratamiento,
                            c.Medicamentos,
                            c.Observaciones
                        FROM [Consultas Médicas] c
                        INNER JOIN Mascotas m
                            ON c.CodigoMascota = m.CodigoMascota
                        WHERE
                            CAST(c.CodigoConsulta AS VARCHAR) LIKE @buscar
                            OR m.Nombre LIKE @buscar
                            OR c.Motivo LIKE @buscar
                            OR c.Diagnostico LIKE @buscar
                        ORDER BY c.CodigoConsulta DESC";

                    SqlDataAdapter adaptador =
                        new SqlDataAdapter(consulta, con);

                    adaptador.SelectCommand.Parameters.AddWithValue(
                        "@buscar", "%" + txtBuscar.Text.Trim() + "%");

                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    dgConsultas.ItemsSource = tabla.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar consultas: " + ex.Message);
            }
        }

        // BUSCAR CONSULTAS
        private void txtBuscar_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            CargarConsultas();
        }

        // SELECCIONAR CONSULTA
        private void dgConsultas_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgConsultas.SelectedItem is DataRowView fila)
            {
                txtCodigo.Text =
                    fila["CodigoConsulta"]?.ToString() ?? string.Empty;

                dpFecha.SelectedDate =
                    Convert.ToDateTime(fila["Fecha"]);

                txtMotivo.Text =
                    fila["Motivo"]?.ToString() ?? string.Empty;

                txtSintomas.Text =
                    fila["Sintomas"]?.ToString() ?? string.Empty;

                txtDiagnostico.Text =
                    fila["Diagnostico"]?.ToString() ?? string.Empty;

                txtTratamiento.Text =
                    fila["Tratamiento"]?.ToString() ?? string.Empty;

                txtMedicamentos.Text =
                    fila["Medicamentos"]?.ToString() ?? string.Empty;

                txtObservaciones.Text =
                    fila["Observaciones"]?.ToString() ?? string.Empty;

                cmbMascota.SelectedValue =
                    Convert.ToInt32(fila["CodigoMascota"]);
            }
        }

        // NUEVA CONSULTA
        private void btnNuevo_Click(
            object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        // REGISTRAR CONSULTA
        private void btnGuardar_Click(
            object sender, RoutedEventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        INSERT INTO [Consultas Médicas]
                        (
                            Fecha,
                            Motivo,
                            Sintomas,
                            Diagnostico,
                            Tratamiento,
                            Medicamentos,
                            Observaciones,
                            CodigoMascota
                        )
                        VALUES
                        (
                            @Fecha,
                            @Motivo,
                            @Sintomas,
                            @Diagnostico,
                            @Tratamiento,
                            @Medicamentos,
                            @Observaciones,
                            @CodigoMascota
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand comando =
                           new SqlCommand(consulta, con))
                    {
                        AgregarParametros(comando);

                        int codigo = Convert.ToInt32(
                            comando.ExecuteScalar());

                        txtCodigo.Text = codigo.ToString();

                        MessageBox.Show(
                            "Consulta médica registrada correctamente.",
                            "Veterinaria Huellitas",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }

                CargarConsultas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al registrar consulta: " + ex.Message);
            }
        }

        // MODIFICAR CONSULTA
        private void btnActualizar_Click(
            object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text, out int codigo))
            {
                MessageBox.Show(
                    "Selecciona una consulta para modificar.");
                return;
            }

            if (!ValidarDatos())
                return;

            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        UPDATE [Consultas Médicas]
                        SET
                            Fecha = @Fecha,
                            Motivo = @Motivo,
                            Sintomas = @Sintomas,
                            Diagnostico = @Diagnostico,
                            Tratamiento = @Tratamiento,
                            Medicamentos = @Medicamentos,
                            Observaciones = @Observaciones,
                            CodigoMascota = @CodigoMascota
                        WHERE CodigoConsulta = @CodigoConsulta";

                    using (SqlCommand comando =
                           new SqlCommand(consulta, con))
                    {
                        AgregarParametros(comando);

                        comando.Parameters.AddWithValue(
                            "@CodigoConsulta", codigo);

                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Consulta modificada correctamente.");

                CargarConsultas();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al modificar consulta: " + ex.Message);
            }
        }

        // ELIMINAR CONSULTA
        private void btnEliminar_Click(
            object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text, out int codigo))
            {
                MessageBox.Show(
                    "Selecciona una consulta para eliminar.");
                return;
            }

            MessageBoxResult respuesta = MessageBox.Show(
                "¿Estás seguro de eliminar esta consulta médica?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (respuesta != MessageBoxResult.Yes)
                return;

            try
            {
                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                        DELETE FROM [Consultas Médicas]
                        WHERE CodigoConsulta = @CodigoConsulta";

                    using (SqlCommand comando =
                           new SqlCommand(consulta, con))
                    {
                        comando.Parameters.AddWithValue(
                            "@CodigoConsulta", codigo);

                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Consulta eliminada correctamente.");

                CargarConsultas();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar consulta: " + ex.Message);
            }
        }

        // CANCELAR
        private void btnCancelar_Click(
            object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        // VALIDAR DATOS
        private bool ValidarDatos()
        {
            if (cmbMascota.SelectedValue == null)
            {
                MessageBox.Show("Selecciona una mascota.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Ingresa el motivo de consulta.");
                txtMotivo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MessageBox.Show("Ingresa el diagnóstico.");
                txtDiagnostico.Focus();
                return false;
            }

            return true;
        }

        // AGREGAR PARÁMETROS SQL
        private void AgregarParametros(SqlCommand comando)
        {
            comando.Parameters.AddWithValue(
                "@Fecha",
                dpFecha.SelectedDate ?? DateTime.Today);

            comando.Parameters.AddWithValue(
                "@Motivo", txtMotivo.Text.Trim());

            comando.Parameters.AddWithValue(
                "@Sintomas", txtSintomas.Text.Trim());

            comando.Parameters.AddWithValue(
                "@Diagnostico", txtDiagnostico.Text.Trim());

            comando.Parameters.AddWithValue(
                "@Tratamiento", txtTratamiento.Text.Trim());

            comando.Parameters.AddWithValue(
                "@Medicamentos", txtMedicamentos.Text.Trim());

            comando.Parameters.AddWithValue(
                "@Observaciones", txtObservaciones.Text.Trim());

            comando.Parameters.AddWithValue(
                "@CodigoMascota",
                Convert.ToInt32(cmbMascota.SelectedValue));
        }

        // DESCARGAR HISTORIAL CLÍNICO EN PDF
        private void btnDescargarPDF_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text, out int codigo))
            {
                MessageBox.Show(
                    "Primero selecciona una consulta médica de la tabla.",
                    "Veterinaria Huellitas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string mascota = "";
                string codigoMascota = "";
                string dueno = "";
                DateTime fecha = DateTime.Today;
                string motivo = "";
                string sintomas = "";
                string diagnostico = "";
                string tratamiento = "";
                string medicamentos = "";
                string observaciones = "";

                using (SqlConnection con = conexion.AbrirConexion())
                {
                    string consulta = @"
                SELECT
                    c.CodigoConsulta,
                    c.Fecha,
                    c.Motivo,
                    c.Sintomas,
                    c.Diagnostico,
                    c.Tratamiento,
                    c.Medicamentos,
                    c.Observaciones,
                    m.CodigoMascota,
                    m.Nombre AS NombreMascota,
                    d.Nombre + ' ' + d.ApellidoPaterno AS NombreDueno
                FROM [Consultas Médicas] c
                INNER JOIN Mascotas m
                    ON c.CodigoMascota = m.CodigoMascota
                INNER JOIN [Dueños] d
                    ON m.CodigoDueño = d.CodigoDueño
                WHERE c.CodigoConsulta = @Codigo";

                    using (SqlCommand comando = new SqlCommand(consulta, con))
                    {
                        comando.Parameters.AddWithValue("@Codigo", codigo);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (!lector.Read())
                            {
                                MessageBox.Show("No se encontró la consulta.");
                                return;
                            }

                            fecha = Convert.ToDateTime(lector["Fecha"]);
                            mascota = lector["NombreMascota"]?.ToString() ?? "";
                            codigoMascota = lector["CodigoMascota"]?.ToString() ?? "";
                            dueno = lector["NombreDueno"]?.ToString() ?? "";

                            motivo = lector["Motivo"]?.ToString() ?? "";
                            sintomas = lector["Sintomas"]?.ToString() ?? "";
                            diagnostico = lector["Diagnostico"]?.ToString() ?? "";
                            tratamiento = lector["Tratamiento"]?.ToString() ?? "";
                            medicamentos = lector["Medicamentos"]?.ToString() ?? "";
                            observaciones = lector["Observaciones"]?.ToString() ?? "";
                        }
                    }
                }

                SaveFileDialog guardar = new SaveFileDialog
                {
                    Title = "Guardar historial clínico",
                    Filter = "Documento PDF (*.pdf)|*.pdf",
                    FileName = $"Historial_Clinico_{codigo}.pdf"
                };

                if (guardar.ShowDialog() != true)
                    return;

                Document.Create(documento =>
                {
                    documento.Page(pagina =>
                    {
                        pagina.Size(PageSizes.A4);
                        pagina.Margin(40);
                        pagina.DefaultTextStyle(x => x.FontSize(11));

                        pagina.Header().Column(columna =>
                        {
                            columna.Item().AlignCenter()
                                .Text("VETERINARIA HUELLITAS")
                                .Bold().FontSize(20).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                            columna.Item().AlignCenter()
                                .Text("HISTORIAL MÉDICO")
                                .Bold().FontSize(16);

                            columna.Item().PaddingVertical(10)
                                .LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Grey.Medium);
                        });

                        pagina.Content().PaddingVertical(15).Column(columna =>
                        {
                            columna.Spacing(10);

                            columna.Item().Text($"N.º de consulta: {codigo}").Bold();

                            columna.Item().Text("DATOS DE LA MASCOTA")
                                .Bold().FontSize(13).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                            columna.Item().Text($"Código: {codigoMascota}");
                            columna.Item().Text($"Nombre: {mascota}");
                            columna.Item().Text($"Dueño: {dueno}");

                            columna.Item().PaddingTop(10)
                                .Text("DATOS DE LA CONSULTA")
                                .Bold().FontSize(13).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                            columna.Item().Text($"Fecha: {fecha:dd/MM/yyyy}");

                            columna.Item().Text("Motivo de consulta:").Bold();
                            columna.Item().Text(motivo);

                            columna.Item().Text("Síntomas:").Bold();
                            columna.Item().Text(sintomas);

                            columna.Item().Text("Diagnóstico:").Bold();
                            columna.Item().Text(diagnostico);

                            columna.Item().Text("Tratamiento:").Bold();
                            columna.Item().Text(tratamiento);

                            columna.Item().Text("Medicamentos:").Bold();
                            columna.Item().Text(medicamentos);

                            columna.Item().Text("Observaciones:").Bold();
                            columna.Item().Text(observaciones);

                            columna.Item().PaddingTop(55).AlignCenter().Column(firma =>
                            {
                                firma.Item()
                                    .Width(220)
                                    .LineHorizontal(1)
                                    .LineColor(QuestPDF.Helpers.Colors.Black);

                                firma.Item()
                                    .PaddingTop(5)
                                    .AlignCenter()
                                    .Text("FIRMA Y SELLO DEL MÉDICO VETERINARIO")
                                    .Bold()
                                    .FontSize(9);
                            });
                        });

                        pagina.Footer().AlignCenter()
                            .Text("Historial clínico - Veterinaria Huellitas")
                            .FontSize(9).FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                    });
                }).GeneratePdf(guardar.FileName);

                MessageBox.Show(
                    "¡Historial clínico PDF generado correctamente!",
                    "Veterinaria Huellitas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al generar el PDF: " + ex.Message,
                    "Veterinaria Huellitas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // LIMPIAR CAMPOS
        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtMotivo.Clear();
            txtSintomas.Clear();
            txtDiagnostico.Clear();
            txtTratamiento.Clear();
            txtMedicamentos.Clear();
            txtObservaciones.Clear();

            dpFecha.SelectedDate = DateTime.Today;
            cmbMascota.SelectedIndex = -1;
            dgConsultas.SelectedItem = null;
        }
    }
}