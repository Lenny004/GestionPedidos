using System.Collections.Generic;
using System.Data;
using GestionPedidos.Models.DTOs;
using GestionPedidos.Models.Entities;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<ProductListDto> ReadAllProducts();
        Product ReadOne(int id);
        bool Create(Product product, int id);
        bool Update(Product product, int id);
    }
}
