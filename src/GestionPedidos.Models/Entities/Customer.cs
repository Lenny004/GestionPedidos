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
        public int IdCity { get; set; }
        public bool IsActive { get; set; }

        // Propiedades de auditoría opcionales
        public int? UserCreation { get; set; }
        public DateTime CreatedAt { get; set; }

        // Propiedad auxiliar para mostrar nombre completo (útil para ComboBoxes)
        public string FullName => $"{FirstName} {LastName}";
    }
}
