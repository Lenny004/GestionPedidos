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
using GestionPedidos.Models.DTO;

namespace GestionPedidos.UI.Forms.Delivery
{
    public partial class FrmDelivery : Form
    {
        private readonly ProductController _productController;
        private readonly CustomerController _customerController;
        private List<ProductSelectDto> _productOptions = new List<ProductSelectDto>();
        private List<CustomerSelectDto> _customerOptions = new List<CustomerSelectDto>();
        private List<CustomerSelectDto> _allCustomers = new List<CustomerSelectDto>();

        public FrmDelivery()
        {
            InitializeComponent();
            _productController = new ProductController();
            _customerController = new CustomerController();

            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            txtStockQuantity.ValueChanged += txtStockQuantity_ValueChanged;
        }

        private void LoadProducts()
        {
            try
            {
                _productOptions = _productController.GetProductsForCombo() ?? new List<ProductSelectDto>();

                cmbProduct.DataSource = null;
                cmbProduct.Items.Clear();
                
                if (_productOptions.Count == 0)
                {
                    cmbProduct.Enabled = false;
                    ResetProductDetails();
                    return;
                }
                cmbProduct.Enabled = true;

                _productOptions.Insert(0, new ProductSelectDto
                {
                    IdProduct = 0,
                    ProductName = "Seleccione un producto",
                    SalePrice = 0m,
                    StockQuantity = 0
                });

                cmbProduct.DataSource = _productOptions;
                cmbProduct.DisplayMember = nameof(ProductSelectDto.ProductName);
                cmbProduct.ValueMember = nameof(ProductSelectDto.IdProduct);
                cmbProduct.SelectedIndex = 0;
                ResetProductDetails();
            }
            catch (Exception ex)
            {
                cmbProduct.DataSource = null;
                cmbProduct.Items.Clear();
                cmbProduct.Enabled = false;
                ResetProductDetails();
                MessageBox.Show($"Error al cargar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadCustomers()
        {
            try
            {
                _allCustomers = _customerController.GetCustomersForCombo() ?? new List<CustomerSelectDto>();
                BindCustomerCombo(_allCustomers);
            }
            catch (Exception ex)
            {
                cmbCustomers.DataSource = null;
                cmbCustomers.Items.Clear();
                cmbCustomers.Enabled = false;
                MessageBox.Show($"Error al cargar los clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindCustomerCombo(IEnumerable<CustomerSelectDto> customers)
        {
            var customerList = customers?.ToList() ?? new List<CustomerSelectDto>();

            cmbCustomers.DataSource = null;
            cmbCustomers.Items.Clear();

            if (customerList.Count == 0)
            {
                cmbCustomers.Enabled = false;
                return;
            }

            cmbCustomers.Enabled = true;

            customerList.Insert(0, new CustomerSelectDto
            {
                IdCustomer = 0,
                FullName = "Seleccione un cliente"
            });

            cmbCustomers.DataSource = customerList;
            cmbCustomers.DisplayMember = nameof(CustomerSelectDto.FullName);
            cmbCustomers.ValueMember = nameof(CustomerSelectDto.IdCustomer);
            cmbCustomers.SelectedIndex = 0;
        }

        private void FrmDelivery_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadProducts();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem is ProductSelectDto selectedProduct)
            {
                if (selectedProduct.IdProduct == 0)
                {
                    ResetProductDetails();
                    return;
                }

                txtSalePrice.Text = selectedProduct.SalePrice.ToString("F2");

                if (selectedProduct.StockQuantity <= 0)
                {
                    txtStockQuantity.Enabled = false;
                    lblSubtotal.Text = "Total: $0.00";
                    return;
                }

                txtStockQuantity.Enabled = true;
                txtStockQuantity.Minimum = 1;
                txtStockQuantity.Maximum = selectedProduct.StockQuantity;
                txtStockQuantity.Value = 1;
                UpdateSubtotal(selectedProduct);
            }
            else
            {
                ResetProductDetails();
            }
        }

        private void txtStockQuantity_ValueChanged(object sender, EventArgs e)
        {
            var selectedProduct = cmbProduct.SelectedItem as ProductSelectDto;
            if (selectedProduct != null && selectedProduct.IdProduct != 0 && txtStockQuantity.Enabled && txtStockQuantity.Value > 0)
            {
                UpdateSubtotal(selectedProduct);
            }
            else
            {
                lblSubtotal.Text = "Total: $0.00";
            }
        }

        private void UpdateSubtotal(ProductSelectDto product)
        {
            var quantity = (int)txtStockQuantity.Value;
            var subtotal = product.SalePrice * quantity;
            lblSubtotal.Text = $"Total: ${subtotal:F2}";
        }

        private void ResetProductDetails()
        {
            txtSalePrice.Text = string.Empty;
            txtStockQuantity.Enabled = false;
            txtStockQuantity.Minimum = 0;
            txtStockQuantity.Maximum = 0;
            txtStockQuantity.Value = 0;
            lblSubtotal.Text = "Total: $0.00";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text?.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                BindCustomerCombo(_allCustomers);
                return;
            }

            var filteredCustomers = _allCustomers
                .Where(c => c.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (filteredCustomers.Count == 0)
            {
                MessageBox.Show("No se encontraron clientes que coincidan con la búsqueda.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindCustomerCombo(filteredCustomers);
            
            if (filteredCustomers.Count > 0)
            {
                cmbCustomers.SelectedIndex = 1;
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            txtSearch.Text = string.Empty;
        }
    }
}
