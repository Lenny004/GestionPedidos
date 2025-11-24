using System.Collections.Generic;
using GestionPedidos.Models.DTOs;
using GestionPedidos.Models.Entities;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<ProductListDto> ReadAllProducts();
        Product ReadOne(int id);
    }
}
