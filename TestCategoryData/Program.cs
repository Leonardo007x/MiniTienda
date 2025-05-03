/**
 * Proyecto MiniTienda - Pruebas de Capa de Acceso a Datos
 * 
 * Este programa proporciona una interfaz de consola para probar las funcionalidades
 * de la clase CategoryData, permitiendo verificar que las operaciones CRUD
 * (Create, Read, Update, Delete) funcionan correctamente con la base de datos.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using System;
using System.Data;
using MiniTienda.Data;

namespace TestCategoryData
{
    /// <summary>
    /// Clase principal del programa de pruebas para CategoryData
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal que muestra un menú interactivo para probar las operaciones CRUD
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos (no utilizados)</param>
        static void Main(string[] args)
        {
            Console.WriteLine("===== PRUEBAS DE CATEGORYDATA =====");
            
            bool continuar = true;
            while (continuar)
            {
                // Mostrar menú de opciones
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Mostrar todas las categorías");
                Console.WriteLine("2. Insertar una nueva categoría");
                Console.WriteLine("3. Actualizar una categoría");
                Console.WriteLine("4. Eliminar una categoría");
                Console.WriteLine("5. Ejecutar todas las pruebas");
                Console.WriteLine("0. Salir");
                
                Console.Write("\nOpción: ");
                string opcion = Console.ReadLine();
                
                // Procesar la opción seleccionada
                switch (opcion)
                {
                    case "1":
                        TestShowCategories();
                        break;
                    case "2":
                        TestSaveCategory();
                        break;
                    case "3":
                        TestUpdateCategory();
                        break;
                    case "4":
                        TestDeleteCategory();
                        break;
                    case "5":
                        RunAllTests();
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
        /// Prueba el método ShowCategories() de la clase CategoryData
        /// Muestra todas las categorías existentes en la base de datos
        /// </summary>
        static void TestShowCategories()
        {
            Console.WriteLine("\n--- Probando ShowCategories() ---");
            try
            {
                // Crear instancia de CategoryData
                CategoryData categoryData = new CategoryData();
                
                // Obtener todas las categorías
                DataTable categories = categoryData.ShowCategories();
                
                // Mostrar resultados
                Console.WriteLine($"Se encontraron {categories.Rows.Count} categorías:");
                
                foreach (DataRow row in categories.Rows)
                {
                    Console.WriteLine($"ID: {row["id"]}, Descripción: {row["description"]}");
                }
                
                Console.WriteLine("Prueba ShowCategories: EXITOSA ✓");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba ShowCategories: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método SaveCategory() de la clase CategoryData
        /// Permite al usuario ingresar datos para una nueva categoría
        /// </summary>
        static void TestSaveCategory()
        {
            Console.WriteLine("\n--- Probando SaveCategory() ---");
            try
            {
                // Solicitar datos de la nueva categoría
                Console.Write("Ingrese el nombre de la categoría: ");
                string name = Console.ReadLine();
                
                Console.Write("Ingrese la descripción de la categoría: ");
                string description = Console.ReadLine();
                
                // Crear instancia de CategoryData y guardar la nueva categoría
                CategoryData categoryData = new CategoryData();
                int categoryId = categoryData.SaveCategory(name, description);
                
                // Mostrar resultado
                Console.WriteLine($"Categoría guardada con ID: {categoryId}");
                Console.WriteLine("Prueba SaveCategory: EXITOSA ✓");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba SaveCategory: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método UpdateCategory() de la clase CategoryData
        /// Muestra las categorías existentes y permite al usuario seleccionar una para actualizar
        /// </summary>
        static void TestUpdateCategory()
        {
            Console.WriteLine("\n--- Probando UpdateCategory() ---");
            try
            {
                // Crear instancia de CategoryData
                CategoryData categoryData = new CategoryData();
                
                // Mostrar las categorías existentes para selección
                DataTable categories = categoryData.ShowCategories();
                
                Console.WriteLine("Categorías disponibles:");
                foreach (DataRow row in categories.Rows)
                {
                    Console.WriteLine($"ID: {row["id"]}, Descripción: {row["description"]}");
                }
                
                // Solicitar ID de la categoría a actualizar
                Console.Write("\nIngrese el ID de la categoría a actualizar: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryId))
                {
                    Console.WriteLine("ID inválido");
                    return;
                }
                
                // Solicitar nuevos datos
                Console.Write("Ingrese el nuevo nombre: ");
                string name = Console.ReadLine();
                
                Console.Write("Ingrese la nueva descripción: ");
                string description = Console.ReadLine();
                
                // Actualizar la categoría
                int affectedRows = categoryData.UpdateCategory(categoryId, name, description);
                
                // Mostrar resultado
                Console.WriteLine($"Filas afectadas: {affectedRows}");
                Console.WriteLine(affectedRows > 0 
                    ? "Prueba UpdateCategory: EXITOSA ✓" 
                    : "Prueba UpdateCategory: FALLIDA ✗ (No se encontró la categoría)");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba UpdateCategory: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Prueba el método DeleteCategory() de la clase CategoryData
        /// Muestra las categorías existentes y permite al usuario seleccionar una para eliminar
        /// </summary>
        static void TestDeleteCategory()
        {
            Console.WriteLine("\n--- Probando DeleteCategory() ---");
            try
            {
                // Crear instancia de CategoryData
                CategoryData categoryData = new CategoryData();
                
                // Mostrar las categorías existentes para selección
                DataTable categories = categoryData.ShowCategories();
                
                Console.WriteLine("Categorías disponibles:");
                foreach (DataRow row in categories.Rows)
                {
                    Console.WriteLine($"ID: {row["id"]}, Descripción: {row["description"]}");
                }
                
                // Solicitar ID de la categoría a eliminar
                Console.Write("\nIngrese el ID de la categoría a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryId))
                {
                    Console.WriteLine("ID inválido");
                    return;
                }
                
                // Solicitar confirmación antes de eliminar
                Console.Write($"¿Está seguro de eliminar la categoría con ID {categoryId}? (S/N): ");
                string confirmacion = Console.ReadLine();
                
                if (confirmacion.ToUpper() != "S")
                {
                    Console.WriteLine("Operación cancelada");
                    return;
                }
                
                // Eliminar la categoría
                int affectedRows = categoryData.DeleteCategory(categoryId);
                
                // Mostrar resultado
                Console.WriteLine($"Filas afectadas: {affectedRows}");
                Console.WriteLine(affectedRows > 0 
                    ? "Prueba DeleteCategory: EXITOSA ✓" 
                    : "Prueba DeleteCategory: FALLIDA ✗ (No se encontró la categoría)");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si falla la prueba
                Console.WriteLine($"Prueba DeleteCategory: ERROR ✗");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Ejecuta una secuencia completa de pruebas automatizadas:
        /// 1. Muestra todas las categorías
        /// 2. Crea una nueva categoría de prueba
        /// 3. Actualiza la categoría creada
        /// 4. Elimina la categoría creada
        /// </summary>
        static void RunAllTests()
        {
            Console.WriteLine("\n=== EJECUTANDO TODAS LAS PRUEBAS ===");
            
            // Mostrar categorías existentes
            TestShowCategories();
            
            // Ejecutar pruebas automatizadas de CRUD
            try
            {
                // Crear una categoría de prueba
                Console.WriteLine("\n--- Prueba automatizada SaveCategory() ---");
                CategoryData categoryData = new CategoryData();
                string testName = "Test Category";
                string testDesc = "Test Description " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                int newId = categoryData.SaveCategory(testName, testDesc);
                Console.WriteLine($"Categoría de prueba creada con ID: {newId}");
                
                // Actualizar la categoría creada
                Console.WriteLine("\n--- Prueba automatizada UpdateCategory() ---");
                string updatedDesc = "Updated Description " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int updateResult = categoryData.UpdateCategory(newId, testName, updatedDesc);
                Console.WriteLine($"Actualización: {updateResult} filas afectadas");
                
                // Eliminar la categoría creada
                Console.WriteLine("\n--- Prueba automatizada DeleteCategory() ---");
                int deleteResult = categoryData.DeleteCategory(newId);
                Console.WriteLine($"Eliminación: {deleteResult} filas afectadas");
                
                Console.WriteLine("\nPruebas automatizadas completadas correctamente!");
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si fallan las pruebas automatizadas
                Console.WriteLine($"Error en pruebas automatizadas: {ex.Message}");
            }
        }
    }
} 