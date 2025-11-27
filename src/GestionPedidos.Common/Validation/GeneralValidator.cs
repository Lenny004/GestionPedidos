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
        /// Valida si una cadena no es nula, vac�a o solo contiene espacios.
        /// </summary>
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Valida si una cadena cumple con la longitud m�nima y m�xima.
        /// </summary>
        public static bool ValidateLength(string value, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.Length >= minLength && value.Length <= maxLength;
        }

        /// <summary>
        /// Valida si el formato del correo electr�nico es correcto.
        /// </summary>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex b�sico para validar formato de email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Valida la fortaleza de una contrase�a (m�nimo 8 caracteres, may�sculas, min�sculas, n�mero y s�mbolo).
        /// </summary>
        public static string ValidatePasswordStrength(string password)
        {
            var errors = new List<string>();

            // Definici�n de caracteres especiales
            // Esta expresi�n incluye caracteres que no son letras ni d�gitos
            var specialChars = new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=', '{', '}', '[', ']', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/' };

            // 1. Regla: M�nimo 6 caracteres
            if (password.Length < 6)
            {
                errors.Add("Debe tener al menos 6 caracteres.");
            }

            // 2. Regla: Al menos una letra may�scula
            if (!password.Any(char.IsUpper))
            {
                errors.Add("Debe contener al menos una letra may�scula.");
            }

            // 3. Regla: Al menos una letra min�scula
            if (!password.Any(char.IsLower))
            {
                errors.Add("Debe contener al menos una letra min�scula.");
            }

            // 4. Regla: Al menos un d�gito (n�mero)
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Debe contener al menos un n�mero.");
            }

            // 5. Regla: Al menos un s�mbolo o car�cter especial
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors.Add("Debe contener al menos un s�mbolo o car�cter especial.");
            }

            // Unir todos los errores
            if (errors.Count > 0)
            {
                return "La contrase�a no cumple con los requisitos:\n- " + string.Join("\n- ", errors);
            }

            return string.Empty; // Devuelve cadena vac�a si es v�lida
        }

        /// <summary>
        /// Valida si el campo de texto solo contiene letras y espacios.
        /// </summary>
        public static bool ValidateOnlyLetters(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$";
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

        #region Validaciones Adicionales Según Estándares

        /// <summary>
        /// Valida el formato y longitud de un número de teléfono (8-20 dígitos según ITU-T)
        /// </summary>
        /// <param name="phone">Número de teléfono a validar</param>
        /// <returns>True si es válido, False en caso contrario</returns>
        public static bool ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remover espacios, guiones y paréntesis para validar solo dígitos
            string cleanPhone = Regex.Replace(phone, @"[\s\-\(\)\+]", "");

            // Validar que solo contenga dígitos y tenga entre 8 y 20 caracteres
            return Regex.IsMatch(cleanPhone, @"^\d{8,20}$");
        }

        /// <summary>
        /// Valida que un número decimal sea positivo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es mayor que cero</returns>
        public static bool ValidatePositiveDecimal(decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// Valida que un número entero sea positivo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es mayor que cero</returns>
        public static bool ValidatePositiveInt(int value)
        {
            return value > 0;
        }

        /// <summary>
        /// Valida que un número entero no sea negativo (puede ser cero)
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es mayor o igual a cero</returns>
        public static bool ValidateNonNegativeInt(int value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Valida que un número decimal no sea negativo (puede ser cero)
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si es mayor o igual a cero</returns>
        public static bool ValidateNonNegativeDecimal(decimal value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Valida el formato de un número decimal con precisión específica
        /// </summary>
        /// <param name="value">Valor como cadena</param>
        /// <param name="maxDecimals">Máximo de decimales permitidos (por defecto 2)</param>
        /// <returns>True si el formato es válido</returns>
        public static bool ValidateDecimalFormat(string value, int maxDecimals = 2)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Patrón para validar decimales: permite números con hasta maxDecimals decimales
            string pattern = $@"^\d+(\.\d{{1,{maxDecimals}}})?$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// Valida que una fecha sea futura
        /// </summary>
        /// <param name="date">Fecha a validar</param>
        /// <returns>True si la fecha es posterior a hoy</returns>
        public static bool ValidateFutureDate(DateTime date)
        {
            return date.Date > DateTime.Now.Date;
        }

        /// <summary>
        /// Valida que una fecha sea pasada
        /// </summary>
        /// <param name="date">Fecha a validar</param>
        /// <returns>True si la fecha es anterior a hoy</returns>
        public static bool ValidatePastDate(DateTime date)
        {
            return date.Date < DateTime.Now.Date;
        }

        /// <summary>
        /// Valida que un rango de fechas sea válido (inicio antes de fin)
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>True si el rango es válido</returns>
        public static bool ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }

        /// <summary>
        /// Valida el formato de email según RFC 5321 (máximo 254 caracteres)
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <param name="maxLength">Longitud máxima (por defecto 254 según RFC 5321)</param>
        /// <returns>True si es válido</returns>
        public static bool ValidateEmailRFC(string email, int maxLength = 254)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            if (email.Length > maxLength)
                return false;

            // Regex mejorado para validar email según RFC 5321
            string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Valida que la longitud de una cadena esté dentro de un rango específico
        /// </summary>
        /// <param name="value">Cadena a validar</param>
        /// <param name="minLength">Longitud mínima</param>
        /// <param name="maxLength">Longitud máxima</param>
        /// <param name="fieldName">Nombre del campo (para mensajes de error)</param>
        /// <returns>Tupla con (IsValid, ErrorMessage)</returns>
        public static (bool IsValid, string ErrorMessage) ValidateLengthRange(string value, int minLength, int maxLength, string fieldName = "Campo")
        {
            if (string.IsNullOrWhiteSpace(value))
                return (false, $"{fieldName} es requerido.");

            int length = value.Trim().Length;

            if (length < minLength)
                return (false, $"{fieldName} debe tener al menos {minLength} caracteres.");

            if (length > maxLength)
                return (false, $"{fieldName} no puede exceder {maxLength} caracteres.");

            return (true, string.Empty);
        }

        /// <summary>
        /// Valida que una cadena solo contenga letras, números y espacios
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <returns>True si solo contiene caracteres alfanuméricos y espacios</returns>
        public static bool ValidateAlphanumeric(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return Regex.IsMatch(value, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ\s]+$");
        }

        /// <summary>
        /// Valida que un valor numérico esté dentro de un rango
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="min">Valor mínimo</param>
        /// <param name="max">Valor máximo</param>
        /// <returns>True si está dentro del rango</returns>
        public static bool ValidateRange(decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Valida que un valor entero esté dentro de un rango
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="min">Valor mínimo</param>
        /// <param name="max">Valor máximo</param>
        /// <returns>True si está dentro del rango</returns>
        public static bool ValidateRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        #endregion
    }
}