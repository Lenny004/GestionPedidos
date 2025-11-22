using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GestionPedidos.Controllers.AuthController;

namespace GestionPedidos.UI.Forms.Delivery

{
    public partial class FrmDelivery : Form
    {
        // Campo que faltaba: controlador de pedidos para la vista de delivery
        private OrderController _orderController;

        public FrmDelivery()
        {
            InitializeComponent();
            _orderController = new OrderController();
        }
        private void FrmDelivery_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = _orderController.GetOrdersForDeliveryView();

               
                dgvOrders.DataSource = orders;

                // Opcional: Ocultar columnas que no quieras ver (como IDs internos)
                // dgvOrders.Columns["IdOrder"].Visible = false;
                // dgvOrders.Columns["IdCustomer"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pedidos: " + ex.Message);
            }
        }
        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
