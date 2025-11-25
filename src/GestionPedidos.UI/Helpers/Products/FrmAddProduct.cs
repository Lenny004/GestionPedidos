using GestionPedidos.Controllers;
using GestionPedidos.Models.DTOs;
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
        public FrmAddProduct()
        {
            InitializeComponent();
            // Configurar rangos para evitar errores de validación
            txtStockQuantity.Maximum = 10000; // 10k como máximo razonable
            txtStockQuantity.Minimum = 0; // No permitir negativos
        }

        private void btnAddC_Click(object sender, EventArgs e)
        {
            
        }
    }
}
