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

namespace GestionPedidos.UI.Helpers.Costumers
{
    public partial class FrmModifyCustomer : Form
    {
        private readonly CustomerController _customerController = new CustomerController();
        private int _customerId;

        public FrmModifyCustomer(int id)
        {
            InitializeComponent();
            _customerId = id;
            LoadCities();
            LoadStatusOptions();
            LoadCustomerData(id);
        }

        private void LoadCities()
        {
            try
            {
                cmbCities.DataSource = _customerController.GetCitiesList();
                cmbCities.DisplayMember = "Text";
                cmbCities.ValueMember = "Value";
            }
            catch { }
        }

        private void LoadStatusOptions()
        {
            var statusOptions = new List<object>
            {
                new { Value = true, Text = "Activo" },
                new { Value = false, Text = "Inactivo" }
            };
            cmbStatus.DataSource = statusOptions;
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
        }

        private void LoadCustomerData(int id)
        {
            var (success, message, customer) = _customerController.ReadOne(id);

            if (success && customer != null)
            {
                lblID.Text = "#" + customer.IdCustomer;
                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;
                txtPhone.Text = customer.Phone ?? "";
                txtAddress.Text = customer.Address ?? "";

                // Seleccionar Ciudad (solo si tiene valor)
                if (customer.IdCity.HasValue)
                {
                    cmbCities.SelectedValue = customer.IdCity.Value;
                }
                else
                {
                    cmbCities.SelectedIndex = -1;
                }

                cmbStatus.SelectedValue = customer.IsActive;
            }
            else
            {
                MessageBox.Show("No se pudo cargar el cliente.");
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int? idCity = cmbCities.SelectedValue != null ? (int?)cmbCities.SelectedValue : null;
                bool isActive = (bool)cmbStatus.SelectedValue;

                var result = _customerController.Update(
                    _customerId,
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtAddress.Text.Trim(),
                    idCity,
                    isActive
                );

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aviso: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    }
}
