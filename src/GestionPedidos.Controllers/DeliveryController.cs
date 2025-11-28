using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using NLog;
using System;
using System.Collections.Generic;

namespace GestionPedidos.Controllers
{
    public class DeliveryController
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IProductsRepository _productsRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DeliveryController()
        {
            _deliveryRepository = new DeliveryRepository();
            _productsRepository = new ProductsRepository();
        }

        /// <summary>
        /// Crea una nueva orden con sus detalles y reduce el stock de productos
        /// </summary>
        public (bool Success, string Message, int OrderId) CreateOrder(int customerId, DateTime deliveryDate, string comments, List<OrderDetailItem> orderItems)
        {
            try
            {
                // Validar datos básicos
                if (customerId <= 0)
                {
                    Logger.Warn("Intento de crear orden sin cliente válido");
                    return (false, "Cliente inválido", 0);
                }

                if (orderItems == null || orderItems.Count == 0)
                {
                    Logger.Warn("Intento de crear orden sin detalles");
                    return (false, "La orden debe contener al menos un producto", 0);
                }

                // Obtener ID del usuario logeado
                int currentUserId = SessionManager.UsuarioId;
                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de crear orden sin sesión de usuario activa");
                    return (false, AppConstants.SESION_EXPIRADA, 0);
                }

                // Calcular total
                decimal total = 0;
                foreach (var item in orderItems)
                {
                    total += item.Subtotal;
                }

                // Crear la orden
                var order = new Order
                {
                    IdCustomer = customerId,
                    DeliveryDate = deliveryDate,
                    Comments = comments,
                    OrderStatus = "Pending",
                    Total = total,
                    UserCreation = currentUserId
                };

                var (orderSuccess, orderId) = _deliveryRepository.CreateOrder(order);

                if (!orderSuccess || orderId <= 0)
                {
                    Logger.Error("No se pudo crear la orden en la base de datos");
                    return (false, "Error al crear la orden", 0);
                }

                // Crear los detalles de la orden y reducir stock
                foreach (var item in orderItems)
                {
                    bool detailSuccess = _deliveryRepository.CreateOrderDetail(
                        orderId,
                        item.IdProduct,
                        item.Quantity,
                        item.UnitPrice
                    );

                    if (!detailSuccess)
                    {
                        Logger.Error($"Error al crear detalle de orden para producto {item.IdProduct}");
                        return (false, $"Error al crear detalle del producto {item.ProductName}", orderId);
                    }

                    // Reducir el stock del producto
                    bool stockReduced = _productsRepository.ReduceStock(item.IdProduct, item.Quantity);
                    if (!stockReduced)
                    {
                        Logger.Error($"Error al reducir stock del producto {item.IdProduct}");
                        return (false, $"Error al actualizar stock del producto {item.ProductName}", orderId);
                    }
                }

                Logger.Info($"Orden {orderId} creada exitosamente para cliente {customerId}. Stock actualizado.");
                return (true, "Pedido creado exitosamente", orderId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al crear pedido");
                return (false, $"Error al crear pedido: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Obtiene todas las órdenes
        /// </summary>
        public (bool Success, string Message, IEnumerable<Order> Orders) GetAllOrders()
        {
            try
            {
                var orders = _deliveryRepository.ReadAllOrders();
                if (orders == null)
                {
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                return (true, "Órdenes recuperadas correctamente", orders);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener órdenes");
                return (false, $"Error al obtener órdenes: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene los detalles de todas las órdenes con información relacionada
        /// </summary>
        public (bool Success, string Message, IEnumerable<OrderDetailListDto> OrderDetails) GetAllOrderDetails()
        {
            try
            {
                var orderDetails = _deliveryRepository.ReadAllDetailOrders();
                if (orderDetails == null)
                {
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var detailList = new List<OrderDetailListDto>(orderDetails);
                if (detailList.Count == 0)
                {
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, detailList);
                }

                return (true, "Detalles de órdenes recuperados correctamente", detailList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener detalles de órdenes");
                return (false, $"Error al obtener detalles de órdenes: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene una orden específica
        /// </summary>
        public (bool Success, string Message, Order Order) GetOrderById(int orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return (false, "ID de orden inválido", null);
                }

                var order = _deliveryRepository.ReadOneOrder(orderId);
                if (order == null)
                {
                    return (false, "Orden no encontrada", null);
                }

                return (true, "Orden encontrada", order);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al obtener orden {orderId}");
                return (false, $"Error al obtener orden: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza una orden
        /// </summary>
        public (bool Success, string Message) UpdateOrder(int orderId, DateTime deliveryDate, string comments, string status)
        {
            try
            {
                var order = _deliveryRepository.ReadOneOrder(orderId);
                if (order == null)
                {
                    return (false, "Orden no encontrada");
                }

                order.DeliveryDate = deliveryDate;
                order.Comments = comments;
                order.OrderStatus = status;

                bool updated = _deliveryRepository.UpdateOrder(order);
                if (!updated)
                {
                    return (false, "No se pudo actualizar la orden");
                }

                Logger.Info($"Orden {orderId} actualizada exitosamente");
                return (true, "Orden actualizada correctamente");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al actualizar orden {orderId}");
                return (false, $"Error al actualizar orden: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancela una orden y restaura el stock de los productos
        /// </summary>
        public (bool Success, string Message) CancelOrder(int orderId)
        {
            try
            {
                var order = _deliveryRepository.ReadOneOrder(orderId);
                if (order == null)
                {
                    return (false, "Orden no encontrada");
                }

                // Si la orden ya está cancelada, no hacer nada
                if (order.OrderStatus == "Cancelled")
                {
                    return (false, "La orden ya está cancelada");
                }

                // Obtener los detalles de la orden para restaurar el stock
                var orderDetails = _deliveryRepository.GetOrderDetails(orderId);

                // Restaurar el stock de cada producto
                foreach (var (productId, quantity) in orderDetails)
                {
                    bool stockRestored = _productsRepository.IncreaseStock(productId, quantity);
                    if (!stockRestored)
                    {
                        Logger.Error($"Error al restaurar stock del producto {productId}");
                        return (false, $"Error al restaurar stock de los productos");
                    }
                }

                // Actualizar el estado de la orden a cancelada
                order.OrderStatus = "Cancelled";
                bool updated = _deliveryRepository.UpdateOrder(order);
                if (!updated)
                {
                    return (false, "No se pudo cancelar la orden");
                }

                Logger.Info($"Orden {orderId} cancelada exitosamente. Stock restaurado.");
                return (true, "Orden cancelada correctamente. Stock restaurado.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al cancelar orden {orderId}");
                return (false, $"Error al cancelar orden: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca detalles de órdenes por cliente, producto, precio o estado
        /// </summary>
        public (bool Success, string Message, IEnumerable<OrderDetailListDto> OrderDetails) SearchOrderDetails(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return (false, "El término de búsqueda no puede estar vacío", null);
                }

                var orderDetails = _deliveryRepository.SearchDetailOrders(searchTerm);
                if (orderDetails == null)
                {
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var detailList = new List<OrderDetailListDto>(orderDetails);
                if (detailList.Count == 0)
                {
                    return (false, "No se encontraron resultados para la búsqueda", detailList);
                }

                return (true, $"Se encontraron {detailList.Count} resultados", detailList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al buscar detalles de órdenes");
                return (false, $"Error en la búsqueda: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza el estado de una orden y maneja la devolución de stock si se cancela
        /// </summary>
        public (bool Success, string Message) UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                // Obtener la orden actual para verificar el estado anterior
                var currentOrder = _deliveryRepository.ReadOneOrder(orderId);
                if (currentOrder == null)
                {
                    return (false, "Orden no encontrada");
                }

                // Si se cambia a Cancelled, restaurar el stock
                if (newStatus.ToLower() == "cancelled" && currentOrder.OrderStatus.ToLower() != "cancelled")
                {
                    // Obtener detalles de la orden
                    var orderDetails = _deliveryRepository.GetOrderDetails(orderId);
                    
                    // Restaurar stock para cada producto
                    foreach (var (productId, quantity) in orderDetails)
                    {
                        var increased = _productsRepository.IncreaseStock(productId, quantity);
                        if (!increased)
                        {
                            return (false, $"No se pudo restaurar stock del producto {productId}");
                        }
                    }
                }

                // Actualizar estado
                var updated = _deliveryRepository.UpdateOrderStatus(orderId, newStatus);
                if (!updated)
                {
                    return (false, "No se pudo actualizar el estado de la orden");
                }

                Logger.Info($"Estado de orden {orderId} actualizado a {newStatus}");
                return (true, $"Estado actualizado a {newStatus} correctamente");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al actualizar estado de orden {orderId}");
                return (false, $"Error al actualizar estado: {ex.Message}");
            }
        }
    }
}

