# Proyecto Homebanking

Este proyecto es una aplicación web de homebanking desarrollada en .NET Core y Vue.js. Permite a los usuarios registrarse, iniciar sesión, crear cuentas bancarias, realizar transferencias entre cuentas, solicitar préstamos y gestionar sus tarjetas de crédito/débito.

## Características principales

- Registro e inicio de sesión de usuarios
- Creación de cuentas bancarias (límite de 3 cuentas por usuario)
- Transferencias entre cuentas propias y de otros usuarios
- Solicitud de préstamos preaprobados y acreditación automática en cuenta
- Creación de tarjetas de crédito y débito (límite de 3 tarjetas por tipo)
- Visualización del historial de transacciones por cuenta

## Tecnologías utilizadas

- Backend: .NET Core, Entity Framework Core, ASP.NET Core Web API
- Base de datos: SQL Server

## Lecciones aprendidas

Durante el desarrollo de este proyecto, hemos aprendido y aplicado conceptos importantes como:

- Implementacion backend con .NET, EF Core y SQL Server
- Autenticación y autorización en aplicaciones web
- Manejo de transacciones y operaciones financieras sensibles
- Validación y manejo de errores en APIs RESTful
- Implementar técnicas de seguridad (Cookies, JWT)

## Instalación y configuración

1. Clonar el repositorio: `git clone https://github.com/tu-usuario/homebanking.git`
2. Configurar la cadena de conexión a la base de datos en `appsettings.json`
3. Ejecutar las migraciones de Entity Framework Core: `update-database`
4. Compilar y ejecutar la aplicación backend: `dotnet run`
