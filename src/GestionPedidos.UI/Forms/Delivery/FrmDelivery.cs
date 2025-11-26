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
        
        // Lista temporal para manejar los productos del pedido en memoria
        private BindingList<OrderDetailItem> _orderItems = new BindingList<OrderDetailItem>();

        public FrmDelivery()
        {
            InitializeComponent();
            _productController = new ProductController();
            _customerController = new CustomerController();

            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            txtStockQuantity.ValueChanged += txtStockQuantity_ValueChanged;
            
            // Configurar el DataGridView para mostrar los productos del pedido
            ConfigureOrderItemsGrid();
        }

        /// <summary>
        /// Configura las columnas del DataGridView para mostrar los items del pedido
        /// </summary>
        private void ConfigureOrderItemsGrid()
        {
            dgvOrderItems.AutoGenerateColumns = false;
            dgvOrderItems.DataSource = _orderItems;

            dgvOrderItems.Columns.Clear();
            
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.ProductName),
                HeaderText = "Product",
                Name = "colProduct",
                ReadOnly = true
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.Quantity),
                HeaderText = "Quantity",
                Name = "colQuantity",
                Width = 100,
                ReadOnly = true
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.UnitPrice),
                HeaderText = "Unit Price",
                Name = "colUnitPrice",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                ReadOnly = true
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.Subtotal),
                HeaderText = "Subtotal",
                Name = "colSubtotal",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" },
                ReadOnly = true
            });
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

        /// <summary>
        /// Agrega el producto seleccionado a la lista temporal del pedido
        /// </summary>
        private void btnAddC_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que hay un producto seleccionado
                if (!(cmbProduct.SelectedItem is ProductSelectDto selectedProduct) || selectedProduct.IdProduct == 0)
                {
                    MessageBox.Show("Por favor seleccione un producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar cantidad
                if (txtStockQuantity.Value <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int quantity = (int)txtStockQuantity.Value;

                // Verificar si el producto ya está en la lista
                var existingItem = _orderItems.FirstOrDefault(i => i.IdProduct == selectedProduct.IdProduct);
                
                if (existingItem != null)
                {
                    // Si ya existe, actualizar la cantidad
                    int newQuantity = existingItem.Quantity + quantity;
                    
                    // Verificar que no exceda el stock disponible
                    if (newQuantity > selectedProduct.StockQuantity)
                    {
                        MessageBox.Show($"La cantidad total ({newQuantity}) excede el stock disponible ({selectedProduct.StockQuantity}).", 
                            "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    existingItem.Quantity = newQuantity;
                    
                    // Forzar actualización del BindingList
                    _orderItems.ResetBindings();
                }
                else
                {
                    // Agregar nuevo item
                    var newItem = new OrderDetailItem(
                        selectedProduct.IdProduct,
                        selectedProduct.ProductName,
                        quantity,
                        selectedProduct.SalePrice
                    );
                    
                    _orderItems.Add(newItem);
                }

                // Actualizar total
                UpdateOrderTotal();

                // Resetear controles
                cmbProduct.SelectedIndex = 0;
                ResetProductDetails();

                MessageBox.Show("Producto agregado al pedido.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina el producto seleccionado de la lista temporal del pedido
        /// </summary>
        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrderItems.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor seleccione un producto a eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("¿Está seguro de eliminar este producto del pedido?", 
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int selectedIndex = dgvOrderItems.SelectedRows[0].Index;
                    _orderItems.RemoveAt(selectedIndex);
                    UpdateOrderTotal();
                    MessageBox.Show("Producto eliminado del pedido.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza el total del pedido sumando todos los subtotales
        /// </summary>
        private void UpdateOrderTotal()
        {
            decimal total = _orderItems.Sum(item => item.Subtotal);
            lblTotal.Text = $"Total: {total:C2}";
        }

        /// <summary>
        /// Confirma y guarda el pedido en la base de datos
        /// </summary>
        private void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que hay un cliente seleccionado
                if (!(cmbCustomers.SelectedItem is CustomerSelectDto selectedCustomer) || selectedCustomer.IdCustomer == 0)
                {
                    MessageBox.Show("Por favor seleccione un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que hay productos en el pedido
                if (_orderItems.Count == 0)
                {
                    MessageBox.Show("Debe agregar al menos un producto al pedido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // TODO: Aquí se debe implementar la lógica para guardar el pedido en la base de datos
                // Por ahora solo mostramos un mensaje de confirmación
                
                var result = MessageBox.Show(
                    $"¿Confirmar pedido para {selectedCustomer.FullName}?\n" +
                    $"Total de productos: {_orderItems.Count}\n" +
                    $"Total: {_orderItems.Sum(i => i.Subtotal):C2}",
                    "Confirmar pedido",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // TODO: Llamar al controlador para guardar el pedido
                    // orderController.CreateOrder(customerId, deliveryDate, comments, _orderItems);
                    
                    MessageBox.Show("Pedido creado exitosamente.\n\nNOTA: Pendiente implementar el guardado en base de datos.", 
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Limpiar el formulario
                    ClearOrderForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al confirmar pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Limpia el formulario después de confirmar un pedido
        /// </summary>
        private void ClearOrderForm()
        {
            _orderItems.Clear();
            cmbCustomers.SelectedIndex = 0;
            cmbProduct.SelectedIndex = 0;
            txtComment.Text = string.Empty;
            dtpDelivery.Value = DateTime.Now;
            UpdateOrderTotal();
            ResetProductDetails();
        }
    }
}
