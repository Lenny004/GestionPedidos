using GestionPedidos.DataAccess.Interfaces;
using GestionPedidos.DataAccess.Repositories;
using GestionPedidos.Models.Entities;
using NLog;
using System;
using System.Collections.Generic;

namespace GestionPedidos.Controllers
{
    public class RoleController
    {
        private readonly IRoleRepository _roleRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RoleController()
        {
            _roleRepository = new RoleRepository();
        }

        /// <summary>
        /// Obtiene todos los roles activos para cargar en ComboBox
        /// </summary>
        public (bool Success, string Message, IEnumerable<Rol> Roles) GetAllRoles()
        {
            try
            {
                Logger.Debug("Solicitando listado de roles");
                var roles = _roleRepository.ReadAllRoles();

                if (roles == null)
                {
                    Logger.Warn("El repositorio devolvió null al solicitar roles");
                    return (false, "No se pudieron cargar los roles", null);
                }

                var roleList = new List<Rol>(roles);

                if (roleList.Count == 0)
                {
                    Logger.Warn("No se encontraron roles activos en el sistema");
                    return (false, "No hay roles disponibles", new List<Rol>());
                }

                Logger.Info($"Se recuperaron {roleList.Count} roles del repositorio");
                return (true, "Roles cargados correctamente", roleList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error al leer roles");
                return (false, $"Error al cargar roles: {ex.Message}", null);
            }
        }
    }
}
