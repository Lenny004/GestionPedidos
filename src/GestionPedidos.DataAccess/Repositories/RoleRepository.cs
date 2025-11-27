using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// Obtiene todos los roles activos del sistema
        /// </summary>
        public IEnumerable<Rol> ReadAllRoles()
        {
            var roles = new List<Rol>();

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            idRole,
                            roleName,
                            description,
                            isActive,
                            createdAt,
                            updatedAt,
                            deletedAt
                        FROM Roles
                        WHERE isActive = 1
                        ORDER BY idRole";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rol = new Rol
                                {
                                    IdRole = Convert.ToInt32(reader["idRole"]),
                                    RoleName = reader["roleName"].ToString(),
                                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : string.Empty,
                                    IsActive = Convert.ToBoolean(reader["isActive"]),
                                    CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                    UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                    DeletedAt = reader["deletedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["deletedAt"]) : null
                                };
                                roles.Add(rol);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer roles: {ex.Message}", ex);
            }

            return roles;
        }
    }
}
