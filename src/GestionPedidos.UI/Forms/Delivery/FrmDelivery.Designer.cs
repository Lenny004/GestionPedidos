namespace GestionPedidos.UI.Forms.Delivery
{
    partial class FrmDelivery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemoveProduct = new Guna.UI2.WinForms.Guna2GradientButton();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dgvOrderItems = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnConfirmOrder = new Guna.UI2.WinForms.Guna2GradientButton();
            this.groupDelivery = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtSearchProduct = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnReloadProduct = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnSearchProduct = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnReload = new Guna.UI2.WinForms.Guna2ImageButton();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbProduct = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtStockQuantity = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtComment = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearch = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnAddC = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDeliveryDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCustomers = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpDelivery = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.guna2ShadowPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.groupDelivery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.groupBox1);
            this.guna2ShadowPanel1.Controls.Add(this.groupDelivery);
            this.guna2ShadowPanel1.Controls.Add(this.lblTitulo);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(12, 12);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.Radius = 8;
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(1096, 689);
            this.guna2ShadowPanel1.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoveProduct);
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.dgvOrderItems);
            this.groupBox1.Controls.Add(this.btnConfirmOrder);
            this.groupBox1.Location = new System.Drawing.Point(22, 407);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1054, 259);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Products";
            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.Animated = true;
            this.btnRemoveProduct.BorderRadius = 8;
            this.btnRemoveProduct.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRemoveProduct.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRemoveProduct.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemoveProduct.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemoveProduct.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRemoveProduct.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.btnRemoveProduct.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            this.btnRemoveProduct.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRemoveProduct.ForeColor = System.Drawing.Color.White;
            this.btnRemoveProduct.Location = new System.Drawing.Point(17, 197);
            this.btnRemoveProduct.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(180, 48);
            this.btnRemoveProduct.TabIndex = 51;
            this.btnRemoveProduct.Text = "Remove Selected";
            this.btnRemoveProduct.Click += new System.EventHandler(this.btnRemoveProduct_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblTotal.Location = new System.Drawing.Point(550, 216);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(150, 29);
            this.lblTotal.TabIndex = 53;
            this.lblTotal.Text = "Total: $0.00";
            // 
            // dgvOrderItems
            // 
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(198)))), ((int)(((byte)(209)))));
            this.dgvOrderItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvOrderItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrderItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(98)))), ((int)(((byte)(127)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrderItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dgvOrderItems.ColumnHeadersHeight = 35;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(198)))), ((int)(((byte)(209)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrderItems.DefaultCellStyle = dataGridViewCellStyle21;
            this.dgvOrderItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(198)))), ((int)(((byte)(209)))));
            this.dgvOrderItems.Location = new System.Drawing.Point(17, 33);
            this.dgvOrderItems.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvOrderItems.MultiSelect = false;
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.ReadOnly = true;
            this.dgvOrderItems.RowHeadersVisible = false;
            this.dgvOrderItems.RowHeadersWidth = 51;
            this.dgvOrderItems.RowTemplate.Height = 24;
            this.dgvOrderItems.Size = new System.Drawing.Size(1024, 152);
            this.dgvOrderItems.TabIndex = 49;
            this.dgvOrderItems.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvOrderItems.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvOrderItems.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvOrderItems.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvOrderItems.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvOrderItems.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvOrderItems.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(198)))), ((int)(((byte)(209)))));
            this.dgvOrderItems.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.dgvOrderItems.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvOrderItems.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvOrderItems.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvOrderItems.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOrderItems.ThemeStyle.HeaderStyle.Height = 35;
            this.dgvOrderItems.ThemeStyle.ReadOnly = true;
            this.dgvOrderItems.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvOrderItems.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvOrderItems.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvOrderItems.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvOrderItems.ThemeStyle.RowsStyle.Height = 24;
            this.dgvOrderItems.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvOrderItems.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnConfirmOrder
            // 
            this.btnConfirmOrder.Animated = true;
            this.btnConfirmOrder.BorderRadius = 8;
            this.btnConfirmOrder.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmOrder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirmOrder.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirmOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConfirmOrder.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnConfirmOrder.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(34)))));
            this.btnConfirmOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfirmOrder.ForeColor = System.Drawing.Color.White;
            this.btnConfirmOrder.Location = new System.Drawing.Point(861, 197);
            this.btnConfirmOrder.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirmOrder.Name = "btnConfirmOrder";
            this.btnConfirmOrder.Size = new System.Drawing.Size(180, 48);
            this.btnConfirmOrder.TabIndex = 52;
            this.btnConfirmOrder.Text = "Confirm Order";
            this.btnConfirmOrder.Click += new System.EventHandler(this.btnConfirmOrder_Click);
            // 
            // groupDelivery
            // 
            this.groupDelivery.Controls.Add(this.txtSearchProduct);
            this.groupDelivery.Controls.Add(this.btnReloadProduct);
            this.groupDelivery.Controls.Add(this.btnSearchProduct);
            this.groupDelivery.Controls.Add(this.btnReload);
            this.groupDelivery.Controls.Add(this.lblSubtotal);
            this.groupDelivery.Controls.Add(this.label5);
            this.groupDelivery.Controls.Add(this.txtSalePrice);
            this.groupDelivery.Controls.Add(this.label4);
            this.groupDelivery.Controls.Add(this.cmbProduct);
            this.groupDelivery.Controls.Add(this.txtStockQuantity);
            this.groupDelivery.Controls.Add(this.label2);
            this.groupDelivery.Controls.Add(this.label3);
            this.groupDelivery.Controls.Add(this.txtComment);
            this.groupDelivery.Controls.Add(this.btnSearch);
            this.groupDelivery.Controls.Add(this.btnAddC);
            this.groupDelivery.Controls.Add(this.txtSearch);
            this.groupDelivery.Controls.Add(this.lblDeliveryDate);
            this.groupDelivery.Controls.Add(this.label1);
            this.groupDelivery.Controls.Add(this.cmbCustomers);
            this.groupDelivery.Controls.Add(this.dtpDelivery);
            this.groupDelivery.CustomBorderThickness = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.groupDelivery.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupDelivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.groupDelivery.Location = new System.Drawing.Point(22, 69);
            this.groupDelivery.Name = "groupDelivery";
            this.groupDelivery.Size = new System.Drawing.Size(1054, 316);
            this.groupDelivery.TabIndex = 50;
            this.groupDelivery.Text = "Delivery information:";
            // 
            // txtSearchProduct
            // 
            this.txtSearchProduct.BorderRadius = 8;
            this.txtSearchProduct.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchProduct.DefaultText = "";
            this.txtSearchProduct.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchProduct.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchProduct.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchProduct.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchProduct.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchProduct.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchProduct.Location = new System.Drawing.Point(300, 144);
            this.txtSearchProduct.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearchProduct.Name = "txtSearchProduct";
            this.txtSearchProduct.PasswordChar = '\0';
            this.txtSearchProduct.PlaceholderText = "Search by name...";
            this.txtSearchProduct.SelectedText = "";
            this.txtSearchProduct.Size = new System.Drawing.Size(249, 48);
            this.txtSearchProduct.TabIndex = 67;
            // 
            // btnReloadProduct
            // 
            this.btnReloadProduct.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnReloadProduct.HoverState.ImageSize = new System.Drawing.Size(35, 35);
            this.btnReloadProduct.Image = global::GestionPedidos.UI.Properties.Resources.reload;
            this.btnReloadProduct.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnReloadProduct.ImageRotate = 0F;
            this.btnReloadProduct.ImageSize = new System.Drawing.Size(33, 33);
            this.btnReloadProduct.Location = new System.Drawing.Point(555, 199);
            this.btnReloadProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReloadProduct.Name = "btnReloadProduct";
            this.btnReloadProduct.PressedState.ImageSize = new System.Drawing.Size(33, 33);
            this.btnReloadProduct.Size = new System.Drawing.Size(51, 48);
            this.btnReloadProduct.TabIndex = 66;
            this.btnReloadProduct.Click += new System.EventHandler(this.btnReloadProduct_Click);
            // 
            // btnSearchProduct
            // 
            this.btnSearchProduct.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnSearchProduct.HoverState.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSearchProduct.Image = global::GestionPedidos.UI.Properties.Resources.search1;
            this.btnSearchProduct.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnSearchProduct.ImageRotate = 0F;
            this.btnSearchProduct.ImageSize = new System.Drawing.Size(30, 30);
            this.btnSearchProduct.Location = new System.Drawing.Point(555, 144);
            this.btnSearchProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearchProduct.Name = "btnSearchProduct";
            this.btnSearchProduct.PressedState.ImageSize = new System.Drawing.Size(30, 30);
            this.btnSearchProduct.Size = new System.Drawing.Size(51, 48);
            this.btnSearchProduct.TabIndex = 65;
            this.btnSearchProduct.Click += new System.EventHandler(this.btnSearchProduct_Click);
            // 
            // btnReload
            // 
            this.btnReload.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnReload.HoverState.ImageSize = new System.Drawing.Size(35, 35);
            this.btnReload.Image = global::GestionPedidos.UI.Properties.Resources.reload;
            this.btnReload.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnReload.ImageRotate = 0F;
            this.btnReload.ImageSize = new System.Drawing.Size(33, 33);
            this.btnReload.Location = new System.Drawing.Point(242, 198);
            this.btnReload.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReload.Name = "btnReload";
            this.btnReload.PressedState.ImageSize = new System.Drawing.Size(33, 33);
            this.btnReload.Size = new System.Drawing.Size(51, 48);
            this.btnReload.TabIndex = 64;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            this.lblSubtotal.Location = new System.Drawing.Point(550, 267);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(178, 29);
            this.lblSubtotal.TabIndex = 63;
            this.lblSubtotal.Text = "Total: $500.99";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.label5.Location = new System.Drawing.Point(877, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 18);
            this.label5.TabIndex = 62;
            this.label5.Text = "Unit Price:";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.AllowDrop = true;
            this.txtSalePrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.txtSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSalePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalePrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.txtSalePrice.Location = new System.Drawing.Point(880, 151);
            this.txtSalePrice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSalePrice.Mask = "000.00";
            this.txtSalePrice.MaximumSize = new System.Drawing.Size(108, 43);
            this.txtSalePrice.MinimumSize = new System.Drawing.Size(108, 43);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.ReadOnly = true;
            this.txtSalePrice.Size = new System.Drawing.Size(108, 43);
            this.txtSalePrice.TabIndex = 61;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.label4.Location = new System.Drawing.Point(297, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 18);
            this.label4.TabIndex = 60;
            this.label4.Text = "Product:";
            // 
            // cmbProduct
            // 
            this.cmbProduct.BackColor = System.Drawing.Color.Transparent;
            this.cmbProduct.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cmbProduct.BorderRadius = 8;
            this.cmbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.cmbProduct.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProduct.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProduct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbProduct.ItemHeight = 40;
            this.cmbProduct.Items.AddRange(new object[] {
            "Activo",
            "Inactivo"});
            this.cmbProduct.Location = new System.Drawing.Point(300, 199);
            this.cmbProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProduct.MaximumSize = new System.Drawing.Size(249, 0);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(249, 46);
            this.cmbProduct.TabIndex = 59;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // txtStockQuantity
            // 
            this.txtStockQuantity.BackColor = System.Drawing.Color.Transparent;
            this.txtStockQuantity.BorderRadius = 8;
            this.txtStockQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStockQuantity.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.txtStockQuantity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtStockQuantity.Location = new System.Drawing.Point(620, 144);
            this.txtStockQuantity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(221, 48);
            this.txtStockQuantity.TabIndex = 57;
            this.txtStockQuantity.UpDownButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            this.txtStockQuantity.ValueChanged += new System.EventHandler(this.txtStockQuantity_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.label2.Location = new System.Drawing.Point(617, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 18);
            this.label2.TabIndex = 58;
            this.label2.Text = "Quantity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.label3.Location = new System.Drawing.Point(297, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 18);
            this.label3.TabIndex = 56;
            this.label3.Text = "Comments:";
            // 
            // txtComment
            // 
            this.txtComment.Animated = true;
            this.txtComment.BorderRadius = 8;
            this.txtComment.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtComment.DefaultText = "";
            this.txtComment.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtComment.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtComment.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtComment.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtComment.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.txtComment.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtComment.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtComment.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtComment.Location = new System.Drawing.Point(300, 72);
            this.txtComment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.PasswordChar = '\0';
            this.txtComment.PlaceholderText = "";
            this.txtComment.SelectedText = "";
            this.txtComment.Size = new System.Drawing.Size(741, 34);
            this.txtComment.TabIndex = 55;
            // 
            // btnSearch
            // 
            this.btnSearch.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnSearch.HoverState.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSearch.Image = global::GestionPedidos.UI.Properties.Resources.search1;
            this.btnSearch.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnSearch.ImageRotate = 0F;
            this.btnSearch.ImageSize = new System.Drawing.Size(30, 30);
            this.btnSearch.Location = new System.Drawing.Point(242, 146);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PressedState.ImageSize = new System.Drawing.Size(30, 30);
            this.btnSearch.Size = new System.Drawing.Size(51, 48);
            this.btnSearch.TabIndex = 54;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAddC
            // 
            this.btnAddC.Animated = true;
            this.btnAddC.BorderRadius = 8;
            this.btnAddC.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddC.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddC.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddC.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddC.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddC.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.btnAddC.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            this.btnAddC.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddC.ForeColor = System.Drawing.Color.White;
            this.btnAddC.Location = new System.Drawing.Point(913, 258);
            this.btnAddC.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddC.Name = "btnAddC";
            this.btnAddC.Size = new System.Drawing.Size(128, 48);
            this.btnAddC.TabIndex = 6;
            this.btnAddC.Text = "Add Product";
            this.btnAddC.Click += new System.EventHandler(this.btnAddC_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BorderRadius = 8;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Location = new System.Drawing.Point(17, 144);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderText = "Search by name, lastname...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(219, 48);
            this.txtSearch.TabIndex = 53;
            // 
            // lblDeliveryDate
            // 
            this.lblDeliveryDate.AutoSize = true;
            this.lblDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.lblDeliveryDate.Location = new System.Drawing.Point(14, 51);
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            this.lblDeliveryDate.Size = new System.Drawing.Size(96, 18);
            this.lblDeliveryDate.TabIndex = 52;
            this.lblDeliveryDate.Text = "Delivery date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.label1.Location = new System.Drawing.Point(14, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 51;
            this.label1.Text = "Customer:";
            // 
            // cmbCustomers
            // 
            this.cmbCustomers.BackColor = System.Drawing.Color.Transparent;
            this.cmbCustomers.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cmbCustomers.BorderRadius = 8;
            this.cmbCustomers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCustomers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomers.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.cmbCustomers.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCustomers.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCustomers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCustomers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCustomers.ItemHeight = 40;
            this.cmbCustomers.Items.AddRange(new object[] {
            "Activo",
            "Inactivo"});
            this.cmbCustomers.Location = new System.Drawing.Point(17, 198);
            this.cmbCustomers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCustomers.Name = "cmbCustomers";
            this.cmbCustomers.Size = new System.Drawing.Size(219, 46);
            this.cmbCustomers.TabIndex = 50;
            // 
            // dtpDelivery
            // 
            this.dtpDelivery.BackColor = System.Drawing.Color.Turquoise;
            this.dtpDelivery.BorderRadius = 8;
            this.dtpDelivery.Checked = true;
            this.dtpDelivery.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(12)))), ((int)(((byte)(69)))));
            this.dtpDelivery.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDelivery.ForeColor = System.Drawing.Color.White;
            this.dtpDelivery.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpDelivery.Location = new System.Drawing.Point(17, 72);
            this.dtpDelivery.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDelivery.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDelivery.Name = "dtpDelivery";
            this.dtpDelivery.Size = new System.Drawing.Size(277, 34);
            this.dtpDelivery.TabIndex = 0;
            this.dtpDelivery.Value = new System.DateTime(2025, 11, 25, 18, 17, 39, 399);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(1)))), ((int)(((byte)(70)))));
            this.lblTitulo.Location = new System.Drawing.Point(16, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(130, 36);
            this.lblTitulo.TabIndex = 30;
            this.lblTitulo.Text = "Delivery";
            // 
            // FrmDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 713);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDelivery";
            this.Text = "FrmDelivery";
            this.Load += new System.EventHandler(this.FrmDelivery_Load);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2ShadowPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).EndInit();
            this.groupDelivery.ResumeLayout(false);
            this.groupDelivery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStockQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvOrderItems;
        private Guna.UI2.WinForms.Guna2GradientButton btnRemoveProduct;
        private Guna.UI2.WinForms.Guna2GradientButton btnConfirmOrder;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTitulo;
        private Guna.UI2.WinForms.Guna2GradientButton btnAddC;
        private Guna.UI2.WinForms.Guna2GroupBox groupDelivery;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpDelivery;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCustomers;
        private System.Windows.Forms.Label lblDeliveryDate;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ImageButton btnSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtComment;
        private Guna.UI2.WinForms.Guna2NumericUpDown txtStockQuantity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProduct;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox txtSalePrice;
        private System.Windows.Forms.Label lblSubtotal;
        private Guna.UI2.WinForms.Guna2ImageButton btnReload;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchProduct;
        private Guna.UI2.WinForms.Guna2ImageButton btnReloadProduct;
        private Guna.UI2.WinForms.Guna2ImageButton btnSearchProduct;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}