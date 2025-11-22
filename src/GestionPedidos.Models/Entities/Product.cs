using System;

namespace GestionPedidos.Models.Entities
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQuantity { get; set; }

        public int? UserCreationId { get; set; }

        public int? UserModificationId { get; set; }

        public bool IsActive { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public User CreatedBy { get; set; }

        public User ModifiedBy { get; set; }

        public Product()
        {
            // Constructor por defecto
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
    }
}
