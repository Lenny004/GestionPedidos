using GestionPedidos.Controllers;
using GestionPedidos.Models.DTOs;
using GestionPedidos.UI.Helpers.Products;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GestionPedidos.UI.Forms.Products
{
    public partial class FrmProducts : Form
    {
        private readonly ProductController _productController = new ProductController();
        private static int selectedProductId = 0;

        public FrmProducts()
        {
            InitializeComponent();
            this.Load += FrmProducts_Load;
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            LoadProductsIntoGrid();
        }

        public void LoadProductsIntoGrid()
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
                DataGridProduct.AutoGenerateColumns = true;
                DataGridProduct.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadProduct(ProductListDto product)
        {
            // Ir a la base de datos a traer TODOS los datos (incluida descripción)
            var (success, message, fullProduct) = _productController.ReadOne(product.IdProduct);
            if (success && fullProduct != null)
            {
                lblID.Text = "#" + fullProduct.IdProduct.ToString();
                lblName.Text = fullProduct.ProductName;
                lblDescription.Text = fullProduct.Description;
                lblStock.Text = "Stock: " + fullProduct.StockQuantity.ToString();
                lblPrice.Text = "Price: " + fullProduct.SalePrice.ToString("C2");
                btnStatus.Text = fullProduct.IsActive ? "Activo" : "Inactivo";
                lblCreationDate.Text = "Creation: " + (fullProduct.CreatedAt.ToString("g"));
                lblAdded.Text = "Added by: " +  fullProduct.UserCreation;
                // Fechas
                bool hasUpdated = fullProduct.UpdatedAt.HasValue;
                lblModifyDate.Visible = hasUpdated;
                lblModifyDate.Text = "Modify: " + (hasUpdated ? fullProduct.UpdatedAt.Value.ToString("g") : "N/A");

                bool hasDeleted = fullProduct.DeletedAt.HasValue;
                lblDeletedDate.Visible = hasDeleted;
                lblDeletedDate.Text = "Deleted: " + (hasDeleted ? fullProduct.DeletedAt.Value.ToString("g") : "N/A");

                // Usuario que modificó
                bool hasModifier = !string.IsNullOrEmpty(fullProduct.UserModification);
                lblModify.Visible = hasModifier;
                lblModify.Text = "Modified by: " + (hasModifier ? fullProduct.UserModification : "N/A")  ;
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar clic en header
            if (e.RowIndex < 0) return;

            // Evitar grid vacío o sin selección
            if (DataGridProduct.SelectedRows.Count == 0) return;

            // Obtener el objeto enlazado a la fila 
            var selectedRow = DataGridProduct.SelectedRows[0];
            var selectedProduct = (ProductListDto)selectedRow.DataBoundItem;
            // Mandamos el objeto a otra función para cargar los datos
            LoadProduct(selectedProduct);
            selectedProductId = selectedProduct.IdProduct;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var addProductForm = new FrmAddProduct())
            {
                if (addProductForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProductsIntoGrid();
                }
            }
        }

        private void btnModifyC_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var modifyProductForm = new FrmModifyProduct(selectedProductId))
            {
                if (modifyProductForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProductsIntoGrid();
                }
            }
        }

        private void btnDeleteC_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (success, message) = _productController.Delete(selectedProductId);
                if (success)
                {
                    MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductsIntoGrid();
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadProductsIntoGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var (success, message, products) = _productController.SearchByName(txtSearch.Text);

                if (!success)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var data = products != null ? new List<ProductListDto>(products) : new List<ProductListDto>();
                DataGridProduct.AutoGenerateColumns = true;
                DataGridProduct.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
