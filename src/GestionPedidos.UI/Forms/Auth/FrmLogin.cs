using System;
using System.Windows.Forms;
using GestionPedidos.Controllers;
using GestionPedidos.UI.Forms.Auth;

namespace GestionPedidos.UI.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        private AuthController _authController = new AuthController();

        public FrmLogin()
        {
            InitializeComponent();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            btnLogin.Click += BtnLogin_Click;
            txtContraseña.KeyPress += TxtContraseña_KeyPress;
            tggMostrarContraseña.CheckedChanged += ChkMostrarContraseña_CheckedChanged;
            btnSignUp.Click += BtnRegistrar_Click;
        }

        private void TxtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                RealizarLogin();
            }
        }

        private void ChkMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            txtContraseña.PasswordChar = tggMostrarContraseña.Checked ? '\0' : '●';
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            RealizarLogin();
        }

        private void RealizarLogin()
        {
            // Deshabilitar botón mientras procesa
            btnLogin.Enabled = false;
            btnLogin.Text = "Loading...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Capturar datos y enviar al controlador
                var (success, message, usuario) = _authController.Login(txtUsuario.Text, txtContraseña.Text);

                if (success)
                {
                    MessageBox.Show(message, "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtContraseña.Clear();

                    // Dar foco según el error
                    if (message.Contains("usuario"))
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
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad de registro aún no implementada.",
                "Registro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void lblForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FrmForgotPassword frmForgotPassword = new FrmForgotPassword();
            frmForgotPassword.ShowDialog();
            this.Show();

        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Ocultar el formulario de Login actual
                this.Hide();

                // 2. Crear una nueva instancia del formulario de Registro
                FrmRegister registerForm = new FrmRegister();

                // 3. Mostrar el formulario de Registro en modo Modal
                // El modo ShowDialog() mantiene la aplicación enfocada en el nuevo formulario
                registerForm.ShowDialog();

                // 4. Mostrar el formulario de Login de nuevo
                // Esto se ejecuta cuando el FrmRegister se cierra.
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al abrir el registro: {ex.Message}", "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}