using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Entities
{
    public class Customer
    {
        public int IdCustomer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? IdCity { get; set; }

        public string City { get; set; }

        public string Department { get; set; }
        public bool IsActive { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
