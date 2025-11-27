using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.Common.Validation;
using GestionPedidos.Common.Services;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionPedidos.Controllers
{
    public class CustomerController
    {
        private readonly ICustomerRepository _customerRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CustomerController()
        {
            _customerRepository = new CustomerRepository();
        }

        // Método que retorna la tupla exacta que pide tu grupo
        public (bool Success, string Message, IEnumerable<CustomerListDto> Customers) ReadAll()
        {
            try
            {
                // 1. Llamada al repositorio (Linea verde de tu imagen)
                var customers = _customerRepository.ReadAllCustomers();

                // Validación básica
                if (customers == null)
                {
                    return (false, "El repositorio devolvió null.", null);
                }

                // 2. Convertir a Lista en memoria (Linea azul de tu imagen)
                var customerList = new List<CustomerListDto>(customers);

                if (customerList.Count == 0)
                {
                    return (true, "No hay clientes registrados.", customerList);
                }

                return (true, "Clientes recuperados correctamente.", customerList);
            }
            catch (Exception ex)
            {
                return (false, $"Error al leer clientes: {ex.Message}", null);
            }
        }

        public (bool Success, string Message, Customer Customer) ReadOne(int id)
        {
            try
            {
                var customer = _customerRepository.ReadOne(id);
                if (customer == null) return (false, "Cliente no encontrado", null);
                return (true, "Encontrado", customer);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error leyendo cliente ID {id}");
                return (false, ex.Message, null);
            }
        }

        // 3. GET CITIES (Para el ComboBox)
        public List<dynamic> GetCitiesList()
        {
            return _customerRepository.GetCitiesForCombo();
        }

        // 4. CREATE
        public (bool Success, string Message) Create(string fname, string lname, string phone, string address, int idCity)
        {
            try
            {
                // Validar nombres (máximo 50 caracteres según estándares)
                var fnameValidation = GeneralValidator.ValidateLengthRange(fname, 2, 50, "Nombre");
                if (!fnameValidation.IsValid)
                    return (false, fnameValidation.ErrorMessage);

                var lnameValidation = GeneralValidator.ValidateLengthRange(lname, 2, 50, "Apellido");
                if (!lnameValidation.IsValid)
                    return (false, lnameValidation.ErrorMessage);

                // Validar que solo contengan letras
                if (!GeneralValidator.ValidateOnlyLetters(fname))
                    return (false, "El nombre debe contener solo letras.");

                if (!GeneralValidator.ValidateOnlyLetters(lname))
                    return (false, "El apellido debe contener solo letras.");

                // Validar teléfono (opcional, pero si se proporciona debe ser válido)
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    if (!GeneralValidator.ValidatePhoneNumber(phone))
                        return (false, AppConstants.TELEFONO_INVALIDO);

                    if (phone.Length > 20)
                        return (false, "El teléfono no puede exceder 20 caracteres.");
                }

                // Validar dirección (máximo 255 caracteres)
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (address.Length > 255)
                        return (false, "La dirección no puede exceder 255 caracteres.");
                }

                // Validar ciudad
                if (idCity <= 0)
                    return (false, "Seleccione una ciudad válida.");

                var customer = new Customer
                {
                    FirstName = fname.Trim(),
                    LastName = lname.Trim(),
                    Phone = phone?.Trim(),
                    Address = address?.Trim(),
                    IdCity = idCity
                };

                // Usamos el ID del usuario logueado para la auditoría
                bool result = _customerRepository.Create(customer, SessionManager.UsuarioId);

                if (result)
                {
                    Logger.Info($"Cliente creado: {fname} {lname}");
                    return (true, Messages.Clientes.CLIENTE_GUARDADO);
                }
                
                return (false, AppConstants.ERROR_GUARDAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creando cliente");
                return (false, $"{AppConstants.ERROR_GUARDAR}: {ex.Message}");
            }
        }

        // 5. UPDATE
        public (bool Success, string Message) Update(int id, string fname, string lname, string phone, string address, int idCity, bool isActive)
        {
            try
            {
                // Validar ID
                if (id <= 0)
                    return (false, "ID inválido.");

                // Validar nombres (máximo 50 caracteres según estándares)
                var fnameValidation = GeneralValidator.ValidateLengthRange(fname, 2, 50, "Nombre");
                if (!fnameValidation.IsValid)
                    return (false, fnameValidation.ErrorMessage);

                var lnameValidation = GeneralValidator.ValidateLengthRange(lname, 2, 50, "Apellido");
                if (!lnameValidation.IsValid)
                    return (false, lnameValidation.ErrorMessage);

                // Validar que solo contengan letras
                if (!GeneralValidator.ValidateOnlyLetters(fname))
                    return (false, "El nombre debe contener solo letras.");

                if (!GeneralValidator.ValidateOnlyLetters(lname))
                    return (false, "El apellido debe contener solo letras.");

                // Validar teléfono (opcional, pero si se proporciona debe ser válido)
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    if (!GeneralValidator.ValidatePhoneNumber(phone))
                        return (false, AppConstants.TELEFONO_INVALIDO);

                    if (phone.Length > 20)
                        return (false, "El teléfono no puede exceder 20 caracteres.");
                }

                // Validar dirección (máximo 255 caracteres)
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (address.Length > 255)
                        return (false, "La dirección no puede exceder 255 caracteres.");
                }

                // Validar ciudad
                if (idCity <= 0)
                    return (false, "Seleccione una ciudad válida.");

                var customer = new Customer
                {
                    IdCustomer = id,
                    FirstName = fname.Trim(),
                    LastName = lname.Trim(),
                    Phone = phone?.Trim(),
                    Address = address?.Trim(),
                    IdCity = idCity,
                    IsActive = isActive
                };

                bool result = _customerRepository.Update(customer, SessionManager.UsuarioId);
                
                if (result)
                {
                    Logger.Info($"Cliente actualizado ID: {id}");
                    return (true, Messages.Clientes.CLIENTE_ACTUALIZADO);
                }
                
                return (false, AppConstants.ERROR_ACTUALIZAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error actualizando cliente");
                return (false, $"{AppConstants.ERROR_ACTUALIZAR}: {ex.Message}");
            }
        }

        // 6. DELETE
        public (bool Success, string Message) Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return (false, "ID inválido.");

                bool result = _customerRepository.Delete(id, SessionManager.UsuarioId);
                
                if (result)
                {
                    Logger.Info($"Cliente eliminado ID: {id}");
                    return (true, Messages.Clientes.CLIENTE_ELIMINADO);
                }
                
                return (false, AppConstants.ERROR_ELIMINAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error eliminando cliente ID: {id}");
                return (false, $"{AppConstants.ERROR_ELIMINAR}: {ex.Message}");
            }
        }

        public (bool Success, string Message, IEnumerable<CustomerListDto> Customers) SearchByName(string name)
        {
            try
            {
                // Validar que se haya ingresado un término de búsqueda
                if (string.IsNullOrWhiteSpace(name))
                    return (false, "Ingrese un nombre para buscar.", null);

                // Validar longitud mínima
                if (name.Trim().Length < 2)
                    return (false, "El término de búsqueda debe tener al menos 2 caracteres.", null);

                var customers = _customerRepository.SearchCustomers(name);

                if (customers == null)
                    return (false, AppConstants.ERROR_GUARDAR, null);

                var customerList = new List<CustomerListDto>(customers);

                if (customerList.Count == 0)
                    return (true, AppConstants.NO_SE_ENCONTRARON_REGISTROS, customerList);

                Logger.Info($"Búsqueda de clientes por '{name}': {customerList.Count} resultados");
                return (true, $"Se encontraron {customerList.Count} cliente(s).", customerList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error buscando clientes: {name}");
                return (false, $"Error al realizar la búsqueda: {ex.Message}", null);
            }
        }

        public List<CustomerSelectDto> GetCustomersForCombo()
        {
            // Aquí idealmente llamarías a un método específico del repositorio: _customerRepository.GetActiveCustomers()
            // Por ahora, simulamos usando el ReadAll y filtrando
            var result = ReadAll();
            if (result.Success)
            {
                return result.Customers
                    .Where(c => c.IsActive)
                    .Select(c => new CustomerSelectDto
                    {
                        IdCustomer = c.IdCustomer,
                        FullName = c.FullName
                    }).ToList();
            }
            return new List<CustomerSelectDto>();
        }
    }
}
