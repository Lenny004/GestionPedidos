using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestionPedidos.DataAccess.Repositories; 
using GestionPedidos.Models.Entities;

namespace GestionPedidos.UI.Forms.Orders
{
    public partial class FrmAddOrder : Form
    {
        // Eliminada la clase anidada CustomerRepository para usar el repositorio del proyecto
        private readonly CustomerRepository _customerRepo;
        private readonly OrderRepository _orderRepo;

        public FrmAddOrder()
        {
            InitializeComponent();
            _customerRepo = new CustomerRepository();
            _orderRepo = new OrderRepository();
        }

        // Este evento se ejecuta cuando se abre la ventanita
        private void FrmAddOrder_Load(object sender, EventArgs e)
        {
            CargarClientes();

            // Configurar fecha mínima (no entregar en el pasado)
            // Asegúrate de que tu DateTimePicker se llame 'dtpDeliveryDate'
            if (dtpDeliveryDate != null)
                dtpDeliveryDate.MinDate = DateTime.Now;
        }

        private void CargarClientes()
        {
            try
            {
                // Usamos el método real del repositorio de datos
                var clientes = _customerRepo.ReadAllCustomers();

                // Asegúrate de que tu ComboBox se llame 'cmbCustomers'
                cmbCustomers.DataSource = clientes;
                cmbCustomers.DisplayMember = "FullName"; // Mostrar propiedad calculada FullName
                cmbCustomers.ValueMember = "IdCustomer";  // Lo que vale (el ID oculto)
                cmbCustomers.SelectedIndex = -1; // Iniciar vacío
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        // ESTE ES EL BOTÓN DE CREAR (Asegúrate de enlazarlo en el diseño)
        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            // 1. Validaciones
            if (cmbCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Recolectar datos de los controles
                int idCliente = (int)cmbCustomers.SelectedValue;
                int idUsuario = 1; // ID Temporal del usuario logueado (Admin/System)
                DateTime fechaEntrega = dtpDeliveryDate.Value;
                string comentarios = txtComments.Text.Trim(); // Asegúrate de que el TextBox se llame txtComments

                // 3. Guardar en Base de Datos
                bool exito = _orderRepo.CreateOrderHeader(idCliente, idUsuario, fechaEntrega, comentarios);

                if (exito)
                {
                    MessageBox.Show("Orden creada correctamente. Ahora puede agregar productos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Esto le dice al formulario padre (Orders) que todo salió bien y debe recargar la tabla
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo crear la orden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento del botón Cancelar o Cerrar (Opcional)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
