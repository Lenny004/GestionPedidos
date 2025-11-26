using System;

namespace GestionPedidos.Models.DTO
{
    /// <summary>
    /// Representa un item temporal del pedido antes de guardarlo en la base de datos.
    /// Se usa en memoria para manejar el carrito de compras.
    /// </summary>
    public class OrderDetailItem
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;

        public OrderDetailItem(int idProduct, string productName, int quantity, decimal unitPrice)
        {
            IdProduct = idProduct;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
