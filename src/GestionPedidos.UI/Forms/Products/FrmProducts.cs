using System;
using System.Windows.Forms;
using GestionPedidos.UI.Helpers.Products;

namespace GestionPedidos.UI.Forms.Products
{
    public partial class FrmProducts : Form
    {
        public FrmProducts()
        {
            InitializeComponent();
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
