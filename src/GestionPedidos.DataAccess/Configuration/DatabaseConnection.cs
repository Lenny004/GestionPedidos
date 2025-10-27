using System;
using System.Configuration;
using System.Data.SqlClient;
using GestionPedidos.Common.Configuration;

namespace GestionPedidos.DataAccess.Configuration
{
    /// <summary>
    /// Gestiona la conexión a la base de datos SQL Server
    /// </summary>
    public static class DatabaseConnection
    {
        private static string _connectionString;

        /// <summary>
        /// Obtiene la cadena de conexión desde variables de entorno (.env)
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        // Intentar obtener desde variables de entorno (.env)
                        _connectionString = EnvironmentLoader.GetConnectionString();
                    }
                    catch (Exception ex)
                    {
                        // falla explícitamente
                        throw new InvalidOperationException(
                            "No se pudo cargar la cadena de conexión desde el archivo .env. " +
                            "Verifica que existan DB_SERVER, DB_NAME, DB_USER/DB_PASSWORD o DB_INTEGRATED_SECURITY.",
                            ex);
                    }
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        throw new InvalidOperationException("No se encontró la cadena de conexión en el archivo .env.");
                    }
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// Crea y retorna una nueva conexión SQL
        /// </summary>
        /// <returns>Nueva instancia de SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Prueba la conexión y retorna el resultado con mensaje detallado
        /// </summary>
        /// <returns>Tupla con el resultado y mensaje de la operación</returns>
        public static (bool Success, string Message) TestConnectionWithMessage()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        // Obtener información del servidor
                        string serverVersion = conn.ServerVersion;
                        string database = conn.Database;

                        return (true, $"Conexión exitosa a la base de datos '{database}'\nServidor: {serverVersion}");
                    }
                    else
                    {
                        return (false, "No se pudo establecer la conexión");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Errores específicos de SQL Server
                string errorMessage;
                switch (sqlEx.Number)
                {
                    case -1:
                        errorMessage = "No se pudo conectar al servidor. Verifique que SQL Server esté ejecutándose.";
                        break;
                    case -2:
                        errorMessage = "Tiempo de espera agotado. El servidor no responde.";
                        break;
                    case 4060:
                        errorMessage = "No se puede abrir la base de datos solicitada.";
                        break;
                    case 18456:
                        errorMessage = "Error de autenticación. Usuario o contraseña incorrectos.";
                        break;
                    default:
                        errorMessage = $"Error SQL {sqlEx.Number}: {sqlEx.Message}";
                        break;
                }
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                return (false, $"Error inesperado: {ex.Message}");
            }
        }
    }
}