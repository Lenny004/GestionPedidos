using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionPedidos.Models.DTOs;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerListDto> ReadAllCustomers();
    }
}
