using Guna.UI2.AnimatorNS;
using System;
using System.Windows.Forms;
using GestionPedidos.Controllers;

namespace GestionPedidos.UI.Forms.Auth
{
    public partial class FrmRegistro : Form
    {
        private AuthController _authController = new AuthController();
        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            RealizarRegistro();
        }

        private void RealizarRegistro()
        {
            // Deshabilitar botón mientras procesa
            btnRegistro.Enabled = false;
            btnRegistro.Text = "Loading...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Capturar datos y enviar al controlador
                var (success, message) = _authController.Registrar(
                    txtUsuario.Text,
                    txtContraseña.Text,
                    txtNombreCompleto.Text,
                    txtCorreo.Text
                );

                if (success)
                {
                    MessageBox.Show(message, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Dar foco según el tipo de error
                    if (message.ToLower().Contains("usuario"))
                        txtUsuario.Focus();
                    else
                        txtContraseña.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistro.Enabled = true;
                btnRegistro.Text = "Registro";
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
