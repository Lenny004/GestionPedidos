using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
using GestionPedidos.UI.Forms.Products;
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
    public partial class FrmAddProduct : Form
    {
        private readonly ProductController _productController = new ProductController();
        public FrmAddProduct()
        {
            InitializeComponent();
            // Configurar rangos para evitar errores de validación
            txtStockQuantity.Maximum = 10000; // 10k como máximo razonable
            txtStockQuantity.Minimum = 0; // No permitir negativos
        }

        private void btnAddC_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtProductName.Text.Trim();
                string description = txtDescription.Text.Trim();
                int stock = (int)txtStockQuantity.Value;
                decimal price = decimal.Parse(txtSalePrice.Text.Trim());

                var resultado = _productController.Create(name, description, stock, price);

                if (resultado.Success)
                {
                    MessageBox.Show(resultado.Message, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultado.Message, "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado. Mensaje: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
