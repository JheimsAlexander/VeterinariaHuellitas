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
using Microsoft.Data.SqlClient;
using VeterinariaHuellitas.Datos;

namespace VeterinariaHuellitas.Ventanas
{
    public partial class Mascotas : Window
    {
        private readonly Conexion conexion = new Conexion();

        public Mascotas()
        {
            InitializeComponent();
            CargarDuenos();
            CargarMascotas();
        }

        // CARGAR DUEÑOS EN LA LISTA DESPLEGABLE
        private void CargarDuenos()
        {
            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    SELECT CodigoDueño,
                    CONCAT(Nombre, ' ', ApellidoPaterno) AS NombreCompleto
                    FROM [Dueños]
                    ORDER BY Nombre";

                using SqlDataAdapter adaptador =
                    new SqlDataAdapter(consulta, cn);

                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                cmbDueno.ItemsSource = tabla.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar dueños: " + ex.Message);
            }
        }

        // CARGAR MASCOTAS Y SU DUEÑO
        private void CargarMascotas(string busqueda = "")
        {
            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    SELECT
                        M.CodigoMascota,
                        M.Nombre,
                        M.Especie,
                        M.Raza,
                        M.Sexo,
                        M.FechaNacimiento,
                        M.Peso,
                        M.CodigoDueño,
                        CONCAT(D.Nombre, ' ', D.ApellidoPaterno) AS NombreDueno
                    FROM Mascotas M
                    INNER JOIN [Dueños] D
                    ON M.CodigoDueño = D.CodigoDueño";

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    consulta += @"
                        WHERE M.Nombre LIKE @Busqueda
                        OR M.Especie LIKE @Busqueda
                        OR M.Raza LIKE @Busqueda
                        OR D.Nombre LIKE @Busqueda
                        OR D.ApellidoPaterno LIKE @Busqueda";
                }

                consulta += " ORDER BY M.CodigoMascota DESC";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                if (!string.IsNullOrWhiteSpace(busqueda))
                    cmd.Parameters.AddWithValue(
                        "@Busqueda", "%" + busqueda.Trim() + "%");

                using SqlDataAdapter adaptador = new SqlDataAdapter(cmd);

                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                dgMascotas.ItemsSource = tabla.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mascotas: " + ex.Message);
            }
        }

        // REGISTRAR MASCOTA
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                cmbEspecie.SelectedItem == null ||
                cmbSexo.SelectedItem == null ||
                cmbDueno.SelectedValue == null)
            {
                MessageBox.Show("Completa el nombre, especie, sexo y dueño.");
                return;
            }

            decimal? peso = ObtenerPeso();

            if (!string.IsNullOrWhiteSpace(txtPeso.Text) && peso == null)
            {
                MessageBox.Show("Introduce un peso válido.");
                return;
            }

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    INSERT INTO Mascotas
                    (Nombre, Especie, Raza, Sexo,
                     FechaNacimiento, Peso, CodigoDueño)
                    VALUES
                    (@Nombre, @Especie, @Raza, @Sexo,
                     @FechaNacimiento, @Peso, @CodigoDueño);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());

                cmd.Parameters.AddWithValue("@Especie",
                    ((ComboBoxItem)cmbEspecie.SelectedItem).Content.ToString());

                cmd.Parameters.AddWithValue("@Raza", txtRaza.Text.Trim());

                cmd.Parameters.AddWithValue("@Sexo",
                    ((ComboBoxItem)cmbSexo.SelectedItem).Content.ToString());

                cmd.Parameters.AddWithValue("@FechaNacimiento",
                    dpNacimiento.SelectedDate.HasValue
                    ? dpNacimiento.SelectedDate.Value
                    : DBNull.Value);

                cmd.Parameters.AddWithValue("@Peso",
                    peso.HasValue ? peso.Value : DBNull.Value);

                cmd.Parameters.AddWithValue("@CodigoDueño",
                    Convert.ToInt32(cmbDueno.SelectedValue));

                int codigo = Convert.ToInt32(cmd.ExecuteScalar());

                MessageBox.Show(
                    "Mascota registrada correctamente.\nCódigo asignado: " + codigo);

                LimpiarCampos();
                CargarMascotas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar mascota: " + ex.Message);
            }
        }

        // MODIFICAR MASCOTA
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecciona una mascota de la tabla.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                cmbEspecie.SelectedItem == null ||
                cmbSexo.SelectedItem == null ||
                cmbDueno.SelectedValue == null)
            {
                MessageBox.Show("Completa los campos obligatorios.");
                return;
            }

            decimal? peso = ObtenerPeso();

            if (!string.IsNullOrWhiteSpace(txtPeso.Text) && peso == null)
            {
                MessageBox.Show("Introduce un peso válido.");
                return;
            }

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    UPDATE Mascotas SET
                    Nombre = @Nombre,
                    Especie = @Especie,
                    Raza = @Raza,
                    Sexo = @Sexo,
                    FechaNacimiento = @FechaNacimiento,
                    Peso = @Peso,
                    CodigoDueño = @CodigoDueño
                    WHERE CodigoMascota = @Codigo";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Codigo", int.Parse(txtCodigo.Text));
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());

                cmd.Parameters.AddWithValue("@Especie",
                    ((ComboBoxItem)cmbEspecie.SelectedItem).Content.ToString());

                cmd.Parameters.AddWithValue("@Raza", txtRaza.Text.Trim());

                cmd.Parameters.AddWithValue("@Sexo",
                    ((ComboBoxItem)cmbSexo.SelectedItem).Content.ToString());

                cmd.Parameters.AddWithValue("@FechaNacimiento",
                    dpNacimiento.SelectedDate.HasValue
                    ? dpNacimiento.SelectedDate.Value
                    : DBNull.Value);

                cmd.Parameters.AddWithValue("@Peso",
                    peso.HasValue ? peso.Value : DBNull.Value);

                cmd.Parameters.AddWithValue("@CodigoDueño",
                    Convert.ToInt32(cmbDueno.SelectedValue));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Mascota modificada correctamente.");

                LimpiarCampos();
                CargarMascotas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar mascota: " + ex.Message);
            }
        }

        // ELIMINAR MASCOTA
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecciona una mascota.");
                return;
            }

            if (MessageBox.Show(
                "¿Deseas eliminar esta mascota?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta =
                    "DELETE FROM Mascotas WHERE CodigoMascota = @Codigo";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Codigo", int.Parse(txtCodigo.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Mascota eliminada correctamente.");

                LimpiarCampos();
                CargarMascotas();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show(
                    "No se puede eliminar esta mascota porque tiene consultas médicas registradas.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            Inicio ventana = new Inicio();
            ventana.Show();
            this.Close();
        }
        // SELECCIONAR MASCOTA
        private void dgMascotas_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgMascotas.SelectedItem is not DataRowView fila)
                return;

            txtCodigo.Text = fila["CodigoMascota"].ToString();
            txtNombre.Text = fila["Nombre"].ToString();
            txtRaza.Text = fila["Raza"].ToString();
            txtPeso.Text = fila["Peso"] == DBNull.Value
                ? ""
                : Convert.ToString(fila["Peso"], CultureInfo.CurrentCulture);

            dpNacimiento.SelectedDate =
                fila["FechaNacimiento"] == DBNull.Value
                ? null
                : Convert.ToDateTime(fila["FechaNacimiento"]);

            SeleccionarCombo(cmbEspecie, fila["Especie"].ToString());
            SeleccionarCombo(cmbSexo, fila["Sexo"].ToString());

            cmbDueno.SelectedValue =
                Convert.ToInt32(fila["CodigoDueño"]);
        }

        private void SeleccionarCombo(ComboBox combo, string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                combo.SelectedIndex = -1;
                return;
            }

            foreach (ComboBoxItem item in combo.Items)
            {
                if (string.Equals(
                    item.Content?.ToString(),
                    valor,
                    StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    return;
                }
            }

            combo.SelectedIndex = -1;
        }

        // VALIDAR PESO
        private decimal? ObtenerPeso()
        {
            if (string.IsNullOrWhiteSpace(txtPeso.Text))
                return null;

            if (decimal.TryParse(
                txtPeso.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal peso) && peso >= 0)
                return peso;

            return null;
        }

        // BÚSQUEDA
        private void txtBuscar_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
                CargarMascotas(txtBuscar.Text);
        }

        // NUEVO
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            txtNombre.Focus();
        }

        // CANCELAR
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            txtBuscar.Clear();
            CargarMascotas();
        }

        // LIMPIAR
        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtRaza.Clear();
            txtPeso.Clear();

            cmbEspecie.SelectedIndex = -1;
            cmbSexo.SelectedIndex = -1;
            cmbDueno.SelectedIndex = -1;

            dpNacimiento.SelectedDate = null;
            dgMascotas.SelectedItem = null;
        }
    }
}