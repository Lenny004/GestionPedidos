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
using GestionPedidos.Common.Services;

namespace GestionPedidos.UI.Forms.Auth
{
    public partial class FrmRegister : Form
    {
        private readonly AuthController _authController = new AuthController();
        private readonly bool _isFirstUse; // Guardar el valor en una variable de clase

        public FrmRegister(Boolean isFirstUse)
        {
            InitializeComponent();
            _isFirstUse = isFirstUse; // Almacenar el valor

            if (_isFirstUse)
            {
                // Ocultamos el lbl de "Do you have a account" 
                lblreturn.Visible = false;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string originalText = btnRegister.Text;
            btnRegister.Enabled = false;
            btnRegister.Text = "Loading...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string fullName = txtFullName.Text.Trim();
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text; // Contraseña en texto plano
                // Determinar el rol basado en si es primer uso
                int idRol = _isFirstUse ? 1 : 2; // 1 = Admin, 2 = Operador

                var resultado = _authController.Registrar(
                    username,
                    password,
                    fullName,
                    email,
                    idRol
                );

                if (resultado.Success)
                {
                    string mensajeAdicional = _isFirstUse ? " como Administrador" : "";

                    MessageBox.Show(resultado.Message, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultado.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado. Mensaje: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = originalText;
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblreturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.Show();
        }

        private void tggMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = tggMostrarContraseña.Checked ? '\0' : '●';
        }
    }
}
