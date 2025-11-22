using GestionPedidos.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        // Método para la vista de Delivery que definimos:
        List<OrderDeliveryDTO> GetOrdersForDelivery();

        // Métodos CRUD comunes para Pedidos:
        // bool AddOrder(Order newOrder);
        // Order GetOrderById(int id);
        // bool UpdateOrderStatus(int idOrder, string newStatus, int userId);
        // bool DeleteOrder(int idOrder);
        bool CreateOrderHeader(int idCustomer, int idUserCreation, DateTime deliveryDate, string comments);
    }


}
