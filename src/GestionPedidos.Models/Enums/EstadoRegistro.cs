using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Enums
{
    /// <summary>
    /// Estados posibles de un registro en el sistema
    /// </summary>
    public enum EstadoRegistro
    {
        /// <summary>
        /// El registro está inactivo o eliminado (lógicamente)
        /// </summary>
        Inactivo = 0,

        /// <summary>
        /// El registro está activo y disponible
        /// </summary>
        Activo = 1
    }
}
