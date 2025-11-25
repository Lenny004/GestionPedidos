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
using GestionPedidos.Common.Constants;
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
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var productList = new List<ProductListDto>(products);

                if (productList.Count == 0)
                {
                    Logger.Info("No se encontraron productos registrados.");
                    return (true, AppConstants.NO_SE_ENCONTRARON_REGISTROS, productList);
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
                    return (false, Messages.Productos.PRODUCTO_NO_ENCONTRADO, null);
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
                    return (false, AppConstants.CAMPO_REQUERIDO);
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
                    return (false, AppConstants.SESION_EXPIRADA);
                }

                // Pasamos el ID al repositorio
                bool created = _productRepository.Create(product, currentUserId);

                if (!created)
                {
                    Logger.Warn("No se pudo crear el producto: {productName}", product.ProductName);
                    return (false, AppConstants.ERROR_GUARDAR);
                }

                Logger.Info("Producto creado exitosamente: {productName}", product.ProductName);
                return (true, Messages.Productos.PRODUCTO_GUARDADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al crear producto: {name}");
                return (false, $"{AppConstants.ERROR_GUARDAR}: {ex.Message}");
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
                    return (false, AppConstants.CAMPO_REQUERIDO);
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
                    return (false, AppConstants.SESION_EXPIRADA);
                }

                bool updated = _productRepository.Update(product, currentUserId);

                if (!updated)
                {
                    Logger.Warn("No se pudo actualizar el producto: {productName}", product.ProductName);
                    return (false, AppConstants.ERROR_ACTUALIZAR);
                }

                Logger.Info("Producto actualizado exitosamente: {productName}", product.ProductName);
                return (true, Messages.Productos.PRODUCTO_ACTUALIZADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al actualizar producto: {name}");
                return (false, $"{AppConstants.ERROR_ACTUALIZAR}: {ex.Message}");
            }
        }

        public (bool Success, string Message) Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Logger.Warn("Intento de eliminar producto con ID inválido");
                    return (false, "ID de producto inválido");
                }

                int currentUserId = SessionManager.UsuarioId;

                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de eliminar producto sin sesión de usuario activa");
                    return (false, AppConstants.SESION_EXPIRADA);
                }

                bool deleted = _productRepository.Delete(id, currentUserId);

                if (!deleted)
                {
                    Logger.Warn("No se pudo eliminar el producto con ID: {id}", id);
                    return (false, AppConstants.ERROR_ELIMINAR);
                }

                Logger.Info("Producto eliminado exitosamente. ID: {id}", id);
                return (true, Messages.Productos.PRODUCTO_ELIMINADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al eliminar producto ID: {id}");
                return (false, $"{AppConstants.ERROR_ELIMINAR}: {ex.Message}");
            }
        }
    }
}
