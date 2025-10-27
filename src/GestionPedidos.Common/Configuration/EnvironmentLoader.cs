using System;
using System.Collections.Generic;
using System.IO;

namespace GestionPedidos.Common.Configuration
{
    /// <summary>
    /// Carga y gestiona variables de entorno desde archivo .env
    /// </summary>
    public static class EnvironmentLoader
    {
        private static Dictionary<string, string> _variables = new Dictionary<string, string>();
        private static bool _loaded = false;

        /// <summary>
        /// Carga las variables de entorno desde el archivo .env
        /// </summary>
        public static void Load()
        {
            if (_loaded)
                return;

            try
            {
                // Buscar el archivo .env en diferentes ubicaciones
                string envPath = FindEnvFile();

                if (string.IsNullOrEmpty(envPath))
                {
                    throw new FileNotFoundException("No se encontró el archivo .env. Asegúrate de crear uno basado en .env.example");
                }

                // Leer y procesar el archivo
                string[] lines = File.ReadAllLines(envPath);

                foreach (string line in lines)
                {
                    // Ignorar líneas vacías o comentarios
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                        continue;
                    // Separar clave y valor
                    int separatorIndex = line.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        string key = line.Substring(0, separatorIndex).Trim();
                        string value = line.Substring(separatorIndex + 1).Trim();

                        // Remover comillas si existen
                        if (value.StartsWith("\"") && value.EndsWith("\""))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }

                        _variables[key] = value;
                    }
                }

                _loaded = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar el archivo .env: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca el archivo .env en diferentes ubicaciones
        /// </summary>
        private static string FindEnvFile()
        {
            // 1. Directorio actual de la aplicación
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            string envPath = Path.Combine(currentDir, ".env");
            if (File.Exists(envPath))
                return envPath;

            // 2. Directorio de la solución (subir desde bin\Debug)
            DirectoryInfo dirInfo = new DirectoryInfo(currentDir);
            while (dirInfo != null && dirInfo.Parent != null)
            {
                envPath = Path.Combine(dirInfo.FullName, ".env");
                if (File.Exists(envPath))
                    return envPath;

                // Verificar si existe un archivo .sln (indica que es la raíz)
                if (Directory.GetFiles(dirInfo.FullName, "*.sln").Length > 0)
                {
                    envPath = Path.Combine(dirInfo.FullName, ".env");
                    if (File.Exists(envPath))
                        return envPath;
                    break;
                }

                dirInfo = dirInfo.Parent;
            }
            return null;
        }

        /// <summary>
        /// Obtiene el valor de una variable de entorno
        /// </summary>
        /// <param name="key">Nombre de la variable</param>
        /// <returns>Valor de la variable</returns>
        public static string GetVariable(string key)
        {
            if (!_loaded)
                Load();

            if (_variables.ContainsKey(key))
                return _variables[key];

            return null;
        }

        /// <summary>
        /// Obtiene el valor de una variable de entorno con un valor por defecto
        /// </summary>
        /// <param name="key">Nombre de la variable</param>
        /// <param name="defaultValue">Valor por defecto si no existe</param>
        /// <returns>Valor de la variable o el valor por defecto</returns>
        public static string GetVariable(string key, string defaultValue)
        {
            string value = GetVariable(key);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Obtiene el valor de una variable como entero
        /// </summary>
        public static int GetInt(string key, int defaultValue = 0)
        {
            string value = GetVariable(key);
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Obtiene el valor de una variable como booleano
        /// </summary>
        public static bool GetBool(string key, bool defaultValue = false)
        {
            string value = GetVariable(key);
            if (bool.TryParse(value, out bool result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Verifica si una variable existe
        /// </summary>
        public static bool HasVariable(string key)
        {
            if (!_loaded)
                Load();

            return _variables.ContainsKey(key);
        }

        /// <summary>
        /// Obtiene la cadena de conexión construida desde las variables de entorno
        /// </summary>
        public static string GetConnectionString()
        {
            if (!_loaded)
                Load();

            string server = GetVariable("DB_SERVER");
            string database = GetVariable("DB_NAME");
            string integratedSecurity = GetVariable("DB_INTEGRATED_SECURITY", "True");
            string trustCertificate = GetVariable("DB_TRUST_SERVER_CERTIFICATE", "True");

            // Verificar si usa autenticación SQL
            string user = GetVariable("DB_USER");
            string password = GetVariable("DB_PASSWORD");

            string connectionString;

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                // Autenticación SQL
                connectionString = $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate={trustCertificate};";
            }
            else
            {
                // Autenticación Windows (Integrated Security)
                connectionString = $"Server={server};Database={database};Integrated Security={integratedSecurity};TrustServerCertificate={trustCertificate};";
            }

            return connectionString;
        }
    }
}
