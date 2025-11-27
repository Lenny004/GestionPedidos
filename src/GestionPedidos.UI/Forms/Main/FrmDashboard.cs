using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionPedidos.UI.Forms.About;
using GestionPedidos.UI.Forms.Customers;
using GestionPedidos.UI.Forms.Products;
using GestionPedidos.UI.Forms.Delivery;
using GestionPedidos.UI.Forms.Users;
using GestionPedidos.UI.Forms.History;
using GestionPedidos.Common.Security;
using GestionPedidos.Models.Enums;

namespace GestionPedidos.UI.Forms.Main
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
            ConfigureButtons();
            ApplySecurityPermissions();
        }

        private void ApplySecurityPermissions()
        {
            // Verificamos si el usuario actual es un Operador (Rol = 2)
            if (SessionManager.Rol == TipoRoles.Operador)
            {
                // OCULTAR BOTONES RESTRINGIDOS

                // 1. Productos
                if (btnProducts != null) btnProducts.Visible = false;

                // 2. Usuarios
                if (btnUsers != null) btnUsers.Visible = false;

                // 3. Historial (Ahora sí lo ocultamos porque ya existe el botón)
                if (btnHistory != null) btnHistory.Visible = false;
            }
        }

        private void ConfigureButtons()
        {
            SetupButton(btnDashboard);
            SetupButton(btnCustomer);
            SetupButton(btnProducts);
            SetupButton(btnDelivery);
            SetupButton(btnOrders);
            SetupButton(btnUsers);
            SetupButton(btnAboutUs);
            SetupButton(btnHistory);

            // Marcar dashboard como activo por defecto
            btnDashboard.Checked = true;
        }

        private void SetupButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;
            
            // Configurar comportamiento de RadioButton (uno activo a la vez)
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            
            // Copiar estilos de Hover a CheckedState para mantener consistencia visual
            btn.CheckedState.FillColor = btn.HoverState.FillColor;
            btn.CheckedState.ForeColor = btn.HoverState.ForeColor;
            btn.CheckedState.Image = btn.HoverState.Image;
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmCustomer());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmProducts());
        }

        private void btnAboutUs_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmAbout());
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmDelivery());
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmUsers());
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmHistory());
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmDetailOrder());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Confirmar cierre de sesión
            var result = MessageBox.Show(
                "¿Está seguro que desea cerrar sesión?",
                "Cerrar Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Cerrar la sesión actual
                SessionManager.Logout();
                
                // Cerrar el dashboard (esto regresará al login automáticamente)
                this.Close();
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
