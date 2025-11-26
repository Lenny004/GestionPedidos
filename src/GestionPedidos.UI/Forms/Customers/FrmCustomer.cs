using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
using GestionPedidos.UI.Helpers.Costumers;
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
        private int _selectedCustomerId = 0; // ID para saber cuál modificar/eliminar
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

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomers.SelectedRows.Count == 0) return;

            var row = dgvCustomers.SelectedRows[0];
            var customerListDto = (CustomerListDto)row.DataBoundItem;

            _selectedCustomerId = customerListDto.IdCustomer;
            LoadCustomerDetail(_selectedCustomerId); // Cargar detalle lateral
        }

        private void LoadCustomerDetail(int idCustomer)
        {
            var (success, message, fullCustomer) = _customerController.ReadOne(idCustomer);

            if (success && fullCustomer != null)
            {
                // --- DATOS BÁSICOS ---
                lblID.Text = "#" + fullCustomer.IdCustomer.ToString();
                lblName.Text = fullCustomer.FullName;
                lblPhone.Text = "Phone: " + fullCustomer.Phone;
                lblAddress.Text = fullCustomer.Address;
                lblCity.Text = fullCustomer.City + ", " + fullCustomer.Department;

                if (btnStatus != null)
                    btnStatus.Text = fullCustomer.IsActive ? "Activo" : "Inactivo";

                // Fecha de creación
                lblCreatedBy.Text = $"Creation Date: {fullCustomer.CreatedAt:g}";

                // SECCIÓN DE Fechas
                bool hasUpdated = fullCustomer.UpdatedAt.HasValue;
                bool hasDeleted = fullCustomer.DeletedAt.HasValue;

                // Fecha de Modificación
                if (lblModifyDate != null)
                {
                    lblModifyDate.Visible = hasUpdated; // Solo visible si hay update
                    lblModifyDate.Text = hasUpdated ? $"Modify Date: {fullCustomer.UpdatedAt.Value:g}" : "";
                }

                // Fecha de Modificación
                if (lblDeletedDate != null)
                {
                    lblDeletedDate.Visible = hasDeleted; // Solo visible si hay delete
                    lblDeletedDate.Text = hasDeleted ? $"Deleted Date: {fullCustomer.DeletedAt.Value:g}" : "";
                }

                // Usuario Creador
                if (lblAddedBy != null)
                {
                    lblAddedBy.Visible = true;
                    lblAddedBy.Text = $"Added by: {fullCustomer.UserCreation}";
                }

                // Usuario Modificador
                if (lblModifiedBy != null)
                {
                    lblModifiedBy.Visible = hasUpdated; // Solo visible si hay update

                    if (hasUpdated)
                    {
                        string modifier = !string.IsNullOrEmpty(fullCustomer.UserModification) ? fullCustomer.UserModification : "N/A";
                        lblModifiedBy.Text = $"Modify by: {modifier}";
                    }
                }
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddC_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAddCustomer())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadCustomersIntoGrid();
            }
        }

        private void btnModifyC_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                MessageBox.Show("Seleccione un cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FrmModifyCustomer(_selectedCustomerId))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomersIntoGrid();
                    LoadCustomerDetail(_selectedCustomerId); // Refrescar detalle lateral también
                }
            }
        }

        private void btnDeleteC_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                MessageBox.Show("Seleccione un cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Eliminar cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (success, message) = _customerController.Delete(_selectedCustomerId);
                if (success)
                {
                    MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomersIntoGrid();
                    LoadCustomerDetail(_selectedCustomerId);
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                var (success, message, customers) = _customerController.SearchByName(txtSearch.Text.Trim());

                if (!success)
                {
                    MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var data = customers != null ? new List<CustomerListDto>(customers) : new List<CustomerListDto>();

                dgvCustomers.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomersIntoGrid();
        }
    }
}
