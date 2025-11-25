using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionPedidos.DataAccess.Repositories; // <--- IMPORTANTE PARA QUE RECONOZCA EL REPOSITORIO
using GestionPedidos.UI.Forms.Orders; // Para FrmAddOrder

namespace GestionPedidos.UI.Forms.Orders // (O el namespace que tengas)
{
    public partial class FrmOrders : Form
    {
        // 1. DECLARACIÓN FALTANTE:
        private readonly OrderRepository _orderRepo;

        public FrmOrders()
        {
            InitializeComponent();

            // 2. INICIALIZACIÓN FALTANTE:
            _orderRepo = new OrderRepository();
        }

        private void FrmOrders_Load(object sender, EventArgs e)
        {
            LoadOrdersData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 1. Crear instancia del formulario de agregar
            FrmAddOrder frm = new FrmAddOrder();

            // 2. Mostrarlo como diálogo modal
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // 3. Refrescar la tabla para ver la nueva orden
                LoadOrdersData();
            }
        }

        private void LoadOrdersData()
        {
            try
            {
                // Ahora sí existe _orderRepo
                var orders = _orderRepo.GetOrdersForDelivery();

                // Asignar al DataGridView (sin paréntesis)
                guna2DataGridView1.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}