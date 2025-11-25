using GestionPedidos.Common.Security;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using GestionPedidos.Models.DTOs;
using GestionPedidos.Models.Entities;
using GestionPedidos.Models.Enums;
using GestionPedidos.Common.Validation;
using System.Linq;

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

        public (bool Success, string Message, Product Product) ReadOne(int id)
        {
            try
            {
                Logger.Warn("ID de producto solicitado: {id}", id);
                if (id <= 0) return (false, "ID de producto inválido", null);

                var product = _productRepository.ReadOne(id);

                if (product == null)
                {
                    return (false, "Producto no encontrado", null);
                }

                return (true, "Producto encontrado", product);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al obtener producto ID {id}");
                return (false, $"Error al cargar el producto: {ex.Message}", null);
            }
        }

        public List<object> GetProductStatuses()
        {
            try
            {
                return Enum.GetValues(typeof(EstadoProducto))
                        .OfType<EstadoProducto>() // <-- Usa OfType en vez de Cast
                        .Select(e => new
                        {
                            Text = e.ToString(),
                            Value = (byte)e
                        })
                        .ToList<object>();
            }
            catch (Exception ex)
            {
                // Puedes loguear el error aquí si tienes un sistema de logging
                throw new Exception($"Error al obtener los estados del producto: {ex.Message}", ex);
            }
        }

        public (bool Success, string Message) Create(string name, string description, int stock, decimal price)
        {
            try
            {
                if (!GeneralValidator.IsNotEmpty(name))
                {
                    Logger.Warn("Intento de crear producto sin nombre");
                    return (false, "El nombre del producto es requerido");
                }

                if (stock < 0)
                {
                    Logger.Warn("Intento de crear producto con stock negativo");
                    return (false, "El stock no puede ser negativo");
                }

                if (price < 0)
                {
                    Logger.Warn("Intento de crear producto con precio negativo");
                    return (false, "El precio no puede ser negativo");
                }

                var product = new Product
                {
                    ProductName = name,
                    Description = description,
                    StockQuantity = stock,
                    SalePrice = price
                };

                // Obtener ID del usuario logeado
                int currentUserId = SessionManager.UsuarioId;

                // Validar que haya sesión activa
                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de crear producto sin sesión de usuario activa");
                    return (false, "No se pudo identificar al usuario actual. Por favor inicie sesión nuevamente.");
                }

                // Pasamos el ID al repositorio
                bool created = _productRepository.Create(product, currentUserId);

                if (!created)
                {
                    Logger.Warn("No se pudo crear el producto: {productName}", product.ProductName);
                    return (false, "No se pudo crear el producto.");
                }

                Logger.Info("Producto creado exitosamente: {productName}", product.ProductName);
                return (true, "Producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al crear producto: {name}");
                return (false, $"Error al crear el producto: {ex.Message}");
            }
        }

        public (bool Success, string Message) Update(int id, string name, string description, int stock, decimal price, byte status)
        {
            try
            {
                if (id <= 0)
                {
                    Logger.Warn("Intento de actualizar producto con ID inválido");
                    return (false, "ID de producto inválido");
                }

                if (!GeneralValidator.IsNotEmpty(name))
                {
                    Logger.Warn("Intento de actualizar producto sin nombre");
                    return (false, "El nombre del producto es requerido");
                }

                if (stock < 0)
                {
                    Logger.Warn("Intento de actualizar producto con stock negativo");
                    return (false, "El stock no puede ser negativo");
                }

                if (price < 0)
                {
                    Logger.Warn("Intento de actualizar producto con precio negativo");
                    return (false, "El precio no puede ser negativo");
                }

                var product = new Product
                {
                    IdProduct = id,
                    ProductName = name,
                    Description = description,
                    StockQuantity = stock,
                    SalePrice = price,
                    Status = (EstadoProducto)status // Convertir byte a enum
                };

                int currentUserId = SessionManager.UsuarioId;

                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de actualizar producto sin sesión de usuario activa");
                    return (false, "No se pudo identificar al usuario actual. Por favor inicie sesión nuevamente.");
                }

                bool updated = _productRepository.Update(product, currentUserId);

                if (!updated)
                {
                    Logger.Warn("No se pudo actualizar el producto: {productName}", product.ProductName);
                    return (false, "No se pudo actualizar el producto.");
                }

                Logger.Info("Producto actualizado exitosamente: {productName}", product.ProductName);
                return (true, "Producto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al actualizar producto: {name}");
                return (false, $"Error al actualizar el producto: {ex.Message}");
            }
        }
    }
}
