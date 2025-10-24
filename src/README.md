# Sistema de Gestión de Pedidos

## ?? Configuración del Proyecto

Este proyecto utiliza variables de entorno para proteger las credenciales y configuraciones sensibles.

### Configuración Inicial

1. **Copiar el archivo de ejemplo**
   ```bash
   copy .env.example .env
   ```
   O manualmente: duplica `.env.example` y renómbralo a `.env`

2. **Editar el archivo `.env`** con tus credenciales reales:

   ```env
   # Configuración de Base de Datos
   DB_SERVER=localhost\SQLEXPRESS
   DB_NAME=GestionPedidosDB
   DB_INTEGRATED_SECURITY=True
   DB_TRUST_SERVER_CERTIFICATE=True
   ```

3. **Si usas autenticación SQL Server**, descomenta y configura:
   ```env
   DB_INTEGRATED_SECURITY=False
   DB_USER=tu_usuario
   DB_PASSWORD=tu_contraseña
   ```

### ?? Estructura de Archivos

```
GestionPedidos/
??? .env                    ? TUS credenciales (NO se sube a GitHub)
??? .env.example            ? Plantilla de ejemplo (SÍ se sube a GitHub)
??? .gitignore              ? Protege archivos sensibles
??? src/
    ??? GestionPedidos.UI/
    ??? GestionPedidos.Common/
    ?   ??? Configuration/
    ?       ??? EnvironmentLoader.cs
    ??? ...
```

### ?? Seguridad

- ? El archivo `.env` está en `.gitignore` y **NO se sube a GitHub**
- ? El archivo `.env.example` **SÍ se sube a GitHub** como plantilla
- ? Nunca subas credenciales reales al repositorio

### ?? Variables de Entorno Disponibles

#### Base de Datos
- `DB_SERVER`: Servidor SQL (ej: `localhost\SQLEXPRESS`)
- `DB_NAME`: Nombre de la base de datos
- `DB_INTEGRATED_SECURITY`: `True` para Windows Auth, `False` para SQL Auth
- `DB_TRUST_SERVER_CERTIFICATE`: `True` o `False`
- `DB_USER`: Usuario SQL (opcional, solo si `DB_INTEGRATED_SECURITY=False`)
- `DB_PASSWORD`: Contraseña SQL (opcional, solo si `DB_INTEGRATED_SECURITY=False`)

#### Configuración de la Aplicación
- `APP_NAME`: Nombre de la aplicación
- `APP_VERSION`: Versión
- `APP_YEAR`: Año

#### Sesión
- `SESSION_TIMEOUT`: Tiempo de sesión en minutos (default: 30)
- `MAX_LOGIN_ATTEMPTS`: Intentos máximos de login (default: 3)

#### Seguridad
- `MIN_PASSWORD_LENGTH`: Longitud mínima de contraseña (default: 6)
- `REQUIRE_STRONG_PASSWORD`: `true` o `false`

#### Notificaciones
- `SHOW_NOTIFICATIONS`: `true` o `false`
- `NOTIFICATION_DURATION`: Duración en segundos (default: 3)

#### Stock
- `STOCK_MINIMO_ALERTA`: Nivel de alerta mínimo (default: 10)
- `STOCK_CRITICO_ALERTA`: Nivel crítico (default: 5)

### ?? Uso en Código

```csharp
// Cargar variables (se hace automáticamente en Program.cs)
EnvironmentLoader.Load();

// Obtener cadena de conexión
string connectionString = EnvironmentLoader.GetConnectionString();

// Obtener variables individuales
string dbServer = EnvironmentLoader.GetVariable("DB_SERVER");
int timeout = EnvironmentLoader.GetInt("SESSION_TIMEOUT", 30);
bool showNotifications = EnvironmentLoader.GetBool("SHOW_NOTIFICATIONS", true);
```

### ?? Solución de Problemas

#### Error: "No se encontró el archivo .env"
1. Verifica que el archivo `.env` existe en la raíz del proyecto
2. Copia `.env.example` y renómbralo a `.env`
3. Configura tus credenciales

#### Error: "No se pudo conectar al servidor"
1. Verifica que SQL Server esté ejecutándose
2. Confirma el nombre del servidor en `DB_SERVER`
3. Si usas una instancia nombrada: `localhost\SQLEXPRESS`
4. Si usas la instancia por defecto: `localhost`

#### Error: "No se puede abrir la base de datos"
1. Verifica que la base de datos existe
2. Ejecuta el script de creación de la base de datos
3. Confirma el nombre en `DB_NAME`

### ?? Para Colaboradores

Si clonas este repositorio:

1. **Copia el archivo de ejemplo**:
   ```bash
   copy .env.example .env
   ```

2. **Pide las credenciales al administrador del proyecto** o configura las tuyas propias

3. **Nunca modifiques `.env.example`** con credenciales reales

4. **Nunca hagas commit del archivo `.env`**

---

## ?? Requisitos

- .NET Framework 4.7.2
- SQL Server 2016 o superior
- Visual Studio 2019 o superior

## ??? Compilación

```bash
# Restaurar paquetes NuGet
nuget restore

# Compilar la solución
msbuild GestionPedidos.sln
```

---

**Nota**: Si encuentras algún problema con la configuración, revisa que el archivo `.env` esté correctamente configurado y que todas las variables requeridas estén presentes.
