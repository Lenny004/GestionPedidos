using System;
using GestionPedidos.Models.Enums;

namespace GestionPedidos.Common.Security
{
    /// <summary>
    /// Gestiona la sesión del usuario actual
    /// </summary>
    public static class SessionManager
    {
        private static int? _usuarioId;
        private static string _nombreUsuario;
        private static string _nombreCompleto;
        private static TipoRoles? _rol;
        private static DateTime? _loginTime;

        /// <summary>
        /// ID del usuario actual
        /// </summary>
        public static int UsuarioId
        {
            get => _usuarioId ?? 0;
            private set => _usuarioId = value;
        }

        /// <summary>
        /// Nombre de usuario (username)
        /// </summary>
        public static string NombreUsuario
        {
            get => _nombreUsuario ?? string.Empty;
            private set => _nombreUsuario = value;
        }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public static string NombreCompleto
        {
            get => _nombreCompleto ?? string.Empty;
            private set => _nombreCompleto = value;
        }

        /// <summary>
        /// Rol del usuario actual
        /// </summary>
        public static TipoRoles Rol
        {
            get => _rol ?? TipoRoles.Operador;
            private set => _rol = value;
        }

        /// <summary>
        /// Hora de inicio de sesión
        /// </summary>
        public static DateTime LoginTime
        {
            get => _loginTime ?? DateTime.MinValue;
            private set => _loginTime = value;
        }

        /// <summary>
        /// Indica si hay una sesión activa
        /// </summary>
        public static bool IsLoggedIn => _usuarioId.HasValue;

        /// <summary>
        /// Indica si el usuario actual es administrador
        /// </summary>
        public static bool IsAdmin => IsLoggedIn && Rol == TipoRoles.Administrador;

        /// <summary>
        /// Inicia una nueva sesión de usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="nombreUsuario">Nombre de usuario</param>
        /// <param name="nombreCompleto">Nombre completo</param>
        /// <param name="rol">Rol del usuario</param>
        public static void Login(int usuarioId, string nombreUsuario, string nombreCompleto, TipoRoles rol)
        {
            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
            NombreCompleto = nombreCompleto;
            Rol = rol;
            LoginTime = DateTime.Now;
        }

        /// <summary>
        /// Cierra la sesión actual
        /// </summary>
        public static void Logout()
        {
            _usuarioId = null;
            _nombreUsuario = null;
            _nombreCompleto = null;
            _rol = null;
            _loginTime = null;
        }
    }
}