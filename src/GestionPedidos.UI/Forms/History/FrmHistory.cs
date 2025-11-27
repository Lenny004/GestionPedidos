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


namespace GestionPedidos.UI.Forms.History
{
    public partial class FrmHistory : Form
    {
        private readonly HistoryController _historyController = new HistoryController();

        public FrmHistory()
        {
            InitializeComponent();
            this.Load += FrmHistory_Load;
        }

        private void FrmHistory_Load(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        private void LoadHistoryData()
        {
            try
            {
                var (success, message, historyList) = _historyController.ReadAll();

                if (!success)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var data = historyList != null ? new List<HistoryListDto>(historyList) : new List<HistoryListDto>();

                // Configuración del Grid 'CargaHistorial'
                CargaHistorial.AutoGenerateColumns = true;
                CargaHistorial.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        // Si tienes un botón de "Refrescar" o "Reload" en este form:
        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        private void CargaHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validaciones básicas para evitar clicks en cabeceras o vacío
            if (e.RowIndex < 0) return;
            if (CargaHistorial.SelectedRows.Count == 0) return;

            // Obtener el objeto de la fila seleccionada
            var selectedRow = CargaHistorial.SelectedRows[0];
            var historyItem = (HistoryListDto)selectedRow.DataBoundItem;

            // Cargar detalle arriba
            LoadHistoryDetail(historyItem.Id);
        }

        // Método para llenar los labels de arriba
        private void LoadHistoryDetail(int idHistory)
        {
            var (success, message, item) = _historyController.ReadOne(idHistory);

            if (success && item != null)
            {
                // Usamos el signo $ para mezclar texto fijo con variables {item.X}
                lblHID.Text = $"ID: {item.Id}";  
                lblUser.Text = $"User: {item.User}";
                lblTable.Text = $"Table: {item.Table}";
                lblAction.Text = $"Action: {item.Action}";
                lblRecordID.Text = $"Record ID: {item.RecordId}";
                lblDate.Text = $"Date: {item.Date:g}"; // La 'g' es para formato corto

                // Para la descripción, quizás quieras un salto de línea o solo el texto
                lblDescription.Text = $"Description: {item.Description}";
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
