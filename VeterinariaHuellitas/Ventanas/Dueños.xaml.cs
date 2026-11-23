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
using Microsoft.Data.SqlClient;
using VeterinariaHuellitas.Datos;

namespace VeterinariaHuellitas.Ventanas
{
    public partial class Dueños : Window
    {
        private readonly Conexion conexion = new Conexion();

        public Dueños()
        {
            InitializeComponent();
            CargarDuenos();
        }

        // CARGAR Y BUSCAR DUEÑOS
        private void CargarDuenos(string busqueda = "")
        {
            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"SELECT
                    CodigoDueño,
                    Nombre,
                    ApellidoPaterno,
                    CI,
                    Sexo,
                    Telefono,
                    Direccion,
                    Correo
                    FROM [Dueños]";

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    if (rbCodigo.IsChecked == true)
                        consulta += " WHERE CAST(CodigoDueño AS VARCHAR(20)) LIKE @Busqueda";
                    else if (rbDni.IsChecked == true)
                        consulta += " WHERE CI LIKE @Busqueda";
                    else
                        consulta += " WHERE Nombre LIKE @Busqueda";
                }

                consulta += " ORDER BY CodigoDueño DESC";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                if (!string.IsNullOrWhiteSpace(busqueda))
                    cmd.Parameters.AddWithValue("@Busqueda", "%" + busqueda.Trim() + "%");

                using SqlDataAdapter adaptador = new SqlDataAdapter(cmd);

                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                dgDuenos.ItemsSource = tabla.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los dueños: " + ex.Message);
            }
        }

        // REGISTRAR NUEVO DUEÑO
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCI.Text) ||
                cmbSexo.SelectedItem == null)
            {
                MessageBox.Show("Completa Nombre, Apellido, DNI y Sexo.");
                return;
            }

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    INSERT INTO [Dueños]
                    (Nombre, ApellidoPaterno, ApellidoMaterno,
                     CI, Sexo, Telefono, Direccion, Correo)
                    VALUES
                    (@Nombre, @Apellido, '',
                     @CI, @Sexo, @Telefono, @Direccion, @Correo);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
                cmd.Parameters.AddWithValue("@CI", txtCI.Text.Trim());
                cmd.Parameters.AddWithValue("@Sexo",
                    ((ComboBoxItem)cmbSexo.SelectedItem).Content.ToString());
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());

                int codigo = Convert.ToInt32(cmd.ExecuteScalar());

                CargarDuenos();

                txtCodigo.Text = codigo.ToString();

                MessageBox.Show("Dueño registrado correctamente.\nCódigo asignado: " + codigo);
                LimpiarCampos(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        // MODIFICAR DUEÑO
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecciona un dueño de la tabla.");
                return;
            }

            if (cmbSexo.SelectedItem == null)
            {
                MessageBox.Show("Selecciona el sexo.");
                return;
            }

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta = @"
                    UPDATE [Dueños] SET
                    Nombre = @Nombre,
                    ApellidoPaterno = @Apellido,
                    CI = @CI,
                    Sexo = @Sexo,
                    Telefono = @Telefono,
                    Direccion = @Direccion,
                    Correo = @Correo
                    WHERE CodigoDueño = @Codigo";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Codigo", int.Parse(txtCodigo.Text));
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
                cmd.Parameters.AddWithValue("@CI", txtCI.Text.Trim());
                cmd.Parameters.AddWithValue("@Sexo",
                    ((ComboBoxItem)cmbSexo.SelectedItem).Content.ToString());
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Dueño modificado correctamente.");

                LimpiarCampos();
                CargarDuenos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
        }

        // ELIMINAR DUEÑO
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecciona un dueño de la tabla.");
                return;
            }

            MessageBoxResult respuesta = MessageBox.Show(
                "¿Estás seguro de eliminar este dueño?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (respuesta != MessageBoxResult.Yes)
                return;

            try
            {
                using SqlConnection cn = conexion.AbrirConexion();

                string consulta =
                    "DELETE FROM [Dueños] WHERE CodigoDueño = @Codigo";

                using SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@Codigo", int.Parse(txtCodigo.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Dueño eliminado correctamente.");

                LimpiarCampos();
                CargarDuenos();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show(
                    "No se puede eliminar este dueño porque tiene mascotas registradas.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        // SELECCIONAR UN REGISTRO DE LA TABLA
        private void dgDuenos_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgDuenos.SelectedItem is DataRowView fila)
            {
                txtCodigo.Text = fila["CodigoDueño"]?.ToString() ?? string.Empty;
                txtNombre.Text = fila["Nombre"]?.ToString() ?? string.Empty;
                txtApellido.Text = fila["ApellidoPaterno"]?.ToString() ?? string.Empty;
                txtCI.Text = fila["CI"]?.ToString() ?? string.Empty;
                txtTelefono.Text = fila["Telefono"]?.ToString() ?? string.Empty;
                txtDireccion.Text = fila["Direccion"]?.ToString() ?? string.Empty;
                txtCorreo.Text = fila["Correo"]?.ToString() ?? string.Empty;

                string sexo = fila["Sexo"]?.ToString() ?? string.Empty;

                foreach (ComboBoxItem item in cmbSexo.Items)
                {
                    if (string.Equals(
                        item.Content?.ToString(),
                        sexo,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cmbSexo.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        // BÚSQUEDA AUTOMÁTICA
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CargarDuenos(txtBuscar.Text);
        }

        // CAMBIAR EL TIPO DE BÚSQUEDA
        private void rbBusqueda_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
                CargarDuenos(txtBuscar.Text);
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            Inicio ventana = new Inicio();
            ventana.Show();
            this.Close();
        }
        // BOTÓN NUEVO
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            txtNombre.Focus();
        }

        // BOTÓN CANCELAR
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
            txtBuscar.Clear();
            CargarDuenos();
        }

        // LIMPIAR CAMPOS
        private void LimpiarCampos(bool limpiarCodigo = true)
        {
            if (limpiarCodigo)
                txtCodigo.Clear();

            txtNombre.Clear();
            txtApellido.Clear();
            txtCI.Clear();
            cmbSexo.SelectedIndex = -1;
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtCorreo.Clear();

            dgDuenos.SelectedItem = null;
        }
    }
}