using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerListDto> ReadAllCustomers();
        Customer ReadOne(int idCustomer);
        List<dynamic> GetCitiesForCombo();
        bool Create(Customer customer, int userId);
        bool Update(Customer customer, int userId);
        bool Delete(int id, int userId);
        IEnumerable<CustomerListDto> SearchCustomers(string value);
    }
}
