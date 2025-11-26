using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPedidos.UI.Forms.Customers
{
    public partial class FrmCustomer : Form
    {
        // Instancia del Controlador
        private readonly CustomerController _customerController = new CustomerController();
        public FrmCustomer()
        {
            InitializeComponent();
            this.Load += FrmCustomer_Load;
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomersIntoGrid();
        }

        private void LoadCustomersIntoGrid()
        {
            try
            {
                // Llamamos al método ReadAll que ya creamos anteriormente
                var (success, message, customers) = _customerController.ReadAll();

                if (!success)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertimos a lista y asignamos
                var data = customers != null ? new List<CustomerListDto>(customers) : new List<CustomerListDto>();

                if (data.Count == 0)
                {
                    MessageBox.Show("La consulta se ejecutó correctamente pero no devolvió registros. Verifique que existan datos en la tabla Customers.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Asumiendo que tu tabla se llama dgvCustomers
                dgvCustomers.AutoGenerateColumns = true;
                dgvCustomers.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
