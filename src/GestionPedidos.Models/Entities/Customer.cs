using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Entities
{
    internal class Customer
    {
        public int IdCustomer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Llaves foráneas
        public int IdDepartment { get; set; }
        public int IdCity { get; set; }
        public bool IsActive { get; set; }

        // Auditoría
        public int? UserCreation { get; set; }
        public int? UserModification { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Propiedad auxiliar (no está en BD, pero ayuda en código)
        public string FullName => $"{FirstName} {LastName}";
    }
}
