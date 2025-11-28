using System;

namespace GestionPedidos.Models.DTO
{
    /// <summary>
    /// DTO para mostrar los detalles de órdenes en una tabla
    /// </summary>
    public class OrderDetailListDto
    {
        public int IdOrder { get; set; }
        public int IdDetail { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
