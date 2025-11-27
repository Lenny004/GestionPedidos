using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GestionPedidos.DataAccess.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        /// <summary>
        /// Crea una nueva orden y retorna el ID de la orden creada
        /// </summary>
        public (bool Success, int OrderId) CreateOrder(Order order)
        {
            string query = @"
                INSERT INTO Orders (idCustomer, orderDate, deliveryDate, comments, orderStatus, total, userCreation, isActive, createdAt)
                VALUES (@IdCustomer, @OrderDate, @DeliveryDate, @Comments, @OrderStatus, @Total, @UserCreation, 1, GETDATE());
                SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdCustomer", order.IdCustomer);
                        cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DeliveryDate", order.DeliveryDate);
                        cmd.Parameters.AddWithValue("@Comments", (object)order.Comments ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                        cmd.Parameters.AddWithValue("@Total", order.Total);
                        cmd.Parameters.AddWithValue("@UserCreation", order.UserCreation);

                        var result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int orderId))
                        {
                            return (true, orderId);
                        }

                        return (false, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la orden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un detalle de orden
        /// </summary>
        public bool CreateOrderDetail(int orderId, int productId, int quantity, decimal unitPrice)
        {
            string query = @"
                INSERT INTO OrderDetails (idOrder, idProduct, quantity, unitPrice, subtotal, createdAt)
                VALUES (@IdOrder, @IdProduct, @Quantity, @UnitPrice, @Subtotal, GETDATE())";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        decimal subtotal = quantity * unitPrice;

                        cmd.Parameters.AddWithValue("@IdOrder", orderId);
                        cmd.Parameters.AddWithValue("@IdProduct", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        cmd.Parameters.AddWithValue("@Subtotal", subtotal);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el detalle de la orden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las órdenes
        /// </summary>
        public IEnumerable<Order> ReadAllOrders()
        {
            var orders = new List<Order>();
            string query = @"
                SELECT idOrder, idCustomer, orderDate, deliveryDate, comments, orderStatus, total, 
                       userCreation, isActive, createdAt, updatedAt, deletedAt
                FROM Orders
                WHERE isActive = 1
                ORDER BY createdAt DESC";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                IdOrder = Convert.ToInt32(reader["idOrder"]),
                                IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                OrderDate = reader["orderDate"] != DBNull.Value ? Convert.ToDateTime(reader["orderDate"]) : DateTime.Now,
                                DeliveryDate = reader["deliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["deliveryDate"]) : DateTime.Now,
                                Comments = reader["comments"] != DBNull.Value ? reader["comments"].ToString() : "",
                                OrderStatus = reader["orderStatus"].ToString(),
                                Total = Convert.ToDecimal(reader["total"]),
                                UserCreation = Convert.ToInt32(reader["userCreation"]),
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                                CreatedAt = reader["createdAt"] != DBNull.Value ? Convert.ToDateTime(reader["createdAt"]) : DateTime.Now,
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                DeletedAt = reader["deletedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["deletedAt"]) : null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener todas las órdenes: {ex.Message}", ex);
            }

            return orders;
        }

        /// <summary>
        /// Obtiene una orden específica por ID
        /// </summary>
        public Order ReadOneOrder(int orderId)
        {
            string query = @"
                SELECT idOrder, idCustomer, orderDate, deliveryDate, comments, orderStatus, total, 
                       userCreation, isActive, createdAt, updatedAt, deletedAt
                FROM Orders
                WHERE idOrder = @IdOrder";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", orderId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Order
                                {
                                    IdOrder = Convert.ToInt32(reader["idOrder"]),
                                    IdCustomer = Convert.ToInt32(reader["idCustomer"]),
                                    OrderDate = reader["orderDate"] != DBNull.Value ? Convert.ToDateTime(reader["orderDate"]) : DateTime.Now,
                                    DeliveryDate = reader["deliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["deliveryDate"]) : DateTime.Now,
                                    Comments = reader["comments"] != DBNull.Value ? reader["comments"].ToString() : "",
                                    OrderStatus = reader["orderStatus"].ToString(),
                                    Total = Convert.ToDecimal(reader["total"]),
                                    UserCreation = Convert.ToInt32(reader["userCreation"]),
                                    IsActive = Convert.ToBoolean(reader["isActive"]),
                                    CreatedAt = reader["createdAt"] != DBNull.Value ? Convert.ToDateTime(reader["createdAt"]) : DateTime.Now,
                                    UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                    DeletedAt = reader["deletedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["deletedAt"]) : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la orden: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Actualiza una orden
        /// </summary>
        public bool UpdateOrder(Order order)
        {
            string query = @"
                UPDATE Orders
                SET idCustomer = @IdCustomer, deliveryDate = @DeliveryDate, comments = @Comments, 
                    orderStatus = @OrderStatus, total = @Total, updatedAt = GETDATE()
                WHERE idOrder = @IdOrder";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", order.IdOrder);
                        cmd.Parameters.AddWithValue("@IdCustomer", order.IdCustomer);
                        cmd.Parameters.AddWithValue("@DeliveryDate", order.DeliveryDate);
                        cmd.Parameters.AddWithValue("@Comments", (object)order.Comments ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                        cmd.Parameters.AddWithValue("@Total", order.Total);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la orden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina una orden (soft delete)
        /// </summary>
        public bool DeleteOrder(int orderId)
        {
            string query = @"
                UPDATE Orders
                SET isActive = 0, deletedAt = GETDATE()
                WHERE idOrder = @IdOrder";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", orderId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la orden: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene los detalles de una orden (productos y cantidades)
        /// </summary>
        public IEnumerable<(int ProductId, int Quantity)> GetOrderDetails(int orderId)
        {
            var details = new List<(int, int)>();
            string query = @"
                SELECT idProduct, quantity
                FROM OrderDetails
                WHERE idOrder = @IdOrder";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", orderId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productId = Convert.ToInt32(reader["idProduct"]);
                                int quantity = Convert.ToInt32(reader["quantity"]);
                                details.Add((productId, quantity));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener detalles de la orden: {ex.Message}", ex);
            }

            return details;
        }
    }
}

