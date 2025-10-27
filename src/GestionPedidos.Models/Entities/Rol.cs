using System;

namespace GestionPedidos.Models.Entities
{
    /// <summary>
    /// Entidad que representa un rol de usuario en el sistema.
    /// </summary>
    public class Rol
    {
        public int IdRole { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Rol()
        {
            // Constructor por defecto
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
    }
}
