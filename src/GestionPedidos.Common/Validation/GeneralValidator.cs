using System;
using System.Linq;
using System.Text.RegularExpressions;
using GestionPedidos.Common.Constants;

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
        /// Valida que la contraseña cumpla con los requisitos mínimos y retorna el motivo si no los cumple.
        /// </summary>
        public static (bool IsValid, string ErrorMessage) ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, AppConstants.CONTRASENA_DEBIL);

            if (password.Length < AppConstants.MIN_PASSWORD_LENGTH)
                return (false, AppConstants.CONTRASENA_DEBIL);

            if (!password.Any(char.IsUpper))
                return (false, AppConstants.CONTRASENA_REQUIERE_MAYUSCULA);

            if (!password.Any(char.IsLower))
                return (false, AppConstants.CONTRASENA_REQUIERE_MINUSCULA);

            if (!password.Any(character => !char.IsLetterOrDigit(character)))
                return (false, AppConstants.CONTRASENA_REQUIERE_CARACTER_ESPECIAL);

            return (true, string.Empty);
        }

        /// <summary>
        /// Valida si el campo de texto solo contiene letras y espacios.
        /// </summary>
        public static bool ValidateOnlyLetters(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string pattern = @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$";
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