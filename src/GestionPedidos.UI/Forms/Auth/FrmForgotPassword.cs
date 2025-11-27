using GestionPedidos.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPedidos.UI.Forms.Auth
{
    public partial class FrmForgotPassword : Form
    {
        private readonly AuthController _authController = new AuthController();

        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private void tggMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirmPassword.PasswordChar = tggMostrarContraseña.Checked ? '\0' : '●';
            txtNewPassword.PasswordChar = tggMostrarContraseña.Checked ? '\0' : '●';
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string originalText = btnConfirmar.Text;
            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Loading...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string email = txtEmail.Text.Trim();
                string newPass = txtNewPassword.Text;
                string confirmPass = txtConfirmPassword.Text;

                var result = _authController.ResetPassword(email, newPass, confirmPass);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado. Mensaje: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConfirmar.Enabled = true;
                btnConfirmar.Text = originalText;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
