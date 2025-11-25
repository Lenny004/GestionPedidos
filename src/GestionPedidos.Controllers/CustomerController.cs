using System;
using System.Collections.Generic;
using System.Linq;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.Entities;
using GestionPedidos.Models.DTOs;

namespace GestionPedidos.Controllers
{
    public class CustomerController
    {
        private readonly ICustomerRepository _customerRepository;
        // private static readonly Logger Logger = ... (Si usas NLog)

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
    }
}
