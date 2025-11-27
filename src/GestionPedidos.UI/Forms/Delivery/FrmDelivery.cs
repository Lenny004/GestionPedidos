using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Services;
using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPedidos.UI.Forms.Delivery
{
    public partial class FrmDelivery : Form
    {
        private readonly ProductController _productController;
        private readonly CustomerController _customerController;
        private readonly DeliveryController _deliveryController;
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
            _deliveryController = new DeliveryController();
            
            // Configurar el DataGridView para mostrar los productos del pedido
            ConfigureOrderItemsGrid();
        }

        /// <summary>
        /// Metodo que se ejecuta al cargar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDelivery_Load(object sender, EventArgs e)
        {
            // Cargar clientes y productos
            LoadCustomers();
            LoadProducts();
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
                Name = "colProduct"
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.Quantity),
                HeaderText = "Quantity",
                Name = "colQuantity"
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.UnitPrice),
                HeaderText = "Unit Price",
                Name = "colUnitPrice",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderDetailItem.Subtotal),
                HeaderText = "Subtotal",
                Name = "colSubtotal",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
        }

        /// <summary>
        /// Carga los productos disponibles en el combo box
        /// </summary>
        private void LoadProducts()
        {
            try
            {
                _productOptions = _productController.GetProductsForCombo() ?? new List<ProductSelectDto>();
                BindProductCombo(_productOptions);
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

        /// <summary>
        /// Cargar los clientes disponibles en el combo box
        /// </summary>
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

        /// <summary>
        /// Metodo para enlazar la lista de clientes al combo box
        /// </summary>
        /// <param name="customers"></param>
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

        /// <summary>
        /// Maneja la búsqueda de clientes por nombre (solo activos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text?.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                BindCustomerCombo(_allCustomers);
                return;
            }

            var (success, message, customers) = _customerController.SearchByName(searchText);

            if (!success)
            {
                NotificationService.NotifyValidationError(message ?? AppConstants.NO_SE_ENCONTRARON_REGISTROS);
                return;
            }

            // Si el controller no devolvió nada, usamos lista vacía
            var foundCustomers = customers ?? new List<CustomerListDto>();

            // Convertimos a DTOs y filtramos solo activos
            var customerOptions = foundCustomers
                .Where(c => c.IsActive)
                .Select(c => new CustomerSelectDto
                {
                    IdCustomer = c.IdCustomer,
                    FullName = c.FullName
                })
                .ToList();

            if (customerOptions.Count == 0)
            {
                NotificationService.ShowInfo(AppConstants.NO_SE_ENCONTRARON_REGISTROS, "Sin resultados");
                BindCustomerCombo(customerOptions);
                return;
            }

            BindCustomerCombo(customerOptions);

            if (cmbCustomers.Items.Count > 1)
            {
                cmbCustomers.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Recarga la lista completa de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    cmbCustomers.Enabled = false; // Deshabilitar selección de cliente al agregar productos
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
                    // Llamar al controlador para guardar el pedido
                    var (success, message, orderId) = _deliveryController.CreateOrder(
                        selectedCustomer.IdCustomer,
                        dtpDelivery.Value,
                        txtComment.Text,
                        new List<OrderDetailItem>(_orderItems)
                    );

                    if (success)
                    {
                        MessageBox.Show($"Pedido #{orderId} creado exitosamente.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Limpiar el formulario
                        ClearOrderForm();
                    }
                    else
                    {
                        MessageBox.Show($"Error al crear el pedido: {message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        /// <summary>
        /// Maneja el cambio de selección en el combo de productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Maneja la búsqueda de productos por nombre (solo activos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            var searchText = txtSearchProduct.Text?.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadProducts();
                return;
            }

            try
            {
                var (success, message, products) = _productController.SearchByName(searchText);

                if (!success)
                {
                    NotificationService.NotifyValidationError(message ?? AppConstants.NO_SE_ENCONTRARON_REGISTROS);
                    return;
                }

                // Si el controller no devolvió nada, usamos lista vacía
                var foundProducts = products ?? new List<ProductListDto>();

                // Convertimos a DTOs y filtramos solo activos
                var productOptions = foundProducts
                    .Where(p => p.IsActive)
                    .Select(p => new ProductSelectDto
                    {
                        IdProduct = p.IdProduct,
                        ProductName = p.ProductName,
                        SalePrice = p.SalePrice,
                        StockQuantity = p.StockQuantity
                    })
                    .ToList();

                if (productOptions.Count == 0)
                {
                    NotificationService.ShowInfo(AppConstants.NO_SE_ENCONTRARON_REGISTROS, "Sin resultados");
                    BindProductCombo(productOptions);
                    return;
                }

                BindProductCombo(productOptions);

                if (cmbProduct.Items.Count > 1)
                {
                    cmbProduct.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Recarga la lista completa de productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReloadProduct_Click(object sender, EventArgs e)
        {
            LoadProducts();
            txtSearchProduct.Text = string.Empty;
        }

        /// <summary>
        /// Metodo para enlazar la lista de productos al combo box
        /// </summary>
        /// <param name="products"></param>
        private void BindProductCombo(IEnumerable<ProductSelectDto> products)
        {
            var productList = products?.ToList() ?? new List<ProductSelectDto>();

            cmbProduct.DataSource = null;
            cmbProduct.Items.Clear();

            if (productList.Count == 0)
            {
                cmbProduct.Enabled = false;
                ResetProductDetails();
                return;
            }

            cmbProduct.Enabled = true;

            productList.Insert(0, new ProductSelectDto
            {
                IdProduct = 0,
                ProductName = "Seleccione un producto",
                SalePrice = 0m,
                StockQuantity = 0
            });

            cmbProduct.DataSource = productList;
            cmbProduct.DisplayMember = nameof(ProductSelectDto.ProductName);
            cmbProduct.ValueMember = nameof(ProductSelectDto.IdProduct);
            cmbProduct.SelectedIndex = 0;
            ResetProductDetails();
        }
    }
}
