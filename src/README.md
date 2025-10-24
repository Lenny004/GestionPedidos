# Sistema de Gesti�n de Pedidos

## ?? Configuraci�n del Proyecto

Este proyecto utiliza variables de entorno para proteger las credenciales y configuraciones sensibles.

### Configuraci�n Inicial

1. **Copiar el archivo de ejemplo**
   ```bash
   copy .env.example .env
   ```
   O manualmente: duplica `.env.example` y ren�mbralo a `.env`

2. **Editar el archivo `.env`** con tus credenciales reales:

   ```env
   # Configuraci�n de Base de Datos
   DB_SERVER=localhost\SQLEXPRESS
   DB_NAME=GestionPedidosDB
   DB_INTEGRATED_SECURITY=True
   DB_TRUST_SERVER_CERTIFICATE=True
   ```

3. **Si usas autenticaci�n SQL Server**, descomenta y configura:
   ```env
   DB_INTEGRATED_SECURITY=False
   DB_USER=tu_usuario
   DB_PASSWORD=tu_contrase�a
   ```

### ?? Estructura de Archivos

```
GestionPedidos/
??? .env                    ? TUS credenciales (NO se sube a GitHub)
??? .env.example            ? Plantilla de ejemplo (S� se sube a GitHub)
??? .gitignore              ? Protege archivos sensibles
??? src/
    ??? GestionPedidos.UI/
    ??? GestionPedidos.Common/
    ?   ??? Configuration/
    ?       ??? EnvironmentLoader.cs
    ??? ...
```

### ?? Seguridad

- ? El archivo `.env` est� en `.gitignore` y **NO se sube a GitHub**
- ? El archivo `.env.example` **S� se sube a GitHub** como plantilla
- ? Nunca subas credenciales reales al repositorio

### ?? Variables de Entorno Disponibles

#### Base de Datos
- `DB_SERVER`: Servidor SQL (ej: `localhost\SQLEXPRESS`)
- `DB_NAME`: Nombre de la base de datos
- `DB_INTEGRATED_SECURITY`: `True` para Windows Auth, `False` para SQL Auth
- `DB_TRUST_SERVER_CERTIFICATE`: `True` o `False`
- `DB_USER`: Usuario SQL (opcional, solo si `DB_INTEGRATED_SECURITY=False`)
- `DB_PASSWORD`: Contrase�a SQL (opcional, solo si `DB_INTEGRATED_SECURITY=False`)

#### Configuraci�n de la Aplicaci�n
- `APP_NAME`: Nombre de la aplicaci�n
- `APP_VERSION`: Versi�n
- `APP_YEAR`: A�o

#### Sesi�n
- `SESSION_TIMEOUT`: Tiempo de sesi�n en minutos (default: 30)
- `MAX_LOGIN_ATTEMPTS`: Intentos m�ximos de login (default: 3)

#### Seguridad
- `MIN_PASSWORD_LENGTH`: Longitud m�nima de contrase�a (default: 6)
- `REQUIRE_STRONG_PASSWORD`: `true` o `false`

#### Notificaciones
- `SHOW_NOTIFICATIONS`: `true` o `false`
- `NOTIFICATION_DURATION`: Duraci�n en segundos (default: 3)

#### Stock
- `STOCK_MINIMO_ALERTA`: Nivel de alerta m�nimo (default: 10)
- `STOCK_CRITICO_ALERTA`: Nivel cr�tico (default: 5)

### ?? Uso en C�digo

```csharp
// Cargar variables (se hace autom�ticamente en Program.cs)
EnvironmentLoader.Load();

// Obtener cadena de conexi�n
string connectionString = EnvironmentLoader.GetConnectionString();

// Obtener variables individuales
string dbServer = EnvironmentLoader.GetVariable("DB_SERVER");
int timeout = EnvironmentLoader.GetInt("SESSION_TIMEOUT", 30);
bool showNotifications = EnvironmentLoader.GetBool("SHOW_NOTIFICATIONS", true);
```

### ?? Soluci�n de Problemas

#### Error: "No se encontr� el archivo .env"
1. Verifica que el archivo `.env` existe en la ra�z del proyecto
2. Copia `.env.example` y ren�mbralo a `.env`
3. Configura tus credenciales

#### Error: "No se pudo conectar al servidor"
1. Verifica que SQL Server est� ejecut�ndose
2. Confirma el nombre del servidor en `DB_SERVER`
3. Si usas una instancia nombrada: `localhost\SQLEXPRESS`
4. Si usas la instancia por defecto: `localhost`

#### Error: "No se puede abrir la base de datos"
1. Verifica que la base de datos existe
2. Ejecuta el script de creaci�n de la base de datos
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

## ??? Compilaci�n

```bash
# Restaurar paquetes NuGet
nuget restore

# Compilar la soluci�n
msbuild GestionPedidos.sln
```

---

**Nota**: Si encuentras alg�n problema con la configuraci�n, revisa que el archivo `.env` est� correctamente configurado y que todas las variables requeridas est�n presentes.
