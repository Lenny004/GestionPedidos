using System;
using System.Security.Cryptography;
using System.Text;
using GestionPedidos.Common.Validation;

namespace GestionPedidos.Common.Security
{
    /// <summary>
    /// Clase helper para el manejo seguro de contraseñas
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Genera un hash SHA256 de la contraseña
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash de la contraseña en formato hexadecimal</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir la contraseña a bytes
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Calcular el hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Convertir el hash a string hexadecimal
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Verifica si una contraseña coincide con su hash
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="hash">Hash almacenado</param>
        /// <returns>True si la contraseña es correcta, False en caso contrario</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            string hashOfInput = HashPassword(password);
            return string.Equals(hashOfInput, hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Valida la fortaleza de una contraseña
        /// </summary>
        /// <param name="password">Contraseña a validar</param>
        /// <returns>True si la contraseña cumple los requisitos mínimos</returns>
        public static bool ValidatePasswordStrength(string password)
        {
            var (isValid, _) = GeneralValidator.ValidatePasswordStrength(password);
            return isValid;
        }

        /// <summary>
        /// Genera una contraseña aleatoria
        /// </summary>
        /// <param name="length">Longitud de la contraseña (por defecto 8)</param>
        /// <returns>Contraseña generada</returns>
        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }
    }
}