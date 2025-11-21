using System;
using System.Collections.Generic;
using System.Linq;
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
        public static string ValidatePasswordStrength(string password)
        {
            var errors = new List<string>();

            // Definición de caracteres especiales
            // Esta expresión incluye caracteres que no son letras ni dígitos
            var specialChars = new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '}', '[', ']', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/' };

            // 1. Regla: Mínimo 6 caracteres
            if (password.Length < 6)
            {
                errors.Add("Debe tener al menos 6 caracteres.");
            }

            // 2. Regla: Al menos una letra mayúscula
            if (!password.Any(char.IsUpper))
            {
                errors.Add("Debe contener al menos una letra mayúscula.");
            }

            // 3. Regla: Al menos una letra minúscula
            if (!password.Any(char.IsLower))
            {
                errors.Add("Debe contener al menos una letra minúscula.");
            }

            // 4. Regla: Al menos un dígito (número)
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Debe contener al menos un número.");
            }

            // 5. Regla: Al menos un símbolo o carácter especial <<< ¡NUEVA REGLA!
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors.Add("Debe contener al menos un símbolo o carácter especial.");
            }

            // Unir todos los errores
            if (errors.Count > 0)
            {
                return "La contraseña no cumple con los requisitos:\n- " + string.Join("\n- ", errors);
            }

            return string.Empty; // Devuelve cadena vacía si es válida
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