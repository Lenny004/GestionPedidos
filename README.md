# Sistema de Gestión de Pedidos

## 📖 Descripción
Aplicación de escritorio desarrollada en C# para la gestión integral de pedidos, clientes y productos de una empresa.

## 👥 Equipo de Desarrollo
- **Lenny Adrián Elías Sánchez** - Carnet: ES100124
- **Jonathan Adonay Alas Giron** - Carnet: AG100124
- **Mauro José Alfaro Amaya** - Carnet: AA100424

## 🛠️ Tecnologías Utilizadas
- **Lenguaje**: C# (.NET Framework 4.8)
- **UI Framework**: Windows Forms + Guna UI 2 v2.0.4.6
- **Base de Datos**: SQL Server Express 2019
- **Arquitectura**: MVC (Model-View-Controller)
- **IDE**: Visual Studio 2022

## 📋 Requisitos Previos
- Visual Studio 2019 o superior
- SQL Server Express 2019 o superior
- .NET Framework 4.8
- Windows 10 o superior

## 🚀 Instalación

### 1. Clonar el repositorio
```bash
git clone https://github.com/Lenny004/GestionPedidos.git
cd GestionPedidos
```

### 2. Restaurar la base de datos
```sql
-- Ejecutar scripts en orden:
1. database/scripts/createDatabase.sqL
2. database/scripts/insertInitialData.sql
```

### 3. Configurar conexión
Editar `App.config` con tu cadena de conexión

### 4. Compilar y ejecutar
Abrir `GestionPedidos.sln` en Visual Studio y ejecutar (F5)

## 📚 Estructura del Proyecto
```
src/
├── UI/              # Interfaz gráfica
├── Controllers/     # Lógica de negocio
├── Models/          # Entidades y DTOs
├── DataAccess/      # Repositorios y acceso a BD
└── Common/          # Utilidades compartidas
```

## 🔒 Seguridad
- Contraseñas hasheadas (SHA256)
- Control de acceso basado en roles
- Validación de entrada de datos

## 📝 Licencia
Este proyecto es para fines académicos - Universidad Francisco Gavidia 2025
