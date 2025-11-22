using System;

namespace GestionPedidos.Models.DTOs
{
    /// <summary>
    /// Representa los datos mínimos necesarios para listar productos en pantalla.
    /// Evita exponer la entidad completa y simplifica los mapeos desde consultas SQL.
    /// </summary>
    public class ProductListDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
