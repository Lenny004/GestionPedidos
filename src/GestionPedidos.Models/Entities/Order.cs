using System;

namespace GestionPedidos.Models.Entities
{
    public class Order
    {
        public int IdOrder { get; set; }
        public int IdCustomer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Comments { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public decimal Total { get; set; }
        public int UserCreation { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

