using System;
using System.Windows.Forms;
using GestionPedidos.Common.Configuration;
using GestionPedidos.DataAccess.Configuration;
using GestionPedidos.UI.Forms.Auth;
using GestionPedidos.UI.Forms.Main;

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

            // Verificar si es el primer uso del sistema
            var authController = new Controllers.AuthController();
            bool isFirstUse = authController.IsFirstUse();

            if (false)
            {
                // Mostrar mensaje de bienvenida
                MessageBox.Show(
                    "¡Bienvenido al Sistema de Gestión de Pedidos!\n\n" +
                    "Es la primera vez que se ejecuta la aplicación.\n" +
                    "Por favor, registre el usuario administrador principal.",
                    "Configuración Inicial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Mostrar formulario de primer uso (registro de admin)
                //using (var firstUseForm = new FrmRegister(isFirstUse: true))
                //{
                //    if (firstUseForm.ShowDialog() != DialogResult.OK)
                //    {
                //        MessageBox.Show(
                //            "Debe crear un usuario administrador para usar el sistema.",
                //            "Configuración Requerida",
                //            MessageBoxButtons.OK,
                //            MessageBoxIcon.Warning);
                //        return; // Salir si no completa el registro
                //    }
                //}

                // Después de crear el admin, mostrar login
                MessageBox.Show(
                    "Administrador creado exitosamente.\n\n" +
                    "Ahora puede iniciar sesión con sus credenciales.",
                    "Configuración Completa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            // Mostrar Login
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return; // Canceló o falló login
            }

            // Ya no es necesario Application.Run porque ShowDialog ya maneja el loop
        }
    }
}