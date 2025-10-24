using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Enums
{
    /// <summary>
    /// Tipos de roles de usuario en el sistema
    /// </summary>
    public enum TipoRoles
    {
        /// <summary>
        /// Administrador: acceso total al sistema
        /// </summary>
        Administrador = 1,

        /// <summary>
        /// Operador: acceso limitado para operaciones diarias
        /// </summary>
        Operador = 2
    }
}
