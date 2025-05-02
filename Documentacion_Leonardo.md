# Documentación - Tareas de Leonardo en Mini Tienda

## Sprint 1: Configuración y Desarrollo Inicial

### 1. Configuración del Proyecto
- Se creó un proyecto Class Library (.NET Standard 2.0) llamado MiniTienda.Data
- Se agregó la librería MySql.Data vía NuGet (versión 9.3.0)
- Se configuró la cadena de conexión en App.config para conectarse a MySQL en localhost

### 2. Base de Datos
- Se creó la base de datos "minitienda" en MySQL Server 8.0.42
- Se ejecutó el script SQL para crear la tabla "tbl_categoria"
- Se implementaron procedimientos almacenados para gestionar categorías:
  - sp_show_categories: Muestra todas las categorías
  - sp_save_category: Guarda una nueva categoría
  - sp_update_category: Actualiza una categoría existente
  - sp_delete_category: Elimina una categoría

### 3. Clase Persistence
- Se implementó la clase Persistence.cs para gestionar la conexión a MySQL
- Esta clase provee un método GetConnection() que devuelve una nueva instancia de MySqlConnection
- Utiliza la cadena de conexión definida en App.config

### 4. Clase CategoryData
- Se implementó la clase CategoryData.cs con las operaciones CRUD para categorías
- Métodos implementados:
  - ShowCategories(): Muestra todas las categorías
  - SaveCategory(name, description): Guarda una nueva categoría
  - UpdateCategory(id, name, description): Actualiza una categoría existente
  - DeleteCategory(id): Elimina una categoría

## Sprint 2: Pruebas y Mejoras

### 1. Proyecto de Pruebas
- Se creó el proyecto TestCategoryData para probar todos los métodos de CategoryData
- El proyecto incluye un menú interactivo que permite:
  - Mostrar todas las categorías
  - Insertar una nueva categoría
  - Actualizar una categoría existente
  - Eliminar una categoría
  - Ejecutar todas las pruebas automáticamente

### 2. Mejora de Documentación
- Se agregaron comentarios XML a todas las clases y métodos
- Se creó esta documentación para explicar el trabajo realizado

### 3. Pruebas de Integración
- Se verificó que todos los métodos de CategoryData funcionen correctamente con la base de datos
- Se comprobó la correcta implementación del patrón DAO (Data Access Object)

## Conclusiones

La capa de acceso a datos para categorías ha sido implementada exitosamente. La arquitectura sigue el patrón DAO, separando la lógica de acceso a datos de la lógica de negocio, lo que facilita el mantenimiento y las futuras ampliaciones.

La implementación de procedimientos almacenados en la base de datos permite una mayor eficiencia y seguridad en las operaciones, al tiempo que encapsula la lógica de la base de datos.

El proyecto se encuentra listo para ser integrado con las capas de lógica de negocio y presentación que desarrollarán los otros miembros del equipo. 