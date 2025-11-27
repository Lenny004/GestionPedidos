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
        
        /// <summary>
        /// Reduce el stock de un producto
        /// </summary>
        bool ReduceStock(int productId, int quantity);

        /// <summary>
        /// Incrementa el stock de un producto
        /// </summary>
        bool IncreaseStock(int productId, int quantity);
    }
}

