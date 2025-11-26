using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.DTO
{
    public class ProductSelectDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
