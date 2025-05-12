# Documentación - Tareas de Leonardo en Mini Tienda

## Sprint 1: Configuración y Desarrollo Inicial

### 1. Configuración del Proyecto
- Se creó un proyecto Class Library (.NET Standard 2.0) llamado MiniTienda.Data
- Se agregó la librería MySql.Data vía NuGet (versión 9.3.0)
- Se configuró la cadena de conexión en App.config para conectarse a MySQL en localhost

### 2. Base de Datos
- Se creó la base de datos "minitiendaDB" en MySQL Server 8.0.42
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

## Sprint 2: Implementación de la Capa Lógica de Negocio

### 1. Implementación de UsersLog 
- Se creó la clase UsersLog en MiniTienda.Logic
- Se implementaron los métodos de gestión de usuarios con conexión a UsersData:
  - showUsers(): Obtiene todos los usuarios
  - saveUsers(mail, password, salt, state): Guarda un nuevo usuario
  - updateUsers(id, mail, password, salt, state): Actualiza un usuario existente
- Se agregaron validaciones de datos para garantizar la integridad:
  - Verificación de campos obligatorios
  - Validación de datos nulos o vacíos

### 2. Implementación de ProductsLog
- Se creó la clase ProductsLog en MiniTienda.Logic
- Se implementaron los métodos de gestión de productos con conexión a ProductsData:
  - showProducts(): Obtiene todos los productos
  - saveProducts(code, description, quantity, price, idCategory, idProvider): Guarda un nuevo producto
  - updateProducts(id, code, description, quantity, price, idCategory, idProvider): Actualiza un producto existente
- Se agregaron validaciones de reglas de negocio:
  - Verificación de precio positivo
  - Validación de campos obligatorios
  - Verificación de cantidades válidas

### 3. Pruebas de la Capa Lógica
- Se creó el proyecto MiniTienda.Tests para validar la capa lógica
- Se implementaron pruebas para UsersLog:
  - TestShowUsers(): Prueba la recuperación de usuarios
  - TestSaveUser(): Prueba la validación y guardado de usuarios
- Se implementaron pruebas para ProductsLog:
  - TestShowProducts(): Prueba la recuperación de productos
  - TestSaveProduct(): Prueba la validación y guardado de productos, incluyendo casos de prueba para validaciones
- Se documentaron todos los métodos con comentarios XML para facilitar el mantenimiento 