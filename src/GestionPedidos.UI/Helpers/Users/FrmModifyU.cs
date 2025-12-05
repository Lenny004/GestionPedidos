using GestionPedidos.Controllers;
using GestionPedidos.Common.Services;
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
    public partial class FrmModifyU : Form
    {
        private readonly UsersController _usersController = new UsersController();
        private readonly RoleController _roleController = new RoleController();
        public FrmModifyU(int id)
        {
            InitializeComponent();
            LoadRoles();
            LoadStatus();
            LoadUser(id);
        }
        private void LoadStatus()
        {
            try
            {
                // Llamar al controlador en lugar de acceder directamente al enum
                cmbStatus.DataSource = _usersController.GetUserStatuses();
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se obtuvieron los estados del producto. {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUser(int idUser)
        {
            var (success, message, fullUser) = _usersController.ReadOne(idUser);
            if (success && fullUser != null)
            {
                lblID.Text = "#" + fullUser.IdUser.ToString();
                txtName.Text = fullUser.FullName;
                txtEmail.Text = fullUser.Email;
                
                // Seleccionar el rol correcto en el ComboBox usando el IdRole
                cmbRole.SelectedValue = fullUser.IdRole;
                
                // Seleccionar el estado correcto: 1 = Activo, 0 = Inactivo
                cmbStatus.SelectedValue = (byte)(fullUser.IsActive ? 1 : 0);
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModifyC_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que los campos requeridos estén completos
                if (cmbRole.SelectedValue == null)
                {
                    MessageBox.Show("Por favor, seleccione un rol.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbStatus.SelectedValue == null)
                {
                    MessageBox.Show("Por favor, seleccione un estado.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int userId = int.Parse(lblID.Text.TrimStart('#'));
                string fullName = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                int roleId = Convert.ToInt32(cmbRole.SelectedValue);
                byte statusByte = Convert.ToByte(cmbStatus.SelectedValue);
                bool isActive = statusByte == 1; // 1 = Activo, 0 = Inactivo

                var (success, message) = _usersController.Update(
                    userId,
                    fullName,
                    email,
                    roleId,
                    isActive
                );

                if (success)
                {
                    MessageBox.Show(message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
