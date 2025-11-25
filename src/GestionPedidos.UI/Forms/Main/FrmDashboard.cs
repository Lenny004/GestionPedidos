using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionPedidos.UI.Forms.Customers;
using GestionPedidos.UI.Forms.Delivery;
using GestionPedidos.UI.Forms.Orders;
using GestionPedidos.UI.Forms.Products;

namespace GestionPedidos.UI.Forms.Main
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
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
        private void btnDelivery_Click(object sender, EventArgs e)
        {
            // Reutilizas tu método openChildForm con el formulario de Delivery
            openChildForm(new FrmDelivery());
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            // Reutilizas tu método openChildForm con el formulario de Delivery
            openChildForm(new FrmOrders());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmCustomer());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmProducts());
        }
    }
}
