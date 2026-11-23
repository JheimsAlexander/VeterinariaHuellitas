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

namespace VeterinariaHuellitas.Ventanas
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(
            object sender,
            RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Password;

            if (usuario == "admin" && contraseña == "12345")
            {
                MessageBox.Show(
                    "Bienvenido a Veterinaria Huellitas."
                );

                Inicio ventana = new Inicio();
                ventana.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos."
                );
            }
        }
    }
}