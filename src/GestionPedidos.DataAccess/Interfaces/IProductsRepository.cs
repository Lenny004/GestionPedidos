using System.Collections.Generic;
using GestionPedidos.Models.DTOs;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<ProductListDto> ReadAllProducts();
    }
}
