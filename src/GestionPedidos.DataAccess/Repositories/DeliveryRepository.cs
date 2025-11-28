using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.Models.DTO;
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
        /// Obtiene los detalles de todas las órdenes con información relacionada
        /// </summary>
        public IEnumerable<OrderDetailListDto> ReadAllDetailOrders()
        {
            var orderDetails = new List<OrderDetailListDto>();
            string query = @"
                SELECT
                    o.idOrder,
                    od.idDetail,
                    CONCAT(c.firstName, ' ', c.lastName) AS customerName,
                    p.productName,
                    od.quantity,
                    od.unitPrice,
                    od.subtotal,
                    o.orderDate,
                    o.deliveryDate,
                    o.orderStatus,
                    o.total AS orderTotal,
                    uc.fullName AS createdByUser,
                    o.updatedAt
                FROM OrderDetails od
                JOIN Products p ON p.idProduct = od.idProduct
                JOIN Orders o ON o.idOrder = od.idOrder
                JOIN Customers c ON c.idCustomer = o.idCustomer
                JOIN Users uc ON uc.idUser = o.userCreation
                WHERE o.isActive = 1
                    AND od.deletedAt IS NULL
                ORDER BY o.idOrder DESC, od.idDetail;";

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
                            orderDetails.Add(new OrderDetailListDto
                            {
                                IdOrder = Convert.ToInt32(reader["idOrder"]),
                                IdDetail = Convert.ToInt32(reader["idDetail"]),
                                CustomerName = reader["customerName"] != DBNull.Value ? reader["customerName"].ToString() : "",
                                ProductName = reader["productName"] != DBNull.Value ? reader["productName"].ToString() : "",
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                UnitPrice = Convert.ToDecimal(reader["unitPrice"]),
                                Subtotal = Convert.ToDecimal(reader["subtotal"]),
                                OrderDate = reader["orderDate"] != DBNull.Value ? Convert.ToDateTime(reader["orderDate"]) : DateTime.Now,
                                DeliveryDate = reader["deliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["deliveryDate"]) : DateTime.Now,
                                OrderStatus = reader["orderStatus"] != DBNull.Value ? reader["orderStatus"].ToString() : "",
                                OrderTotal = Convert.ToDecimal(reader["orderTotal"]),
                                CreatedByUser = reader["createdByUser"] != DBNull.Value ? reader["createdByUser"].ToString() : "",
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener detalles de órdenes: {ex.Message}", ex);
            }

            return orderDetails;
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
                ORDER BY idOrder DESC;";

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
                throw new Exception($"Error al obtener las órdenes: {ex.Message}", ex);
            }

            return orders;
        }

        /// <summary>
        /// Busca detalles de órdenes por cliente, producto, precio o estado
        /// </summary>
        public IEnumerable<OrderDetailListDto> SearchDetailOrders(string searchTerm)
        {
            var orderDetails = new List<OrderDetailListDto>();
            string query = @"
                SELECT
                    o.idOrder,
                    od.idDetail,
                    CONCAT(c.firstName, ' ', c.lastName) AS customerName,
                    p.productName,
                    od.quantity,
                    od.unitPrice,
                    od.subtotal,
                    o.orderDate,
                    o.deliveryDate,
                    o.orderStatus,
                    o.total AS orderTotal,
                    uc.fullName AS createdByUser,
                    o.updatedAt
                FROM OrderDetails od
                JOIN Products p ON p.idProduct = od.idProduct
                JOIN Orders o ON o.idOrder = od.idOrder
                JOIN Customers c ON c.idCustomer = o.idCustomer
                JOIN Users uc ON uc.idUser = o.userCreation
                WHERE o.isActive = 1
                    AND od.deletedAt IS NULL
                    AND (CONCAT(c.firstName, ' ', c.lastName) LIKE @SearchTerm
                         OR p.productName LIKE @SearchTerm
                         OR CAST(od.unitPrice AS VARCHAR) LIKE @SearchTerm
                         OR o.orderStatus LIKE @SearchTerm
                         OR CAST(o.total AS VARCHAR) LIKE @SearchTerm)
                ORDER BY o.idOrder DESC, od.idDetail;";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.Add(new OrderDetailListDto
                                {
                                    IdOrder = Convert.ToInt32(reader["idOrder"]),
                                    IdDetail = Convert.ToInt32(reader["idDetail"]),
                                    CustomerName = reader["customerName"] != DBNull.Value ? reader["customerName"].ToString() : "",
                                    ProductName = reader["productName"] != DBNull.Value ? reader["productName"].ToString() : "",
                                    Quantity = Convert.ToInt32(reader["quantity"]),
                                    UnitPrice = Convert.ToDecimal(reader["unitPrice"]),
                                    Subtotal = Convert.ToDecimal(reader["subtotal"]),
                                    OrderDate = reader["orderDate"] != DBNull.Value ? Convert.ToDateTime(reader["orderDate"]) : DateTime.Now,
                                    DeliveryDate = reader["deliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["deliveryDate"]) : DateTime.Now,
                                    OrderStatus = reader["orderStatus"] != DBNull.Value ? reader["orderStatus"].ToString() : "",
                                    OrderTotal = Convert.ToDecimal(reader["orderTotal"]),
                                    CreatedByUser = reader["createdByUser"] != DBNull.Value ? reader["createdByUser"].ToString() : "",
                                    UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar detalles de órdenes: {ex.Message}", ex);
            }

            return orderDetails;
        }

        /// <summary>
        /// Actualiza el estado de una orden
        /// </summary>
        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            string query = @"
                UPDATE Orders
                SET orderStatus = @OrderStatus, 
                    updatedAt = GETDATE()
                WHERE idOrder = @IdOrder";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderStatus", newStatus);
                        cmd.Parameters.AddWithValue("@IdOrder", orderId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el estado de la orden: {ex.Message}", ex);
            }
        }
    }
}

