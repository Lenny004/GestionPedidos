using System;

namespace GestionPedidos.Common.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores de validación
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ValidationException() : base("Error de validación")
        {
        }

        /// <summary>
        /// Constructor con mensaje personalizado
        /// </summary>
        /// <param name="message">Mensaje de error de validación</param>
        public ValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción que causó este error</param>
        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}