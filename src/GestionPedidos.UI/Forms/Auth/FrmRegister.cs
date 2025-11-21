using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionPedidos.Controllers;

namespace GestionPedidos.UI.Forms.Auth
{
    public partial class FrmRegister : Form
    {
        private AuthController _authController = new AuthController();

        public FrmRegister()
        {
            InitializeComponent();
        }

        private bool ValidateForm()
        {
            // Verifica que los 4 campos (FullName, Username, Email, Password) estén llenos
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Todos los campos (Nombre Completo, Usuario, Contraseña y Correo) son obligatorios. Por favor, complete la información.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Ya NO se verifica la coincidencia de contraseñas.

            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // A. VALIDACIONES DE INTERFAZ
            if (!ValidateForm())
            {
                return;
            }
            // B. LLAMADA AL CONTROLADOR
            try
            {
                // 1. Obtener datos de la UI
                string fullName = txtFullName.Text.Trim();
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text; // Contraseña en texto plano

                // 2. Llamar al método Registrar del AuthController
                var resultado = _authController.Registrar(
                    username,
                    password,
                    fullName,
                    email,
                    idRol: 1
                );

                // 3. Manejar el resultado
                if (resultado.Success)
                {
                    MessageBox.Show(resultado.Message, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; 
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show(resultado.Message, "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado. Mensaje: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
