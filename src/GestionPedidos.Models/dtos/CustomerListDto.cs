using System;


namespace GestionPedidos.Models.DTOs
{
    public class CustomerListDto
    {
        public int IdCustomer { get; set; }
        public string FullName { get; set; } // Concatenación de FirstName + LastName
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }       
        public string Department { get; set; } // Nombre del departamento (JOIN)
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }  // Nombre de usuario (JOIN)
    }
}

