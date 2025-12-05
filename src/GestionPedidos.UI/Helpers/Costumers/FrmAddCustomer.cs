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

namespace GestionPedidos.UI.Helpers.Costumers
{
    public partial class FrmAddCustomer : Form
    {
        private readonly CustomerController _customerController = new CustomerController();

        public FrmAddCustomer()
        {
            InitializeComponent();
            LoadCities();
        }

        private void LoadCities()
        {
            try
            {
                cmbCities.DataSource = _customerController.GetCitiesList();
                cmbCities.DisplayMember = "Text";
                cmbCities.ValueMember = "Value";
                cmbCities.SelectedIndex = -1;
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int? idCity = cmbCities.SelectedValue != null ? (int?)cmbCities.SelectedValue : null;
                var result = _customerController.Create(
                    txtFirstName.Text.Trim(), txtLastName.Text.Trim(),
                    txtPhone.Text.Trim(), txtAddress.Text.Trim(), idCity
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    }
}
