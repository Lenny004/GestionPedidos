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

namespace GestionPedidos.UI.Helpers.Products
{
    public partial class FrmModifyProduct : Form
    {
        private readonly ProductController _productController = new ProductController();
        public FrmModifyProduct(int id)
        {
            InitializeComponent();
            // Configurar rangos para evitar errores de validación
            txtStockQuantity.Maximum = 10000; // 10k como máximo razonable
            txtStockQuantity.Minimum = 0; // No permitir negativos
            LoadStatus();
            LoadProduct(id);
        }

        private void LoadStatus()
        {
            try
            {
                // Llamar al controlador en lugar de acceder directamente al enum
                cmbStatus.DataSource = _productController.GetProductStatuses();
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se obtuvieron los estados del producto. {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadProduct(int idProduct)
        {
            var (success, message, fullProduct) = _productController.ReadOne(idProduct);
            if (success && fullProduct != null)
            {
                lblID.Text = "#" + fullProduct.IdProduct.ToString();
                txtProductName.Text = fullProduct.ProductName;
                txtDescription.Text = fullProduct.Description;
                txtStockQuantity.Value = fullProduct.StockQuantity;
                txtSalePrice.Text = fullProduct.SalePrice.ToString("C2");
                cmbStatus.SelectedValue = (byte)fullProduct.Status;
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModifyC_Click(object sender, EventArgs e)
        {
            var (success, message) = _productController.Update(
                int.Parse(lblID.Text.TrimStart('#')),
                txtProductName.Text,
                txtDescription.Text,
                (int)txtStockQuantity.Value,
                decimal.Parse(txtSalePrice.Text),
                Convert.ToByte(cmbStatus.SelectedValue)
            );

            if (success)
            {
                MessageBox.Show(message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
