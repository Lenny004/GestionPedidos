using GestionPedidos.Controllers;
using GestionPedidos.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using GestionPedidos.UI.Helpers;
using GestionPedidos.UI.Helpers.Users;

namespace GestionPedidos.UI.Forms.Users
{
    public partial class FrmUsers : Form
    {
        private readonly UsersController _usersController = new UsersController();
        private static int selectedUserId = 0;

        public FrmUsers()
        {
            InitializeComponent();
            this.Load += FrmUsers_Load;
        }

        private void FrmUsers_Load(object sender, EventArgs e)
        {
            LoadUsersIntoGrid();
        }

        public void LoadUsersIntoGrid()
        {
            try
            {
                var (success, message, users) = _usersController.ReadAll();

                if (!success)
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertimos a lista y asignamos
                var data = users != null ? new List<UsersListDto>(users) : new List<UsersListDto>();

                if (data.Count == 0)
                {
                    MessageBox.Show("La consulta se ejecutó correctamente pero no devolvió registros. Verifique que existan datos en la tabla Users.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DataGridUser.AutoGenerateColumns = true;
                DataGridUser.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserDetail(UsersListDto user)
        {
            // Ir a la base de datos a traer TODOS los datos
            var (success, message, fullUser) = _usersController.ReadOne(user.IdUser);

            if (success && fullUser != null)
            {
                // --- DATOS BÁSICOS ---
                lblID.Text = "#" + fullUser.IdUser.ToString();
                lblUserName.Text = fullUser.Username;
                lblFullName.Text = fullUser.FullName;

                if (lblEmail != null)
                    lblEmail.Text = $"Email: {(string.IsNullOrEmpty(fullUser.Email) ? "N/A" : fullUser.Email)}";

                if (lblRole != null)
                    lblRole.Text = $"Rol: {(fullUser.Rol != null ? fullUser.Rol.RoleName : "N/A")}";

                if (btnStatus != null)
                    btnStatus.Text = fullUser.IsActive ? "Activo" : "Inactivo";

                // Fecha de creación
                if (lblCreationDate != null)
                    lblCreationDate.Text = $"Creation Date: {fullUser.CreatedAt:g}";

                // SECCIÓN DE Fechas
                bool hasUpdated = fullUser.UpdatedAt.HasValue;
                bool hasDeleted = fullUser.DeletedAt.HasValue;

                // Fecha de Modificación
                if (lblModifyDate != null)
                {
                    lblModifyDate.Visible = hasUpdated;
                    lblModifyDate.Text = hasUpdated ? $"Modify Date: {fullUser.UpdatedAt.Value:g}" : "";
                }

                // Fecha de Eliminación
                if (lblDeletedDate != null)
                {
                    lblDeletedDate.Visible = hasDeleted;
                    lblDeletedDate.Text = hasDeleted ? $"Deleted Date: {fullUser.DeletedAt.Value:g}" : "";
                }
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar clic en header
            if (e.RowIndex < 0) return;

            // Evitar grid vacío o sin selección
            if (DataGridUser.SelectedRows.Count == 0) return;

            // Obtener el objeto enlazado a la fila 
            var selectedRow = DataGridUser.SelectedRows[0];
            var selectedUser = (UsersListDto)selectedRow.DataBoundItem;
            // Mandamos el objeto a otra función para cargar los datos
            LoadUserDetail(selectedUser);
            selectedUserId = selectedUser.IdUser;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (var addUserForm = new FrmAddU())
            {
                if (addUserForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsersIntoGrid();
                }
            }
        }

        private void btnModifyU_Click(object sender, EventArgs e)
        {
            if (selectedUserId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var modifyUserForm = new FrmModifyU(selectedUserId))
            {
                if (modifyUserForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsersIntoGrid();
                }
            }
        }

        private void btnDeleteU_Click(object sender, EventArgs e)
        {
            if (selectedUserId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro que desea eliminar este usuario?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (success, message) = _usersController.Delete(selectedUserId);
                if (success)
                {
                    MessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersIntoGrid();
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadUsersIntoGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var (success, message, users) = _usersController.SearchByName(txtSearch.Text.Trim());

                if (!success)
                {
                    MessageBox.Show(message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var data = users != null ? new List<UsersListDto>(users) : new List<UsersListDto>();

                DataGridUser.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}