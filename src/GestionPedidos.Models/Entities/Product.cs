using System;
using GestionPedidos.Models.Enums;

namespace GestionPedidos.Models.Entities
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQuantity { get; set; }
        public string UserCreation { get; set; }
        public string UserModification { get; set; }
    
        // Cambiar de bool a EstadoProducto
        public EstadoProducto Status { get; set; }
    
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    
        public Product()
        {
            Status = EstadoProducto.Activo;
            CreatedAt = DateTime.Now;
        }
    
        // Propiedad de compatibilidad para la base de datos
        public bool IsActive 
        { 
            get => Status == EstadoProducto.Activo;
            set => Status = value ? EstadoProducto.Activo : EstadoProducto.Inactivo;
        }
    }
}
