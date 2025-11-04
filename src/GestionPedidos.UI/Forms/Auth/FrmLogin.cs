using System;
using System.Windows.Forms;
using GestionPedidos.Controllers;

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
            txtContraseña.KeyPress += TxtContraseña_KeyPress;
            tggMostrarContraseña.CheckedChanged += ChkMostrarContraseña_CheckedChanged;
            btnRegistrar.Click += BtnRegistrar_Click;
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
            Cursor = Cursors.WaitCursor;

            try
            {
                // Capturar datos y enviar al controlador
                var (success, message, usuario) = _authController.Login(txtUsuario.Text, txtContraseña.Text);

                if (success)
                {
                    MessageBox.Show(message, "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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
                Cursor = Cursors.Default;
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            // Ocultar el formulario de login
            Hide();
            
            FrmRegistro registroForm = new FrmRegistro();
            DialogResult resultado = registroForm.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                // Registro exitoso, regresar al login y mostrar mensaje
                MessageBox.Show("Registro completado. Por favor inicia sesión con tus credenciales.", 
                    "Registro Exitoso", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Show();
                Activate();
            }
            else
            {
                // Usuario canceló manualmente, mostrar login de nuevo
                Show();
                Activate();
            }
            
            registroForm.Dispose();
        }
    }
}