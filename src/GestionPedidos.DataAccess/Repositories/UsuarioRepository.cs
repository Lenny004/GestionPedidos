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
        public bool ExisteUsuario(string userName)
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Users WHERE userName = @userName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia del usuario: {ex.Message}", ex);
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
                        cmd.Parameters.AddWithValue("@idRole", user.Rol.IdRole);
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
    }
}
