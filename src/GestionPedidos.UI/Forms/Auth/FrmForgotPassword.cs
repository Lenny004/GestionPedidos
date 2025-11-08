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
        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private void tggMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirmPassword.PasswordChar = tggMostrarContraseña.Checked ? '\0' : '●';
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            AuthController controller = new AuthController();

            // Llama al método del controlador
            var result = controller.ResetPassword(email, newPass, confirmPass);

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
