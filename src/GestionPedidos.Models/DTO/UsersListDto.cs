using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.DTO
{
    public class UsersListDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
