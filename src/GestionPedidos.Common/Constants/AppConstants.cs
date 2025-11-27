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

        #region Longitudes de Campos según Estándares
        // Usuarios
        /// <summary>
        /// Longitud máxima del nombre de usuario (50 según estándares corporativos)
        /// </summary>
        public const int MAX_USERNAME_LENGTH = 50;

        /// <summary>
        /// Longitud mínima del nombre de usuario
        /// </summary>
        public const int MIN_USERNAME_LENGTH = 3;

        /// <summary>
        /// Longitud máxima del nombre completo (100 caracteres)
        /// </summary>
        public const int MAX_FULLNAME_LENGTH = 100;

        /// <summary>
        /// Longitud máxima del email según RFC 5321 (254 caracteres)
        /// </summary>
        public const int MAX_EMAIL_LENGTH = 254;

        /// <summary>
        /// Longitud máxima del hash de contraseña (255 para SHA-256 con sal)
        /// </summary>
        public const int MAX_PASSWORD_HASH_LENGTH = 255;

        // Clientes
        /// <summary>
        /// Longitud máxima del nombre del cliente (50 caracteres)
        /// </summary>
        public const int MAX_CUSTOMER_FIRSTNAME_LENGTH = 50;

        /// <summary>
        /// Longitud máxima del apellido del cliente (50 caracteres)
        /// </summary>
        public const int MAX_CUSTOMER_LASTNAME_LENGTH = 50;

        /// <summary>
        /// Longitud máxima del teléfono según ITU-T (20 caracteres)
        /// </summary>
        public const int MAX_PHONE_LENGTH = 20;

        /// <summary>
        /// Longitud mínima del teléfono (8 dígitos)
        /// </summary>
        public const int MIN_PHONE_LENGTH = 8;

        /// <summary>
        /// Longitud máxima de la dirección (255 caracteres)
        /// </summary>
        public const int MAX_ADDRESS_LENGTH = 255;

        // Productos
        /// <summary>
        /// Longitud máxima del nombre del producto (100 caracteres)
        /// </summary>
        public const int MAX_PRODUCT_NAME_LENGTH = 100;

        /// <summary>
        /// Longitud máxima de la descripción del producto (500 caracteres)
        /// </summary>
        public const int MAX_PRODUCT_DESCRIPTION_LENGTH = 500;

        /// <summary>
        /// Precio máximo permitido (DECIMAL(10,2) = 99,999,999.99)
        /// </summary>
        public const decimal MAX_PRICE = 99999999.99m;

        /// <summary>
        /// Precio mínimo permitido
        /// </summary>
        public const decimal MIN_PRICE = 0.01m;

        // Pedidos
        /// <summary>
        /// Longitud máxima de los comentarios del pedido (500 caracteres)
        /// </summary>
        public const int MAX_ORDER_COMMENTS_LENGTH = 500;

        /// <summary>
        /// Longitud máxima del estado del pedido (20 caracteres)
        /// </summary>
        public const int MAX_ORDER_STATUS_LENGTH = 20;

        // Catálogos
        /// <summary>
        /// Longitud máxima del nombre de rol (50 caracteres)
        /// </summary>
        public const int MAX_ROLE_NAME_LENGTH = 50;

        /// <summary>
        /// Longitud máxima de la descripción de rol (200 caracteres)
        /// </summary>
        public const int MAX_ROLE_DESCRIPTION_LENGTH = 200;

        /// <summary>
        /// Longitud máxima del nombre de departamento (100 caracteres)
        /// </summary>
        public const int MAX_DEPARTMENT_NAME_LENGTH = 100;

        /// <summary>
        /// Longitud máxima del nombre de ciudad (100 caracteres)
        /// </summary>
        public const int MAX_CITY_NAME_LENGTH = 100;

        // Historial
        /// <summary>
        /// Longitud máxima del nombre de tabla afectada (50 caracteres)
        /// </summary>
        public const int MAX_HISTORY_TABLE_LENGTH = 50;

        /// <summary>
        /// Longitud máxima del tipo de acción (20 caracteres)
        /// </summary>
        public const int MAX_HISTORY_ACTION_LENGTH = 20;

        /// <summary>
        /// Longitud máxima de la descripción del historial (500 caracteres)
        /// </summary>
        public const int MAX_HISTORY_DESCRIPTION_LENGTH = 500;

        // Umbrales de Stock
        /// <summary>
        /// Umbral de stock bajo (10 unidades)
        /// </summary>
        public const int LOW_STOCK_THRESHOLD = 10;

        /// <summary>
        /// Umbral de stock crítico (5 unidades)
        /// </summary>
        public const int CRITICAL_STOCK_THRESHOLD = 5;
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
