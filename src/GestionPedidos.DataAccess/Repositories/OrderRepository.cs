using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces; // Asegúrate de tener este namespace
using GestionPedidos.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    // 1. Cambiado a 'public'
    // 2. Implementa la interfaz 'IOrderRepository'
    public class OrderRepository : IOrderRepository
    {
        public List<OrderDeliveryDTO> GetOrdersForDelivery()
        {
            var orders = new List<OrderDeliveryDTO>();

            // Consulta SQL para obtener pedidos en estado de despacho
            // 

           
            string query = @"
                SELECT 
                    O.idOrder, O.orderDate, O.orderStatus, O.total,
                    C.idCustomer, C.firstName + ' ' + C.lastName AS CustomerFullName, C.phone, 
                    C.address AS DeliveryAddress, CI.cityName, D.departmentName
                FROM Orders O
                INNER JOIN Customers C ON C.idCustomer = O.idCustomer
                INNER JOIN Cities CI ON CI.idCity = C.idCity 
                INNER JOIN Departments D ON D.idDepartment = CI.idDepartment
                WHERE O.orderStatus IN ('Pending', 'InProcess') AND O.isActive = 1";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new OrderDeliveryDTO
                            {
                                IdOrder = Convert.ToInt32(reader["idOrder"]),
                                OrderDate = Convert.ToDateTime(reader["orderDate"]),
                                OrderStatus = reader["orderStatus"].ToString(),
                                Total = Convert.ToDecimal(reader["total"]),
                                CustomerFullName = reader["CustomerFullName"].ToString(),
                                CustomerPhone = reader["phone"].ToString(),
                                DeliveryAddress = reader["DeliveryAddress"].ToString(),
                                DeliveryCity = reader["cityName"].ToString(),
                                DeliveryDepartment = reader["departmentName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error y logging
                throw new Exception("Error al obtener pedidos para despacho.", ex);
            }
            return orders;
        }

        public bool CreateOrderHeader(int idCustomer, int idUserCreation, DateTime deliveryDate, string comments)
        {
            string query = @"
                INSERT INTO Orders (idCustomer, orderDate, deliveryDate, comments, orderStatus, total, userCreation, isActive)
                VALUES (@idCustomer, GETDATE(), @deliveryDate, @comments, 'Pending', 0, @userCreation, 1)";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@idCustomer", idCustomer);
                    cmd.Parameters.AddWithValue("@userCreation", idUserCreation);
                    cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);

                    // Validación para que no falle si el comentario está vacío
                    if (string.IsNullOrEmpty(comments))
                        cmd.Parameters.AddWithValue("@comments", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@comments", comments);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la cabecera de la orden.", ex);
            }
        }

    }
}