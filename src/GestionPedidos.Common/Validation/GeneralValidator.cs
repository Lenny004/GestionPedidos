using System;
using System.Text.RegularExpressions;

namespace GestionPedidos.Common.Validation
{
    /// <summary>
    /// Contiene validaciones generales reutilizables para los controladores y servicios.
    /// </summary>
    public static class GeneralValidator
    {
        /// <summary>
        /// Valida si una cadena no es nula, vacía o solo contiene espacios.
        /// </summary>
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Valida si una cadena cumple con la longitud mínima y máxima.
        /// </summary>
        public static bool ValidateLength(string value, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.Length >= minLength && value.Length <= maxLength;
        }

        /// <summary>
        /// Valida si el formato del correo electrónico es correcto.
        /// </summary>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex básico para validar formato de email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Valida la fortaleza de una contraseña (mínimo 8 caracteres, mayúsculas, minúsculas, número y símbolo).
        /// </summary>
        public static bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Debe tener al menos una mayúscula, una minúscula, un número y un carácter especial
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        /// <summary>
        /// Valida si el campo de texto solo contiene letras y espacios.
        /// </summary>
        public static bool ValidateOnlyLetters(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// Valida si el campo de texto solo contiene dígitos numéricos.
        /// </summary>
        public static bool ValidateOnlyNumbers(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return Regex.IsMatch(value, @"^\d+$");
        }
    }
}