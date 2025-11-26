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

namespace GestionPedidos.UI.Forms.Main
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
            ConfigureButtons();
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
    }
}
