using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Models.Enums
{
    /// <summary>
    /// Estados del ciclo de vida de un pedido
    /// </summary>
    public enum EstadoPedido
    {
        /// <summary>
        /// Pedido creado, esperando procesamiento
        /// </summary>
        Pendiente = 0,

        /// <summary>
        /// Pedido en proceso de preparación
        /// </summary>
        EnProceso = 1,

        /// <summary>
        /// Pedido entregado al cliente
        /// </summary>
        Entregado = 2,

        /// <summary>
        /// Pedido cancelado por el cliente o sistema
        /// </summary>
        Cancelado = 3
    }
}
