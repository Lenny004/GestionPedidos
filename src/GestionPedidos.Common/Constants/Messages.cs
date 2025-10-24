using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPedidos.Common.Constants
{
    /// <summary>
    /// Mensajes del sistema organizados por módulo
    /// </summary>
    public static class Messages
    {
        #region Clientes
        public static class Clientes
        {
            public const string CLIENTE_GUARDADO = "Cliente guardado exitosamente";
            public const string CLIENTE_ACTUALIZADO = "Cliente actualizado exitosamente";
            public const string CLIENTE_ELIMINADO = "Cliente eliminado exitosamente";
            public const string CLIENTE_NO_ENCONTRADO = "Cliente no encontrado";
            public const string CLIENTE_TIENE_PEDIDOS = "No se puede eliminar el cliente porque tiene pedidos asociados";
        }
        #endregion

        #region Productos
        public static class Productos
        {
            public const string PRODUCTO_GUARDADO = "Producto guardado exitosamente";
            public const string PRODUCTO_ACTUALIZADO = "Producto actualizado exitosamente";
            public const string PRODUCTO_ELIMINADO = "Producto eliminado exitosamente";
            public const string PRODUCTO_NO_ENCONTRADO = "Producto no encontrado";
            public const string STOCK_BAJO = "¡Advertencia! El stock del producto está bajo";
            public const string STOCK_AGOTADO = "¡Alerta! El producto está agotado";
        }
        #endregion

        #region Pedidos
        public static class Pedidos
        {
            public const string PEDIDO_CREADO = "Pedido creado exitosamente";
            public const string PEDIDO_ACTUALIZADO = "Pedido actualizado exitosamente";
            public const string PEDIDO_CANCELADO = "Pedido cancelado exitosamente";
            public const string PEDIDO_ENTREGADO = "Pedido marcado como entregado";
            public const string PEDIDO_NO_ENCONTRADO = "Pedido no encontrado";
            public const string AGREGAR_PRODUCTOS = "Debe agregar al menos un producto al pedido";
        }
        #endregion

        #region Usuarios
        public static class Usuarios
        {
            public const string USUARIO_CREADO = "Usuario creado exitosamente";
            public const string USUARIO_ACTUALIZADO = "Usuario actualizado exitosamente";
            public const string USUARIO_ELIMINADO = "Usuario eliminado exitosamente";
            public const string USUARIO_NO_ENCONTRADO = "Usuario no encontrado";
            public const string USUARIO_YA_EXISTE = "El nombre de usuario ya existe";
        }
        #endregion

        #region Confirmaciones
        public static class Confirmaciones
        {
            public const string CONFIRMAR_ELIMINACION = "¿Está seguro que desea eliminar este registro?";
            public const string CONFIRMAR_CANCELACION = "¿Está seguro que desea cancelar? Se perderán los cambios no guardados";
            public const string CONFIRMAR_SALIR = "¿Está seguro que desea salir del sistema?";
            public const string CONFIRMAR_CANCELAR_PEDIDO = "¿Está seguro que desea cancelar este pedido?";
        }
        #endregion
    }
}
