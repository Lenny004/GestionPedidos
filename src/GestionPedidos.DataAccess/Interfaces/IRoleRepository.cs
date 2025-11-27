using GestionPedidos.Models.Entities;
using System.Collections.Generic;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Obtiene todos los roles activos del sistema
        /// </summary>
        /// <returns>Lista de roles</returns>
        IEnumerable<Rol> ReadAllRoles();
    }
}