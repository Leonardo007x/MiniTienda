/**
 * Proyecto MiniTienda - Pruebas de la Capa de Acceso a Datos
 * 
 * Este programa proporciona una interfaz de consola para probar las funcionalidades
 * de las clases UsersData y ProductsData, permitiendo verificar que las operaciones CRUD
 * (Create, Read, Update) funcionan correctamente con la base de datos.
 * 
 * Autor: Leonardo
 * Fecha: Mayo 2025 
 */

using System;
using System.Data;
using MiniTienda.Data;
using MiniTienda.Logic;

namespace TestUserProductData
{
    /// <summary>
    /// Clase principal del programa de pruebas para UsersData y ProductsData
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal que muestra un menú interactivo para probar las operaciones CRUD
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos (no utilizados)</param>
        static void Main(string[] args)
        {
            Console.WriteLine("===== PRUEBAS DE USERSDATA Y PRODUCTSDATA =====");
            
            bool continuar = true;
            while (continuar)
            {
                // Mostrar menú de opciones
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Mostrar todos los usuarios");
                Console.WriteLine("2. Insertar un nuevo usuario");
                Console.WriteLine("3. Actualizar un usuario");
                Console.WriteLine("4. Mostrar todos los productos");
                Console.WriteLine("5. Insertar un nuevo producto");
                Console.WriteLine("6. Actualizar un producto");
                Console.WriteLine("0. Salir");
                
                Console.Write("\nOpción: ");
                string opcion = Console.ReadLine();
                
                // Procesar la opción seleccionada
                switch (opcion)
                {
                    case "1":
                        TestShowUsers();
                        break;
                    case "2":
                        TestSaveUsers();
                        break;
                    case "3":
                        TestUpdateUsers();
                        break;
                    case "4":
                        TestShowProducts();
                        break;
                    case "5":
                        TestSaveProducts();
                        break;
                    case "6":
                        TestUpdateProducts();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }
            }
            
            Console.WriteLine("\nPruebas finalizadas. Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Prueba el método showUsers() de la clase UsersData
        /// Muestra todos los usuarios existentes en la base de datos
        /// </summary>
        static void TestShowUsers()
        {
            Console.WriteLine("\n--- Probando showUsers() ---");
            try
            {
                // Crear instancia de UsersData
                UsersData usersData = new UsersData();
                
                // Obtener todos los usuarios
                DataSet users = usersData.showUsers();
                
                // Verificar si hay tablas en el DataSet
                if (users.Tables.Count == 0 || users.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No se encontraron usuarios.");
                    return;
                }
                
                // Mostrar resultados
                Console.WriteLine($"Se encontraron {users.Tables[0].Rows.Count} usuarios:");
                
                foreach (DataRow row in users.Tables[0].Rows)
                {
                    // spSelectUsers devuelve usu_id, usu_correo, usu_contrasena, usu_salt, usu_estado
                    Console.WriteLine($"ID: {row["usu_id"]}, Correo: {row["usu_correo"]}, Estado: {row["usu_estado"]}");
                }
                
                Console.WriteLine("Prueba showUsers: EXITOSA ✓");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba showUsers: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método saveUsers() de la clase UsersData
        /// Permite al usuario ingresar datos para un nuevo usuario
        /// </summary>
        static void TestSaveUsers()
        {
            Console.WriteLine("\n--- Probando saveUsers() ---");
            try
            {
                // Solicitar datos del nuevo usuario
                Console.Write("Ingrese el correo del usuario: ");
                string mail = Console.ReadLine();
                
                Console.Write("Ingrese la contraseña del usuario: ");
                string password = Console.ReadLine();
                
                // Para simplificar, usamos valores simples para salt y estado
                string salt = "salt" + DateTime.Now.Ticks;
                string state = "activo";
                
                // Crear instancia de UsersData y guardar el nuevo usuario
                UsersData usersData = new UsersData();
                bool resultado = usersData.saveUsers(mail, password, salt, state);
                
                // Mostrar resultado
                if (resultado)
                {
                    Console.WriteLine("Usuario guardado exitosamente.");
                    Console.WriteLine("Prueba saveUsers: EXITOSA ✓");
                }
                else
                {
                    Console.WriteLine("No se pudo guardar el usuario.");
                    Console.WriteLine("Prueba saveUsers: FALLIDA ✗");
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba saveUsers: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método updateUsers() de la clase UsersData
        /// Permite actualizar un usuario existente
        /// </summary>
        static void TestUpdateUsers()
        {
            Console.WriteLine("\n--- Probando updateUsers() ---");
            try
            {
                // Mostrar usuarios existentes para selección
                UsersData usersData = new UsersData();
                DataSet users = usersData.showUsers();
                
                if (users.Tables.Count == 0 || users.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No hay usuarios disponibles para actualizar.");
                    return;
                }
                
                Console.WriteLine("Usuarios disponibles:");
                foreach (DataRow row in users.Tables[0].Rows)
                {
                    // spSelectUsers devuelve usu_id, usu_correo, usu_contrasena, usu_salt, usu_estado
                    Console.WriteLine($"ID: {row["usu_id"]}, Correo: {row["usu_correo"]}, Estado: {row["usu_estado"]}");
                }
                
                // Solicitar ID del usuario a actualizar
                Console.Write("\nIngrese el ID del usuario a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int userId))
                {
                    Console.WriteLine("ID inválido");
                    return;
                }
                
                // Solicitar nuevos datos
                Console.Write("Ingrese el nuevo correo: ");
                string mail = Console.ReadLine();
                
                Console.Write("Ingrese la nueva contraseña: ");
                string password = Console.ReadLine();
                
                // Para simplificar, usamos valores simples para salt y estado
                string salt = "salt" + DateTime.Now.Ticks;
                string state = "activo";
                
                // Actualizar el usuario
                bool resultado = usersData.updateUsers(userId, mail, password, salt, state);
                
                // Mostrar resultado
                if (resultado)
                {
                    Console.WriteLine("Usuario actualizado exitosamente.");
                    Console.WriteLine("Prueba updateUsers: EXITOSA ✓");
                }
                else
                {
                    Console.WriteLine("No se pudo actualizar el usuario.");
                    Console.WriteLine("Prueba updateUsers: FALLIDA ✗");
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba updateUsers: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método showProducts() de la clase ProductsData
        /// Muestra todos los productos existentes en la base de datos
        /// </summary>
        static void TestShowProducts()
        {
            Console.WriteLine("\n--- Probando showProducts() ---");
            try
            {
                // Crear instancia de ProductsData
                ProductsData productsData = new ProductsData();
                
                // Obtener todos los productos
                DataSet products = productsData.showProducts();
                
                // Verificar si hay tablas en el DataSet
                if (products.Tables.Count == 0 || products.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No se encontraron productos.");
                    return;
                }
                
                // Mostrar resultados
                Console.WriteLine($"Se encontraron {products.Tables[0].Rows.Count} productos:");
                
                foreach (DataRow row in products.Tables[0].Rows)
                {
                    Console.WriteLine($"Código: {row[0]}, Descripción: {row[1]}, Cantidad: {row[2]}, Precio: {row[3]}");
                }
                
                Console.WriteLine("Prueba showProducts: EXITOSA ✓");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba showProducts: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método saveProducts() de la clase ProductsData
        /// Permite al usuario ingresar datos para un nuevo producto
        /// </summary>
        static void TestSaveProducts()
        {
            Console.WriteLine("\n--- Probando saveProducts() ---");
            try
            {
                // Solicitar datos del nuevo producto
                Console.Write("Ingrese el código del producto: ");
                string code = Console.ReadLine();
                
                Console.Write("Ingrese la descripción del producto: ");
                string description = Console.ReadLine();
                
                Console.Write("Ingrese la cantidad del producto: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.WriteLine("Cantidad inválida");
                    return;
                }
                
                Console.Write("Ingrese el precio del producto: ");
                if (!double.TryParse(Console.ReadLine(), out double price))
                {
                    Console.WriteLine("Precio inválido");
                    return;
                }
                
                Console.Write("Ingrese el ID de la categoría: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryId))
                {
                    Console.WriteLine("ID de categoría inválido");
                    return;
                }
                
                Console.Write("Ingrese el ID del proveedor: ");
                if (!int.TryParse(Console.ReadLine(), out int providerId))
                {
                    Console.WriteLine("ID de proveedor inválido");
                    return;
                }
                
                // Crear instancia de ProductsData y guardar el nuevo producto
                ProductsData productsData = new ProductsData();
                bool resultado = productsData.saveProducts(code, description, quantity, price, categoryId, providerId);
                
                // Mostrar resultado
                if (resultado)
                {
                    Console.WriteLine("Producto guardado exitosamente.");
                    Console.WriteLine("Prueba saveProducts: EXITOSA ✓");
                }
                else
                {
                    Console.WriteLine("No se pudo guardar el producto.");
                    Console.WriteLine("Prueba saveProducts: FALLIDA ✗");
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba saveProducts: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método updateProducts() de la clase ProductsData
        /// Permite actualizar un producto existente
        /// </summary>
        static void TestUpdateProducts()
        {
            Console.WriteLine("\n--- Probando updateProducts() ---");
            try
            {
                // Crear instancia de ProductsData
                ProductsData productsData = new ProductsData();
                
                // Mostrar productos existentes para selección
                DataSet products = productsData.showProducts();
                
                if (products.Tables.Count == 0 || products.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No hay productos disponibles para actualizar.");
                    return;
                }
                
                Console.WriteLine("Productos disponibles:");
                int index = 0;
                foreach (DataRow row in products.Tables[0].Rows)
                {
                    Console.WriteLine($"Índice: {index}, Código: {row[0]}, Descripción: {row[1]}");
                    index++;
                }
                
                // Solicitar ID del producto a actualizar
                Console.Write("\nIngrese el ID del producto a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("ID inválido");
                    return;
                }
                
                // Solicitar nuevos datos
                Console.Write("Ingrese el nuevo código del producto: ");
                string code = Console.ReadLine();
                
                Console.Write("Ingrese la nueva descripción del producto: ");
                string description = Console.ReadLine();
                
                Console.Write("Ingrese la nueva cantidad del producto: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.WriteLine("Cantidad inválida");
                    return;
                }
                
                Console.Write("Ingrese el nuevo precio del producto: ");
                if (!double.TryParse(Console.ReadLine(), out double price))
                {
                    Console.WriteLine("Precio inválido");
                    return;
                }
                
                Console.Write("Ingrese el nuevo ID de la categoría: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryId))
                {
                    Console.WriteLine("ID de categoría inválido");
                    return;
                }
                
                Console.Write("Ingrese el nuevo ID del proveedor: ");
                if (!int.TryParse(Console.ReadLine(), out int providerId))
                {
                    Console.WriteLine("ID de proveedor inválido");
                    return;
                }
                
                // Actualizar el producto
                bool resultado = productsData.updateProducts(productId, code, description, quantity, price, categoryId, providerId);
                
                // Mostrar resultado
                if (resultado)
                {
                    Console.WriteLine("Producto actualizado exitosamente.");
                    Console.WriteLine("Prueba updateProducts: EXITOSA ✓");
                }
                else
                {
                    Console.WriteLine("No se pudo actualizar el producto.");
                    Console.WriteLine("Prueba updateProducts: FALLIDA ✗");
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba updateProducts: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
