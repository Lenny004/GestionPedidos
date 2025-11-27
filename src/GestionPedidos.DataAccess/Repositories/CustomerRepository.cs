using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.Entities;
using GestionPedidos.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        // 1. READ ALL
        public IEnumerable<CustomerListDto> ReadAllCustomers()
        {
            var customers = new List<CustomerListDto>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT c.idCustomer, c.firstName + ' ' + c.lastName AS FullName,
                               c.phone, c.address, c.isActive,
                               ci.cityName AS City, d.departmentName AS Department,
                               uCreated.userName AS CreatedBy
                        FROM Customers c
                        LEFT JOIN Cities ci ON c.idCity = ci.idCity
                        LEFT JOIN Departments d ON ci.idDepartment = d.idDepartment
                        LEFT JOIN Users uCreated ON c.userCreation = uCreated.idUser";

                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerListDto
                            {
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                FullName = reader["FullName"].ToString(),
                                Phone = reader["phone"]?.ToString() ?? "",
                                Address = reader["address"]?.ToString() ?? "",
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                                City = reader["City"]?.ToString() ?? "N/A",
                                Department = reader["Department"]?.ToString() ?? "N/A",
                                CreatedBy = reader["CreatedBy"]?.ToString() ?? "System"
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error al leer clientes", ex); }
            return customers;
        }

        // 2. READ ONE (Detalle)
        public Customer ReadOne(int idCustomer)
        {
            Customer customer = null;
            string query = @"
                SELECT c.idCustomer, c.firstName, c.lastName, c.phone, c.address, c.isActive, c.idCity,
                        c.createdAt, c.updatedAt, c.deletedAt,
                        ci.cityName, d.departmentName,
                        uCreat.userName AS CreatedBy, uMod.userName AS ModifiedBy
                FROM Customers c
                LEFT JOIN Cities ci ON c.idCity = ci.idCity
                LEFT JOIN Departments d ON ci.idDepartment = d.idDepartment
                LEFT JOIN Users uCreat ON c.userCreation = uCreat.idUser
                LEFT JOIN Users uMod ON c.userModification = uMod.idUser
                WHERE c.idCustomer = @IdCustomer";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCustomer", idCustomer);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                Phone = reader["phone"]?.ToString(),
                                Address = reader["address"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                                City = reader["cityName"]?.ToString(),
                                Department = reader["departmentName"]?.ToString(),
                                UserCreation = reader["CreatedBy"]?.ToString() ?? "Sistema",
                                UserModification = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : null,
                                CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                DeletedAt = reader["deletedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["deletedAt"]) : null,
                                IdCity = reader["idCity"] != DBNull.Value ? Convert.ToInt32(reader["idCity"]) : 0
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error al leer detalle del cliente.", ex); }
            return customer;
        }

        // 3. GET CITIES
        public List<dynamic> GetCitiesForCombo()
        {
            var list = new List<dynamic>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT idCity, cityName FROM Cities WHERE isActive = 1";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new { Value = Convert.ToInt32(reader["idCity"]), Text = reader["cityName"].ToString() });
                        }
                    }
                }
            }
            catch (Exception) { }
            return list;
        }

        // 4. CREATE
        public bool Create(Customer customer, int userId)
        {
            string query = @"INSERT INTO Customers (firstName, lastName, phone, address, idCity, userCreation, isActive) 
                            VALUES (@FirstName, @LastName, @Phone, @Address, @IdCity, @UserCreation, 1)";
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object)customer.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdCity", customer.IdCity);
                    cmd.Parameters.AddWithValue("@UserCreation", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { throw new Exception("Error al crear cliente.", ex); }
        }

        // 5. UPDATE
        public bool Update(Customer customer, int userId)
        {
            string query = @"UPDATE Customers SET 
                            firstName = @FirstName, lastName = @LastName, phone = @Phone, 
                            address = @Address, idCity = @IdCity, isActive = @IsActive,
                            userModification = @UserMod, updatedAt = GETDATE()
                            WHERE idCustomer = @IdCustomer";
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@IdCustomer", customer.IdCustomer);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object)customer.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdCity", customer.IdCity);
                    cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                    cmd.Parameters.AddWithValue("@UserMod", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { throw new Exception("Error al actualizar cliente.", ex); }
        }

        // 6. DELETE
        public bool Delete(int id, int userId)
        {
            string query = "UPDATE Customers SET isActive = 0, updatedAt = GETDATE(), deletedAt = GETDATE(), userModification = @UserMod WHERE idCustomer = @Id";
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@UserMod", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { throw new Exception("Error al eliminar cliente.", ex); }
        }

        // BUSCAR CLIENTES
        public IEnumerable<CustomerListDto> SearchCustomers(string value)
        {
            var customers = new List<CustomerListDto>();

            // Consulta SQL Corregida (El JOIN de Departamento ahora apunta a CIUDAD)
            string query = @"
                SELECT 
                    c.idCustomer, 
                    c.firstName + ' ' + c.lastName AS FullName,
                    c.phone, 
                    c.address, 
                    c.isActive,
                    ci.cityName AS City, 
                    d.departmentName AS Department,
                    uCreated.userName AS CreatedBy
                FROM Customers c
                LEFT JOIN Cities ci ON c.idCity = ci.idCity
                LEFT JOIN Departments d ON ci.idDepartment = d.idDepartment -- <--- CORRECCIÓN CLAVE
                LEFT JOIN Users uCreated ON c.userCreation = uCreated.idUser
                WHERE (c.firstName + ' ' + c.lastName LIKE @Val 
                       OR c.firstName LIKE @Val 
                       OR c.lastName LIKE @Val 
                       OR ci.cityName LIKE @Val 
                       OR d.departmentName LIKE @Val)";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    // Agregamos los comodines % para que busque coincidencias parciales
                    // Ejemplo: Si escribes "Jon", buscará "%Jon%"
                    cmd.Parameters.AddWithValue("@Val", "%" + value + "%");

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerListDto
                            {
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                FullName = reader["FullName"].ToString(),
                                Phone = reader["phone"] != DBNull.Value ? reader["phone"].ToString() : "",
                                Address = reader["address"] != DBNull.Value ? reader["address"].ToString() : "",
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                                City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "N/A",
                                Department = reader["Department"] != DBNull.Value ? reader["Department"].ToString() : "N/A",
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : "System"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar clientes", ex);
            }
            return customers;
        }
    }
}