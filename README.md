🐾 Veterinaria Huellitas

Sistema de gestión veterinaria desarrollado con C#, WPF y Microsoft SQL Server

📌 1. Descripción del proyecto

Veterinaria Huellitas es un sistema de escritorio desarrollado para facilitar la gestión y administración de la información de una clínica veterinaria.

La aplicación permite registrar y administrar propietarios, mascotas y consultas médicas, manteniendo organizada la información y facilitando el seguimiento del historial clínico de cada mascota.

El proyecto utiliza C#, Windows Presentation Foundation (WPF), XAML y Microsoft SQL Server como tecnologías principales.

🎯 2. Objetivos del proyecto

Objetivo general

Desarrollar un sistema de gestión veterinaria que permita organizar y administrar la información de los propietarios, sus mascotas y las consultas médicas realizadas.

Objetivos específicos

- Registrar y administrar la información de los propietarios.
- Registrar y gestionar los datos de las mascotas.
- Establecer la relación entre cada mascota y su propietario.
- Registrar consultas médicas y almacenar la información clínica.
- Consultar el historial médico de las mascotas.
- Generar documentos PDF de las consultas médicas.
- Mostrar estadísticas generales de los registros del sistema.

🛠️ 3. Tecnologías utilizadas

Tecnología| Utilidad
C#| Lenguaje de programación
.NET| Plataforma de desarrollo
WPF| Desarrollo de interfaces gráficas de escritorio
XAML| Diseño de las interfaces
Microsoft SQL Server| Sistema de gestión de base de datos
Microsoft.Data.SqlClient| Conexión entre C# y SQL Server
QuestPDF| Generación de documentos PDF
Visual Studio 2022| Entorno de desarrollo

🏗️ 4. Arquitectura del proyecto

El sistema Veterinaria Huellitas presenta una estructura organizada por carpetas, separando la interfaz gráfica, los modelos de datos, la conexión con la base de datos y los recursos visuales.

📂 Estructura de directorios

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
├── Recursos/
│   └── fondo-inicio.jpg
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
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
└── VeterinariaHuellitas.csproj

📋 Descripción de los componentes

Carpeta o archivo| Responsabilidad
"Datos/"| Gestiona la conexión con SQL Server.
"Modelos/"| Define las entidades y estructuras de datos del sistema.
"Recursos/"| Almacena imágenes y elementos visuales.
"Ventanas/"| Contiene las interfaces gráficas y su lógica de funcionamiento.
"App.xaml"| Define la configuración y los recursos generales de la aplicación.
"MainWindow.xaml"| Contiene la ventana principal de inicio de la aplicación.
"VeterinariaHuellitas.csproj"| Define la configuración del proyecto y sus dependencias.

🔄 Organización del funcionamiento

La aplicación organiza sus responsabilidades de la siguiente manera:

1. Presentación: las ventanas WPF y los archivos XAML permiten la interacción con el usuario.
2. Modelos: representan los datos de propietarios, mascotas y consultas médicas.
3. Acceso a datos: la clase "Conexion.cs" establece la comunicación con SQL Server.
4. Persistencia: SQL Server almacena la información del sistema.

Esta organización facilita el mantenimiento, la comprensión y la ampliación de las funcionalidades de Veterinaria Huellitas.

🗄️ 5. Diseño de la base de datos

La base de datos del sistema se denomina VeterinariaHuellitas y está diseñada para organizar la información de propietarios, mascotas y consultas médicas.

Tabla: Dueños

Almacena los datos personales y de contacto de los propietarios.

Campo| Descripción
CodigoDueño| Identificador del propietario
Nombre| Nombre del propietario
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
CodigoDueño| Identificador del propietario relacionado

Tabla: Consultas Médicas

Almacena la información clínica de las consultas realizadas.

Campo| Descripción
CodigoConsulta| Identificador de la consulta
Fecha| Fecha de la consulta
Motivo| Motivo de consulta
Sintomas| Síntomas presentados
Diagnostico| Diagnóstico médico
Tratamiento| Tratamiento indicado
Medicamentos| Medicamentos prescritos
Observaciones| Observaciones adicionales
CodigoMascota| Identificador de la mascota relacionada

Relaciones entre las tablas

- Un propietario puede tener varias mascotas.
- Cada mascota pertenece a un propietario.
- Una mascota puede tener varias consultas médicas.
- Cada consulta médica corresponde a una mascota.

Estas relaciones permiten mantener organizada la información y consultar el historial clínico de cada mascota.

⚙️ 6. Módulos y funcionalidades del sistema

6.1. Inicio de sesión

Permite acceder a la aplicación mediante una pantalla de autenticación.

6.2. Ventana principal

Presenta el menú de navegación y un resumen general de la información registrada.

Incluye estadísticas obtenidas directamente de la base de datos:

- Total de propietarios.
- Total de mascotas.
- Total de consultas médicas.

6.3. Gestión de propietarios

Permite administrar la información de los propietarios mediante las siguientes operaciones:

- Registrar propietarios.
- Consultar registros.
- Seleccionar información.
- Modificar datos.
- Eliminar registros.

6.4. Gestión de mascotas

Permite registrar y administrar las mascotas de la veterinaria.

Incluye la selección del propietario correspondiente para mantener la relación entre ambas tablas.

6.5. Historial clínico

Permite registrar y consultar las consultas médicas de las mascotas.

La información clínica contempla:

- Motivo de consulta.
- Síntomas.
- Diagnóstico.
- Tratamiento.
- Medicamentos.
- Observaciones.

6.6. Generación de documentos PDF

El sistema incorpora la generación de documentos PDF mediante la biblioteca QuestPDF.

Los documentos incluyen:

- Identificación de la veterinaria.
- Información de la mascota.
- Datos del propietario.
- Fecha de consulta.
- Información clínica registrada.
- Espacio para la firma y sello del médico veterinario.

🔌 7. Conexión con SQL Server

La conexión con la base de datos se administra mediante la clase "Conexion.cs", ubicada en la carpeta "Datos".

Se utiliza la biblioteca "Microsoft.Data.SqlClient" para establecer la comunicación entre la aplicación y Microsoft SQL Server.

La cadena de conexión debe configurarse de acuerdo con la instancia de SQL Server instalada en cada equipo.

Importante: la configuración de conexión puede requerir modificaciones para ejecutar el proyecto en otros equipos.

💻 8. Requisitos del sistema

Para ejecutar el proyecto se necesita:

- Sistema operativo Windows.
- Visual Studio 2022.
- SDK de .NET correspondiente al proyecto.
- Microsoft SQL Server o SQL Server Express.
- Base de datos "VeterinariaHuellitas".
- Paquete NuGet "Microsoft.Data.SqlClient".
- Paquete NuGet "QuestPDF".

🚀 9. Instalación y ejecución

1. Instalar Visual Studio 2022.
2. Instalar el SDK de .NET requerido por el proyecto.
3. Instalar y configurar Microsoft SQL Server.
4. Crear la base de datos "VeterinariaHuellitas".
5. Ejecutar el script SQL incluido en la carpeta "BaseDatos", si está disponible.
6. Configurar la cadena de conexión en "Datos/Conexion.cs".
7. Abrir la solución "VeterinariaHuellitas.slnx" en Visual Studio.
8. Restaurar los paquetes NuGet.
9. Compilar la solución.
10. Ejecutar la aplicación.

📸 10. Capturas de pantalla

En esta sección se pueden incorporar imágenes que muestran el funcionamiento de los principales módulos:

- Inicio de sesión.
- Ventana principal.
- Gestión de propietarios.
- Gestión de mascotas.
- Historial clínico.
- Documento PDF generado.

👨‍💻 11. Autor

Desarrollado por: Jheims Alexander Mamani Quispe.

Proyecto académico: Sistema de gestión veterinaria Veterinaria Huellitas.

📄 12. Licencia

Este proyecto se publica con fines académicos, educativos y de revisión.

No se ha establecido una licencia específica que autorice su reutilización o distribución.

🔐 13. Seguridad y acceso de demostración

El sistema incorpora un inicio de sesión de demostración para facilitar las pruebas académicas.

Las credenciales actuales son provisionales y no deben utilizarse en un entorno de producción.

Antes de implementar el sistema en un entorno real, se recomienda incorporar autenticación segura, almacenamiento protegido de credenciales y gestión de usuarios y permisos.

La conexión a SQL Server utiliza autenticación de Windows y debe configurarse según la instancia disponible en cada equipo.

---

Veterinaria Huellitas — Gestión, organización y cuidado de nuestras mascotas. 🐾
