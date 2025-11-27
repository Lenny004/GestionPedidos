using System;
using System.Windows.Forms;

namespace GestionPedidos.Common.Services
{
    /// <summary>
    /// Servicio centralizado para mostrar notificaciones al usuario
    /// </summary>
    public static class NotificationService
    {
        #region Configuración de Iconos y Títulos
        private const string TITLE_SUCCESS = "Operación Exitosa";
        private const string TITLE_ERROR = "Error";
        private const string TITLE_WARNING = "Advertencia";
        private const string TITLE_INFO = "Información";
        private const string TITLE_CONFIRMATION = "Confirmación";
        #endregion

        #region Métodos Principales de Notificación

        /// <summary>
        /// Muestra una notificación de éxito
        /// </summary>
        /// <param name="message">Mensaje a mostrar</param>
        /// <param name="title">Título personalizado (opcional)</param>
        public static void ShowSuccess(string message, string title = null)
        {
            MessageBox.Show(
                message,
                title ?? TITLE_SUCCESS,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Muestra una notificación de error
        /// </summary>
        /// <param name="message">Mensaje de error a mostrar</param>
        /// <param name="title">Título personalizado (opcional)</param>
        public static void ShowError(string message, string title = null)
        {
            MessageBox.Show(
                message,
                title ?? TITLE_ERROR,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Muestra una notificación de advertencia
        /// </summary>
        /// <param name="message">Mensaje de advertencia a mostrar</param>
        /// <param name="title">Título personalizado (opcional)</param>
        public static void ShowWarning(string message, string title = null)
        {
            MessageBox.Show(
                message,
                title ?? TITLE_WARNING,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        /// <summary>
        /// Muestra una notificación informativa
        /// </summary>
        /// <param name="message">Mensaje informativo a mostrar</param>
        /// <param name="title">Título personalizado (opcional)</param>
        public static void ShowInfo(string message, string title = null)
        {
            MessageBox.Show(
                message,
                title ?? TITLE_INFO,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion

        #region Métodos de Confirmación

        /// <summary>
        /// Muestra un diálogo de confirmación con opciones Sí/No
        /// </summary>
        /// <param name="message">Mensaje de confirmación</param>
        /// <param name="title">Título personalizado (opcional)</param>
        /// <returns>True si el usuario confirmó, False si canceló</returns>
        public static bool ShowConfirmation(string message, string title = null)
        {
            DialogResult result = MessageBox.Show(
                message,
                title ?? TITLE_CONFIRMATION,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Muestra un diálogo de confirmación con opciones Sí/No/Cancelar
        /// </summary>
        /// <param name="message">Mensaje de confirmación</param>
        /// <param name="title">Título personalizado (opcional)</param>
        /// <returns>DialogResult con la opción seleccionada</returns>
        public static DialogResult ShowConfirmationWithCancel(string message, string title = null)
        {
            return MessageBox.Show(
                message,
                title ?? TITLE_CONFIRMATION,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );
        }

        #endregion

        #region Notificaciones Específicas del Sistema

        /// <summary>
        /// Notifica al usuario sobre la creación exitosa de un registro
        /// </summary>
        /// <param name="entityName">Nombre de la entidad creada (ej: "Cliente", "Producto")</param>
        public static void NotifyCreated(string entityName)
        {
            ShowSuccess($"{entityName} creado exitosamente.", "Registro Creado");
        }

        /// <summary>
        /// Notifica al usuario sobre la actualización exitosa de un registro
        /// </summary>
        /// <param name="entityName">Nombre de la entidad actualizada</param>
        public static void NotifyUpdated(string entityName)
        {
            ShowSuccess($"{entityName} actualizado exitosamente.", "Registro Actualizado");
        }

        /// <summary>
        /// Notifica al usuario sobre la eliminación exitosa de un registro
        /// </summary>
        /// <param name="entityName">Nombre de la entidad eliminada</param>
        public static void NotifyDeleted(string entityName)
        {
            ShowSuccess($"{entityName} eliminado exitosamente.", "Registro Eliminado");
        }

        /// <summary>
        /// Notifica al usuario sobre un cambio de estado
        /// </summary>
        /// <param name="entityName">Nombre de la entidad</param>
        /// <param name="newStatus">Nuevo estado</param>
        public static void NotifyStatusChanged(string entityName, string newStatus)
        {
            ShowInfo($"{entityName} cambió su estado a: {newStatus}", "Cambio de Estado");
        }

        /// <summary>
        /// Notifica sobre stock bajo de un producto
        /// </summary>
        /// <param name="productName">Nombre del producto</param>
        /// <param name="currentStock">Stock actual</param>
        public static void NotifyLowStock(string productName, int currentStock)
        {
            ShowWarning(
                $"¡Advertencia! El producto '{productName}' tiene stock bajo.\nStock actual: {currentStock} unidades.",
                "Stock Bajo"
            );
        }

        /// <summary>
        /// Notifica sobre stock agotado de un producto
        /// </summary>
        /// <param name="productName">Nombre del producto</param>
        public static void NotifyOutOfStock(string productName)
        {
            ShowError(
                $"¡Alerta! El producto '{productName}' está agotado.\nStock actual: 0 unidades.",
                "Stock Agotado"
            );
        }

        /// <summary>
        /// Notifica validaciones fallidas
        /// </summary>
        /// <param name="validationMessage">Mensaje de validación</param>
        public static void NotifyValidationError(string validationMessage)
        {
            ShowWarning(validationMessage, "Validación");
        }

        /// <summary>
        /// Notifica errores de base de datos
        /// </summary>
        /// <param name="errorMessage">Mensaje de error</param>
        public static void NotifyDatabaseError(string errorMessage)
        {
            ShowError(
                $"Ocurrió un error en la base de datos:\n{errorMessage}",
                "Error de Base de Datos"
            );
        }

        #endregion
    }
}
