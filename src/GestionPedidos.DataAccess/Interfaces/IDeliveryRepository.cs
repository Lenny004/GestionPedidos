using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using System.Collections.Generic;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IDeliveryRepository
    {
        /// <summary>
        /// Crea una nueva orden y retorna el ID de la orden creada
        /// </summary>
        (bool Success, int OrderId) CreateOrder(Order order);

        /// <summary>
        /// Crea un detalle de orden
        /// </summary>
        bool CreateOrderDetail(int orderId, int productId, int quantity, decimal unitPrice);

        /// <summary>
        /// Obtiene todas las órdenes
        /// </summary>
        IEnumerable<Order> ReadAllOrders();

        /// <summary>
        /// Obtiene los detalles de todas las órdenes con información relacionada
        /// </summary>
        IEnumerable<OrderDetailListDto> ReadAllDetailOrders();

        /// <summary>
        /// Obtiene una orden específica por ID
        /// </summary>
        Order ReadOneOrder(int orderId);

        /// <summary>
        /// Actualiza una orden
        /// </summary>
        bool UpdateOrder(Order order);

        /// <summary>
        /// Elimina una orden
        /// </summary>
        bool DeleteOrder(int orderId);

        /// <summary>
        /// Obtiene los detalles de una orden (productos, cantidades, precios)
        /// </summary>
        IEnumerable<(int ProductId, int Quantity)> GetOrderDetails(int orderId);

        /// <summary>
        /// Busca detalles de órdenes por cliente, producto, precio o estado
        /// </summary>
        IEnumerable<OrderDetailListDto> SearchDetailOrders(string searchTerm);

        /// <summary>
        /// Actualiza el estado de una orden
        /// </summary>
        bool UpdateOrderStatus(int orderId, string newStatus);
    }
}
