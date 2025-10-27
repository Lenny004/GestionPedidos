using GestionPedidos.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Autentica un usuario en el sistema
        /// </summary>
        /// <param name="userName">Nombre del usuario</param>
        /// <param name="passwordHash">Contraseña</param>
        /// <returns>Usuario si las credenciales son correctas, null sino</returns>
        User Autenticar(string userName, string passwordHash);

        /// <summary>
        /// Verifica si un nombre de usuario ya existe
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>True si existe, False si no</returns>
        bool ExisteUsuario(string userName);

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True si se creo correctamente, False sino</returns>
        bool CrearUsuario(User user);
    }
}
