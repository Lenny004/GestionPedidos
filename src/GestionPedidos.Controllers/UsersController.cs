using GestionPedidos.Common.Constants;
using GestionPedidos.Common.Security;
using GestionPedidos.Common.Services;
using GestionPedidos.Common.Validation;
using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.DTO;
using GestionPedidos.Models.Entities;
using GestionPedidos.Models.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionPedidos.Controllers
{
    public class UsersController
    {
        private readonly IUserRepository _userRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UsersController()
        {
            _userRepository = new UsuarioRepository();
        }

        public (bool Success, string Message, IEnumerable<UsersListDto> Users) ReadAll()
        {
            try
            {
                Logger.Debug("Solicitando listado completo de usuarios");

                var users = _userRepository.ReadAllUsers();

                if (users == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar usuarios");
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var userList = new List<UsersListDto>(users);

                if (userList.Count == 0)
                {
                    Logger.Info("No se encontraron usuarios registrados.");
                    return (true, AppConstants.NO_SE_ENCONTRARON_REGISTROS, userList);
                }

                Logger.Info($"Se recuperaron {userList.Count} usuarios del repositorio");
                return (true, "Usuarios recuperados correctamente.", userList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer registros en usuarios");
                return (false, "Error al leer registros en usuarios", null);
            }
        }

        public (bool Success, string Message, User User) ReadOne(int id)
        {
            try
            {
                Logger.Debug("ID de usuario solicitado: {id}", id);
                if (id <= 0) return (false, "ID de usuario inválido", null);

                var user = _userRepository.ReadOne(id);

                if (user == null)
                {
                    return (false, Messages.Usuarios.USUARIO_NO_ENCONTRADO, null);
                }

                return (true, "Usuario encontrado", user);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al obtener usuario ID {id}");
                return (false, $"Error al cargar el usuario: {ex.Message}", null);
            }
        }

        public (bool Success, string Message) Create(string userName, string password, string fullName, string email, int idRole)
        {
            try
            {
                Logger.Debug($"Intento de creación de usuario: {userName}");

                // Validar que haya sesión activa
                int currentUserId = SessionManager.UsuarioId;
                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de crear usuario sin sesión de usuario activa");
                    return (false, AppConstants.SESION_EXPIRADA);
                }

                // Validar nombre de usuario (máximo 50 caracteres según estándares)
                var userNameValidation = GeneralValidator.ValidateLengthRange(userName, 3, 50, "Nombre de usuario");
                if (!userNameValidation.IsValid)
                {
                    Logger.Warn($"Validación fallida en nombre de usuario: {userNameValidation.ErrorMessage}");
                    return (false, userNameValidation.ErrorMessage);
                }

                // Validar que el username no tenga espacios
                if (userName.Trim().Contains(" "))
                {
                    Logger.Warn($"Intento de crear usuario con espacios: {userName}");
                    return (false, "El nombre de usuario no puede contener espacios.");
                }

                // Validar contraseña
                if (!GeneralValidator.IsNotEmpty(password))
                {
                    Logger.Warn($"Intento de crear usuario sin contraseña para usuario: {userName}");
                    return (false, "La contraseña es requerida");
                }

                // Validar nombre completo (máximo 100 caracteres según estándares)
                var fullNameValidation = GeneralValidator.ValidateLengthRange(fullName, 3, 100, "Nombre completo");
                if (!fullNameValidation.IsValid)
                {
                    Logger.Warn($"Validación fallida en nombre completo: {fullNameValidation.ErrorMessage}");
                    return (false, fullNameValidation.ErrorMessage);
                }

                // Validar rol
                if (idRole <= 0)
                {
                    Logger.Warn($"Intento de crear usuario sin rol válido para usuario: {userName}");
                    return (false, "Debe seleccionar un rol válido");
                }

                // Validar formato de correo según RFC 5321 (máximo 254 caracteres)
                if (!string.IsNullOrWhiteSpace(email))
                {
                    if (!GeneralValidator.ValidateEmailRFC(email, 254))
                    {
                        Logger.Warn($"Intento de crear usuario con email inválido: {email}");
                        return (false, "El formato del correo electrónico es inválido o excede 254 caracteres.");
                    }
                }

                // Validar fortaleza de contraseña
                string passwordValidationMessage = GeneralValidator.ValidatePasswordStrength(password);

                if (!string.IsNullOrEmpty(passwordValidationMessage))
                {
                    Logger.Warn($"Contraseña débil en creación de usuario: {userName}. Errores: {passwordValidationMessage.Replace('\n', ' ')}");
                    return (false, passwordValidationMessage);
                }

                // Verificar si el usuario o email ya existe
                if (_userRepository.ExisteUsuario(userName, email))
                {
                    Logger.Warn($"Intento de crear usuario con nombre/email existente: {userName}");
                    return (false, Messages.Usuarios.USUARIO_YA_EXISTE);
                }

                // Crear nuevo usuario
                User nuevoUsuario = new User
                {
                    Username = userName.Trim(),
                    PasswordHash = PasswordHelper.HashPassword(password),
                    FullName = fullName.Trim(),
                    Email = email?.Trim(),
                    IdRole = idRole,
                    Rol = new Rol { IdRole = idRole },
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                bool created = _userRepository.CrearUsuario(nuevoUsuario);

                if (!created)
                {
                    Logger.Warn("No se pudo crear el usuario: {userName}", nuevoUsuario.Username);
                    return (false, AppConstants.ERROR_GUARDAR);
                }

                Logger.Info("Usuario creado exitosamente: {userName} ({fullName}) - Rol ID: {idRole}", nuevoUsuario.Username, nuevoUsuario.FullName, idRole);
                return (true, Messages.Usuarios.USUARIO_CREADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al crear usuario: {userName}");
                return (false, $"{AppConstants.ERROR_GUARDAR}: {ex.Message}");
            }
        }

        public (bool Success, string Message) Update(int id, string fullName, string email, int idRole, bool isActive)
        {
            try
            {
                // Validar ID
                if (id <= 0)
                {
                    Logger.Warn("Intento de actualizar usuario con ID inválido");
                    return (false, "ID de usuario inválido");
                }

                // Validar nombre completo (máximo 100 caracteres según estándares)
                var fullNameValidation = GeneralValidator.ValidateLengthRange(fullName, 3, 100, "Nombre completo");
                if (!fullNameValidation.IsValid)
                {
                    Logger.Warn($"Validación fallida en nombre completo: {fullNameValidation.ErrorMessage}");
                    return (false, fullNameValidation.ErrorMessage);
                }

                // Validar rol
                if (idRole <= 0)
                {
                    Logger.Warn("Intento de actualizar usuario sin rol válido");
                    return (false, "Debe seleccionar un rol válido");
                }

                // Validar email según RFC 5321 (máximo 254 caracteres)
                if (!string.IsNullOrWhiteSpace(email))
                {
                    if (!GeneralValidator.ValidateEmailRFC(email, 254))
                    {
                        Logger.Warn($"Intento de actualizar usuario con email inválido: {email}");
                        return (false, "El formato del correo electrónico es inválido o excede 254 caracteres.");
                    }
                }

                var user = new User
                {
                    IdUser = id,
                    FullName = fullName.Trim(),
                    Email = email?.Trim(),
                    IdRole = idRole,
                    IsActive = isActive
                };

                bool updated = _userRepository.Update(user);

                if (!updated)
                {
                    Logger.Warn($"No se pudo actualizar el usuario ID: {id}");
                    return (false, AppConstants.ERROR_ACTUALIZAR);
                }

                Logger.Info($"Usuario actualizado exitosamente ID: {id}");
                return (true, Messages.Usuarios.USUARIO_ACTUALIZADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al actualizar usuario");
                return (false, $"{AppConstants.ERROR_ACTUALIZAR}: {ex.Message}");
            }
        }

        public (bool Success, string Message) Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Logger.Warn("Intento de eliminar usuario con ID inválido");
                    return (false, "ID de usuario inválido");
                }

                int currentUserId = SessionManager.UsuarioId;

                if (currentUserId <= 0)
                {
                    Logger.Warn("Intento de eliminar usuario sin sesión de usuario activa");
                    return (false, AppConstants.SESION_EXPIRADA);
                }

                // Evitar que un usuario se elimine a sí mismo
                if (id == currentUserId)
                {
                    Logger.Warn("Intento de auto-eliminación de usuario ID: {id}", id);
                    return (false, "No puede eliminar su propia cuenta");
                }

                bool deleted = _userRepository.Delete(id, currentUserId);

                if (!deleted)
                {
                    Logger.Warn("No se pudo eliminar el usuario con ID: {id}", id);
                    return (false, AppConstants.ERROR_ELIMINAR);
                }

                Logger.Info("Usuario eliminado exitosamente. ID: {id}", id);
                return (true, Messages.Usuarios.USUARIO_ELIMINADO);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al eliminar usuario ID: {id}");
                return (false, $"{AppConstants.ERROR_ELIMINAR}: {ex.Message}");
            }
        }

        public (bool Success, string Message, IEnumerable<UsersListDto> Users) SearchByName(string name)
        {
            try
            {
                Logger.Debug("Buscando usuarios por nombre: {name}", name);
                
                if (!GeneralValidator.IsNotEmpty(name))
                {
                    Logger.Warn("Intento de búsqueda de usuarios con nombre vacío");
                    return (false, AppConstants.CAMPO_REQUERIDO, null);
                }
                
                var users = _userRepository.SearchUsers(name);

                if (users == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar usuarios");
                    return (false, AppConstants.NO_SE_ENCONTRARON_REGISTROS, null);
                }

                var userList = new List<UsersListDto>(users);

                if (userList.Count == 0)
                {
                    Logger.Info("No se encontraron usuarios registrados.");
                    return (true, AppConstants.NO_SE_ENCONTRARON_REGISTROS, userList);
                }

                Logger.Info($"Se recuperaron {userList.Count} usuarios del repositorio");
                return (true, "Usuarios recuperados correctamente.", userList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error al buscar usuarios por nombre: {name}");
                return (false, $"Error al buscar usuarios: {ex.Message}", null);
            }
        }

        public List<object> GetUserStatuses()
        {
            try
            {
                return Enum.GetValues(typeof(EstadoUsuario))
                        .OfType<EstadoUsuario>()
                        .Select(e => new
                        {
                            Text = e.ToString(),
                            Value = (byte)e
                        })
                        .ToList<object>();
            }
            catch (Exception ex)
            {
                // Puedes loguear el error aquí si tienes un sistema de logging
                throw new Exception($"Error al obtener los estados del producto: {ex.Message}", ex);
            }
        }
    }
}