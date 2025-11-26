using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.Common.Validation;
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
                if (!GeneralValidator.IsNotEmpty(fname) || !GeneralValidator.IsNotEmpty(lname))
                    return (false, "Nombre y Apellido son requeridos.");

                if (idCity <= 0) return (false, "Seleccione una ciudad válida.");

                var customer = new Customer
                {
                    FirstName = fname,
                    LastName = lname,
                    Phone = phone,
                    Address = address,
                    IdCity = idCity
                };

                // Usamos el ID del usuario logueado para la auditoría
                bool result = _customerRepository.Create(customer, SessionManager.UsuarioId);

                return result ? (true, Messages.Clientes.CLIENTE_GUARDADO) : (false, AppConstants.ERROR_GUARDAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creando cliente");
                return (false, ex.Message);
            }
        }

        // 5. UPDATE
        public (bool Success, string Message) Update(int id, string fname, string lname, string phone, string address, int idCity, bool isActive)
        {
            try
            {
                if (id <= 0) return (false, "ID inválido");

                if (!GeneralValidator.IsNotEmpty(fname) || !GeneralValidator.IsNotEmpty(lname))
                    return (false, "Nombre y Apellido son requeridos.");

                var customer = new Customer
                {
                    IdCustomer = id,
                    FirstName = fname,
                    LastName = lname,
                    Phone = phone,
                    Address = address,
                    IdCity = idCity,
                    IsActive = isActive
                };

                bool result = _customerRepository.Update(customer, SessionManager.UsuarioId);
                return result ? (true, Messages.Clientes.CLIENTE_ACTUALIZADO) : (false, AppConstants.ERROR_ACTUALIZAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error actualizando cliente");
                return (false, ex.Message);
            }
        }

        // 6. DELETE
        public (bool Success, string Message) Delete(int id)
        {
            try
            {
                bool result = _customerRepository.Delete(id, SessionManager.UsuarioId);
                return result ? (true, Messages.Clientes.CLIENTE_ELIMINADO) : (false, AppConstants.ERROR_ELIMINAR);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error eliminando cliente");
                return (false, ex.Message);
            }
        }

        public (bool Success, string Message, IEnumerable<CustomerListDto> Customers) SearchByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return (false, "Ingrese un nombre para buscar.", null);
                }

                var customers = _customerRepository.SearchCustomers(name);

                if (customers == null)
                    return (false, "Error al realizar la búsqueda.", null);

                var customerList = new List<CustomerListDto>(customers);

                if (customerList.Count == 0)
                    return (true, "No se encontraron coincidencias.", customerList);

                return (true, "Búsqueda exitosa.", customerList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error buscando clientes: {name}");
                return (false, ex.Message, null);
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
