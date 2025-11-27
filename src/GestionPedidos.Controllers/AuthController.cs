using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.Common.Services;
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
                TipoRoles rol;

                if (usuario.TipoRol.HasValue)
                {
                    rol = usuario.TipoRol.Value;
                }
                else if (usuario.Rol != null && Enum.IsDefined(typeof(TipoRoles), usuario.Rol.IdRole))
                {
                    rol = (TipoRoles)usuario.Rol.IdRole;
                }
                else
                {
                    rol = TipoRoles.Operador;
                }

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
            string nombreCompleto, string correo, int idRol)
        {
            try
            {
                Logger.Debug($"Intento de registro para usuario: {nombreUsuario}");

                if (idRol <= 0)
                {
                    Logger.Warn($"Intento de registro sin rol válido para usuario: {nombreUsuario}");
                    return (false, "El rol seleccionado no es válido.");
                }

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
                string passwordValidationMessage = GeneralValidator.ValidatePasswordStrength(contraseña);

                if (!string.IsNullOrEmpty(passwordValidationMessage))
                {
                    Logger.Warn($"Contraseña débil en registro para usuario: {nombreUsuario}. Errores: {passwordValidationMessage.Replace('\n', ' ')}");

                    // Si la validación falla, retorna el mensaje detallado
                    return (false, passwordValidationMessage);
                }

                // Verificar si el usuario ya existe
                if (_usuarioRepository.ExisteUsuario(nombreUsuario, correo))
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
                    IdRole = idRol,
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

        public (bool Success, string Message) ResetPassword(string email, string newPassword, string confirmPassword)
        {
            // Validación de campos UI
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
            {
                return (false, "El email y la contraseña son obligatorios.");
            }

            // Validación de coincidencia de contraseñas
            if (newPassword != confirmPassword)
            {
                return (false, "La nueva contraseña y la confirmación no coinciden.");
            }
            
            // Validación de seguridad de la contraseña
            string passwordValidationMessage = GeneralValidator.ValidatePasswordStrength(newPassword);

            if (!string.IsNullOrEmpty(passwordValidationMessage))
            {
                return (false, passwordValidationMessage);
            }

            // Hashear la nueva contraseña
            string passwordHash = PasswordHelper.HashPassword(newPassword);

            // Validar que la nueva contraseña no sea igual a la actual
            if (_usuarioRepository.VerifyPassword(email, passwordHash))
            {
                Logger.Warn($"Intento de restablecer contraseña con la misma contraseña actual: {email}");
                return (false, "La nueva contraseña debe ser diferente a la contraseña actual.");
            }

            bool success = _usuarioRepository.UpdatePasswordByEmail(email, passwordHash);

            if (success)
            {
                return (true, "¡Contraseña actualizada con éxito!");
            }

            // Mensaje genérico por seguridad (no indicamos si el email no existe)
            return (false, "Error: No se pudo actualizar la contraseña. Verifique el correo o contacte a soporte.");
        }

        public bool IsUserLoggedIn() => SessionManager.IsLoggedIn;

        /// <summary>
        /// Verifica si es el primer uso del sistema (no hay usuarios registrados)
        /// </summary>
        public bool IsFirstUse()
        {
            try
            {
                return !_usuarioRepository.HasAnyUser();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al verificar primer uso del sistema");
                return false; // Por seguridad, asumimos que no es primer uso
            }
        }

        /// <summary>
        /// Registra el primer usuario administrador del sistema
        /// </summary>
        public (bool Success, string Message) RegistrarPrimerAdmin(string nombreUsuario, string contraseña,
            string nombreCompleto, string correo)
        {
            try
            {
                Logger.Debug($"Intento de registro de primer administrador: {nombreUsuario}");

                // Verificar que realmente sea primer uso
                if (!IsFirstUse())
                {
                    Logger.Warn("Intento de registro de primer admin cuando ya existen usuarios");
                    return (false, "Ya existen usuarios en el sistema. Use el proceso de registro normal.");
                }

                // Validaciones completas
                if (!GeneralValidator.IsNotEmpty(nombreUsuario))
                {
                    return (false, "El nombre de usuario es requerido");
                }

                if (!GeneralValidator.IsNotEmpty(contraseña))
                {
                    return (false, "La contraseña es requerida");
                }

                if (!GeneralValidator.IsNotEmpty(nombreCompleto))
                {
                    return (false, "El nombre completo es requerido");
                }

                // Validar formato de correo si se proporciona
                if (!string.IsNullOrWhiteSpace(correo) && !GeneralValidator.ValidateEmail(correo))
                {
                    return (false, AppConstants.CORREO_INVALIDO);
                }

                // Validar fortaleza de contraseña
                string passwordValidationMessage = GeneralValidator.ValidatePasswordStrength(contraseña);
                if (!string.IsNullOrEmpty(passwordValidationMessage))
                {
                    return (false, passwordValidationMessage);
                }

                // Crear primer administrador con idRole = 1
                User primerAdmin = new User
                {
                    Username = nombreUsuario.Trim(),
                    PasswordHash = PasswordHelper.HashPassword(contraseña),
                    FullName = nombreCompleto.Trim(),
                    Email = correo?.Trim(),
                    IdRole = 1, // Administrador
                    Rol = new Rol { IdRole = 1 },
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                bool resultado = _usuarioRepository.CrearUsuario(primerAdmin);

                if (resultado)
                {
                    Logger.Info($"Primer administrador registrado exitosamente: {nombreUsuario} ({nombreCompleto})");
                    return (true, "Primer administrador creado exitosamente. Ahora puede iniciar sesión.");
                }
                else
                {
                    Logger.Error($"No se pudo crear el primer administrador: {nombreUsuario}");
                    return (false, "No se pudo crear el administrador");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error inesperado al registrar primer admin: {nombreUsuario}");
                return (false, "Error al registrar administrador. Intente nuevamente.");
            }
        }
    }
}