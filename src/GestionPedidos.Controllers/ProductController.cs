using GestionPedidos.Common.Security;
using GestionPedidos.Common.Services;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using GestionPedidos.Models.DTO;
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
                        .OfType<EstadoProducto>()
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
                // Validar nombre del producto (máximo 100 caracteres)
                var nameValidation = GeneralValidator.ValidateLengthRange(name, 2, 100, "Nombre del producto");
                if (!nameValidation.IsValid)
                {
                    Logger.Warn($"Validación fallida en nombre: {nameValidation.ErrorMessage}");
                    return (false, nameValidation.ErrorMessage);
                }

                // Validar descripción (máximo 500 caracteres, opcional)
                if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
                {
                    Logger.Warn("Descripción excede 500 caracteres");
                    return (false, "La descripción no puede exceder 500 caracteres.");
                }

                // Validar stock (debe ser no negativo)
                if (!GeneralValidator.ValidateNonNegativeInt(stock))
                {
                    Logger.Warn($"Intento de crear producto con stock negativo: {stock}");
                    return (false, "El stock no puede ser negativo.");
                }

                // Validar precio (debe ser positivo y con formato decimal correcto)
                if (!GeneralValidator.ValidatePositiveDecimal(price))
                {
                    Logger.Warn($"Intento de crear producto con precio no válido: {price}");
                    return (false, "El precio debe ser mayor a cero.");
                }

                // Validar que el precio no exceda el límite (99,999,999.99)
                if (!GeneralValidator.ValidateRange(price, 0.01m, 99999999.99m))
                {
                    Logger.Warn($"Precio fuera de rango válido: {price}");
                    return (false, "El precio debe estar entre $0.01 y $99,999,999.99");
                }

                var product = new Product
                {
                    ProductName = name.Trim(),
                    Description = description?.Trim(),
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
                
                // Notificar si el stock es bajo al momento de crear
                if (stock <= 10 && stock > 0)
                {
                    Logger.Info($"Producto creado con stock bajo: {name} ({stock} unidades)");
                }

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
                // Validar ID
                if (id <= 0)
                {
                    Logger.Warn("Intento de actualizar producto con ID inválido");
                    return (false, "ID de producto inválido");
                }

                // Validar nombre del producto (máximo 100 caracteres)
                var nameValidation = GeneralValidator.ValidateLengthRange(name, 2, 100, "Nombre del producto");
                if (!nameValidation.IsValid)
                {
                    Logger.Warn($"Validación fallida en nombre: {nameValidation.ErrorMessage}");
                    return (false, nameValidation.ErrorMessage);
                }

                // Validar descripción (máximo 500 caracteres, opcional)
                if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
                {
                    Logger.Warn("Descripción excede 500 caracteres");
                    return (false, "La descripción no puede exceder 500 caracteres.");
                }

                // Validar stock (debe ser no negativo)
                if (!GeneralValidator.ValidateNonNegativeInt(stock))
                {
                    Logger.Warn($"Intento de actualizar producto con stock negativo: {stock}");
                    return (false, "El stock no puede ser negativo.");
                }

                // Validar precio (debe ser positivo)
                if (!GeneralValidator.ValidatePositiveDecimal(price))
                {
                    Logger.Warn($"Intento de actualizar producto con precio no válido: {price}");
                    return (false, "El precio debe ser mayor a cero.");
                }

                // Validar que el precio no exceda el límite (99,999,999.99)
                if (!GeneralValidator.ValidateRange(price, 0.01m, 99999999.99m))
                {
                    Logger.Warn($"Precio fuera de rango válido: {price}");
                    return (false, "El precio debe estar entre $0.01 y $99,999,999.99");
                }

                var product = new Product
                {
                    IdProduct = id,
                    ProductName = name.Trim(),
                    Description = description?.Trim(),
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
                
                // Notificar si el stock es bajo
                if (stock <= 10 && stock > 0)
                {
                    Logger.Info($"Producto actualizado con stock bajo: {name} ({stock} unidades)");
                }

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

        public (bool Success, string Message, IEnumerable<ProductListDto> Products) SearchByName(string name)
        {
            try
            {
                Logger.Debug("Buscando productos por nombre: {name}", name);
                
                // Validar que se haya ingresado un término de búsqueda
                if (!GeneralValidator.IsNotEmpty(name))
                {
                    Logger.Warn("Intento de búsqueda de productos con nombre vacío");
                    return (false, "Ingrese un nombre para buscar.", null);
                }

                // Validar longitud mínima
                if (name.Trim().Length < 2)
                    return (false, "El término de búsqueda debe tener al menos 2 caracteres.", null);

                var products = _productRepository.SearchProducts(name);

                if (products == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar productos");
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var productList = new List<ProductListDto>(products);

                if (productList.Count == 0)
                {
                    Logger.Info("No se encontraron productos con el término de búsqueda.");
                    return (true, AppConstants.NO_SE_ENCONTRARON_REGISTROS, productList);
                }

                Logger.Info($"Se recuperaron {productList.Count} productos del repositorio");
                return (true, $"Se encontraron {productList.Count} producto(s).", productList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al buscar productos por nombre: {name}");
                return (false, $"Error al buscar productos: {ex.Message}", null);
            }
        }

        public List<ProductSelectDto> GetProductsForCombo()
        {
            try
            {
                var products = _productRepository.ReadProductsForCombo();

                if (products == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar productos para combo");
                    return new List<ProductSelectDto>();
                }

                return products.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al obtener productos para combo");
                return new List<ProductSelectDto>();
            }
        }
    }
}
