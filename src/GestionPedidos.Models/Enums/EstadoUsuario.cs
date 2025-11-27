using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Enums
{
    public enum EstadoUsuario
    {
        /// <summary>
        /// Producto agotado, no disponible para la venta
        /// </summary>
        Inactivo = 0,
        /// <summary>
        /// Producto disponible para la venta
        /// </summary>
        Activo = 1,
    }
}
