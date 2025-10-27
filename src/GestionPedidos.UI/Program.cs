using System;
using System.Windows.Forms;
using GestionPedidos.Common.Configuration;
using GestionPedidos.DataAccess.Configuration;

namespace GestionPedidos.UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Cargar variables de entorno desde el archivo .env
                EnvironmentLoader.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar la configuración:\n\n{ex.Message}\n\n" +
                    "Asegúrate de que el archivo .env existe en la raíz del proyecto.\n" +
                    "Puedes copiar .env.example y renombrarlo a .env",
                    "Error de Configuración",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mostrar splash screen o mensaje de inicio (opcional)
            Application.DoEvents();

            // Probar conexión a base de datos al iniciar
            var connectionResult = DatabaseConnection.TestConnectionWithMessage();
            bool success = connectionResult.Item1;
            string message = connectionResult.Item2;
            
            if (!success)
            {
                MessageBox.Show(
                    $"No se pudo conectar a la base de datos.\n\n{message}\n\n" +
                    "Verifique:\n" +
                    "1. Que SQL Server esté ejecutándose\n" +
                    "2. La cadena de conexión en el archivo .env\n" +
                    "3. Que la base de datos 'OrderManagementDB' exista",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                // Preguntar si desea continuar sin conexión (para desarrollo)
                var result = MessageBox.Show(
                    "¿Desea abrir la aplicación de todos modos?\n\n" +
                    "Nota: Las funcionalidades que requieren base de datos no estarán disponibles.",
                    "Continuar sin conexión",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.No)
                {
                    return; // Salir de la aplicación
                }
            }
            else
            {
                // Iniciar la aplicación
                Application.Run(new Form1());
            }
        }
    }
}