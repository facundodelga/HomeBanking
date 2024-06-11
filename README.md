Proyecto Homebanking
Este proyecto es una aplicación web de homebanking desarrollada en .NET Core y Vue.js. Permite a los usuarios registrarse, iniciar sesión, crear cuentas bancarias, realizar transferencias entre cuentas, solicitar préstamos y gestionar sus tarjetas de crédito/débito.
Características principales

Registro e inicio de sesión de usuarios
Creación de cuentas bancarias (límite de 3 cuentas por usuario)
Transferencias entre cuentas propias y de otros usuarios
Solicitud de préstamos preaprobados y acreditación automática en cuenta
Creación de tarjetas de crédito y débito (límite de 3 tarjetas por tipo)
Visualización del historial de transacciones por cuenta

Tecnologías utilizadas

Backend: .NET Core, Entity Framework Core, ASP.NET Core Web API
Frontend: Vue.js, Axios, Bootstrap
Base de datos: SQL Server

Instalación y configuración

Clonar el repositorio: git clone https://github.com/tu-usuario/homebanking.git
Configurar la cadena de conexión a la base de datos en appsettings.json
Ejecutar las migraciones de Entity Framework Core: dotnet ef database update
Compilar y ejecutar la aplicación backend: dotnet run
Navegar a la carpeta client y ejecutar npm install para instalar las dependencias del frontend
Iniciar la aplicación frontend: npm run serve

Lecciones aprendidas y mejoras futuras
Durante el desarrollo de este proyecto, hemos aprendido y aplicado conceptos importantes como:

Autenticación y autorización en aplicaciones web
Manejo de transacciones y operaciones financieras sensibles
Validación y manejo de errores en APIs RESTful
Integración de frontend y backend utilizando Vue.js y .NET Core
Pruebas automatizadas y buenas prácticas de desarrollo

En futuras iteraciones, nos enfocaremos en:

Implementar técnicas de seguridad más avanzadas (JWT, OAuth)
Mejorar el manejo de concurrencia y escalabilidad
Explorar arquitecturas de microservicios y contenedores
Implementar registros de auditoría y notificaciones al usuario
Mejorar la documentación y la experiencia de usuario
