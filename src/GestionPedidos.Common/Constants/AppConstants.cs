using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Common.Constants
{
    /// <summary>
    /// Constantes utilizadas en toda la aplicación
    /// </summary>
    public static class AppConstants
    {
        #region Información de la Aplicación
        /// <summary>
        /// Nombre de la aplicación
        /// </summary>
        public const string APP_NAME = "Sistema de Gestión de Pedidos";

        /// <summary>
        /// Versión actual del sistema
        /// </summary>
        public const string APP_VERSION = "1.0.0";

        /// <summary>
        /// Año de desarrollo
        /// </summary>
        public const string APP_YEAR = "2025";
        #endregion

        #region Mensajes de Validación
        /// <summary>
        /// Mensaje genérico para campos requeridos
        /// </summary>
        public const string CAMPO_REQUERIDO = "Este campo es requerido";

        /// <summary>
        /// Mensaje para formatos inválidos
        /// </summary>
        public const string FORMATO_INVALIDO = "El formato ingresado es inválido";

        /// <summary>
        /// Mensaje para teléfonos inválidos
        /// </summary>
        public const string TELEFONO_INVALIDO = "El número de teléfono debe tener 8 dígitos";

        /// <summary>
        /// Mensaje para correos inválidos
        /// </summary>
        public const string CORREO_INVALIDO = "El formato del correo electrónico es inválido";

        /// <summary>
        /// Mensaje para contraseñas débiles
        /// </summary>
        public const string CONTRASENA_DEBIL = "La contraseña debe tener al menos 6 caracteres";

    /// <summary>
    /// Mensaje cuando falta una letra mayúscula en la contraseña
    /// </summary>
    public const string CONTRASENA_REQUIERE_MAYUSCULA = "La contraseña debe contener al menos una letra mayúscula";

    /// <summary>
    /// Mensaje cuando falta una letra minúscula en la contraseña
    /// </summary>
    public const string CONTRASENA_REQUIERE_MINUSCULA = "La contraseña debe contener al menos una letra minúscula";

    /// <summary>
    /// Mensaje cuando falta un carácter especial en la contraseña
    /// </summary>
    public const string CONTRASENA_REQUIERE_CARACTER_ESPECIAL = "La contraseña debe contener al menos un carácter especial";
        #endregion

        #region Mensajes de Operaciones CRUD
        /// <summary>
        /// Mensaje de éxito al guardar
        /// </summary>
        public const string GUARDADO_EXITOSO = "Los datos se guardaron correctamente";

        /// <summary>
        /// Mensaje de éxito al actualizar
        /// </summary>
        public const string ACTUALIZADO_EXITOSO = "Los datos se actualizaron correctamente";

        /// <summary>
        /// Mensaje de éxito al eliminar
        /// </summary>
        public const string ELIMINADO_EXITOSO = "El registro se eliminó correctamente";

        /// <summary>
        /// Mensaje de error al guardar
        /// </summary>
        public const string ERROR_GUARDAR = "Ocurrió un error al guardar los datos";

        /// <summary>
        /// Mensaje de error al actualizar
        /// </summary>
        public const string ERROR_ACTUALIZAR = "Ocurrió un error al actualizar los datos";

        /// <summary>
        /// Mensaje de error al eliminar
        /// </summary>
        public const string ERROR_ELIMINAR = "Ocurrió un error al eliminar el registro";

        /// <summary>
        /// Mensaje cuando no se encuentran registros
        /// </summary>
        public const string NO_SE_ENCONTRARON_REGISTROS = "No se encontraron registros";
        #endregion

        #region Mensajes de Autenticación
        /// <summary>
        /// Credenciales incorrectas
        /// </summary>
        public const string CREDENCIALES_INCORRECTAS = "Usuario o contraseña incorrectos";

        /// <summary>
        /// Usuario bloqueado
        /// </summary>
        public const string USUARIO_BLOQUEADO = "Usuario bloqueado. Contacte al administrador";

        /// <summary>
        /// Sesión expirada
        /// </summary>
        public const string SESION_EXPIRADA = "Su sesión ha expirado. Por favor, inicie sesión nuevamente";
        #endregion

        #region Configuración de Sesión
        /// <summary>
        /// Tiempo de expiración de sesión en minutos
        /// </summary>
        public const int SESSION_TIMEOUT_MINUTES = 30;

        /// <summary>
        /// Máximo de intentos de login
        /// </summary>
        public const int MAX_LOGIN_ATTEMPTS = 3;

        /// <summary>
        /// Longitud mínima de contraseña
        /// </summary>
        public const int MIN_PASSWORD_LENGTH = 6;
        #endregion

        #region Formato de Fechas
        /// <summary>
        /// Formato de fecha corto (dd/MM/yyyy)
        /// </summary>
        public const string FECHA_FORMATO = "dd/MM/yyyy";

        /// <summary>
        /// Formato de fecha y hora (dd/MM/yyyy HH:mm:ss)
        /// </summary>
        public const string FECHA_HORA_FORMATO = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Formato de hora (HH:mm)
        /// </summary>
        public const string HORA_FORMATO = "HH:mm";
        #endregion

        #region Configuración de Base de Datos
        /// <summary>
        /// Nombre de la cadena de conexión en App.config
        /// </summary>
        public const string CONNECTION_STRING_NAME = "GestionPedidosDB";
        #endregion

        #region Mensajes de Pedidos
        /// <summary>
        /// Stock insuficiente para procesar pedido
        /// </summary>
        public const string STOCK_INSUFICIENTE = "Stock insuficiente para completar el pedido";

        /// <summary>
        /// Pedido procesado exitosamente
        /// </summary>
        public const string PEDIDO_PROCESADO = "El pedido se procesó correctamente";

        /// <summary>
        /// Pedido cancelado
        /// </summary>
        public const string PEDIDO_CANCELADO = "El pedido ha sido cancelado";
        #endregion
    }
}
