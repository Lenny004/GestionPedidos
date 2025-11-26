using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<CustomerListDto> ReadAllCustomers()
        {
            var customers = new List<CustomerListDto>();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Usamos alias (c, ci, d, u) y LEFT JOIN como en el ejemplo
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
                        LEFT JOIN Departments d ON c.idDepartment = d.idDepartment
                        LEFT JOIN Users uCreated ON c.userCreation = uCreated.idUser";

                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new CustomerListDto
                            {
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                FullName = reader["FullName"].ToString(),
                                Phone = reader["phone"] != DBNull.Value ? reader["phone"].ToString() : "",
                                Address = reader["address"] != DBNull.Value ? reader["address"].ToString() : "",
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                                City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "N/A",
                                Department = reader["Department"] != DBNull.Value ? reader["Department"].ToString() : "N/A",
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : "System"
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes lanzar la excepción o retornarla vacía según la lógica de tu grupo
                throw new Exception("Error al leer registros de clientes", ex);
            }
            return customers;
        }
    }
}