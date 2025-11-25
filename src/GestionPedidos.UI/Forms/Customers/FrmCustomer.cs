using GestionPedidos.Controllers;
using GestionPedidos.Models.DTOs;
using GestionPedidos.UI.Forms.Customers; // Asegúrate de que FrmAddCustomer esté aquí
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GestionPedidos.UI.Forms.Customers
{
    public partial class FrmCustomer : Form
    {
        // 1. Instancia del Controlador
        private readonly CustomerController _customerController = new CustomerController();

        public FrmCustomer()
        {
            InitializeComponent();
            // 2. Suscripción al evento Load (igual que en Products)
            this.Load += FrmCustomer_Load;
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomersIntoGrid();
        }

        // 3. Cargar la Grilla (Igual que LoadProductsIntoGrid)
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

                // Asumiendo que tu tabla se llama dgvCustomers
                dgvCustomers.AutoGenerateColumns = true;
                dgvCustomers.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. Cargar el Detalle Lateral (Igual que LoadProduct)
        private void LoadCustomerDetail(CustomerListDto customer)
        {
            // AQUÍ ESTÁ EL DETALLE: Necesitamos implementar ReadOne en CustomerController
            // Por ahora, usaré los datos que ya vienen en el DTO para que no te de error,
            // pero lo ideal es hacer el _customerController.ReadOne(customer.IdCustomer) después.

            if (customer != null)
            {
                // Asumiendo que tienes estos Labels en tu diseño de Customers
                lblID.Text = "#" + customer.IdCustomer.ToString();
                lblName.Text = customer.FullName;
                lblAddress.Text = customer.Address;
                lblPhone.Text = "Tel: " + customer.Phone;
                lblCity.Text = customer.City + ", " + customer.Department;

                // Estado
                btnStatus.Text = customer.IsActive ? "Activo" : "Inactivo";

                // Auditoría (Si el DTO lo tiene, si no, hay que agregarlo o usar ReadOne)
                lblCreatedBy.Text = "Created By: " + customer.CreatedBy;
            }
        }

        // 5. Botón Agregar (Igual que btnAddC_Click)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Abrimos el formulario que creamos antes (FrmAddCustomer)
            using (var addCustomerForm = new FrmAddCustomer())
            {
                if (addCustomerForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Si guardó correctamente, recargamos la tabla
                    LoadCustomersIntoGrid();
                }
            }
        }

        // 6. Evento Click en la Celda (Igual que DataGridProduct_CellClick)
        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar clic en header
            if (e.RowIndex < 0) return;

            // Evitar grid vacío
            if (dgvCustomers.SelectedRows.Count == 0) return;

            // Obtener el objeto
            var selectedRow = dgvCustomers.SelectedRows[0];

            // Hacemos cast al DTO de Clientes
            var selectedCustomer = (CustomerListDto)selectedRow.DataBoundItem;

            // Cargamos los labels
            LoadCustomerDetail(selectedCustomer);
        }
    }
}