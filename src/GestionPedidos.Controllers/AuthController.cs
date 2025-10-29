using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.Enums;
using GestionPedidos.Models.Entities;
using System;
using static GestionPedidos.Common.Constants.Messages;
using GestionPedidos.Common.Validation;
using NLog;

namespace GestionPedidos.Controllers
{
    public class AuthController
    {
        private readonly IUserRepository _usuarioRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AuthController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        /// <summary>
        /// Realiza el login del usuario
        /// </summary>
        public (bool Success, string Message, User Usuario) Login(string nombreUsuario, string contraseña)
        {
            try
            {
                Logger.Debug($"Intento de login para usuario: {nombreUsuario}");

                // Validaciones de entrada (NULL/VACÍO)
                if (!GeneralValidator.IsNotEmpty(nombreUsuario))
                {
                    Logger.Warn("Intento de login sin nombre de usuario");
                    return (false, "El nombre de usuario es requerido", null);
                }

                if (!GeneralValidator.IsNotEmpty(contraseña))
                {
                    Logger.Warn($"Intento de login sin contraseña para usuario: {nombreUsuario}");
                    return (false, "La contraseña es requerida", null);
                }

                // Eliminar espacios
                nombreUsuario = nombreUsuario.Trim();

                // Validar longitud mínima de contraseña
                if (contraseña.Length < AppConstants.MIN_PASSWORD_LENGTH)
                {
                    Logger.Warn($"Contraseña muy corta para usuario: {nombreUsuario}");
                    return (false, AppConstants.CONTRASENA_DEBIL, null);
                }

                // Hashear contraseña
                string contraseñaHash = PasswordHelper.HashPassword(contraseña);

                // Autenticar contraseña en la base de datos
                User usuario = _usuarioRepository.Autenticar(nombreUsuario, contraseñaHash);

                if (usuario == null)
                {
                    Logger.Warn($"Credenciales incorrectas para usuario: {nombreUsuario}");
                    return (false, AppConstants.CREDENCIALES_INCORRECTAS, null);
                }

                // Verificar estado del usuario
                if (!usuario.IsActive)
                {
                    Logger.Warn($"Intento de login con usuario bloqueado: {nombreUsuario}");
                    return (false, AppConstants.USUARIO_BLOQUEADO, null);
                }

                // Determinar rol
                TipoRoles rol = (usuario.Rol != null && usuario.Rol.IdRole == 1)
                    ? TipoRoles.Administrador
                    : TipoRoles.Operador;

                // Crear sesión
                SessionManager.Login(usuario.IdUser, usuario.Username, usuario.FullName, rol);

                // Retornar éxito
                Logger.Info($"Login exitoso - Usuario: {nombreUsuario} ({usuario.FullName}) - Rol: {rol}");
                return (true, $"Bienvenido {usuario.FullName}", usuario);
            }
            catch (Exception ex)
            {
                // El logger ayuda a registrar los errores en un archivo
                Logger.Error(ex, $"Error inesperado en Login para usuario: {nombreUsuario}");
                return (false, "Error al iniciar sesión. Intente nuevamente.", null);
            }
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        public (bool Success, string Message) Registrar(string nombreUsuario, string contraseña,
            string nombreCompleto, string correo, int idRol = 2)
        {
            try
            {
                Logger.Debug($"Intento de registro para usuario: {nombreUsuario}");

                // Validaciones completas en el controlador
                if (!GeneralValidator.IsNotEmpty(nombreUsuario))
                {
                    Logger.Warn("Intento de registro sin nombre de usuario");
                    return (false, "El nombre de usuario es requerido");
                }

                if (!GeneralValidator.IsNotEmpty(contraseña))
                {
                    Logger.Warn($"Intento de registro sin contraseña para usuario: {nombreUsuario}");
                    return (false, "La contraseña es requerida");
                }

                if (!GeneralValidator.IsNotEmpty(nombreCompleto))
                {
                    Logger.Warn($"Intento de registro sin nombre completo para usuario: {nombreUsuario}");
                    return (false, "El nombre completo es requerido");
                }

                // Validar formato de correo solo si se proporciona
                if (!string.IsNullOrWhiteSpace(correo) && !GeneralValidator.ValidateEmail(correo))
                {
                    Logger.Warn($"Formato de correo inválido en registro: {correo}");
                    return (false, AppConstants.CORREO_INVALIDO);
                }

                // Validar fortaleza de contraseña
                if (!GeneralValidator.ValidatePasswordStrength(contraseña))
                {
                    Logger.Warn($"Contraseña débil en registro para usuario: {nombreUsuario}");
                    return (false, AppConstants.CONTRASENA_DEBIL);
                }

                // Verificar si el usuario ya existe
                if (_usuarioRepository.ExisteUsuario(nombreUsuario))
                {
                    Logger.Warn($"Intento de registro con usuario existente: {nombreUsuario}");
                    return (false, Messages.Usuarios.USUARIO_YA_EXISTE);
                }

                // Crear nuevo usuario
                User nuevoUsuario = new User
                {
                    Username = nombreUsuario.Trim(),
                    PasswordHash = PasswordHelper.HashPassword(contraseña),
                    FullName = nombreCompleto.Trim(),
                    Email = correo?.Trim(),
                    Rol = new Rol { IdRole = idRol },
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                bool resultado = _usuarioRepository.CrearUsuario(nuevoUsuario);

                if (resultado)
                {
                    Logger.Info($"Usuario registrado exitosamente: {nombreUsuario} ({nombreCompleto}) - Rol ID: {idRol}");
                    return (true, Messages.Usuarios.USUARIO_CREADO);
                }
                else
                {
                    Logger.Error($"No se pudo crear el usuario en la base de datos: {nombreUsuario}");
                    return (false, "No se pudo crear el usuario");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error inesperado en registro de usuario: {nombreUsuario}");
                return (false, "Error al registrar usuario. Intente nuevamente.");
            }
        }

        public void Logout()
        {
            string username = SessionManager.NombreUsuario;
            SessionManager.Logout();
            Logger.Info($"Usuario cerró sesión: {username}");
        }

        public bool IsUserLoggedIn() => SessionManager.IsLoggedIn;
    }
}