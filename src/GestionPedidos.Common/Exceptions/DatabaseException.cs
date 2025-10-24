using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Common.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores de base de datos
    /// </summary>
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DatabaseException() : base("Ocurrió un error en la base de datos")
        {
        }

        /// <summary>
        /// Constructor con mensaje personalizado
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public DatabaseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción que causó este error</param>
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
