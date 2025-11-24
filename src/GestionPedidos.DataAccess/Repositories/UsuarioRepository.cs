using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.Entities;
using System;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class UsuarioRepository : IUserRepository
    {
        /// <summary>
        /// Autentica al usuario en la base de datos.
        /// </summary>
        public User Autenticar(string userName, string passwordHash)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT u.idUser, u.userName, u.fullName, u.email, 
                                u.isActive, u.idRole, r.roleName
                        FROM Users u
                        INNER JOIN Roles r ON r.idRole = u.idRole 
                        WHERE u.userName = @Username 
                        AND u.passwordHash = @PasswordHash
                        AND u.isActive = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", userName);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    IdUser = Convert.ToInt32(reader["idUser"]),
                                    Username = reader["userName"].ToString(),
                                    FullName = reader["fullName"].ToString(),
                                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                                    IsActive = Convert.ToBoolean(reader["isActive"]),
                                    IdRole = Convert.ToInt32(reader["idRole"]),
                                    Rol = new Rol
                                    {
                                        IdRole = Convert.ToInt32(reader["idRole"]),
                                        RoleName = reader["roleName"].ToString()
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al autenticar usuario: {ex.Message}", ex);
            }
            return null;
        }

        /// <summary>
        /// Verifica si un nombre de usuario ya existe en la base de datos.
        /// </summary>
        public bool ExisteUsuario(string userName, string email)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // Usamos OR para verificar si el nombre de usuario O el correo ya existe.
                    string query = "SELECT COUNT(*) FROM Users WHERE userName = @userName OR email = @email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@email", email); // Agregamos el parámetro Email

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia del usuario/email: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos.
        /// </summary>
        public bool CrearUsuario(User user)

        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO Users 
                            (userName, passwordHash, fullName, email, isActive, idRole, createdAt)
                        VALUES 
                            (@userName, @passwordHash, @fullName, @email, @isActive, @idRole, @createdAt)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", user.Username);
                        cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@fullName", user.FullName);
                        cmd.Parameters.AddWithValue("@email", (object)user.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@isActive", user.IsActive);
                        int roleId = user.IdRole != 0
                            ? user.IdRole
                            : user.Rol != null ? user.Rol.IdRole : 0;

                        if (roleId == 0)
                        {
                            throw new ArgumentException("El usuario debe tener un rol asignado antes de crear el registro.", nameof(user));
                        }

                        cmd.Parameters.AddWithValue("@idRole", roleId);
                        cmd.Parameters.AddWithValue("@createdAt", user.CreatedAt);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear usuario: {ex.Message}", ex);
            }
        }

        public bool VerifyPassword(string email, string passwordHash)
        {
            string query = "SELECT passwordHash FROM Users WHERE email = @Email AND isActive = 1";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string currentHash = Convert.ToString(result);
                        return string.Equals(currentHash, passwordHash, StringComparison.OrdinalIgnoreCase);
                    }

                    return false;
                }
                catch (Exception)
                {
                    // Manejo de errores de base de datos
                    return false;
                }
            }
        }

        public bool UpdatePasswordByEmail(string email, string newPasswordHash)
        {
            // Esta consulta actualiza el hash en la tabla Users
            string query = @"
                UPDATE Users
                SET passwordHash = @NewHash,
                    updatedAt = GETDATE()
                WHERE email = @Email AND isActive = 1;
                SELECT @@ROWCOUNT; 
            ";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@NewHash", newPasswordHash);
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result) == 1; // 1 fila afectada = Éxito
                    }
                    return false;
                }
                catch (Exception)
                {
                    // Manejo de errores de base de datos
                    return false;
                }
            }
        }
    }
}
