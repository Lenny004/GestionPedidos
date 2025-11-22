using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class CustomerRepository // <--- ¡Debe ser public!
    {
        public List<Customer> GetCustomersForCombo()
        {
            var list = new List<Customer>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Seleccionamos firstName y lastName por separado para mapear correctamente
                    string query = "SELECT idCustomer, firstName, lastName FROM Customers WHERE isActive = 1";

                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer
                            {
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener lista de clientes", ex);
            }
            return list;
        }
    }
}