using GestionPedidos.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionPedidos.UI.Helpers.Users
{
    public partial class FrmAddU : Form
    {
        private readonly UsersController _usersController = new UsersController();
        private readonly RoleController _roleController = new RoleController();

        public FrmAddU()
        {
            InitializeComponent();
            this.Load += FrmAddU_Load;
        }

        private void FrmAddU_Load(object sender, EventArgs e)
        {
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                var (success, message, roles) = _roleController.GetAllRoles();

                if (!success || roles == null)
                {
                    MessageBox.Show(message, "Error al cargar roles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Configurar ComboBox
                cmbRole.DataSource = new List<Models.Entities.Rol>(roles);
                cmbRole.DisplayMember = "RoleName";  // Muestra el nombre del rol
                cmbRole.ValueMember = "IdRole";       // Valor interno es el ID
                
                // Seleccionar "Operador" por defecto (idRole = 2)
                cmbRole.SelectedValue = 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddU_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtName.Text.Trim();
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                // Obtener el rol seleccionado del ComboBox
                if (cmbRole.SelectedValue == null)
                {
                    MessageBox.Show("Por favor, seleccione un rol.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedRoleId = Convert.ToInt32(cmbRole.SelectedValue);

                var (success, message) = _usersController.Create(
                    username,
                    password,
                    fullName,
                    email,
                    idRole: selectedRoleId
                );

                if (success)
                {
                    MessageBox.Show(message, "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado. Mensaje: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
