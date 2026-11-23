# VeterinariaHuellitas
Sistema de gestion para una veterinaria desarrollado en C# WPF y SQL SERVER
🐾 Veterinaria Huellitas

Sistema de gestión veterinaria desarrollado con C#, WPF y SQL Server

📌 1. Descripción del proyecto

Veterinaria Huellitas es un sistema de escritorio desarrollado para facilitar la gestión de información de una clínica veterinaria.

El sistema permite registrar y administrar propietarios, mascotas y consultas médicas, manteniendo organizada la información y facilitando el acceso al historial clínico de cada mascota.

El proyecto fue desarrollado utilizando C#, Windows Presentation Foundation (WPF), .NET 8 y Microsoft SQL Server.

🎯 2. Objetivos

Objetivo general

Desarrollar un sistema de gestión veterinaria que permita organizar y administrar la información de los propietarios, sus mascotas y las consultas médicas realizadas.

Objetivos específicos

- Registrar y gestionar la información de los propietarios.
- Registrar y gestionar los datos de las mascotas.
- Relacionar cada mascota con su propietario.
- Registrar consultas médicas y almacenar su información clínica.
- Consultar el historial médico de las mascotas.
- Generar documentos PDF de las consultas médicas.
- Mostrar estadísticas generales de los registros del sistema.

🛠️ 3. Tecnologías utilizadas

Tecnología| Utilidad
C#| Lenguaje de programación
.NET 8| Plataforma de desarrollo
WPF| Diseño de la interfaz gráfica
XAML| Estructura visual de las ventanas
SQL Server| Sistema de gestión de base de datos
Microsoft.Data.SqlClient| Conexión entre C# y SQL Server
QuestPDF| Generación de documentos PDF
Visual Studio 2022| Entorno de desarrollo

🏗️ 4. Arquitectura del proyecto

El proyecto está organizado en carpetas para mantener separados los modelos de datos, la conexión a la base de datos y las ventanas del sistema.

VeterinariaHuellitas/
│
├── Datos/
│   └── Conexion.cs
│
├── Modelos/
│   ├── Dueño.cs
│   ├── Mascota.cs
│   └── ConsultaMedica.cs
│
├── Ventanas/
│   ├── Login.xaml
│   ├── Login.xaml.cs
│   ├── Inicio.xaml
│   ├── Inicio.xaml.cs
│   ├── Dueños.xaml
│   ├── Dueños.xaml.cs
│   ├── Mascotas.xaml
│   ├── Mascotas.xaml.cs
│   ├── HistorialClínico.xaml
│   └── HistorialClínico.xaml.cs
│
├── Recursos/
│
├── App.xaml
├── App.xaml.cs
└── MainWindow.xaml

Descripción de las carpetas

- Datos: contiene la clase encargada de establecer la conexión con SQL Server.
- Modelos: contiene las clases que representan las entidades principales del sistema.
- Ventanas: contiene las interfaces XAML y su código asociado.
- Recursos: contiene los recursos visuales utilizados por la aplicación.

🗄️ 5. Diseño de la base de datos

La base de datos utilizada se denomina "VeterinariaHuellitas".

Está compuesta por tres tablas principales:

Tabla: Dueños

Almacena los datos personales y de contacto de los propietarios.

Campo| Descripción
CodigoDueño| Identificador del propietario
Nombre| Nombre
ApellidoPaterno| Apellido paterno
ApellidoMaterno| Apellido materno
CI| Documento de identidad
Sexo| Sexo
Telefono| Número de teléfono
Direccion| Dirección
Correo| Correo electrónico

Tabla: Mascotas

Almacena la información de las mascotas registradas.

Campo| Descripción
CodigoMascota| Identificador de la mascota
Nombre| Nombre de la mascota
Especie| Especie
Raza| Raza
Sexo| Sexo
FechaNacimiento| Fecha de nacimiento
Peso| Peso
CodigoDueño| Propietario relacionado

Tabla: Consultas Médicas

Almacena la información clínica de las consultas realizadas.

Campo| Descripción
CodigoConsulta| Identificador de la consulta
Fecha| Fecha de la consulta
Motivo| Motivo de consulta
Sintomas| Síntomas presentados
Diagnostico| Diagnóstico
Tratamiento| Tratamiento indicado
Medicamentos| Medicamentos
Observaciones| Observaciones
CodigoMascota| Mascota relacionada

Relaciones entre tablas

- Un propietario puede tener varias mascotas.
- Cada mascota pertenece a un propietario.
- Una mascota puede tener varias consultas médicas.
- Cada consulta médica corresponde a una mascota.

⚙️ 6. Funcionamiento y desarrollo del sistema

6.1. Módulo de inicio de sesión

Permite acceder al sistema mediante una pantalla de autenticación.

6.2. Ventana principal

Presenta el menú de navegación y un resumen general de la información registrada.

Muestra estadísticas de:

- Total de propietarios.
- Total de mascotas.
- Total de consultas médicas.

Los valores se obtienen directamente de la base de datos mediante consultas SQL.

6.3. Módulo de propietarios

Permite administrar la información de los propietarios.

Operaciones principales:

- Registrar propietarios.
- Consultar registros.
- Seleccionar información.
- Modificar datos.
- Eliminar registros.

6.4. Módulo de mascotas

Permite registrar y administrar las mascotas de la veterinaria.

Incluye la selección del propietario relacionado para mantener la relación entre ambas tablas.

6.5. Módulo de historial clínico

Permite registrar y consultar las consultas médicas de las mascotas.

La información incluye motivo, síntomas, diagnóstico, tratamiento, medicamentos y observaciones.

6.6. Generación de documentos PDF

El sistema incorpora la generación de documentos PDF mediante la biblioteca QuestPDF.

El documento contiene:

- Identificación de la veterinaria.
- Información de la mascota.
- Datos del propietario.
- Fecha de consulta.
- Información clínica registrada.
- Espacio para la firma y sello del médico veterinario.

🔌 7. Conexión con SQL Server

La conexión a la base de datos se administra mediante la clase "Conexion.cs", ubicada en la carpeta "Datos".

Se utiliza "Microsoft.Data.SqlClient" para establecer la comunicación entre la aplicación y SQL Server.

Para ejecutar el proyecto, es necesario configurar la cadena de conexión según la instancia de SQL Server instalada en el equipo.

Importante: la configuración de conexión incluida en el código puede necesitar modificaciones para funcionar en otros equipos.

💻 8. Requisitos para ejecutar el proyecto

- Windows.
- Visual Studio 2022.
- .NET 8 SDK.
- SQL Server Express o una instancia compatible de SQL Server.
- Base de datos "VeterinariaHuellitas".
- Paquetes NuGet "Microsoft.Data.SqlClient" y "QuestPDF".

🚀 9. Instalación y ejecución

1. Instalar Visual Studio 2022 y el SDK de .NET 8.
2. Instalar y configurar SQL Server.
3. Crear la base de datos "VeterinariaHuellitas".
4. Crear las tablas y relaciones descritas en la sección de diseño de la base de datos.
5. Configurar la cadena de conexión en "Datos/Conexion.cs".
6. Abrir la solución del proyecto en Visual Studio 2022.
7. Restaurar los paquetes NuGet.
8. Compilar la solución.
9. Ejecutar la aplicación.

📸 10. Capturas de pantalla

En esta sección se incorporarán imágenes de las principales ventanas del sistema:

- Inicio de sesión.
- Ventana principal.
- Registro de propietarios.
- Registro de mascotas.
- Historial clínico.
- Documento PDF generado.

👨‍💻 11. Autor

Desarrollado por Jheims Alexander Mamani Quispe.

Proyecto académico: Sistema de gestión veterinaria Veterinaria Huellitas.

📄 12. Licencia

Este proyecto se publica con fines académicos y de revisión. No se ha establecido una licencia de reutilización específica.

🔐 13. Seguridad y acceso de demostración

El sistema incorpora un inicio de sesión de demostración para facilitar las pruebas académicas.

Las credenciales actuales son provisionales y no deben utilizarse en un entorno de producción.

Antes de implementar el sistema en un entorno real, se recomienda incorporar autenticación segura, almacenamiento protegido de credenciales y gestión de usuarios y permisos.

La conexión a SQL Server utiliza autenticación de Windows y debe configurarse según la instancia disponible en cada equipo.


Veterinaria Huellitas — Gestión, organización y cuidado de nuestras mascotas. 🐾
