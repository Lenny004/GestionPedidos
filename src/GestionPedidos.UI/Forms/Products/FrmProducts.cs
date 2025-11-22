using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestionPedidos.Controllers;
using GestionPedidos.Models.DTOs;
using GestionPedidos.UI.Helpers.Products;

namespace GestionPedidos.UI.Forms.Products
{
    public partial class FrmProducts : Form
    {
        private readonly ProductController _productController = new ProductController();

        public FrmProducts()
        {
            InitializeComponent();
            this.Load += FrmProducts_Load;
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            LoadProductsIntoGrid();
        }

        private void LoadProductsIntoGrid()
        {
            try
            {
                var (success, message, products) = _productController.ReadAll();

                if (!success)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var data = products != null ? new List<ProductListDto>(products) : new List<ProductListDto>();
                guna2DataGridView1.AutoGenerateColumns = true;
                guna2DataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAddC_Click(object sender, EventArgs e)
        {
            using (var addProductForm = new FrmAddProduct())
            {
                addProductForm.ShowDialog(this);
            }
        }

        private void btnModifyC_Click(object sender, EventArgs e)
        {
            using (var modifyProductForm = new FrmModifyProduct())
            {
                modifyProductForm.ShowDialog(this);
            }
        }
    }
}
