using System;

namespace GestionPedidos.Models.DTOs
{
    // Usando los nombres de las columnas de Orders y Customer para el DTO
    public class OrderDeliveryDTO
    {
        // Datos del Pedido (Orders)
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } // Para filtrar por 'InProcess' o 'Pending'
        public decimal Total { get; set; }

        // Datos del Cliente (Customers)
        public int IdCustomer { get; set; }
        public string CustomerFullName { get; set; } // Nombre completo del cliente
        public string CustomerPhone { get; set; }

        // Datos de Ubicación (Cities/Departments - Asumiremos que Orders o Customers tiene campos de dirección)
        public string DeliveryAddress { get; set; } // Campo de dirección de la tabla Customers o Orders
        public string DeliveryCity { get; set; }
        public string DeliveryDepartment { get; set; }
    }
}