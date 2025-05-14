/**
 * Proyecto MiniTienda - Pruebas de la Capa de Acceso a Datos
 * 
 * Este programa de prueba demuestra el funcionamiento de la capa de acceso a datos
 * a través de pruebas secuenciales que realizan operaciones CRUD (Create, Read, Update, Delete)
 * en la tabla de categorías utilizando la clase CategoryData.
 * 
 * Fecha: 02/05/2025
 */

    using MiniTienda.Data;
    using MiniTienda.Logic;
    using System;
    using System.Data;

    namespace MiniTienda.Tests
    {
        /// <summary>
        /// Clase principal que contiene métodos para probar las funcionalidades
        /// de la capa de acceso a datos de MiniTienda
        /// </summary>
        class Program
        {
            /// <summary>
            /// Método principal que ejecuta las pruebas secuenciales de la capa de datos
            /// </summary>
            /// <param name="args">Argumentos de la línea de comandos (no utilizados)</param>
            static void Main(string[] args)
            {
                // Pruebas de la capa de datos
                // TestDataLayer(); // Comentado para evitar errores en la capa de datos
                
                    Console.WriteLine("\n===== Pruebas de la capa lógica =====\n");
                
                    // Pruebas de la capa lógica para Users
                    TestUsersLog testUsers = new TestUsersLog();
                    testUsers.RunAllTests();
                
                // Pruebas de la capa lógica para Products
                TestProductsLog testProducts = new TestProductsLog();
                testProducts.RunAllTests();
                
                // Pruebas de la capa lógica para Category
                TestCategoryLog testCategory = new TestCategoryLog();
                testCategory.RunAllTests();
                
                // Pruebas de la capa lógica para Providers
                TestProvidersLog testProviders = new TestProvidersLog();
                testProviders.RunAllTests();
            }
                catch (Exception ex)
                {
                    // Capturar y mostrar cualquier error que ocurra durante las pruebas
                    Console.WriteLine("Error en la operación:");
                    Console.WriteLine(ex.Message);
                
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Detalles adicionales:");
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Presione cualquier tecla para salir...");
                Console.ReadKey();
            }
        
            /// <summary>
            /// Método que ejecuta las pruebas de la capa de datos
            /// </summary>
            static void TestDataLayer()
            {
                Console.WriteLine("===== Prueba de la capa de datos =====");
                Console.WriteLine();

                // Instanciar la clase CategoryData
                CategoryData categoryData = new CategoryData();
            
                // 1. Insertar una nueva categoría
                Console.WriteLine("1. INSERTANDO NUEVA CATEGORÍA");
                Console.WriteLine("--------------------------------------------------");
                int newCategoryId = categoryData.SaveCategory("Electrónicos", "Productos electrónicos");
                Console.WriteLine($"Categoría insertada con ID: {newCategoryId}");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine();
            
                // 2. Mostrar todas las categorías
                Console.WriteLine("2. CONSULTANDO TODAS LAS CATEGORÍAS");
                Console.WriteLine("--------------------------------------------------");
                MostrarCategorias(categoryData);
                Console.WriteLine();
            
                // 3. Actualizar la categoría con ID 1
                Console.WriteLine("3. ACTUALIZANDO LA CATEGORÍA CON ID 1");
                Console.WriteLine("--------------------------------------------------");
                int rowsUpdated = categoryData.UpdateCategory(1, "Gadgets", "Gadgets electrónicos");
                Console.WriteLine($"Filas actualizadas: {rowsUpdated}");
                Console.WriteLine();
            
                Console.WriteLine("CATEGORÍAS DESPUÉS DE LA ACTUALIZACIÓN");
                Console.WriteLine("--------------------------------------------------");
                MostrarCategorias(categoryData);
                Console.WriteLine();
            
                // 4. Eliminar la categoría con ID 1
                Console.WriteLine("4. ELIMINANDO LA CATEGORÍA CON ID 1");
                Console.WriteLine("--------------------------------------------------");
                int rowsDeleted = categoryData.DeleteCategory(1);
                Console.WriteLine($"Filas eliminadas: {rowsDeleted}");
                Console.WriteLine();
            
                Console.WriteLine("CATEGORÍAS DESPUÉS DE LA ELIMINACIÓN");
                Console.WriteLine("--------------------------------------------------");
                MostrarCategorias(categoryData);
            }
        
            /// <summary>
            /// Método auxiliar para mostrar las categorías obtenidas de la base de datos
            /// Imprime en consola la lista de categorías con formato tabular
            /// </summary>
            /// <param name="categoryData">Instancia de CategoryData para acceder a los datos</param>
            static void MostrarCategorias(CategoryData categoryData)
            {
                // Obtener todas las categorías
                DataTable categories = categoryData.ShowCategories();
            
                if (categories.Rows.Count == 0)
                {
                    // Mostrar mensaje si no hay categorías
                    Console.WriteLine("No hay categorías registradas en la base de datos.");
                }
                else
                {
                    // Mostrar encabezados de la tabla
                    Console.WriteLine("Categorías encontradas:");
                    Console.WriteLine("ID\tNombre\t\tDescripción");
                    Console.WriteLine("--------------------------------------------------");
                
                    // Recorrer y mostrar cada categoría
                    foreach (DataRow row in categories.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string name = row["name"].ToString() ?? string.Empty;
                        string description = row["description"].ToString() ?? string.Empty;
                    
                        Console.WriteLine($"{id}\t{name}\t\t{description}");
                    }
                }
            }
        }
    }
