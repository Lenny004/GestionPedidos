using System.Collections.Generic;
using System.Data;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<ProductListDto> ReadAllProducts();
        Product ReadOne(int id);
        bool Create(Product product, int id);
        bool Update(Product product, int id);
        bool Delete(int id, int userId);
        IEnumerable<ProductSelectDto> ReadProductsForCombo();
        IEnumerable<ProductListDto> SearchProducts(string productName);
    }
}
