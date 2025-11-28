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
using NLog;

namespace GestionPedidos.UI.Forms.Delivery
{
    public partial class FrmDetailOrder : Form
    {
        private DeliveryController _deliveryController;
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private int _currentOrderId = 0;

        public FrmDetailOrder()
        {
            InitializeComponent();
            _deliveryController = new DeliveryController();
        }

        private void FrmDetailOrder_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar el ComboBox con los estados (sin disparar el evento)
                if (cmbChangeStatus != null)
                {
                    cmbChangeStatus.SelectedIndexChanged -= cmbChangeStatus_SelectedIndexChanged;
                    cmbChangeStatus.Items.Clear();
                    cmbChangeStatus.Items.Add("Pending");
                    cmbChangeStatus.Items.Add("InProcess");
                    cmbChangeStatus.Items.Add("Delivered");
                    cmbChangeStatus.Items.Add("Cancelled");
                    cmbChangeStatus.SelectedIndex = 0;
                    cmbChangeStatus.SelectedIndexChanged += cmbChangeStatus_SelectedIndexChanged;
                }

                LoadOrderDetails();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error en FrmDetailOrder_Load");
                MessageBox.Show($"Error al cargar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                var result = _deliveryController.GetAllOrderDetails();
                
                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (result.OrderDetails == null || !result.OrderDetails.Any())
                {
                    MessageBox.Show("No hay detalles de órdenes disponibles", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Vincular datos al DataGridView
                DataGridDetail.DataSource = result.OrderDetails.ToList();
                ConfigureDataGridColumns();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al cargar detalles de órdenes");
                throw;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadOrderDetails();
                    return;
                }

                var result = _deliveryController.SearchOrderDetails(searchTerm);
                
                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var detailList = new List<OrderDetailListDto>(result.OrderDetails);
                DataGridDetail.DataSource = detailList;
                ConfigureDataGridColumns();
                MessageBox.Show($"Búsqueda completada: {result.Message}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error en búsqueda");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Clear();
                LoadOrderDetails();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al recargar");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridColumns()
        {
            try
            {
                if (DataGridDetail.Columns.Count == 0) return;

                // Configurar formato de columnas
                DataGridDetail.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                DataGridDetail.Columns["UnitPrice"].DefaultCellStyle.Format = "C2";
                DataGridDetail.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                DataGridDetail.Columns["OrderTotal"].DefaultCellStyle.Format = "C2";
                
                DataGridDetail.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                DataGridDetail.Columns["DeliveryDate"].DefaultCellStyle.Format = "yyyy-MM-dd";

                // Configurar ancho de columnas
                DataGridDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Hacer la columna IdOrder más estrecha si es necesario
                if (DataGridDetail.Columns["IdOrder"] != null)
                    DataGridDetail.Columns["IdOrder"].Width = 80;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error configurando columnas del DataGrid");
            }
        }

        private void DataGridDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar clic en header
            if (e.RowIndex < 0) return;

            // Evitar grid vacío o sin selección
            if (DataGridDetail.SelectedRows.Count == 0) return;

            try
            {
                // Obtener el objeto enlazado a la fila
                var selectedRow = DataGridDetail.SelectedRows[0];
                var selectedOrderDetail = (OrderDetailListDto)selectedRow.DataBoundItem;

                // Cargar detalle en los labels
                LoadOrderDetailInfo(selectedOrderDetail);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error en DataGridDetail_CellClick");
                MessageBox.Show($"Error al cargar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderDetailInfo(OrderDetailListDto orderDetail)
        {
            try
            {
                // Guardar el ID de la orden actual
                _currentOrderId = orderDetail.IdOrder;

                // --- DATOS BÁSICOS ---
                lblID.Text = "#" + orderDetail.IdOrder.ToString();
                lblProduct.Text = orderDetail.ProductName;
                lblCustomer.Text = orderDetail.CustomerName;
                lblComments.Text = orderDetail.OrderStatus;
                lblTotal.Text = "Total mount: " +  orderDetail.OrderTotal.ToString("C2");
                lblQuantity.Text = "Quantity: " + orderDetail.Quantity.ToString();
                lblSubtotal.Text = "Subtotal: "  + orderDetail.Subtotal.ToString("C2");
                lblPrice.Text = "Unit Price: " + orderDetail.UnitPrice.ToString("C2");

                if (btnStatus != null)
                    btnStatus.Text = orderDetail.OrderStatus;

                // Fecha de orden
                lblOrderDate.Text = $"Order Date: {orderDetail.OrderDate:g}";

                // SECCIÓN DE Fechas
                bool hasUpdated = orderDetail.UpdatedAt.HasValue;

                // Fecha de Entrega
                if (lblDeliveryDate != null)
                {
                    lblDeliveryDate.Visible = true;
                    lblDeliveryDate.Text = $"Delivery Date: {orderDetail.DeliveryDate:g}";
                }

                // Usuario Creador
                if (lblAddedby != null)
                {
                    lblAddedby.Visible = true;
                    lblAddedby.Text = $"Added by: {orderDetail.CreatedByUser}";
                }

                // Usuario Modificador
                if (lblModifyby != null)
                {
                    lblModifyby.Visible = hasUpdated;

                    if (hasUpdated)
                    {
                        lblModifyby.Text = $"Modify Date: {orderDetail.UpdatedAt.Value:g}";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al cargar información del detalle de orden");
                throw;
            }
        }

        private void LoadOrderDetailInfo_UpdateOrderId(OrderDetailListDto orderDetail)
        {
            _currentOrderId = orderDetail.IdOrder;
        }

        private void cmbChangeStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentOrderId <= 0)
            {
                MessageBox.Show("Por favor, seleccione una orden primero", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var comboBox = sender as ComboBox;
                if (comboBox == null || comboBox.SelectedIndex < 0) return;

                // Obtener el estado actual del label
                string currentStatus = lblComments.Text;

                // Validar si la orden ya está cancelada
                if (currentStatus.ToLower() == "cancelled")
                {
                    MessageBox.Show("No se puede cambiar el estado de una orden cancelada.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox.SelectedIndex = GetStatusIndex(currentStatus);
                    return;
                }

                string newStatus = comboBox.SelectedItem.ToString();
                
                // No cambiar si es el mismo estado
                if (newStatus.ToLower() == currentStatus.ToLower())
                {
                    return;
                }

                // Mostrar cuadro de confirmación
                string confirmMessage = $"¿Desea cambiar el estado a '{newStatus}'?";
                if (newStatus.ToLower() == "cancelled")
                {
                    confirmMessage += "\n\nNota: Se restaurará el stock de los productos de esta orden.";
                }

                if (MessageBox.Show(confirmMessage, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = _deliveryController.UpdateOrderStatus(_currentOrderId, newStatus);
                    if (result.Success)
                    {
                        MessageBox.Show(result.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetails();
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Restaurar el ComboBox al estado actual
                        comboBox.SelectedIndex = GetStatusIndex(currentStatus);
                    }
                }
                else
                {
                    // Restaurar el ComboBox al estado actual
                    comboBox.SelectedIndex = GetStatusIndex(currentStatus);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al cambiar estado");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetStatusIndex(string status)
        {
            // Retorna el índice del ComboBox según el estado
            switch (status.ToLower())
            {
                case "pending": return 0;
                case "inprocess": return 1;
                case "delivered": return 2;
                case "cancelled": return 3;
                default: return 0;
            }
        }
    }
}
