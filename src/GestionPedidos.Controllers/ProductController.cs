using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using GestionPedidos.Models.DTOs;

namespace GestionPedidos.Controllers
{
    public class ProductController
    {
        private readonly IProductsRepository _productRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProductController()
        {
            _productRepository = new ProductsRepository();
        }

        public (bool Success, string Message, IEnumerable<ProductListDto> Products) ReadAll()
        {
            try
            {
                Logger.Debug("Solicitando listado completo de productos");

                var products = _productRepository.ReadAllProducts();

                if (products == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar productos");
                    return (false, "No se pudo obtener la lista de productos.", null);
                }

                var productList = new List<ProductListDto>(products);

                if (productList.Count == 0)
                {
                    Logger.Info("No se encontraron productos registrados.");
                    return (true, "No hay productos registrados.", productList);
                }

                Logger.Info($"Se recuperaron {productList.Count} productos del repositorio");
                return (true, "Productos recuperados correctamente.", productList);
            }
            catch (Exception ex)
            {
                // El logger ayuda a registrar los errores en un archivo
                Logger.Error(ex, $"Error al leer registros en productos");
                return (false, "Error al leer registros en productos", null);
            }
        }
    }
}
