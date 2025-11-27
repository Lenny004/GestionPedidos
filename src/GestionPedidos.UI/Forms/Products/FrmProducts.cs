using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
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

                // Convertimos a lista y asignamos
                var data = products != null ? new List<ProductListDto>(products) : new List<ProductListDto>();

                if (data.Count == 0)
                {
                    MessageBox.Show("La consulta se ejecutó correctamente pero no devolvió registros. Verifique que existan datos en la tabla Products.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DataGridProduct.AutoGenerateColumns = true;
                DataGridProduct.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductDetail(int idProduct)
        {
            var (success, message, fullProduct) = _productController.ReadOne(idProduct);

            if (success && fullProduct != null)
            {
                // --- DATOS BÁSICOS ---
                lblID.Text = "#" + fullProduct.IdProduct.ToString();
                lblName.Text = fullProduct.ProductName;
                lblDescription.Text = fullProduct.Description;
                lblStock.Text = "Stock: " + fullProduct.StockQuantity.ToString();
                lblPrice.Text = "Price: " + fullProduct.SalePrice.ToString("C2");

                if (btnStatus != null)
                    btnStatus.Text = fullProduct.IsActive ? "Activo" : "Inactivo";

                // Fecha de creación
                lblCreationDate.Text = $"Creation Date: {fullProduct.CreatedAt:g}";

                // SECCIÓN DE Fechas
                bool hasUpdated = fullProduct.UpdatedAt.HasValue;
                bool hasDeleted = fullProduct.DeletedAt.HasValue;

                // Fecha de Modificación
                if (lblModifyDate != null)
                {
                    lblModifyDate.Visible = hasUpdated;
                    lblModifyDate.Text = hasUpdated ? $"Modify Date: {fullProduct.UpdatedAt.Value:g}" : "";
                }

                // Fecha de Eliminación
                if (lblDeletedDate != null)
                {
                    lblDeletedDate.Visible = hasDeleted;
                    lblDeletedDate.Text = hasDeleted ? $"Deleted Date: {fullProduct.DeletedAt.Value:g}" : "";
                }

                // Usuario Creador
                if (lblAdded != null)
                {
                    lblAdded.Visible = true;
                    lblAdded.Text = $"Added by: {fullProduct.UserCreation}";
                }

                // Usuario Modificador
                if (lblModify != null)
                {
                    lblModify.Visible = hasUpdated;

                    if (hasUpdated)
                    {
                        string modifier = !string.IsNullOrEmpty(fullProduct.UserModification) ? fullProduct.UserModification : "N/A";
                        lblModify.Text = $"Modify by: {modifier}";
                    }
                }
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

            selectedProductId = selectedProduct.IdProduct;
            LoadProductDetail(selectedProductId); // Cargar detalle lateral
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
                    LoadProductDetail(selectedProductId); // Refrescar detalle lateral también
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
                    LoadProductDetail(selectedProductId); // Refrescar detalle lateral también
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProductsIntoGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var (success, message, products) = _productController.SearchByName(txtSearch.Text.Trim());

                if (!success)
                {
                    MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var data = products != null ? new List<ProductListDto>(products) : new List<ProductListDto>();

                DataGridProduct.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
