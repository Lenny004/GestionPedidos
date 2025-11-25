using GestionPedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Entities
{
    /// <summary>
    /// Entidad que representa un usuario en el sistema
    /// </summary>
    public class User
    {
        public int IdUser { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public int IdRole { get; set; }

        public Rol Rol { get; set; }

        public TipoRoles? TipoRol
        {
            get
            {
                return Enum.IsDefined(typeof(TipoRoles), IdRole)
                    ? (TipoRoles)IdRole
                    : (TipoRoles?)null;
            }
        }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public User()
        {
            // Constructor por defecto
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
    }
}
